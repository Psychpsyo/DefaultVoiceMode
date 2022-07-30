using FrooxEngine;
using HarmonyLib;
using NeosModLoader;
using System.Reflection;

namespace DefaultVoiceMode
{
    public class DefaultVoiceMode : NeosMod
    {
        public override string Name => "DefaultVoiceMode";
        public override string Author => "Psychpsyo";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/Psychpsyo/DefaultVoiceMode";

        public static bool setInitialVoiceMode = false;

        [AutoRegisterConfigKey]
        public static ModConfigurationKey<VoiceMode> defaultVoiceMode = new ModConfigurationKey<VoiceMode>("Default Voice Mode", "What to set your voice mode to on startup.", () => VoiceMode.Normal);

        private static ModConfiguration config;
        public override void OnEngineInit() {
            config = GetConfiguration();

            Harmony harmony = new Harmony("Psychpsyo.DefaultVoiceMode");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(VoiceModeSync), "OnCommonUpdate")]
        class SetDefaultVoiceModeOnStartup
        {
            static void Postfix(VoiceModeSync __instance) {
                if (!setInitialVoiceMode) {
                    __instance.FocusedWorldVoiceMode.Value = config.GetValue(defaultVoiceMode);
                    __instance.GlobalMute.Value = config.GetValue(defaultVoiceMode) == VoiceMode.Mute;
                    setInitialVoiceMode = true;
                }
            }
        }
    }
}