using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class UIController : MonoBehaviour
{
    //shows whose turn it is currently
    [SerializeField] private TMP_Text currTurnText;
    //button to roll dice
    [SerializeField] private Button RollTheDice;

    //Mapping of color to player
    private Dictionary<PlayerType, TurnViewData> PlayerTypeDataMap;

    //to replay game
    [SerializeField] private Button ReplayGame;
    [SerializeField] private Image Overlay;

    //inject dependancies
    [Inject] private SignalBus _signalBus;
    private void OnEnable() {
        ReplayGame.onClick.RemoveAllListeners();
        ReplayGame.onClick.AddListener(ReplayGameDelegate);

        _signalBus.Subscribe<TurnSignal.RenderTurnSignal>(RenderTurnViewer);
        _signalBus.Subscribe<TurnSignal.SetDiceState>(SetDiceState);
        _signalBus.Subscribe<GameplaySignals.GameComplete>(OnGameComplete);
        RollTheDice.onClick.RemoveAllListeners();
        RollTheDice.onClick.AddListener(delegate {
            _signalBus.Fire<TurnSignal.RollDice>();
        });
    }

    private void OnGameComplete() {
        Overlay.gameObject.SetActive(true);
    }

    private void ReplayGameDelegate() {
        // Get the active scene's name
        string sceneName = SceneManager.GetActiveScene().name;

        // Reload the active scene
        SceneManager.LoadScene(sceneName);
    }
    private void SetDiceState(TurnSignal.SetDiceState signal) {
        RollTheDice.interactable = signal.state;
    }

    private void Awake() {
        SetupPlayerTypeDataMapping();
    }

    private void SetupPlayerTypeDataMapping() {
        PlayerTypeDataMap = new Dictionary<PlayerType, TurnViewData>();
        PlayerTypeDataMap.Add(PlayerType.Player1, new TurnViewData { color = Color.yellow, text = "YELLOW" });
        PlayerTypeDataMap.Add(PlayerType.Player2, new TurnViewData { color = Color.red, text = "RED" });
        PlayerTypeDataMap.Add(PlayerType.Player3, new TurnViewData { color = Color.green, text = "GREEN" });
        PlayerTypeDataMap.Add(PlayerType.Player4, new TurnViewData { color = Color.blue, text = "BLUE" });
    }

    private void RenderTurnViewer(TurnSignal.RenderTurnSignal signalData) {
        PlayerType playerType = signalData.playerType;
        if (currTurnText != null) {
            TurnViewData currData = PlayerTypeDataMap[playerType];
            currTurnText.color = currData.color;
            currTurnText.text = currData.text;
        }
    }
    private void OnDisable() {
        _signalBus.TryUnsubscribe<TurnSignal.RenderTurnSignal>(RenderTurnViewer);
        _signalBus.TryUnsubscribe<TurnSignal.SetDiceState>(SetDiceState);
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            OnGameComplete();
        }
    }
}


public class TurnViewData {
    public Color color;
    public string text;
}
