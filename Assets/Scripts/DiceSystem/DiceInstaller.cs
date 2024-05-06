using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DiceInstaller : MonoInstaller
{
    [SerializeField] private DiceController DiceController;
    public override void InstallBindings() {
        Container.Bind<IRollDice>().To<DiceController>().FromInstance(DiceController).AsSingle();
    }
}
