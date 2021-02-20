using UnityEngine.UI;
using UnityEngine;

public class TimeToSolveManager : MonoBehaviour {

    public Slider remainingTimeSlider;
    public Image fillSlider;
    public float timeStart, timeIncrease, timeMax;
    public float timeWarning;
    public Color sliderColorOk;
    public Color sliderColorWarn;

    private GameEventManager gameEventManager;
    private GameDataManager gameDataManager;
    private Game game;
    private AudioSource audioSource;
    private bool isTimeRunning;

    void Awake() {
        gameEventManager = GameEventManager.Instance;
        gameDataManager = GameDataManager.Instance;
        game = gameDataManager.game;
        audioSource = GetComponent<AudioSource>();
        isTimeRunning = false;

        gameEventManager.OnGameStateChanged += OnGameStateChanged;
        gameEventManager.OnLevelFinished += OnLevelFinished;
    }

    void Update() {
        remainingTimeSlider.value = game.timeRemaining / timeMax;

        if(!isTimeRunning) {
            return;
        }

        game.timeRemaining = Mathf.Clamp(game.timeRemaining - Time.deltaTime, 0, timeMax);
        if(game.timeRemaining == 0) {
            gameDataManager.SetGameState(GameState.GameOverScreen);
        }

        updateTimeWarningSound();
        updateTimeWarningColor();
    }

    void OnGameStateChanged(GameState gameState, bool reset) {
        isTimeRunning = gameState == GameState.Game;
        game.timeRemaining = timeStart;
        gameObject.SetActive(gameState == GameState.Game);
    }

    void OnLevelFinished(int level) {
        game.timeRemaining += timeIncrease;
    }

    void updateTimeWarningSound() {
        if(game.timeRemaining < timeWarning && !audioSource.isPlaying) {
            audioSource.Play();
        }

        if(game.timeRemaining > timeWarning && audioSource.isPlaying) {
            audioSource.Stop();
        }
    }

    void updateTimeWarningColor() {
        fillSlider.color = game.timeRemaining > timeWarning ? sliderColorOk : sliderColorWarn;
    }
}
