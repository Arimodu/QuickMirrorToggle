using SiraUtil.Logging;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace QuickMirrorToggle
{
    internal class MirrorManager : IInitializable, IDisposable
    {
        private readonly MirrorRendererSO MirrorRenderer;
        private readonly MirrorRendererGraphicsSettingsPresets MirrorGraphicsPreset;
        [Inject] private readonly MainSettingsModelSO MainSettingsModel;
        [Inject] private readonly QMTConfig _config;
        [Inject] private readonly SiraLog _logger;
        [Inject] private readonly QMTUI UI;

        public MirrorManager()
        {
            MirrorRenderer = Resources.FindObjectsOfTypeAll<MirrorRendererSO>().First();
            MirrorGraphicsPreset = Resources.FindObjectsOfTypeAll<MirrorRendererGraphicsSettingsPresets>().First();
        }

        public void Initialize()
        {
            _logger.Info("Initializing MirrorManager");
            _config.GameMirrorSetting = (MirrorState)(int)MainSettingsModel.mirrorGraphicsSettings;
            SetMirrorState(_config.MirrorState);
            UI.QMTToggleValue = _config.GameMirrorSetting == MirrorState.Off ? (_config.MirrorState != MirrorState.Off) : (_config.MirrorState == MirrorState.Off);

            _config.OnChanged += Config_OnChanged;
        }

        private void Config_OnChanged(QMTConfig newConfig)
        {
            SetMirrorState(newConfig.MirrorState);
        }

        public void SetMirrorState(MirrorState state)
        {
            var preset = MirrorGraphicsPreset.presets[MainSettingsModel.mirrorGraphicsSettings]; // Default to game default if anything unexpected happens
            switch (state)
            {
                case MirrorState.Off:
                    preset = MirrorGraphicsPreset.presets[0];
                    break;
                case MirrorState.Low:
                    preset = MirrorGraphicsPreset.presets[1];
                    break;
                case MirrorState.Medium:
                    preset = MirrorGraphicsPreset.presets[2];
                    break;
                case MirrorState.High:
                    preset = MirrorGraphicsPreset.presets[3];
                    break;
                default:
                    break;
            }

            SetMirrorFromPreset(preset);
        }

        private void SetMirrorFromPreset(MirrorRendererGraphicsSettingsPresets.Preset preset)
        {
            _logger.Info($"Setting mirror to {preset.mirrorType}");
            MirrorRenderer.Init(preset.reflectLayers, preset.stereoTextureWidth, preset.stereoTextureHeight, preset.monoTextureWidth, preset.monoTextureHeight, preset.maxAntiAliasing, preset.enableBloomPrePassFog);
        }

        public void Dispose()
        {
            _config.OnChanged -= Config_OnChanged;
        }
    }
}
