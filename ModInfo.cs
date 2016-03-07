using ColossalFramework.Plugins;
using ICities;
using System;

namespace Mod_Lang_CHS
{
    public class ModInfo : IUserMod
    {
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
                return "简体中文";
            }
        }

        public void OnEnabled()
        {
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
