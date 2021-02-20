using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResetButton : MonoBehaviour, IPointerClickHandler {

    public Image image;
    public float blinkTime;
    public float blinkStartDelay;
    public Color defaultColor;
    public Color blinkColor;

    private UIEventManager uIEventManager;
    private GameEventManager gameEventManager;
    private AudioSource audioSource;
    private Game game;
    private bool startBlinking;
    private bool isBlinking;
    private float lastBlinkColorChange;
    private float startTimeBlinking;

    void Awake() {
        uIEventManager = UIEventManager.Instance;
        gameEventManager = GameEventManager.Instance;
        audioSource = GetComponent<AudioSource>();
        game = GameDataManager.Instance.game;
        lastBlinkColorChange = 0;
        startBlinking = false;
        isBlinking = false;

        gameEventManager.OnGameStateChanged += OnGameStateChanged;
    }

    void Update() {
        bool hasValidMoves = game.puzzle != null && game.puzzle.HasValidMoves();
        if(hasValidMoves) {
            startBlinking = false;
        } else if(!startBlinking && !isBlinking) {
            startBlinking = true;
            startTimeBlinking = 0;
        }

        isBlinking = startTimeBlinking > blinkStartDelay && !hasValidMoves;
        if(isBlinking) {
            if(lastBlinkColorChange > blinkTime) {
                image.color = image.color == defaultColor ? blinkColor : defaultColor;
                lastBlinkColorChange = 0;
            }
        } else {
            image.color = defaultColor;
        }

        lastBlinkColorChange += Time.deltaTime;
        startTimeBlinking += Time.deltaTime;
    }

    private void OnGameStateChanged(GameState gameState, bool reset) {
        gameObject.SetActive(gameState == GameState.Game || gameState == GameState.Tutorial);
    }

    public void OnPointerClick(PointerEventData eventData) {
        uIEventManager.FireResetPuzzle();
        audioSource.Play();
    }
}
