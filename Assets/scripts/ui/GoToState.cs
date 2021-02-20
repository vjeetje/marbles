using System;
using UnityEngine;
using UnityEngine.UI;

public class GoToState : MonoBehaviour {

    public Button button;
    public GameState gameState;
    public bool toggle;
    public GameState[] activeGameStates;

    private GameDataManager gameDataManager;
    private GameEventManager gameEventManager;
    private GameState previousGameState;

    void Awake() {
        gameDataManager = GameDataManager.Instance;
        gameEventManager = GameEventManager.Instance;

        gameEventManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newGameState, bool reset) {
        gameObject.SetActive(Array.IndexOf(activeGameStates, newGameState) > -1);
    }

    void Start() {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        GameState newGameState;
        if(toggle) {
            newGameState = gameDataManager.GetGameState() == GameState.MenuScreen ? previousGameState : GameState.MenuScreen;
            previousGameState = gameDataManager.GetGameState();
        } else {
            newGameState = gameState;
        }
        gameDataManager.SetGameState(newGameState);
    }
}
