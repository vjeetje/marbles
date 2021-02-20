using UnityEngine;
using System.Collections;

public class BoardAnimator : MonoBehaviour
{

    public float pieceSpeed;
    public float constructDelayFactor, constructDistance;

    private Game game;

    public void Awake()
    {
        game = GameDataManager.Instance.game;
    }

    public IEnumerator SlideIn()
    {
        float time = constructDistance / pieceSpeed;
        foreach (Vector2Int pos in game.board.PositionsIterator())
        {
            Vector3 from = pos.XYZ() - constructDistance * Vector2.one.XYZ();
            float delay = (pos - new Vector2(game.board.width, game.board.height)).magnitude * constructDelayFactor;
            iTween.MoveFrom(game.board.pieces[pos.y, pos.x], iTween.Hash("position", from, "time", time, "delay", delay, "easeType", iTween.EaseType.easeInOutQuart));
        }
        float maxDelay = new Vector2(game.board.width, game.board.height).magnitude * constructDelayFactor;
        yield return new WaitForSeconds(time + maxDelay);
    }
}
