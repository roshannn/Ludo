using Zenject;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Net.NetworkInformation;
using System;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;

public class PieceController : MonoBehaviour {
    //sprite renderer of piece
    public SpriteRenderer spriteRenderer;

    //statemachine used on the piece
    public StateMachine<PieceController, PieceStates> pieceStateMachine;
    //stores the curr state of the piece
    public PieceStates currPieceState;
    //Access the current state instance of the object
    public IState<PieceController> GetCurrentStateInstance => pieceStateMachine.getCurrentState;
    //reference to the player base that the piece belongs to
    public PlayerBaseController playerBase;
    //player the piece belongs to
    public PlayerType currPlayerType;

    //original color to restore after highlighting
    private Color originalColor;
    //original state to default to sorting layer
    private int origSortingLayer = 0;
    //change to true when we can play this piece
    public bool isValidMove = false;

    //used to cache and kill tween when highlighting is no longer needed
    private Tween loopTween;

    //Injections
    [Inject] private IRollDice _IRollDice;
    [Inject] private SignalBus _signalBus;
    private void Awake() {
        InitializeStateMachine();
    }


    private void Start() {
        InitializeCacheValues();
        //begin in on base State
        pieceStateMachine.ChangeState(PieceStates.PieceInBase);
    }
    private void InitializeStateMachine() {
        pieceStateMachine = StateMachine<PieceController, PieceStates>.InstantiateStateMachine(this);
    }

    private void InitializeCacheValues() {
        origSortingLayer = spriteRenderer.sortingOrder;
        originalColor = spriteRenderer.color;
    }

    public void OnPieceClicked() {
        Debug.Log("piece clicked");
        if(isValidMove) {
            if(currPieceState == PieceStates.PieceInBase) {
                pieceStateMachine.ChangeState(PieceStates.PieceOnBoard);
            } else if(currPieceState == PieceStates.PieceOnBoard) {
                (GetCurrentStateInstance as PieceOnBoard).MovePiece(_IRollDice.GetCurrentRoll());
            }

        }
    }

    public void HighlightPiece() {
        isValidMove = true;
        loopTween = spriteRenderer.DOColor(new Color(1, 0, 1), 0.2f).SetLoops(-1, LoopType.Yoyo);
    }

    public void ResetHighlight() {
        loopTween.Kill();
        spriteRenderer.color = originalColor;
    }

    public void SetPlayerType(PlayerType playerType) {
        currPlayerType = playerType;
    }
    public void SetPlayerBase(PlayerBaseController _playerBase) {
        playerBase = _playerBase;
    }

    public void OnKill() {
        Sequence seq = (GetCurrentStateInstance as PieceOnBoard).MovePieceOnKill();
        if(seq!= null) {
            seq.Play().OnComplete(() => {
                pieceStateMachine.ChangeState(PieceStates.PieceInBase);
            });
        }

    }
    public void SetSortingLayerAhead() {
        this.gameObject.SetActive(false);
        spriteRenderer.sortingOrder++;
        this.gameObject.SetActive(true);
    }

    internal void ResetSortingLayer() {
        spriteRenderer.sortingOrder = origSortingLayer;
    }
}
