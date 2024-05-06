using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds reference to sectors of the map and is used to generate paths for any piece
public class SectorController : MonoBehaviour
{
	//the move area of the sector
	public List<Tile> sectorPath;
	//the path which leads to home
	public List<Tile> finalPath;

	//starting index for a player that spawns in this sector
	private int startingIndex = 8;
	//last tile that the player is at before going onto final path
	private int endingIndex = 6;
	//return starting path for the piece that spawns in this sector
	public List<Tile> GetStartingPath => sectorPath.GetRange(startingIndex,sectorPath.Count - startingIndex);
	//return the path upto the ending point before piece goes onto the final path
	public List<Tile> GetEndingPath => sectorPath.GetRange(0, endingIndex + 1);
}
