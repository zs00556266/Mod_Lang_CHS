using ColossalFramework.Plugins;
using System;
using System.IO;
using System.Reflection;

namespace Mod_Lang_CHS
{
    public class ModMain
    {

        private static string locale_name = "zh-cn";

        private static string locale_md5 = "8AB75B7B345353C90A5C580F81925133";

        private static bool installed = false;

        public enum Platform
        {
            Windows,
            Linux,
            Mac
        }

        public static void setup()
        {
            setupLocaleFile();
            setupLocale();
        }

        public static Platform getPlatform()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    // Well, there are chances MacOSX is reported as Unix instead of MacOSX.
                    // Instead of platform check, we'll do a feature checks (Mac specific root folders)
                    if (Directory.Exists("/Applications")
                        & Directory.Exists("/System")
                        & Directory.Exists("/Users")
                        & Directory.Exists("/Volumes"))
                        return Platform.Mac;
                    else
                        return Platform.Linux;

                case PlatformID.MacOSX:
                    return Platform.Mac;

                default:
                    return Platform.Windows;
            }
        }

        private static string getLocalePath()
        {
            switch (getPlatform())
            {
                case Platform.Windows:
                    return "Files\\Locale\\" + locale_name + ".locale";
                case Platform.Mac:
                    return "Cities.app/Contents/Resources/Files/Locale/" + locale_name + ".locale";
                case Platform.Linux:
                    return "Files/Locale/" + locale_name + ".locale";
            }

            return "";
        }


        private static void setupLocaleFile()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            Stream st = asm.GetManifestResourceStream(asm.GetName().Name + "."+ locale_name + ".locale");
            
            string dst_path = getLocalePath();

            installed = File.Exists(dst_path);

            if (installed && checkMD5(dst_path))
            {
#if (DEBUG)
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "first install:false, md5 equals:true");
#endif
                return;
            }

            //File.Delete(dst_path);
            FileStream dst = File.OpenWrite(dst_path);

            byte[] buffer = new byte[300 * 1024];
            int len = 0;
            while ((len = st.Read(buffer, 0, buffer.Length)) > 0)
            {
                dst.Write(buffer, 0, len);
            }
            dst.Close();
            st.Close();

            installed = File.Exists(dst_path);

        }

        private static bool checkMD5(String filepath)
        {
            string md5 = getMD5Hash(filepath);
#if (DEBUG)
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("use loacle: {0}, mod loacle: {1}, equal:{2}", md5, locale_md5, locale_md5.Equals(md5)));
#endif
            return md5 == null ? true : locale_md5.Equals(md5);
        }

        private static void setupLocale()
        {
            ColossalFramework.Globalization.LocaleManager.ForceReload();

            if (installed)
            {
                int localeIndex = ColossalFramework.Globalization.LocaleManager.instance.GetLocaleIndex(locale_name);
                ColossalFramework.Globalization.LocaleManager.instance.LoadLocaleByIndex(localeIndex);
                ColossalFramework.SavedString lang_setting = new ColossalFramework.SavedString("localeID", "gameSettings");
                lang_setting.value = locale_name;
                ColossalFramework.GameSettings.SaveAll();
            }

            /*
            if (first_install)
            {
                string[] locales = ColossalFramework.Globalization.LocaleManager.instance.supportedLocaleIDs;
                for (int i = 0; i < locales.Length; i++)
                {
                    if (locales[i].Equals(locale_name))
                    {
                        ColossalFramework.Globalization.LocaleManager.instance.LoadLocaleByIndex(i);
                        ColossalFramework.Globalization.LocaleManager.instance.

                        ColossalFramework.SavedString lang_setting = new ColossalFramework.SavedString("localeID", "gameSettings");

                        lang_setting.value = locale_name;
                        ColossalFramework.GameSettings.SaveAll();
                        break;
                    }
                }
            }
            */
        }


        private static string getMD5Hash(string pathName)
        {

            string strResult = "";

            string strHashData = "";

            byte[] arrbytHashValue;

            System.IO.FileStream oFileStream = null;

            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();

            try
            {

                oFileStream = new System.IO.FileStream(pathName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);

                arrbytHashValue = oMD5Hasher.ComputeHash(oFileStream);

                oFileStream.Close();

                strHashData = System.BitConverter.ToString(arrbytHashValue);

                strHashData = strHashData.Replace("-", "");

                strResult = strHashData;

            }
            catch (System.Exception ex)
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, String.Format("getMD5Hash Error : {0}", ex.Message));
                return null;

            }

            return strResult;

        }
    }
}
