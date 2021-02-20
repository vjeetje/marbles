using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button StartGameButton;
    public Button TutorialButton;
    public Button QuitButton;

    private GameDataManager gameDataManager;
    private GameEventManager gameEventManager;

    void Awake() {
        StartGameButton.onClick.AddListener(StartGameOnClick);
        TutorialButton.onClick.AddListener(TutorialOnClick);
        QuitButton.onClick.AddListener(QuitOnClick);

        gameDataManager = GameDataManager.Instance;
        gameEventManager = GameEventManager.Instance;

        gameEventManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void Update() {
        StartGameButton.interactable = gameDataManager.playerProgress.tutorialCompleted;
    }

    private void OnGameStateChanged(GameState gameState, bool reset) {
        gameObject.SetActive(gameState == GameState.MenuScreen);
    }

    private void StartGameOnClick() {
        gameDataManager.SetGameState(GameState.Game, true);
    }

    private void TutorialOnClick() {
        gameDataManager.SetGameState(GameState.Tutorial, true);
    }

    private void QuitOnClick() {
        Application.Quit();
    }
}
