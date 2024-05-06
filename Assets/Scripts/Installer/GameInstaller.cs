using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller {
    public override void InstallBindings() {
        DiContainerAPI.Container = Container;
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<GameplaySignals.GameComplete>();
    }
}
