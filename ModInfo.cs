using ColossalFramework.Plugins;
using ICities;
using System;

namespace Mod_Lang_CHS
{
    public class ModInfo : IUserMod
    {
        private static string version = "3(1.4.0-f3)";

        public string Description
        {
            get
            {
                return "让天际线学会简体中文";
            }
        }
    

        public string Name
        {
            get
            {
                return "简体中文ver." + version;
            }
        }

        public void OnEnabled()
        {
#if (DEBUG)
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "Mod Lang CHS OnEnabled");
#endif

            try
            {
                ModMain.setup();
            }
            catch (Exception e)
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, e.ToString());
            }
        }

        public void OnDisabled()
        {
            //DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "Mod_Lang_CHT disabled");
        }
    }
}
