using BepInEx.Configuration;
using MadokaMagica.Modules;

namespace MadokaMagica.Jet.Content
{
    public static class ZiluConfig
    {
        public static ConfigEntry<bool> someConfigBool;
        public static ConfigEntry<float> someConfigFloat;
        public static ConfigEntry<float> someConfigFloatWithCustomRange;

        public static void Init()
        {
        }
    }
}
