using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class StateMachineBehaviourInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<StateMachineBehaviourService>().AsSingle();
    }
}
