using ColossalFramework.Plugins;
using System;
using System.IO;
using System.Reflection;

namespace Mod_Lang_CHS
{
    public class ModMain
    {

        private static string locale_name = "zh-cn";

        private static bool first_install = true;

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

            first_install = File.Exists(dst_path);

            File.Delete(dst_path);
            FileStream dst = File.OpenWrite(dst_path);

            byte[] buffer = new byte[8 * 1024];
            int len = 0;
            while ((len = st.Read(buffer, 0, buffer.Length)) > 0)
            {
                dst.Write(buffer, 0, len);
            }
            dst.Close();
            st.Close();

        }

        private static void setupLocale()
        {
            ColossalFramework.Globalization.LocaleManager.ForceReload();
            if (first_install)
            {
                string[] locales = ColossalFramework.Globalization.LocaleManager.instance.supportedLocaleIDs;
                for (int i = 0; i < locales.Length; i++)
                {
#if (DEBUG)
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("Locale index: {0}, ID: {1}", i, locales[i]));
#endif
                    if (locales[i].Equals(locale_name))
                    {
#if (DEBUG)
                        DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("Find locale {0} at index: {1}", locale_name, i));
#endif
                        ColossalFramework.Globalization.LocaleManager.instance.LoadLocaleByIndex(i);

                        ColossalFramework.SavedString lang_setting = new ColossalFramework.SavedString("localeID", "gameSettings");
#if (DEBUG)
                        DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("Current Language Setting: {0}", lang_setting.value));
#endif
                        lang_setting.value = locale_name;
                        ColossalFramework.GameSettings.SaveAll();
                        break;
                    }
                }
            }
        }
    }
}
