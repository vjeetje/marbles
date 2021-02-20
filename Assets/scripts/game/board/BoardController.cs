using System.Collections;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public GameObject piece;

    private BoardAnimator boardAnimator;
    private Game game;

    public void Awake()
    {
        game = GameDataManager.Instance.game;
        boardAnimator = GetComponent<BoardAnimator>();
    }

    public IEnumerator CreateBoard(int width, int height)
    {
        game.board = new Board(width, height);
        foreach (Vector2Int pos in game.board.PositionsIterator())
        {
            game.board.AddPiece(CreatePiece(pos), pos);
        }
        yield return boardAnimator.SlideIn();
    }

    private GameObject CreatePiece(Vector2Int pos)
    {
        GameObject boardPiece = Instantiate(piece, pos.XYZ(), Quaternion.identity, this.transform);
        boardPiece.name = "Tile(" + pos.x + ", " + pos.y + ")";
        return boardPiece;
    }

    private IEnumerator DestroyBoard()
    {
        // todo add animation
        foreach (GameObject piece in game.board.pieces)
        {
            Destroy(piece);
        }
        yield return new WaitForSeconds(0);
    }
}
