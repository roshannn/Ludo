using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class DiceController : MonoBehaviour,IRollDice {
    [SerializeField] private List<Sprite> diceFaces;
    [SerializeField] private SpriteRenderer diceRenderer;

    public int currRoll;
    //sides of a dice
    private int sides = 6;
    //debug and check gameplay to ensure no bugs come
    [SerializeField]private bool debug;
    [SerializeField]private int debugRoll;


    public async UniTask<int> RollDice() {
        if (debug) {
            currRoll = debugRoll;
        } else {
            currRoll = UnityEngine.Random.Range(1, sides + 1);
        }
        Debug.Log($"You rolled a {currRoll}");
        await PlayAnimation(currRoll,10);
        return currRoll;
    }

    private async UniTask PlayAnimation(int currRoll,int iterations) {
        for(int i = 0; i < iterations; i++) {
            int rand = UnityEngine.Random.Range(0, sides);
            diceRenderer.sprite = diceFaces[rand];
            await UniTask.Delay(TimeSpan.FromMilliseconds(100));
        }
        diceRenderer.sprite = diceFaces[currRoll-1];
    }

    public int GetCurrentRoll() {
        return currRoll;
    }
}
