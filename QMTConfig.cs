using IPA.Config.Stores.Attributes;
using System;

namespace QuickMirrorToggle
{
    public class QMTConfig
    {
        [UseConverter(typeof(StringEnumConverter<MirrorState>))]
        public virtual MirrorState MirrorState { get; set; } = MirrorState.Off;
        [UseConverter(typeof(StringEnumConverter<MirrorState>))]
        public virtual MirrorState QMTMirrorSetting { get; set; } = MirrorState.High;
        [UseConverter(typeof(StringEnumConverter<MirrorState>))]
        public virtual MirrorState GameMirrorSetting { get; set; } = MirrorState.Medium;

        public event Action<QMTConfig> OnChanged;
        public void Changed()
        {
            OnChanged?.Invoke(this);
        }
    }
}
