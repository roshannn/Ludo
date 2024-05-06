using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardController : MonoBehaviour,IGeneratePath {
    //reference to the sectors
    [SerializeField] private SectorController sector1;
    [SerializeField] private SectorController sector2;
    [SerializeField] private SectorController sector3;
    [SerializeField] private SectorController sector4;

    //List to generate path from
    private List<SectorController> sectors;


    private void Awake() {
        sectors = new List<SectorController>() { sector1, sector2, sector3, sector4 };
    }

    //Get the path of the piece when it is spawned
    public List<Tile> GeneratePath(PlayerType player) {
        List<Tile> currentPiecePath = new List<Tile>();
        int startIndex = (int)player;
        //add starting path
        currentPiecePath.AddRange(sectors[startIndex].GetStartingPath);
        
        for(int i = (startIndex + 1)% sectors.Count; i!=startIndex;i++,i%=sectors.Count) {
            currentPiecePath.AddRange(sectors[i].sectorPath);
        }

        currentPiecePath.AddRange(sectors[startIndex].GetEndingPath);
        currentPiecePath.AddRange(sectors[startIndex].finalPath);
        return currentPiecePath;
    }
}
