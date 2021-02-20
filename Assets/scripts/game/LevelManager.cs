using UnityEngine.UI;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public Text levelText;

    private GameEventManager gameEventManager;
    private Game game;

    void Awake()
    {
        gameEventManager = GameEventManager.Instance;
        game = GameDataManager.Instance.game;

        gameEventManager.OnGameStateChanged += OnGameStateChanged;
        gameEventManager.OnLevelFinished += OnLevelFinished;
    }

    void Start()
    {
        game.level = 1;
    }

    void Update () {
        levelText.text = "Level: " + game.level;
    }

    private void OnGameStateChanged(GameState gameState, bool reset) {
        gameObject.SetActive(gameState == GameState.Game);
    }

    private void OnLevelFinished(int level)
    {
        game.level++;
    }
}
