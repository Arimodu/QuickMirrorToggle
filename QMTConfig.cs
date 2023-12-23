using System;

namespace QuickMirrorToggle
{
    public class QMTConfig
    {
        public virtual MirrorState MirrorState { get; set; } = MirrorState.Off;
        public virtual MirrorState QMTMirrorSetting { get; set; } = MirrorState.High;
        public virtual MirrorState GameMirrorSetting { get; set; } = MirrorState.Medium;

        public event Action<QMTConfig> OnChanged;
        public void Changed()
        {
            OnChanged?.Invoke(this);
        }
    }
}
