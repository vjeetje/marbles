using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public Text levelText;
    public Text highestLevelText;

    public Button NewGameButton;
    public Button QuitButton;

    private GameDataManager gameDataManager;
    private GameEventManager gameEventManager;

    private void Awake() {
        NewGameButton.onClick.AddListener(NewGameOnClick);
        QuitButton.onClick.AddListener(QuitOnClick);

        gameDataManager = GameDataManager.Instance;
        gameEventManager = GameEventManager.Instance;

        gameEventManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void Update() {
        levelText.text = "Level: " + gameDataManager.game.level;
        highestLevelText.text = "Highest Level: " + gameDataManager.playerProgress.highScore;
    }

    private void OnGameStateChanged(GameState gameState, bool reset) {
        gameObject.SetActive(gameState == GameState.GameOverScreen);
    }

    private void NewGameOnClick() {
        gameDataManager.SetGameState(GameState.Game, true);
    }

    private void QuitOnClick() {
        Application.Quit();
    }
}
