using System;
using NeosModLoader;
using HarmonyLib;
using FrooxEngine;
using BaseX;

namespace SquishPanels
{
    public class PanelPatcher : NeosMod
    {
        public override string Author => "Cyro";
        public override string Name => "SquishPanels";
        public override string Version => "1.0.1";

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("net.cyro.SquishSpector");
            harmony.PatchAll();
        }


        [HarmonyPatch(typeof(NeosPanel),"OnChanges")]
        class NeosPanelOnChangePatcher
        {
            static void Postfix(NeosPanel __instance)
            {
                if (__instance.WhiteList.Count > 0 && __instance.WhiteList.Count < 2)
                {
                    Slot ContentSlot = __instance.WhiteList[0].Slot;
                    float3 LocalScl = ContentSlot.LocalScale;
                    ContentSlot.Scale_Field.TweenFromTo(new float3(LocalScl.x, 0f, LocalScl.z), LocalScl, 0.22f);
                    StaticAudioClip clip = __instance.Slot.AttachAudioClip(new Uri("neosdb:///bbdf36b8f036a5c30f7019d68c1fbdd4032bb1d4c9403bcb926bb21cd0ca3c1a.wav"));
                    AudioOutput audio = __instance.World.PlayOneShot(__instance.Slot.GlobalPosition, clip, 1f, true, 1f, __instance.Slot, AudioDistanceSpace.Local, false);
                    audio.DopplerLevel.Value = 0f;
                    audio.DistanceSpace.Value = AudioDistanceSpace.Global;
                    __instance.WhiteList.Add(null);
                }
            }
        }

        [HarmonyPatch(typeof(NeosPanel),"OnClose")]
        class NeosPanelOnClosePatcher
        {
            static bool Prefix(NeosPanel.TitleButton button, NeosPanel __instance)
            {
                Slot ContentSlot = (AccessTools.Field(typeof(NeosPanel), "_contentSlot").GetValue(__instance) as SyncRef<Slot>).Target;
                float3 LocalScl = ContentSlot.LocalScale;
                ContentSlot.Scale_Field.TweenFromTo(LocalScl, new float3(LocalScl.x, 0f, LocalScl.z), 0.22f, CurvePreset.Sine, null, new Action(__instance.Slot.Destroy));
                StaticAudioClip clip = __instance.Slot.AttachAudioClip(new Uri("neosdb:///e600ed8a6895325613b82a50fd2a8ea2ac64151adc5c48c913d33d584fdf75d5.wav"));
                AudioOutput audio = __instance.World.PlayOneShot(__instance.Slot.GlobalPosition, clip, 1f, true, 1f, __instance.Slot, AudioDistanceSpace.Local, false);
                audio.DopplerLevel.Value = 0f;
                audio.DistanceSpace.Value = AudioDistanceSpace.Global;
                return false;
            }
        }
    }
}
