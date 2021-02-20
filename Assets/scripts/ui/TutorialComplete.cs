using UnityEngine;
using UnityEngine.UI;

public class TutorialComplete : MonoBehaviour {

    public Button StartGameButton;
    public Button QuitButton;

    private GameDataManager gameDataManager;
    private GameEventManager gameEventManager;

    void Awake() {
        StartGameButton.onClick.AddListener(StartGameOnClick);
        QuitButton.onClick.AddListener(QuitOnClick);

        gameDataManager = GameDataManager.Instance;
        gameEventManager = GameEventManager.Instance;

        gameEventManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState gameState, bool reset) {
        gameObject.SetActive(gameState == GameState.TutorialCompleteScreen);
    }

    private void StartGameOnClick() {
        gameDataManager.SetGameState(GameState.Game, true);
    }

    private void QuitOnClick() {
        Application.Quit();
    }
}
