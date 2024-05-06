using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using Zenject;

public class PlayerContoller : MonoBehaviour {
    //Pieces of this Player
    [SerializeField] private List<PieceController> pieces;

    //set this to player 1,2,3 or 4
    public PlayerType playerType;
    //this is the reference to the player base
    [SerializeField] private PlayerBaseController playerBase;

    //set this once the players game is complete
    public bool gameComplete;

    //Injecting Signal Bus
    [Inject] SignalBus _signalBus;

    private int journeysCompleted = 0;
    private void Awake() {
        InitializePiecesAndBase();
    }

    //set pieces and base dependancies
    private void InitializePiecesAndBase() {
        playerBase.SetPlayerType(playerType);
        foreach(PieceController piece in pieces) {
            piece.SetPlayerType(playerType);
            piece.SetPlayerBase(playerBase);
        }
    }

    //check if piece can be moved
    public void CheckWhichPiecesCanBeMoved(int roll) {
        int countOfUnmovablePieces = 0;
        foreach(var piece in pieces) {
            if(piece.currPieceState == PieceStates.PieceInBase && roll == 6 ) {
                piece.HighlightPiece();
                piece.SetSortingLayerAhead();
            }
            else if(piece.currPieceState == PieceStates.PieceOnBoard && (piece.GetCurrentStateInstance as PieceOnBoard).ValidateMove(roll)) {
                piece.HighlightPiece();
                piece.SetSortingLayerAhead();
            } else {
                countOfUnmovablePieces++;
            }
        }
        //if unmovable pieces is same as pieces then we move ahead to next turn
        if(countOfUnmovablePieces == pieces.Count) {
            _signalBus.Fire<TurnSignal.TurnComplete>();
        }
    }

    //reset pieces once current turn is complete
    public void ResetPieces() {
        foreach(var piece in pieces) {
            piece.isValidMove = false;
            piece.ResetHighlight();
            piece.ResetSortingLayer();
        }
    }

    //once a piece finishes journey, increment journeycomplete counter and check whether game is over for the player
    internal void AddToJourneyComplete() {
        journeysCompleted++;
        if(journeysCompleted == pieces.Count) {
            gameComplete = true;
        }
    }
}
