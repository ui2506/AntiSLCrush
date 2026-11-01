// Copyright (c) 2026 ui_2506.
// Licensed under the ui_2506 Non-Commercial License.
// See LICENSE.md in the repository root for full license terms.

using HarmonyLib;
using Mirror;
using System.Collections.Generic;

namespace AntiSLCrush.Patches
{
    [HarmonyPatch(typeof(NetworkServer), nameof(NetworkServer.OnTransportData))]
    internal static class OnTransportDataPatch
    {
        private static readonly List<int> _kickedList = [];

        private static bool Prefix(int connectionId)
        {
            if (connectionId == 0 || NetworkServer.connections.ContainsKey(connectionId))
                return true;

            if (!_kickedList.Contains(connectionId))
            {
                Main.Log($"[OnTransportDataPath] Unknown connectionId:{connectionId}, disconnecting.");

                _kickedList.Add(connectionId);
                Transport.active?.ServerDisconnect(connectionId);
            }

            return false;
        }

        internal static void Clear() => _kickedList.Clear();
    }
}
