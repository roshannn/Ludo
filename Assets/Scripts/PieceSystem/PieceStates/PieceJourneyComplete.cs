using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceJourneyComplete : IState<PieceController> {
    public void OnStateEnter(PieceController StateObject) {
        StateObject.currPieceState = PieceStates.PieceJourneyComplete;
    }

    public void OnStateExit(PieceController StateObject) {
        
    }
}
