using UnityEngine;

public class GameDataManager : Singleton<GameDataManager> {
    public Game game = new Game();
    public PlayerProgress playerProgress;

    private GameEventManager gameEventManager;
    private GameState gameState;

    protected GameDataManager() { }

    void Awake() {
        gameEventManager = GameEventManager.Instance;
        //ResetPlayerPrefs();
        loadPlayerProgress();
    }

    void Update() {
        if(playerProgress.highScore < game.level) {
            playerProgress.highScore = game.level;
            SavePlayerProgress();
        }
    }

    private void ResetPlayerPrefs() {
        PlayerPrefs.DeleteKey("highScore");
        PlayerPrefs.DeleteKey("tutorialCompleted");
    }

    public GameState GetGameState() {
        return gameState;
    }

    public void SetGameState(GameState newGameState, bool reset = false) {
        gameEventManager.FireGameStateChanged(gameState = newGameState, reset);

        if(gameState == GameState.TutorialCompleteScreen) {
            playerProgress.tutorialCompleted = true;
            SavePlayerProgress();
        }
    }

    private void loadPlayerProgress() {
        playerProgress = new PlayerProgress();
        playerProgress.highScore = PlayerPrefs.GetInt("highScore", 1);
        playerProgress.tutorialCompleted = PlayerPrefs.GetInt("tutorialCompleted", 0) == 1 ? true : false;
    }

    public void SavePlayerProgress() {
        PlayerPrefs.SetInt("highScore", playerProgress.highScore);
        PlayerPrefs.SetInt("tutorialCompleted", playerProgress.tutorialCompleted ? 1 : 0);
    }
}
