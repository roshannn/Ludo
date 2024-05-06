using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Tile : MonoBehaviour {
    //this is the type of tile and its behaviour
    public TileType tileType;
    //pieces that are currently on this tile are stored in thhis
    private List<PieceController> piecesOnTile;
    [Inject] SignalBus _signalBus;

    private void Awake() {
        piecesOnTile = new List<PieceController>();
    }

    //call this once a piece lands on this
    public void OnTileLanding(PieceController landingPiece) {
        if (tileType == TileType.Normal) {
            if (piecesOnTile.Count > 0) {
                bool killed = false;
                Action RemoveKilledPieces = null;
                foreach (var piece in piecesOnTile) {
                    if (ValidateKill(piece.currPlayerType, landingPiece.currPlayerType)) {
                        killed= true;
                        piece.OnKill();
                        RemoveKilledPieces += () => piecesOnTile.Remove(piece);
                    }
                }
                if(killed) {
                    RemoveKilledPieces?.Invoke();
                    _signalBus.Fire<TurnSignal.OnKill>();
                }
            }
        }
        if(tileType == TileType.Home) {
            _signalBus.Fire<TurnSignal.OnJourneyComplete>();
            landingPiece.pieceStateMachine.ChangeState(PieceStates.PieceJourneyComplete);
        }
        
        piecesOnTile.Add(landingPiece);
        _signalBus.Fire<TurnSignal.TurnComplete>();

    }
    //call this once a piece leaves a tile
    public void OnTileLeave(PieceController leavingPiece) {
        if(piecesOnTile.Contains(leavingPiece)) {
            piecesOnTile.Remove(leavingPiece);
        }
    }

    private bool ValidateKill(PlayerType currPlayerType, PlayerType landingPlayerType) {
        return currPlayerType != landingPlayerType;
    }
}
