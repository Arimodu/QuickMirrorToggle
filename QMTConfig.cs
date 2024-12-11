using IPA.Config.Stores.Attributes;
using System;
using BeatSaber.Settings;

namespace QuickMirrorToggle
{
    public class QMTConfig
    {
        [UseConverter(typeof(StringEnumConverter<QualitySettings.MirrorQuality>))]
        public virtual QualitySettings.MirrorQuality MirrorState { get; set; } = QualitySettings.MirrorQuality.Off;
        [UseConverter(typeof(StringEnumConverter<QualitySettings.MirrorQuality>))]
        public virtual QualitySettings.MirrorQuality QMTMirrorSetting { get; set; } = QualitySettings.MirrorQuality.High;
        [UseConverter(typeof(StringEnumConverter<QualitySettings.MirrorQuality>))]
        public virtual QualitySettings.MirrorQuality GameMirrorSetting { get; set; } = QualitySettings.MirrorQuality.Medium;

        public event Action<QMTConfig> OnChanged;
        public void Changed()
        {
            OnChanged?.Invoke(this);
        }
    }
}
