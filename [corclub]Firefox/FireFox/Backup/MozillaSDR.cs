using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;

namespace MozillaPasswords
{
    /*
     * SDR decoder in C#, based on Wejn <wejn@box.cz>, itself based on sdrtest.c
     * All conditions mentioned in sdrtest apply.
     * 
     * N�cessite les dlls de NSPR4 et de NSS3 dans le dossier de l'ex�cutable (par ex : bin\Debug et bin\Release)
     * T�l�charger ces deux libraries � :
     * -> ftp://ftp.mozilla.org/pub/mozilla.org/nspr/releases/v4.6.7/src/nspr-4.6.7.tar.gz
     * -> https://ftp.mozilla.org/pub/mozilla.org/security/nss/releases/NSS_3_11_4_RTM/msvc6.0/WINNT5.0_OPT.OBJ/nss-3.11.4.zip
     * 
     * Extraire les dlls du dossier lib de ces archives et les placer dans le dossier de l'ex�cutable
     *
     * Author: ShareVB
     */
    public class MozillaSDR : IDisposable
    {
        //un item de s�curit� de NSS
        [StructLayout(LayoutKind.Sequential)]
        private struct SECItem {
            public Int32 type;
            public IntPtr data;
            public Int32 len;
        };
        //d�finit la callback qui sera appel�e pour r�cup�rer le mot de passe maitre du profile qui stocke le mot de passe � d�crypter
        [DllImport("nss3.dll",CallingConvention=CallingConvention.Cdecl)]
        private static extern int PK11_SetPasswordFunc(MulticastDelegate callback);
        //initialise NSS
        [DllImport("nss3.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NSS_Init([MarshalAs(UnmanagedType.LPStr)]string profile_dir);
        //d�code une chaine en base64 et renvoie un SECItem contenant la chaine d�cod�e
        [DllImport("nss3.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr NSSBase64_DecodeBuffer(IntPtr p1, IntPtr p2, [MarshalAs(UnmanagedType.LPStr)]string encoded, int encoded_len);
        //d�crypte un texte avec PK11SDR_Encrypt
        [DllImport("nss3.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int PK11SDR_Decrypt(IntPtr encrypted_item, ref SECItem text, int p1);
        //Lib�re les donn�es point�es par un SECItem (bDestroy indique si l'on doit aussi lib�rer le SECItem)
        [DllImport("nss3.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SECITEM_FreeItem(ref SECItem item, int bDestroy);
        [DllImport("nss3.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SECITEM_FreeItem(IntPtr item,int bDestroy);
        //lib�re NSS
        [DllImport("nss3.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NSS_Shutdown();
        //lib�re les ressources de NSS
        [DllImport("libnspr4.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int PR_Cleanup();

        //mot de passe ma�tre
        private static string password = null;

        //permet � NSS de r�cup�rer le mot de passe maitre
        private static IntPtr My_GetModulePassword(IntPtr slot, int retry, IntPtr arg){
                if(retry == 0 && password != null){
                        return Marshal.StringToHGlobalAnsi(password);
                }else{
                        return IntPtr.Zero;
                }
        }

        /// <summary>
        /// Initialise NSS avec un dossier de profile et son mot de passe ma�tre (habituellement null)
        /// </summary>
        /// <param name="profile_dir">dossier du profile contenant le mot de passe � d�crypter</param>
        /// <param name="master_passwd">mot de passe maitre du profile</param>
        /// <returns>0 si succ�s, != 0 si erreur</returns>
        private int nss_init(string profile_dir, string master_passwd) {
            int    rv;
            password = master_passwd;

            //Environment.CurrentDirectory = profile_dir;

            /*
             * Initialize the Security libraries.
             */
            //cr�e ou r�cup�re un d�l�gu� CDECL pour fournir comme callback de r�cup�ration de mot de passe
            MulticastDelegate deleg = CallConvDelegateBuilder.CreateDelegateInstance(
                typeof(MozillaSDR), "My_GetModulePassword",CallingConvention.Cdecl, null);
            PK11_SetPasswordFunc(deleg);

            rv = NSS_Init(profile_dir);
            if (rv != 0)
                return -1;
            else
                return 0;
        }

        /// <summary>
        /// D�crypte un texte pour le profile en cours et le mot de passe maitre en cours
        /// </summary>
        /// <param name="b64_encrypted_data">texte encod� en base 64 � d�crypter</param>
        /// <param name="buf">r�sultat d�crypt�</param>
        /// <returns>0 si succ�s, != 0 si erreur</returns>
        private int nss_decrypt(string b64_encrypted_data, out string buf)
        {
            //valeur de retour
            int retval = 0;
            //r�sultat du d�chiffrement
            int rv;
            //texte d�chiffr�
            SECItem text = new SECItem();
            //texte d�cod� mais crypt�
            IntPtr ok = IntPtr.Zero;
            buf = string.Empty;

            text.data = IntPtr.Zero; text.len = 0;

            /* input was base64 encoded.  Decode it. */
            ok = NSSBase64_DecodeBuffer(IntPtr.Zero, IntPtr.Zero,
                   b64_encrypted_data, b64_encrypted_data.Length);

            if (ok == IntPtr.Zero)
            {
                retval = -1;
            }
            else
            {
                /* Decrypt the value */
                rv = PK11SDR_Decrypt(ok, ref text, 0);
                if (rv != 0)
                {
                    retval = -2;
                }
                else
                {
                    //r�cup�re la chaine ASCII
                    buf = Marshal.PtrToStringAnsi(text.data,text.len);
                    /* == cleanup and adjust pointers == */
                    SECITEM_FreeItem(ref text, 0);
                }
                SECITEM_FreeItem(ok, 1);
            }

            //lib�re le texte d�cod� allou� par la fonction de d�codage
            if (text.data != IntPtr.Zero)
                Marshal.FreeHGlobal(text.data);

            return retval;
        }

        /// <summary>
        /// D�code une chaine en base64
        /// </summary>
        /// <param name="b64_encrypted_data">texte encod�</param>
        /// <param name="buf">r�sultat du d�codage</param>
        /// <returns>0 si succ�s, != 0 si erreur</returns>
        private int nss_decode(string b64_encrypted_data, out string buf)
        {
            //valeur de retour
            int retval = 0;
            //texte d�chiffr�
            SECItem text = new SECItem();
            //texte d�cod� mais crypt�
            IntPtr ok = IntPtr.Zero;
            buf = string.Empty;

            /* input was base64 encoded.  Decode it. */
            ok = NSSBase64_DecodeBuffer(IntPtr.Zero, IntPtr.Zero,
                   b64_encrypted_data, b64_encrypted_data.Length);

            if (ok == IntPtr.Zero)
            {
                retval = -1;
            }
            else
            {
                //r�cup�re le SECItem point�
                text = (SECItem)Marshal.PtrToStructure(ok, typeof(SECItem));
                //r�cup�re la chaine contenue dans le SECItem
                buf = Marshal.PtrToStringAnsi(text.data, text.len);

                //lib�re le SECItem
                SECITEM_FreeItem(ok, 1);
            }

            return retval;
        }

        /// <summary>
        /// Lib�re NSS
        /// </summary>
        /// <returns>0 si succ�s, != 0 si erreur</returns>
        private int nss_free()
        {
            if (NSS_Shutdown() != 0)
                return -1;
            //peut bloquer dans certain cas, donc cleanup par windows � la fin du programme (solution sugg�r�e par Mozilla)
            //PR_Cleanup();
            return 0;
        }

        /// <summary>
        /// Initialise NSS avec un dossier de profile et son mot de passe ma�tre (habituellement null)
        /// </summary>
        /// <param name="profile_dir">dossier du profile contenant le mot de passe � d�crypter</param>
        /// <param name="master_passwd">mot de passe maitre du profile</param>
        public MozillaSDR(string profile_dir, string password)
        {
            nss_init(profile_dir,password);
        }

        /// <summary>
        /// D�crypte un mot de passe crypt�
        /// </summary>
        /// <param name="encrypted">texte encrypt� et encod� en base 64</param>
        /// <returns>renvoie le texte d�crypt� ou string.Empty</returns>
        public string DecryptPassword(string encrypted)
        {
            string ret = null;
            if (nss_decrypt(encrypted, out ret) != 0)
                return string.Empty;
            else
                return ret;
        }
        /// <summary>
        /// D�code un texte encod� en base64
        /// </summary>
        /// <param name="encoded">texte encod�</param>
        /// <returns>renvoie le texte d�cod� ou string.Empty</returns>
        public string DecodePassword(string encoded)
        {
            string ret = null;
            if (nss_decode(encoded, out ret) != 0)
                return string.Empty;
            else
                return ret;
        }

        #region IDisposable Members

        public void Dispose()
        {
            //lib�re les ressources on dispose
            nss_free();
        }

        #endregion
    }
}
