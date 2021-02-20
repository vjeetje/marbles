using UnityEngine;

[System.Serializable]
public class Move
{

    public Vector2Int pos;
    public Vector2Int dir;

    public Move(Vector2Int pos, Vector2Int dir)
    {
        this.pos = pos;
        this.dir = dir;
    }
}
