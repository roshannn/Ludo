using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PieceOnBoard : IState<PieceController> {

    private List<Tile> currPath;
    private Tile currTile;
    private int currPathIndex;
    [Inject] private IGeneratePath _generatePath;
    [Inject] private SignalBus _signalBus;
    private PieceController currStateObject;



    public void OnStateEnter(PieceController StateObject) {
        currStateObject = StateObject;
        StateObject.currPieceState = PieceStates.PieceOnBoard;
        InitializePiece(StateObject);
    }
    private void InitializePiece(PieceController StateObject) {
        currPath = _generatePath.GeneratePath(StateObject.currPlayerType);
        currTile = currPath[0];
        currTile.OnTileLanding(StateObject);
        currPathIndex = 0;
        StateObject.transform.position = currTile.transform.position;
    }


    //check if the piece can move the roll amount
    public bool ValidateMove(int roll) {
        return currPathIndex + roll < currPath.Count;
    }
    //move the piece to the location of roll value
    public void MovePiece(int roll) {
        // Calculate the new path index
        currTile.OnTileLeave(currStateObject);
        int newPathIndex = currPathIndex + roll;
        newPathIndex = Mathf.Clamp(newPathIndex, 0, currPath.Count - 1); // Ensure the index is within bounds

        // Create a DOTween sequence
        Sequence moveSequence = DOTween.Sequence();

        // Loop from the current index to the new path index
        for (int i = currPathIndex + 1; i <= newPathIndex; i++) {
            // Add a tween to the sequence for each step to the next tile
            moveSequence.Append(currStateObject.transform.DOMove(currPath[i].transform.position, 0.5f)); // Move to next tile with a half-second tween
        }

        // Handle completion
        moveSequence.OnComplete(() => {
            currPathIndex = newPathIndex; // Update the current path index
            currTile = currPath[currPathIndex];
            currTile.OnTileLanding(currStateObject);
            currStateObject.transform.position = currPath[currPathIndex].transform.position; // Ensure the position is exactly at the tile
        });
    }

    public Sequence MovePieceOnKill() {
        Sequence moveSequence = DOTween.Sequence();
        for(int i = currPathIndex - 1; i >= 0; i--) {
            moveSequence.Append(currStateObject.transform.DOMove(currPath[i].transform.position, 0.05f));
        }
        return moveSequence;
    }
    public void OnStateExit(PieceController StateObject) {

    }

}
