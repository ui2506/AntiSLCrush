using HarmonyLib;
using InventorySystem.Items.Firearms.Modules;
using System.Collections.Generic;
using System.Reflection.Emit;
using NorthwoodLib.Pools;
using static HarmonyLib.AccessTools;

namespace AntiSLCrush.Patches
{
    [HarmonyPatch(typeof(LinearAdsModule), nameof(LinearAdsModule.OnHolstered))]
    internal static class LinearAdsOnHolsteredPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Call && instruction.operand == PropertyGetter(typeof(LinearAdsModule), nameof(LinearAdsModule.AdsTarget)));

            newInstructions[index] = new CodeInstruction(OpCodes.Ldc_I4_0);
            newInstructions[index + 1] = new CodeInstruction(OpCodes.Stfld, Field(typeof(LinearAdsModule), nameof(LinearAdsModule._userInput)));

            for (int i = 0; i < newInstructions.Count; i++)
                yield return newInstructions[i];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
