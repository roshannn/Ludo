using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PieceInBase : IState<PieceController> {
    private PlayerPiecePoint playerPiecePoint;
    PieceController currStateObject;
    public void OnStateEnter(PieceController StateObject) {
        currStateObject = StateObject;
        StateObject.currPieceState = PieceStates.PieceInBase;
        playerPiecePoint = StateObject.playerBase.PlayerPiecePoints.First(x => x.currPieceController == null);
        playerPiecePoint.currPieceController = StateObject;
        StateObject.transform.position = playerPiecePoint.transform.position;
    }

    public void OnStateExit(PieceController StateObject) {
        playerPiecePoint.currPieceController = null;
    }
}
