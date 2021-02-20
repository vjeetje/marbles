using UnityEngine;

public static class VectorExtensions {

    public static Vector2 XZ(this Vector3 v) {
        return new Vector2(v.x, v.z);
    }

    public static Vector2Int XZ(this Vector3Int v) {
        return new Vector2Int(v.x, v.z);
    }

    public static Vector3Int XYZ(this Vector2Int v) {
        return new Vector3Int(v.x, 0, v.y);
    }

    public static Vector3 XYZ(this Vector2 v) {
        return new Vector3(v.x, 0, v.y);
    }

    public static Vector2Int RoundToUnitVector(this Vector2 v) {
        foreach(Vector2Int dir in Problem.DIRECTIONS) {
            if(Vector2.Angle(v, dir) <= 45) {
                return dir;
            }
        }
        return Vector2Int.zero;
    }

    public static Vector2 perpendicular(this Vector2 v) {
        return new Vector2(v.y, v.x);
    }
}
