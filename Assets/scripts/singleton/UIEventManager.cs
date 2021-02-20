using UnityEngine;

public class UIEventManager : Singleton<UIEventManager>
{

    protected UIEventManager() { }

    public delegate void MovePieceHandler(GameObject piece, Vector2Int pos, Vector2Int direction);
    public event MovePieceHandler OnMovePiece;

    public delegate void ResetPuzzleHandler();
    public event ResetPuzzleHandler OnResetPuzzle;

    public void FireMovePiece(GameObject piece, Vector2Int pos, Vector2Int direction)
    {
        if (OnMovePiece != null)
        {
            OnMovePiece(piece, pos, direction);
        }
    }

    public void FireResetPuzzle() {
        if(OnResetPuzzle != null) {
            OnResetPuzzle();
        }
    }
}
