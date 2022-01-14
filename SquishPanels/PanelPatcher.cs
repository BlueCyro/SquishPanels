using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public override string Version => "1.0.0";

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("net.cyro.SquishSpector");
            harmony.PatchAll();
        }


        [HarmonyPatch(typeof(NeosPanel),"OnAttach")]
        class SceneInspectorPatcher
        {
            static void Postfix(NeosPanel __instance)
            {
                __instance.Slot.Scale_Field.TweenFromTo(new float3(1f, 0f, 1f), new float3(1f, 1f, 1f), 0.22f);
                StaticAudioClip clip = __instance.Slot.AttachAudioClip(new Uri("neosdb:///bbdf36b8f036a5c30f7019d68c1fbdd4032bb1d4c9403bcb926bb21cd0ca3c1a.wav"));
                AudioOutput audio = __instance.World.PlayOneShot(__instance.Slot.GlobalPosition, clip, 1f, true, 1f, __instance.Slot, AudioDistanceSpace.Local, false);
                audio.DopplerLevel.Value = 0f;
                audio.DistanceSpace.Value = AudioDistanceSpace.Global;
            }
        }

        [HarmonyPatch(typeof(NeosPanel),"OnClose")]
        class NeosPanelPatcher
        {
            static bool Prefix(NeosPanel.TitleButton button, NeosPanel __instance)
            {
                float3 LocalScl = __instance.Slot.LocalScale;
                __instance.Slot.Scale_Field.TweenFromTo(LocalScl, new float3(LocalScl.x, 0f, LocalScl.z), 0.22f, CurvePreset.Sine, null, new Action(__instance.Slot.Destroy));
                StaticAudioClip clip = __instance.Slot.AttachAudioClip(new Uri("neosdb:///e600ed8a6895325613b82a50fd2a8ea2ac64151adc5c48c913d33d584fdf75d5.wav"));
                AudioOutput audio = __instance.World.PlayOneShot(__instance.Slot.GlobalPosition, clip, 1f, true, 1f, __instance.Slot, AudioDistanceSpace.Local, false);
                audio.DopplerLevel.Value = 0f;
                audio.DistanceSpace.Value = AudioDistanceSpace.Global;
                return false;
            }
        }
    }
}
