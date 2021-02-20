using UnityEngine;

[System.Serializable]
public class Movement {
    public GameObject go;
    public Vector2Int posStart;
    public Vector2Int posEnd;

    public Movement(Vector2Int posStart, Vector2Int posEnd) {
        this.posStart = posStart;
        this.posEnd = posEnd;
    }

    public Movement(GameObject go, Vector2Int posStart, Vector2Int posEnd) : this(posStart, posEnd) {
        this.go = go;
    }
}
