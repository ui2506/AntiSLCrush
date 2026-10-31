// Copyright (c) 2026 ui_2506.
// Licensed under the ui_2506 Non-Commercial License.
// See LICENSE.md in the repository root for full license terms.

using HarmonyLib;
using Mirror;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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

        private static Exception Finalizer(int connectionId, Exception __exception)
        {
            if (__exception is not EndOfStreamException)
                return __exception;

            StackFrame[] frames = new StackTrace(__exception).GetFrames();

            if (frames == null || !Array.Exists(frames, frame => frame.GetMethod()?.DeclaringType == typeof(Unbatcher) && frame.GetMethod().Name == nameof(Unbatcher.GetNextMessage)))
                return __exception;

            if (NetworkServer.connections.TryGetValue(connectionId, out NetworkConnectionToClient connection))
                connection.Disconnect();
            else
                Transport.active?.ServerDisconnect(connectionId);

            Main.Log($"[OnTransportDataPatch] Malformed packet from connectionId:{connectionId}, disconnecting (truncated batch).", true);
            return null;
        }

        internal static void Clear() => _kickedList.Clear();
    }
}
