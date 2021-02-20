using System.Collections;
using UnityEngine;

[System.Serializable]
public class Board
{

    public int width;
    public int height;
    public GameObject[,] pieces;

    public Board(int width, int height)
    {
        this.width = width;
        this.height = height;
        pieces = new GameObject[height, width];
    }

    public IEnumerable PositionsIterator()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                yield return new Vector2Int(x, y);
            }
        }
    }

    public void AddPiece(GameObject piece, Vector2Int pos)
    {
        pieces[pos.y, pos.x] = piece;
    }
}
