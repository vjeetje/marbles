using UnityEngine;
using System.Collections;

public class PuzzleController : MonoBehaviour {
    private PuzzleAnimator puzzleAnimator;
    private PuzzleLifecycleManager puzzleLifecycleManager;
    private UIEventManager uIEventManager;
    private GameEventManager gameEventManager;
    private SoundManager soundManager;

    private Game game;
    private bool isAnimatingPiece;

    void Awake() {
        puzzleAnimator = GetComponent<PuzzleAnimator>();
        uIEventManager = UIEventManager.Instance;
        gameEventManager = GameEventManager.Instance;
        puzzleLifecycleManager = GetComponent<PuzzleLifecycleManager>();
        soundManager = SoundManager.Instance;

        game = GameDataManager.Instance.game;

        uIEventManager.OnMovePiece += OnMovePiece;
    }

    public IEnumerator ResetPuzzle() {
        game.puzzle = puzzleLifecycleManager.ResetPuzzle(game.puzzle);
        yield return puzzleAnimator.DropPiecesToBoard(game.puzzle.pieces);
    }

    public IEnumerator CreateFirstPuzzle() {
        puzzleLifecycleManager.DestroyPuzzle(game.puzzle);
        game.puzzle = puzzleLifecycleManager.GenerateFirstPuzzle(game.board.width, game.board.height);
        yield return puzzleAnimator.DropPiecesToBoard(game.puzzle.pieces);
    }

    public IEnumerator CreateNextPuzzle() {
        puzzleLifecycleManager.DestroyPuzzle(game.puzzle);
        game.puzzle = puzzleLifecycleManager.GenerateNextPuzzle(game.board.width, game.board.height);
        yield return puzzleAnimator.DropPiecesToBoard(game.puzzle.pieces);
    }

    private void OnMovePiece(GameObject piece, Vector2Int pos, Vector2Int dir) {
        StartCoroutine(MovePieceRoutine(pos, dir));
    }

    private IEnumerator MovePieceRoutine(Vector2Int pos, Vector2Int dir) {
        if(isAnimatingPiece) {
            yield break;
        }

        isAnimatingPiece = true;
        GameObject piece = game.puzzle.GetPiece(pos);
        if(game.puzzle.IsKissing(pos, dir)) {
            soundManager.Play("moveError");
            yield return puzzleAnimator.AnimateKissingError(piece, game.puzzle.GetChildPiece(pos + dir));
        } else if(!game.puzzle.HitsOtherPiece(pos, dir)) {
            Vector2 posTo = game.puzzle.GetPositionAfterMovingPiece(pos, dir);
            yield return puzzleAnimator.AnimateNoHitMovingPieceError(piece, pos, posTo);
            soundManager.Play("moveError");
        } else {
            yield return PushPiece(piece, pos, dir);
        }
        isAnimatingPiece = false;
    }

    public IEnumerator PushPiece(GameObject pieceOriginal, Vector2Int posOriginal, Vector2Int dir) {
        foreach(Movement movement in game.puzzle.PushPiece(posOriginal, dir)) {
            if(game.puzzle.IsValidPosition(movement.posEnd)) {
                yield return puzzleAnimator.MovePieceOnBoard(movement.go, movement.posStart, movement.posEnd);
                soundManager.Play("marbleHit");
            } else {
                yield return puzzleAnimator.MovePieceOffBoard(movement.go, movement.posStart, movement.posEnd);
                puzzleLifecycleManager.DestroyPiece(game.puzzle, movement.go, movement.posStart);
            }
        }
        gameEventManager.FirePiecePushed(pieceOriginal, dir);
    }
}
