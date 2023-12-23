﻿using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.GameplaySetup;
#if LATEST
using BeatSaberMarkupLanguage.Util;
#else
using BeatSaberMarkupLanguage.Components;
#endif
using SiraUtil.Logging;
using System.Linq;
using UnityEngine;
using Zenject;

namespace QuickMirrorToggle
{
    internal class QMTUI : NotifiableSingleton<QMTUI>, IInitializable
    {
        private const string QMT_TOGGLE = "" +
            "<bg id='qmt-root'>" +
            "<horizontal anchor-pos-x='-80' anchor-pos-y='65' preferred-width='40' horizontal-fit='PreferredSize' ignore-layout='true'>" +
            "<toggle-setting id='qmt-toggle' hover-hint='His name is Teddy ;3' text='~qmt-toggle-text' size-delta-x='1' size-delta-y='1' bind-value='true' apply-on-change='true' value='qmt-toggle-value'/>" +
            "</horizontal>" +
            "</bg>";
        [UIValue("qmt-mirror-setting-options")]
        private string[] _mirrorSettings = new string[] { "Low", "Medium", "High" };
        [Inject] private readonly QMTConfig _config;
        [Inject] private readonly SiraLog _logger;
        private string _qmtText = "Disable Mirror";
        private bool _qmtToggleValue = false;

        [UIComponent("qmt-root")]
        private RectTransform _qmtRectTransform;

        [UIValue("qmt-toggle-text")]
        public string QMTText
        {
            get => _qmtText;
            set
            {
                _qmtText = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("qmt-toggle-value")]
        public bool QMTToggleValue
        {
            get => _qmtToggleValue;
            set
            {
                _logger.Info("QMTToggleValue set to " + value);
                _qmtToggleValue = value;
                NotifyPropertyChanged();
                if (_config.GameMirrorSetting == MirrorState.Off)
                {
                    _config.MirrorState = value ? _config.QMTMirrorSetting : MirrorState.Off;
                }
                else
                {
                    _config.MirrorState = value ? MirrorState.Off : _config.GameMirrorSetting;
                }
                _config.Changed();
            }
        }

        [UIValue("qmt-mirror-setting-value")]
        public string SetValue
        {
            get => _config.QMTMirrorSetting.ToString();
            set
            {
                _config.QMTMirrorSetting = (MirrorState)System.Enum.Parse(typeof(MirrorState), value);
                if (_config.GameMirrorSetting == MirrorState.Off && _config.MirrorState != MirrorState.Off)
                {
                    _config.MirrorState = _config.QMTMirrorSetting;
                }
                _config.Changed();
            }
        }

        public void Initialize()
        {
            GameplaySetup.instance.AddTab("Quick Mirror Toggle", "QuickMirrorToggle.SettingsUI.bsml", this);
            BSMLParser.instance.Parse(QMT_TOGGLE, Resources.FindObjectsOfTypeAll<LevelSelectionNavigationController>().First().gameObject, this);
            _qmtRectTransform.localScale *= 0.6f;
            QMTText = _config.GameMirrorSetting == MirrorState.Off ? "Enable Mirror" : "Disable Mirror";
            _config.OnChanged += Config_OnChanged;
        }

        private void Config_OnChanged(QMTConfig newConfig)
        {
            switch (newConfig.GameMirrorSetting)
            {
                case MirrorState.Off:
                    QMTText = "Enable Mirror";
                    break;
                case MirrorState.Low:
                case MirrorState.Medium:
                case MirrorState.High:
                    QMTText = "Disable Mirror";
                    break;
                default:
                    break;
            }
        }
    }
}
