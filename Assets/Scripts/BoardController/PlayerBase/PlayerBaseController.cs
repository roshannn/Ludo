using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseController : MonoBehaviour
{
	//know which player the base belongs to 
	public PlayerType currPlayerType;
	//points at which the pieces spawn
	public List<PlayerPiecePoint> PlayerPiecePoints;

    public void SetPlayerType(PlayerType playerType) {
        currPlayerType = playerType;
    }

}
