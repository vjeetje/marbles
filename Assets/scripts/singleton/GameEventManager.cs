using UnityEngine;

public class GameEventManager : Singleton<GameEventManager> {
    protected GameEventManager() { }

    public delegate void GameStateChangedHandler(GameState gameState, bool reset);
    public event GameStateChangedHandler OnGameStateChanged;

    public delegate void PieceAddedHandler(GameObject piece, Vector2Int pos);
    public event PieceAddedHandler OnPieceAdded;

    public delegate void PiecePushedHandler(GameObject piece, Vector2Int dir);
    public event PiecePushedHandler OnPiecePushed;

    public delegate void PieceRemovedHandler(GameObject piece, Vector2Int pos);
    public event PieceRemovedHandler OnPieceRemoved;

    public delegate void LevelFinishedHandler(int level);
    public event LevelFinishedHandler OnLevelFinished;

    public void FireGameStateChanged(GameState gameState, bool reset) {
        if(OnGameStateChanged != null) {
            OnGameStateChanged(gameState, reset);
        }
    }

    public void FirePieceAdded(GameObject piece, Vector2Int pos) {
        if(OnPieceAdded != null) {
            OnPieceAdded(piece, pos);
        }
    }

    public void FirePiecePushed(GameObject piece, Vector2Int dir) {
        if(OnPiecePushed != null) {
            OnPiecePushed(piece, dir);
        }
    }

    public void FirePieceRemoved(GameObject piece, Vector2Int pos) {
        if(OnPieceRemoved != null) {
            OnPieceRemoved(piece, pos);
        }
    }

    public void FireLevelFinished(int level) {
        if(OnLevelFinished != null) {
            OnLevelFinished(level);
        }
    }
}
