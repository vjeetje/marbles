using System;
using UnityEngine;

public class ActiveOnGameState : MonoBehaviour {

    public GameState[] activeGameStates;

    private GameEventManager gameEventManager;

    void Awake() {
        gameEventManager = GameEventManager.Instance;

        gameEventManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newGameState, bool reset) {
        gameObject.SetActive(Array.IndexOf(activeGameStates, newGameState) > -1);
    }
}
