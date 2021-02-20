using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    private BoardController boardController;
    private PuzzleController puzzleController;
    private GameEventManager gameEventManager;
    private GameDataManager gameDataManager;
    private UIEventManager uIEventManager;
    private SoundManager soundManager;

    private bool creatingBoard;
    private Game game;

    void Awake() {
        boardController = GetComponentInChildren<BoardController>();
        puzzleController = GetComponentInChildren<PuzzleController>();

        gameEventManager = GameEventManager.Instance;
        gameDataManager = GameDataManager.Instance;
        uIEventManager = UIEventManager.Instance;
        soundManager = SoundManager.Instance;

        game = gameDataManager.game;

        gameEventManager.OnGameStateChanged += OnGameStateChanged;
        gameEventManager.OnPiecePushed += OnPiecePushed;
        uIEventManager.OnResetPuzzle += OnResetPuzzle;
    }

    void Start() {
        StartCoroutine(CreateBoard());
        if(gameDataManager.playerProgress.tutorialCompleted) {
            gameDataManager.SetGameState(GameState.Game);
        } else {
            gameDataManager.SetGameState(GameState.Tutorial);
        }
    }

    private void OnGameStateChanged(GameState gameState, bool reset) {
        if((gameState == GameState.Game || gameState == GameState.Tutorial)
            && (reset || game.puzzle == null)) {
            StartCoroutine(CreatePuzzle());
        }
    }

    private void OnPiecePushed(GameObject piece, Vector2Int pos) {
        if(game.puzzle.IsSolved()) {
            soundManager.Play("PuzzleSolved");
            if(gameDataManager.GetGameState() == GameState.Game) {
                gameEventManager.FireLevelFinished(game.level);
                StartCoroutine(puzzleController.CreateNextPuzzle());
            } else if(gameDataManager.GetGameState() == GameState.Tutorial) {
                StartCoroutine(puzzleController.CreateNextPuzzle());
            }
        }
    }

    private void OnResetPuzzle() {
        StartCoroutine(puzzleController.ResetPuzzle());
    }

    IEnumerator CreateBoard() {
        creatingBoard = true;
        yield return boardController.CreateBoard(7, 7);
        creatingBoard = false;
    }

    IEnumerator CreatePuzzle() {
        while(creatingBoard) {
            yield return new WaitForSeconds(0.1f);
        }
        yield return puzzleController.CreateFirstPuzzle();
    }
}
