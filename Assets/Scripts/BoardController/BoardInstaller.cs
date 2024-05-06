using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class BoardInstaller : MonoInstaller {
    public BoardController boardController;
    public override void InstallBindings() {
        Container.Bind<IGeneratePath>().To<BoardController>().FromInstance(boardController).AsSingle();
        Container.DeclareSignal<TurnSignal.TurnComplete>().OptionalSubscriber();
        Container.DeclareSignal<TurnSignal.OnKill>().OptionalSubscriber();
        Container.DeclareSignal<TurnSignal.OnJourneyComplete>().OptionalSubscriber();
        Container.DeclareSignal<TurnSignal.SetDiceState>().OptionalSubscriber();
        Container.DeclareSignal<TurnSignal.RollDice>().OptionalSubscriber();
        Container.DeclareSignal<TurnSignal.RenderTurnSignal>().OptionalSubscriber();
    }
}
