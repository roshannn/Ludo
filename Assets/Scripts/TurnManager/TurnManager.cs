using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class TurnManager : MonoBehaviour {
    //Players in the game
    [SerializeField] private List<PlayerContoller> players;
    //reference of the curr player
    private PlayerContoller currPlayer;

    //injecting dependancies
    [Inject] private SignalBus _signalBus;
    [Inject] private IRollDice _RollDice;

    //index of the player currently playing
    private int currPlayerIndex = 0;
    //sets this to check just in case the player rolls 6 more than 3 times his/her turn gets over;
    private int current6RollStreak = 0;
    //set to true when the current turn player kills and enemy to play again
    private bool onKill = false;
    //set to true when turn should repeat on journey complete
    private bool onJourneyComplete = false;
    //caching the currentRoll
    private int currRoll = 0;
    private void OnEnable() {
        //subscribing to signals
        _signalBus.Subscribe<TurnSignal.TurnComplete>(OnTurnComplete);
        _signalBus.Subscribe<TurnSignal.OnKill>(OnKill);
        _signalBus.Subscribe<TurnSignal.OnJourneyComplete>(OnJourneyComplete);
        _signalBus.Subscribe<TurnSignal.RollDice>(RollDice);
    }

    private void Start() {
        currPlayer = players[currPlayerIndex];
        _signalBus.Fire(new TurnSignal.RenderTurnSignal() { playerType = currPlayer.playerType });
    }
    private async void RollDice() {
        _signalBus.Fire(new TurnSignal.SetDiceState() { state = false });
        currRoll = await _RollDice.RollDice();
        currPlayer.CheckWhichPiecesCanBeMoved(currRoll);
    }
    //check whether game has ended
    private void CheckForGameCompletion() {
        int noOfCompletedPlayers = players.FindAll(x => x.gameComplete).Count;
        if(noOfCompletedPlayers > players.Count-1) {
            //GameOver;
            _signalBus.Fire<GameplaySignals.GameComplete>();
        }
    }
    //Set Kill as true and let curr User Play Again
    private void OnKill() {
        onKill = true;
    }
    //Set Journey Complete and check whether game is complete;
    private void OnJourneyComplete() {
        currPlayer.AddToJourneyComplete();
        onJourneyComplete = true;
        CheckForGameCompletion();
    }

    private async void OnTurnComplete() {
        Debug.Log("Turn Complete");
        currPlayer.ResetPieces();
        
        //check if player just killed another player and then continue next turn;
        if (onKill) {
            onKill = false;
            current6RollStreak= 0;
            //check if journey is complete and game isnt complete to play next turn
        } else if (onJourneyComplete && !currPlayer.gameComplete) {
            //continue turn
            current6RollStreak = 0;
            onJourneyComplete = false;
        } else if (currRoll == 6 && current6RollStreak < 2) {
            //continue turn
            current6RollStreak++;
        } else {
            //next players turn;
            current6RollStreak = 0;
            MoveToNextPlayer();
            _signalBus.Fire(new TurnSignal.RenderTurnSignal() { playerType = currPlayer.playerType });
        }
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        _signalBus.Fire(new TurnSignal.SetDiceState() { state = true });
    }

    //go to next player if current player turn is done
    private void MoveToNextPlayer() {
        currPlayerIndex++;
        currPlayerIndex %= players.Count;
        currPlayer = players[currPlayerIndex];
        //check if players turn is done then go to next player
        if (currPlayer.gameComplete) {
            MoveToNextPlayer();
        }
    }

    private void OnDisable() {
        _signalBus.TryUnsubscribe<TurnSignal.TurnComplete>(OnTurnComplete);
        _signalBus.TryUnsubscribe<TurnSignal.OnKill>(OnKill);
        _signalBus.TryUnsubscribe<TurnSignal.OnJourneyComplete>(OnJourneyComplete);
        _signalBus.TryUnsubscribe<TurnSignal.RollDice>(RollDice);
    }

}

public struct TurnSignal {
    //call this to roll the dice
    public struct RollDice { }
    //call this to set the dice state
    public struct SetDiceState { public bool state; }
    // call this once turn is complete
    public struct TurnComplete { }
    //call this once a piece kills another piece
    public struct OnKill { }
    //call this once the journey is complete
    public struct OnJourneyComplete { }
    //use this to render the ui on screen
    public struct RenderTurnSignal { public PlayerType playerType; }
}
