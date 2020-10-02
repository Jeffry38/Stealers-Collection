﻿namespace DomaNet.SystemInfo
{
    using System;
    using System.Runtime.InteropServices;

    internal class OSInfo : Structures
    {
        public static string OSBit
        {
            get
            {
                if (Environment.Is64BitOperatingSystem == true)
                {
                    return "x64";
                }
                else
                {
                    return "x32";
                }
            }
        }

        public static readonly OperatingSystem osVersion = Environment.OSVersion;
        public static readonly string MachineName = Environment.MachineName;
        public static readonly string UserName = Environment.UserName;
        public static readonly string SystemDir = Environment.SystemDirectory;
        public static readonly int ProcessorCount = Environment.ProcessorCount;

        static OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();

        public static string ServicePack
        {
            get
            {
                string servicePack = string.Empty;
                if (NativeMethods.GetVersionEx(ref osVersionInfo))
                {
                    servicePack = new OSVERSIONINFOEX
                    {
                        dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX))
                    }.szCSDVersion;
                }

                return servicePack;
            }
        }

        #region PRODUCT

        private const int PRODUCT_UNDEFINED = 0x00000000;
        private const int PRODUCT_ULTIMATE = 0x00000001;
        private const int PRODUCT_HOME_BASIC = 0x00000002;
        private const int PRODUCT_HOME_PREMIUM = 0x00000003;
        private const int PRODUCT_ENTERPRISE = 0x00000004;
        private const int PRODUCT_HOME_BASIC_N = 0x00000005;
        private const int PRODUCT_BUSINESS = 0x00000006;
        private const int PRODUCT_STANDARD_SERVER = 0x00000007;
        private const int PRODUCT_DATACENTER_SERVER = 0x00000008;
        private const int PRODUCT_SMALLBUSINESS_SERVER = 0x00000009;
        private const int PRODUCT_ENTERPRISE_SERVER = 0x0000000A;
        private const int PRODUCT_STARTER = 0x0000000B;
        private const int PRODUCT_DATACENTER_SERVER_CORE = 0x0000000C;
        private const int PRODUCT_STANDARD_SERVER_CORE = 0x0000000D;
        private const int PRODUCT_ENTERPRISE_SERVER_CORE = 0x0000000E;
        private const int PRODUCT_ENTERPRISE_SERVER_IA64 = 0x0000000F;
        private const int PRODUCT_BUSINESS_N = 0x00000010;
        private const int PRODUCT_WEB_SERVER = 0x00000011;
        private const int PRODUCT_CLUSTER_SERVER = 0x00000012;
        private const int PRODUCT_HOME_SERVER = 0x00000013;
        private const int PRODUCT_STORAGE_EXPRESS_SERVER = 0x00000014;
        private const int PRODUCT_STORAGE_STANDARD_SERVER = 0x00000015;
        private const int PRODUCT_STORAGE_WORKGROUP_SERVER = 0x00000016;
        private const int PRODUCT_STORAGE_ENTERPRISE_SERVER = 0x00000017;
        private const int PRODUCT_SERVER_FOR_SMALLBUSINESS = 0x00000018;
        private const int PRODUCT_SMALLBUSINESS_SERVER_PREMIUM = 0x00000019;
        private const int PRODUCT_HOME_PREMIUM_N = 0x0000001A;
        private const int PRODUCT_ENTERPRISE_N = 0x0000001B;
        private const int PRODUCT_ULTIMATE_N = 0x0000001C;
        private const int PRODUCT_WEB_SERVER_CORE = 0x0000001D;
        private const int PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT = 0x0000001E;
        private const int PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY = 0x0000001F;
        private const int PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING = 0x00000020;
        private const int PRODUCT_SERVER_FOUNDATION = 0x00000021;
        private const int PRODUCT_HOME_PREMIUM_SERVER = 0x00000022;
        private const int PRODUCT_SERVER_FOR_SMALLBUSINESS_V = 0x00000023;
        private const int PRODUCT_STANDARD_SERVER_V = 0x00000024;
        private const int PRODUCT_DATACENTER_SERVER_V = 0x00000025;
        private const int PRODUCT_ENTERPRISE_SERVER_V = 0x00000026;
        private const int PRODUCT_DATACENTER_SERVER_CORE_V = 0x00000027;
        private const int PRODUCT_STANDARD_SERVER_CORE_V = 0x00000028;
        private const int PRODUCT_ENTERPRISE_SERVER_CORE_V = 0x00000029;
        private const int PRODUCT_HYPERV = 0x0000002A;
        private const int PRODUCT_STORAGE_EXPRESS_SERVER_CORE = 0x0000002B;
        private const int PRODUCT_STORAGE_STANDARD_SERVER_CORE = 0x0000002C;
        private const int PRODUCT_STORAGE_WORKGROUP_SERVER_CORE = 0x0000002D;
        private const int PRODUCT_STORAGE_ENTERPRISE_SERVER_CORE = 0x0000002E;
        private const int PRODUCT_STARTER_N = 0x0000002F;
        private const int PRODUCT_PROFESSIONAL = 0x00000030;
        private const int PRODUCT_PROFESSIONAL_N = 0x00000031;
        private const int PRODUCT_SB_SOLUTION_SERVER = 0x00000032;
        private const int PRODUCT_SERVER_FOR_SB_SOLUTIONS = 0x00000033;
        private const int PRODUCT_STANDARD_SERVER_SOLUTIONS = 0x00000034;
        private const int PRODUCT_STANDARD_SERVER_SOLUTIONS_CORE = 0x00000035;
        private const int PRODUCT_SB_SOLUTION_SERVER_EM = 0x00000036;
        private const int PRODUCT_SERVER_FOR_SB_SOLUTIONS_EM = 0x00000037;
        private const int PRODUCT_SOLUTION_EMBEDDEDSERVER = 0x00000038;
        private const int PRODUCT_SOLUTION_EMBEDDEDSERVER_CORE = 0x00000039;
        private const int PRODUCT_ESSENTIALBUSINESS_SERVER_MGMT = 0x0000003B;
        private const int PRODUCT_ESSENTIALBUSINESS_SERVER_ADDL = 0x0000003C;
        private const int PRODUCT_ESSENTIALBUSINESS_SERVER_MGMTSVC = 0x0000003D;
        private const int PRODUCT_ESSENTIALBUSINESS_SERVER_ADDLSVC = 0x0000003E;
        private const int PRODUCT_SMALLBUSINESS_SERVER_PREMIUM_CORE = 0x0000003F;
        private const int PRODUCT_CLUSTER_SERVER_V = 0x00000040;
        private const int PRODUCT_EMBEDDED = 0x00000041;
        private const int PRODUCT_STARTER_E = 0x00000042;
        private const int PRODUCT_HOME_BASIC_E = 0x00000043;
        private const int PRODUCT_HOME_PREMIUM_E = 0x00000044;
        private const int PRODUCT_PROFESSIONAL_E = 0x00000045;
        private const int PRODUCT_ENTERPRISE_E = 0x00000046;
        private const int PRODUCT_ULTIMATE_E = 0x00000047;

        #endregion

        #region VERSIONS

        private const int VER_NT_WORKSTATION = 1;
        private const int VER_NT_DOMAIN_CONTROLLER = 2;
        private const int VER_NT_SERVER = 3;
        private const int VER_SUITE_SMALLBUSINESS = 1;
        private const int VER_SUITE_ENTERPRISE = 2;
        private const int VER_SUITE_TERMINAL = 16;
        private const int VER_SUITE_DATACENTER = 128;
        private const int VER_SUITE_SINGLEUSERTS = 256;
        private const int VER_SUITE_PERSONAL = 512;
        private const int VER_SUITE_BLADE = 1024;

        #endregion

        static string s_Edition;
        static string s_Name;

        public static string Edition
        {
            get
            {
                if (s_Edition != null)
                {
                    return s_Edition;
                }

                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

                var edition = string.Empty;
                if (NativeMethods.GetVersionEx(ref osVersionInfo))
                {
                    #region VERSION 4

                    if (osVersion.Version.Major == 4)
                    {
                        if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
                        {
                            edition = "Workstation";
                        }
                        else if (osVersionInfo.wProductType == VER_NT_SERVER)
                        {
                            if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) != 0)
                            {
                                edition = "Enterprise Server";
                            }
                            else
                            {
                                edition = "Standard Server";
                            }
                        }
                    }
                    #endregion VERSION 4

                    #region VERSION 5

                    else if (osVersion.Version.Major == 5)
                    {
                        if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
                        {
                            if ((osVersionInfo.wSuiteMask & VER_SUITE_PERSONAL) != 0)
                            {
                                edition = "Home";
                            }
                            else
                            {
                                if (NativeMethods.GetSystemMetrics(86) == 0)
                                {
                                    edition = "Professional";
                                }
                                else
                                {
                                    edition = "Tablet Edition";
                                }
                            }
                        }
                        else if (osVersionInfo.wProductType == VER_NT_SERVER)
                        {
                            if (osVersion.Version.Minor == 0)
                            {
                                if ((osVersionInfo.wSuiteMask & VER_SUITE_DATACENTER) != 0)
                                {
                                    edition = "Datacenter Server";
                                }
                                else if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) != 0)
                                {
                                    edition = "Advanced Server";
                                }
                                else
                                {
                                    edition = "Server";
                                }
                            }
                            else
                            {
                                if ((osVersionInfo.wSuiteMask & VER_SUITE_DATACENTER) != 0)
                                {
                                    edition = "Datacenter";
                                }
                                else if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) != 0)
                                {
                                    edition = "Enterprise";
                                }
                                else if ((osVersionInfo.wSuiteMask & VER_SUITE_BLADE) != 0)
                                {
                                    edition = "Web Edition";
                                }
                                else edition = "Standard";
                            }
                        }
                    }
                    #endregion VERSION 5

                    #region VERSION 6

                    else if (osVersion.Version.Major == 6)
                    {
                        if (NativeMethods.GetProductInfo(osVersion.Version.Major, osVersion.Version.Minor, osVersionInfo.wServicePackMajor, osVersionInfo.wServicePackMinor, out int Checker))
                        {
                            switch (Checker)
                            {
                                case PRODUCT_BUSINESS: edition = "Business"; break;
                                case PRODUCT_BUSINESS_N: edition = "Business N"; break;
                                case PRODUCT_CLUSTER_SERVER: edition = "HPC Edition"; break;
                                case PRODUCT_CLUSTER_SERVER_V: edition = "HPC Edition without Hyper-V"; break;
                                case PRODUCT_DATACENTER_SERVER: edition = "Datacenter Server"; break;
                                case PRODUCT_DATACENTER_SERVER_CORE: edition = "Datacenter Server (core installation)"; break;
                                case PRODUCT_DATACENTER_SERVER_V: edition = "Datacenter Server without Hyper-V"; break;
                                case PRODUCT_DATACENTER_SERVER_CORE_V: edition = "Datacenter Server without Hyper-V (core installation)"; break;
                                case PRODUCT_EMBEDDED: edition = "Embedded"; break;
                                case PRODUCT_ENTERPRISE: edition = "Enterprise"; break;
                                case PRODUCT_ENTERPRISE_N: edition = "Enterprise N"; break;
                                case PRODUCT_ENTERPRISE_E: edition = "Enterprise E"; break;
                                case PRODUCT_ENTERPRISE_SERVER: edition = "Enterprise Server"; break;
                                case PRODUCT_ENTERPRISE_SERVER_CORE: edition = "Enterprise Server (core installation)"; break;
                                case PRODUCT_ENTERPRISE_SERVER_CORE_V: edition = "Enterprise Server without Hyper-V (core installation)"; break;
                                case PRODUCT_ENTERPRISE_SERVER_IA64: edition = "Enterprise Server for Itanium-based Systems"; break;
                                case PRODUCT_ENTERPRISE_SERVER_V: edition = "Enterprise Server without Hyper-V"; break;
                                case PRODUCT_ESSENTIALBUSINESS_SERVER_MGMT: edition = "Essential Business Server MGMT"; break;
                                case PRODUCT_ESSENTIALBUSINESS_SERVER_ADDL: edition = "Essential Business Server ADDL"; break;
                                case PRODUCT_ESSENTIALBUSINESS_SERVER_MGMTSVC: edition = "Essential Business Server MGMTSVC"; break;
                                case PRODUCT_ESSENTIALBUSINESS_SERVER_ADDLSVC: edition = "Essential Business Server ADDLSVC"; break;
                                case PRODUCT_HOME_BASIC: edition = "Home Basic"; break;
                                case PRODUCT_HOME_BASIC_N: edition = "Home Basic N"; break;
                                case PRODUCT_HOME_BASIC_E: edition = "Home Basic E"; break;
                                case PRODUCT_HOME_PREMIUM: edition = "Home Premium"; break;
                                case PRODUCT_HOME_PREMIUM_N: edition = "Home Premium N"; break;
                                case PRODUCT_HOME_PREMIUM_E: edition = "Home Premium E"; break;
                                case PRODUCT_HOME_PREMIUM_SERVER: edition = "Home Premium Server"; break;
                                case PRODUCT_HYPERV: edition = "Microsoft Hyper-V Server"; break;
                                case PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT: edition = "Windows Essential Business Management Server"; break;
                                case PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING: edition = "Windows Essential Business Messaging Server"; break;
                                case PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY: edition = "Windows Essential Business Security Server"; break;
                                case PRODUCT_PROFESSIONAL: edition = "Professional"; break;
                                case PRODUCT_PROFESSIONAL_N: edition = "Professional N"; break;
                                case PRODUCT_PROFESSIONAL_E: edition = "Professional E"; break;
                                case PRODUCT_SB_SOLUTION_SERVER: edition = "SB Solution Server"; break;
                                case PRODUCT_SB_SOLUTION_SERVER_EM: edition = "SB Solution Server EM"; break;
                                case PRODUCT_SERVER_FOR_SB_SOLUTIONS: edition = "Server for SB Solutions"; break;
                                case PRODUCT_SERVER_FOR_SB_SOLUTIONS_EM: edition = "Server for SB Solutions EM"; break;
                                case PRODUCT_SERVER_FOR_SMALLBUSINESS: edition = "Windows Essential Server Solutions"; break;
                                case PRODUCT_SERVER_FOR_SMALLBUSINESS_V: edition = "Windows Essential Server Solutions without Hyper-V"; break;
                                case PRODUCT_SERVER_FOUNDATION: edition = "Server Foundation"; break;
                                case PRODUCT_SMALLBUSINESS_SERVER: edition = "Windows Small Business Server"; break;
                                case PRODUCT_SMALLBUSINESS_SERVER_PREMIUM: edition = "Windows Small Business Server Premium"; break;
                                case PRODUCT_SMALLBUSINESS_SERVER_PREMIUM_CORE: edition = "Windows Small Business Server Premium (core installation)"; break;
                                case PRODUCT_SOLUTION_EMBEDDEDSERVER: edition = "Solution Embedded Server"; break;
                                case PRODUCT_SOLUTION_EMBEDDEDSERVER_CORE: edition = "Solution Embedded Server (core installation)"; break;
                                case PRODUCT_STANDARD_SERVER: edition = "Standard Server"; break;
                                case PRODUCT_STANDARD_SERVER_CORE: edition = "Standard Server (core installation)"; break;
                                case PRODUCT_STANDARD_SERVER_SOLUTIONS: edition = "Standard Server Solutions"; break;
                                case PRODUCT_STANDARD_SERVER_SOLUTIONS_CORE: edition = "Standard Server Solutions (core installation)"; break;
                                case PRODUCT_STANDARD_SERVER_CORE_V: edition = "Standard Server without Hyper-V (core installation)"; break;
                                case PRODUCT_STANDARD_SERVER_V: edition = "Standard Server without Hyper-V"; break;
                                case PRODUCT_STARTER: edition = "Starter"; break;
                                case PRODUCT_STARTER_N: edition = "Starter N"; break;
                                case PRODUCT_STARTER_E: edition = "Starter E"; break;
                                case PRODUCT_STORAGE_ENTERPRISE_SERVER: edition = "Enterprise Storage Server"; break;
                                case PRODUCT_STORAGE_ENTERPRISE_SERVER_CORE: edition = "Enterprise Storage Server (core installation)"; break;
                                case PRODUCT_STORAGE_EXPRESS_SERVER: edition = "Express Storage Server"; break;
                                case PRODUCT_STORAGE_EXPRESS_SERVER_CORE: edition = "Express Storage Server (core installation)"; break;
                                case PRODUCT_STORAGE_STANDARD_SERVER: edition = "Standard Storage Server"; break;
                                case PRODUCT_STORAGE_STANDARD_SERVER_CORE: edition = "Standard Storage Server (core installation)"; break;
                                case PRODUCT_STORAGE_WORKGROUP_SERVER: edition = "Workgroup Storage Server"; break;
                                case PRODUCT_STORAGE_WORKGROUP_SERVER_CORE: edition = "Workgroup Storage Server (core installation)"; break;
                                case PRODUCT_UNDEFINED: edition = "Unknown product"; break;
                                case PRODUCT_ULTIMATE: edition = "Ultimate"; break;
                                case PRODUCT_ULTIMATE_N: edition = "Ultimate N"; break;
                                case PRODUCT_ULTIMATE_E: edition = "Ultimate E"; break;
                                case PRODUCT_WEB_SERVER: edition = "Web Server"; break;
                                case PRODUCT_WEB_SERVER_CORE: edition = "Web Server (core installation)"; break;
                                default:
                                    break;
                            }
                        }
                    }
                    #endregion VERSION 6
                }

                s_Edition = edition;
                return edition;
            }
        }

        public static string Name
        {
            get
            {
                if (s_Name != null)
                {
                    return s_Name;
                }

                var name = "Unknown";
                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

                if (NativeMethods.GetVersionEx(ref osVersionInfo))
                {
                    switch (osVersion.Platform)
                    {
                        case PlatformID.Win32S: name = "Windows 3.1"; break;
                        case PlatformID.WinCE: name = "Windows CE"; break;

                        case PlatformID.Win32Windows:
                            {
                                if (osVersion.Version.Major == 4)
                                {
                                    switch (osVersion.Version.Minor)
                                    {
                                        case 0:
                                            {
                                                if (osVersionInfo.szCSDVersion == "B" || osVersionInfo.szCSDVersion == "C")
                                                {
                                                    name = "Windows 95 OSR2";
                                                }
                                                else
                                                {
                                                    name = "Windows 95";
                                                }
                                                break;
                                            }
                                        case 10:
                                            {
                                                if (osVersionInfo.szCSDVersion == "A")
                                                {
                                                    name = "Windows 98 Second Edition";
                                                }
                                                else
                                                {
                                                    name = "Windows 98";
                                                }
                                                break;
                                            }
                                        case 90:
                                            {
                                                name = "Windows Me";
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                }
                                break;
                            }
                        case PlatformID.Win32NT:
                            {
                                switch (osVersion.Version.Major)
                                {
                                    case 3:
                                        {
                                            name = "Windows NT 3.51"; break;
                                        }
                                    case 4:
                                        {
                                            switch (osVersionInfo.wProductType)
                                            {
                                                case 1:
                                                    {
                                                        name = "Windows NT 4.0";
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        name = "Windows NT 4.0 Server";
                                                        break;
                                                    }
                                                default:
                                                    break;
                                            }
                                            break;
                                        }
                                    case 5:
                                        {
                                            switch (osVersion.Version.Minor)
                                            {
                                                case 0:
                                                    {
                                                        name = "Windows 2000";
                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        name = "Windows XP";
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        name = "Windows Server 2003";
                                                        break;
                                                    }
                                                default:
                                                    break;
                                            }
                                            break;
                                        }
                                    case 6:
                                        switch (osVersion.Version.Minor)
                                        {
                                            case 0:
                                                switch (osVersionInfo.wProductType)
                                                {
                                                    case 1:
                                                        {
                                                            name = "Windows Vista";
                                                            break;
                                                        }
                                                    case 3:
                                                        {
                                                            name = "Windows Server 2008";
                                                            break;
                                                        }
                                                    default:
                                                        break;
                                                }
                                                break;

                                            case 1:
                                                switch (osVersionInfo.wProductType)
                                                {
                                                    case 1:
                                                        {
                                                            name = "Windows 7";
                                                            break;
                                                        }
                                                    case 3:
                                                        {
                                                            name = "Windows Server 2008 R2";
                                                            break;
                                                        }
                                                    default:
                                                        break;
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            }
                        default:
                            break;
                    }
                }
                s_Name = name;
                return name;
            }
        }
    }
}