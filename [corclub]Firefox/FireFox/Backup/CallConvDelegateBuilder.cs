using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace MozillaPasswords
{
    /// <summary>
    /// Cette classe permet de g�n�rer un d�l�gu� avec une convention d'appel particuli�re
    /// Ne marche PAS avec le framework 1.0/1.1
    /// </summary>
    public class CallConvDelegateBuilder
    {
        //cache des d�l�gu�s cr��s (n�cessaire dans la mesure o� la g�n�ration de classe au runtime est un processus relativement long)
        private static Dictionary<string, Type> generated_delegates = new Dictionary<string, Type>();

        /// <summary>
        /// Renvoie le nom de la classe g�n�r�e (sans le Delegate de fin)
        /// </summary>
        /// <param name="delegateMethodInfo">m�thode dont on va g�n�rer un d�l�gu�</param>
        /// <param name="callConv">convention d'appel du d�l�gu� � g�n�rer</param>
        /// <returns></returns>
        private static string GetNewDelegateName(MethodInfo delegateMethodInfo, CallingConvention callConv)
        {
            return delegateMethodInfo.Name + callConv;
        }

        /// <summary>
        /// G�n�re un Type d�riv� de MulticastDelegate suivant la signature d'une m�thode
        /// </summary>
        /// <param name="delegateMethodInfo">information sur la m�thode qui servira � g�n�rer le d�l�gu�</param>
        /// <param name="callConv">convention d'appel du d�l�gu�</param>
        /// <param name="delegName">nom du d�l�gu� � g�n�rer</param>
        /// <returns></returns>
        private static Type CreateDelegate(MethodInfo delegateMethodInfo, CallingConvention callConv,string delegName)
        {
            //Un d�l�gu� est une classe :
            //-> d�rivant de MulticastDelegate
            //-> ayant un constructeur prenant deux param�tres (la m�thode � appeler et l'objet sur lequel appeler la m�thode)
            //-> une m�thode Invoke pour appeler la m�thode de mani�re synchrone
            //-> une paire de m�thodes BeginInvoke/EndInvoke pour appeler la m�thode de mani�re asynchrone
            //-> la signature des trois m�thodes pr�c�dentes d�pend de la signature de la m�thode � appeler
            //   cette classe est donc g�n�r� � la compilation par rapport � la signature d�clar�e avec le mot cl� "delegate"

            //g�n�re un nom pour l'assembly qui contiendra le type du d�l�gu�
            AssemblyName asm_name = new AssemblyName(delegName);
            //nom du NetModule  qui contiendra le type
            string moduleName = asm_name.Name + "Module.dll";
            //construit un assembly pour enregistrement et ex�cution pour contenir le type du d�l�gu� dans l'AppDomain courant
            AssemblyBuilder asm_builder = AppDomain.CurrentDomain.DefineDynamicAssembly(asm_name, AssemblyBuilderAccess.RunAndSave);
            //construit un module
            ModuleBuilder mod_builder = asm_builder.DefineDynamicModule(moduleName, moduleName, true);
            //construit un type d�riv� de MulticastDelegate pour le d�l�gu�
            TypeBuilder type_builder = mod_builder.DefineType(
                asm_name.Name + "Delegate",
                TypeAttributes.Class | TypeAttributes.AnsiClass | TypeAttributes.AutoClass | TypeAttributes.Sealed,
                typeof(System.MulticastDelegate));

            //type de retour de la m�thode
            Type method_return = delegateMethodInfo.ReturnType;
            //modopt et modreq du type de retour des m�thodes du d�l�gu�s (permet de sp�cifier la convention d'appel)
            Type[] method_return_modreq = null;
            Type[] method_return_modopt = null;

            //ajout du modopt correspondant � la convention d'appel
            switch (callConv)
            {
                case CallingConvention.Cdecl:
                    method_return_modopt = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) };
                    break;
                case CallingConvention.FastCall:
                    method_return_modopt = new Type[] { typeof(System.Runtime.CompilerServices.CallConvFastcall) };
                    break;
                case CallingConvention.StdCall:
                    method_return_modopt = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) };
                    break;
                case CallingConvention.ThisCall:
                    method_return_modopt = new Type[] { typeof(System.Runtime.CompilerServices.CallConvThiscall) };
                    break;
                case CallingConvention.Winapi:
                    method_return_modopt = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) };
                    break;
                default:
                    break;
            }

            //r�cup�re la liste des param�tres de la m�thode qui sert de base d�l�gu�
            //ainsi que leurs �ventuels modopt et modreq
            List<Type> method_parameters = new List<Type>();
            List<Type[]> method_parameters_modreq = new List<Type[]>();
            List<Type[]> method_parameters_modopt = new List<Type[]>();
            foreach (ParameterInfo par in delegateMethodInfo.GetParameters())
            {
                method_parameters.Add(par.ParameterType);
                method_parameters_modreq.Add(par.GetRequiredCustomModifiers());
                method_parameters_modopt.Add(par.GetOptionalCustomModifiers());
            }

            //=======================================================================================================================
            //g�n�re le constructeur du d�l�gu� : .ctor(object objThis,IntPtr method_pointer)
            //prend deux param�tres : 
            //-> objThis : l'instance d'objet sur lequel appeler la m�thode (null si static)
            //-> method_pointer : le pointeur vers la m�thode � appeler
            ConstructorBuilder construct_builder = type_builder.DefineConstructor(
                //constructeur public
                //ne cachant que le constructeur de la classe de base avec la m�me signature
                //avec un nom sp�cial vu qu'il s'agit d'un constructeur 
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName | MethodAttributes.SpecialName, 
                CallingConventions.Standard, 
                new Type[] { typeof(object), typeof(IntPtr) });
            //pas de corps : g�n�r� au runtime par le CLR
            construct_builder.SetImplementationFlags(MethodImplAttributes.Managed | MethodImplAttributes.Runtime);
            //=======================================================================================================================

            //=======================================================================================================================
            //g�n�re la m�thode EndInvoke du d�l�gu� (m�thode permettant de r�cup�rer le r�sultat d'un appel asynchrone
            //type_retour_m�thode EndInvoke(IAsyncResult  status)
            MethodBuilder end_invoke = type_builder.DefineMethod(
                "EndInvoke", 
                //m�thode publique, ne cachant qu'une m�thode de la classe de base ayant une signature identique
                //m�thode ne participant pas au polymorphisme depuis la classe de base (appartient � cette classe seulement "new")
                //(normal, vu que une m�thode avec le m�me nom peut exister dans la classe de base mais pas avec la m�me signature)
                //mais pouvant �tre overrid�e dans une sous classe
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, 
                CallingConventions.Standard);
            //d�finit un param�tre de type IAsyncResult contenant le status de l'appel asynchrone
            end_invoke.SetParameters(typeof(System.IAsyncResult));
            //renvoyant le r�sultat de la m�thode appel�e
            end_invoke.SetReturnType(method_return);
            //pas de corps : g�n�r� au runtime par le CLR
            end_invoke.SetImplementationFlags(MethodImplAttributes.Managed | MethodImplAttributes.Runtime);
            //=======================================================================================================================

            //=======================================================================================================================
            //r�cup�re le constructeur "internal" de la classe MarshalAsAttribute 
            //(permettant d'initialiser l'attribut avec toutes ses propri�t�s)
            ConstructorInfo marshalas_ctor = typeof(MarshalAsAttribute).GetConstructor(
                //pas public puisqu'internal
                BindingFlags.NonPublic | BindingFlags.Instance,null,
                //constructeur avec un param�tre pour chaque propri�t�
                new Type[] { typeof(UnmanagedType) , typeof(VarEnum) , typeof(Type) , 
                             typeof(UnmanagedType) , typeof(short) , typeof(int) , typeof(string) , 
                             typeof(Type) , typeof(string) , typeof(int) },null);

            //g�n�re le m�thode Invoke du d�l�gu� (m�thode permettant d'appeler de mani�re synchrone la m�thode contenue dans le d�l�gu�)
            //type_retour_m�thode Invoke(param�tres_m�thode)
            MethodBuilder invoke = type_builder.DefineMethod(
                "Invoke",
                //m�thode publique, ne cachant qu'une m�thode de la classe de base ayant une signature identique
                //m�thode ne participant pas au polymorphisme depuis la classe de base (appartient � cette classe seulement "new")
                //(normal, vu que une m�thode avec le m�me nom peut exister dans la classe de base mais pas avec la m�me signature)
                //mais pouvant �tre overrid�e dans une sous classe
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, 
                CallingConventions.Standard,
                //type de retour
                method_return,
                //�ventuel modreq de la m�thode
                method_return_modreq,
                //modopt indiquant la convention d'appel de la m�thode
                method_return_modopt,
                //les param�tres
                method_parameters.ToArray(),
                //les modreq et modopt des param�tres
                method_parameters_modreq.ToArray(),
                method_parameters_modopt.ToArray());
            //pour chaque param�tre de la m�thode
            foreach (ParameterInfo par in delegateMethodInfo.GetParameters())
            {
                //lui ajouter ses attributs (in, ref, out...) et son nom
                ParameterBuilder b = invoke.DefineParameter(par.Position+1, par.Attributes, par.Name);
                //pour chaque attribut de Marshalling du param�tre
                foreach (MarshalAsAttribute mar in par.GetCustomAttributes(typeof(MarshalAsAttribute),false))
                {
                    //ajouter au param�tre l'attribut MarshalAs correspondant
                    //n�cessite d'avoir un constructeur de l'attribut et les valeurs des param�tres de ce constructeur
                    b.SetCustomAttribute(new CustomAttributeBuilder(marshalas_ctor, 
                        new object[] { 
                            mar.Value, mar.SafeArraySubType, mar.SafeArrayUserDefinedSubType, 
                            mar.ArraySubType, mar.SizeParamIndex, mar.SizeConst, 
                            mar.MarshalType, mar.MarshalTypeRef, mar.MarshalCookie, mar.IidParameterIndex
                        }));  
                }
            }
            //pas de corps : g�n�r� au runtime par le CLR
            invoke.SetImplementationFlags(MethodImplAttributes.Managed | MethodImplAttributes.Runtime);
            //=======================================================================================================================

            //=======================================================================================================================
            //g�n�re le m�thode BeginInvoke du d�l�gu� 
            //(m�thode permettant d'appeler de mani�re asynchrone la m�thode contenue dans le d�l�gu�)
            //(n�cessite EndInvoke pour r�cup�rer le r�sultat de l'ex�cution asynchrone)
            //type_retour_m�thode BeginInvoke(param�tres_m�thode,AsyncCallback callback,object arg)
            //(on peut passer une m�thode dans le param�tre callback. 
            //cette m�thode sera d�clencher lorsque la m�thode contenue dans le d�l�gu� renvoie sont r�sultat
            //elle (callback) recevra arg en param�tre

            //ajoute les deux derni�res param�tres de BeginInvoke � la liste des param�tres de la m�thode
            method_parameters.Add(typeof(System.AsyncCallback));
            method_parameters.Add(typeof(object));
            //ajoute le nombre de cases n�cessaires au modopt et modreq des param�tres
            //sinon DefineMethod lance une exception
            if (method_parameters_modopt.Count > 0)
            {
                method_parameters_modopt.Add(null);
                method_parameters_modopt.Add(null);
            }
            if (method_parameters_modreq.Count > 0)
            {
                method_parameters_modreq.Add(null);
                method_parameters_modreq.Add(null);
            }

            MethodBuilder begin_invoke = type_builder.DefineMethod(
                "BeginInvoke",
                //m�thode publique, ne cachant qu'une m�thode de la classe de base ayant une signature identique
                //m�thode ne participant pas au polymorphisme depuis la classe de base (appartient � cette classe seulement "new")
                //(normal, vu que une m�thode avec le m�me nom peut exister dans la classe de base mais pas avec la m�me signature)
                //mais pouvant �tre overrid�e dans une sous classe
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, 
                CallingConventions.Standard,
                //renvoie un r�sultat d'ex�cution asynchrone n�cessaire � la r�cup�ration du retour de la m�thode
                typeof(System.IAsyncResult),
                //�ventuel modreq de la m�thode
                method_return_modreq,
                //modopt indiquant la convention d'appel de la m�thode
                method_return_modopt,
                //les param�tres
                method_parameters.ToArray(),
                //les modreq et modopt des param�tres
                method_parameters_modreq.ToArray(),
                method_parameters_modopt.ToArray());
            //pour chaque param�tre de la m�thode
            foreach (ParameterInfo par in delegateMethodInfo.GetParameters())
            {
                //lui ajouter ses attributs (in, ref, out...) et son nom
                ParameterBuilder b = begin_invoke.DefineParameter(par.Position + 1, par.Attributes, par.Name);
                //pour chaque attribut de Marshalling du param�tre
                foreach (MarshalAsAttribute mar in par.GetCustomAttributes(typeof(MarshalAsAttribute), false))
                {
                    //ajouter au param�tre l'attribut MarshalAs correspondant
                    //n�cessite d'avoir un constructeur de l'attribut et les valeurs des param�tres de ce constructeur
                    b.SetCustomAttribute(new CustomAttributeBuilder(marshalas_ctor,
                        new object[] { 
                            mar.Value, mar.SafeArraySubType, mar.SafeArrayUserDefinedSubType, 
                            mar.ArraySubType, mar.SizeParamIndex, mar.SizeConst, 
                            mar.MarshalType, mar.MarshalTypeRef, mar.MarshalCookie, mar.IidParameterIndex
                        }));
                }
            }
            //ajoute les attributs et noms des deux derniers param�tres
            begin_invoke.DefineParameter(method_parameters.Count - 1, ParameterAttributes.None, "callback");
            begin_invoke.DefineParameter(method_parameters.Count, ParameterAttributes.None, "o");
            //pas de corps : g�n�r� au runtime par le CLR
            begin_invoke.SetImplementationFlags(MethodImplAttributes.Managed | MethodImplAttributes.Runtime);
            //=======================================================================================================================

            //g�n�re le Type ainsi d�fini
            Type t = type_builder.CreateType();

            //Debug : enregistrer l'assembly g�n�rer
            //asm_builder.Save(moduleName);

            return t;
        }

        /// <summary>
        /// Cr�e une instance d'un d�l�gu� charg� d'appeler la m�thode d�crite par delegateMethodInfo sur objThis
        /// ou la m�thode static d�crite par delegateMethodInfo
        /// suivant une convention d'appel particuli�re
        /// </summary>
        /// <param name="delegateMethodInfo">information sur la m�thode qui servira � g�n�rer le d�l�gu� et qui sera appel�e par celui-ci</param>
        /// <param name="callConv">convention d'appel du d�l�gu�</param>
        /// <param name="objThis">instance de l'objet qui contient la m�thode � appeler par le d�l�gu� cr�� ou null si la m�thode est statique</param>
        /// <returns>renvoie une instance du d�l�gu� qui appelera la m�thode de l'objet pass�</returns>
        public static MulticastDelegate CreateDelegateInstance(MethodInfo delegateMethodInfo, CallingConvention callConv,object objThis)
        {
            //g�n�re le nom du d�l�gu� � g�n�rer
            string delegName = GetNewDelegateName(delegateMethodInfo,callConv);
            Type delegate_type;
            //si on l'a d�j� g�n�r�
            if (generated_delegates.ContainsKey(delegName))
                //on r�cup�re
                delegate_type = generated_delegates[delegName];
            else
            {
                //sinon, on le cr�e et on l'ajoute � la liste
                delegate_type = CreateDelegate(delegateMethodInfo, callConv, delegName);
                generated_delegates.Add(delegName,delegate_type);
            }
            //cr�e une instance du d�l�gu�, initialis� avec l'objet qui contient la m�thode (ou null si static) et le pointeur vers la m�thode � appeler
            //rappel : une m�thode d'instance n'est jamais qu'une simple fonction qui prend en premier (ou dernier) param�tre, un pointeur vers une instance de la classe
            return (MulticastDelegate)Activator.CreateInstance(delegate_type, new object[] { objThis, delegateMethodInfo.MethodHandle.GetFunctionPointer() });
        }

        /// <summary>
        /// Cr�e une instance d'un d�l�gu� charg� d'appeler la m�thode objThis."methodName" 
        /// ou la m�thode static methodContainer."methodName"
        /// suivant une convention d'appel particuli�re
        /// </summary>
        /// <param name="methodContainer">Type contenant la m�thode methodName</param>
        /// <param name="methodName">nom de la m�thode dont on veut cr�er un d�l�gu�</param>
        /// <param name="callConv">convention d'appel du d�l�gu�</param>
        /// <param name="objThis">instance de l'objet qui contient la m�thode � appeler par le d�l�gu� cr�� ou null si la m�thode est statique</param>
        /// <returns>renvoie une instance du d�l�gu� qui appelera la m�thode de l'objet pass�</returns>
        public static MulticastDelegate CreateDelegateInstance(Type methodContainer, string methodName, CallingConvention callConv, object objThis)
        {
            //r�cup�re les infos sur la m�thode
            MethodInfo deleg = methodContainer.GetMethod(
                methodName, 
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                );
            return CreateDelegateInstance(deleg, callConv, objThis);
        }
    }
}
