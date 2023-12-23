﻿using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace QuickMirrorToggle
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    [NoEnableDisable]
    public class Plugin
    {
        private readonly QMTConfig _config;
        [Init]
        public Plugin(IPALogger logger, Config config, Zenjector zenject)
        {
            zenject.UseLogger(logger);
            _config = config.Generated<QMTConfig>();
            zenject.Install(Location.App, Container =>
            {
                Container.BindInstance(_config).AsSingle();
                Container.QueueForInject(_config);
            });

            zenject.Install(Location.Menu, Container =>
            {
                Container.BindInterfacesAndSelfTo<QMTUI>().AsSingle();
                Container.BindInterfacesAndSelfTo<MirrorManager>().AsSingle();
            });
        }
    }

    public enum MirrorState
    {
        Off,
        Low,
        Medium,
        High
    }
}
