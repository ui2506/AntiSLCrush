// Copyright (c) 2026 ui_2506.
// Licensed under the ui_2506 Non-Commercial License.
// See LICENSE.md in the repository root for full license terms.

global using Logger = LabApi.Features.Console.Logger;

using AntiSLCrush.Patches;
using HarmonyLib;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AntiSLCrush
{
    public sealed class Main : Plugin<Config>
    {
        public const string PluginName = "AntiSLCrush";

        public override string Name { get; } = PluginName;
        public override string Author { get; } = "ui_2506";
        public override string Description { get; } = PluginName;
        public override Version Version { get; } = new Version(3, 2, 1);
        public override Version RequiredApiVersion { get; } = LabApi.Features.LabApiProperties.CurrentVersion;
        public override LoadPriority Priority { get; } = LoadPriority.Lowest;

        private static float _lastLogTime = 0;
        private static readonly Harmony _harmony = new(PluginName);

        public override void Enable()
        {
            _harmony.PatchAll();

            LabApi.Events.Handlers.ServerEvents.RoundRestarted += OnTransportDataPatch.Clear;
        }

        public override void Disable()
        {
            _harmony.UnpatchAll(_harmony.Id);

            LabApi.Events.Handlers.ServerEvents.RoundRestarted -= OnTransportDataPatch.Clear;
        }

        internal static void Log(string message, bool isError = false)
        {
            Debug.LogWarning(message);

            if (isError && _lastLogTime + 1 < Time.time)
            {
                Logger.Error(message);
                _lastLogTime = Time.time;
            }
            else
            {
                Logger.Debug(message, Config.Instance.Debug);
            }
        }
    }
}