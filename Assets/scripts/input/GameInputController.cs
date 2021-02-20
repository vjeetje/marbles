using UnityEngine;

public class GameInputController : MonoBehaviour {
    public Color defaultColor;
    public Color pieceDraggingColor;
    public float dirSensitivity;

    private static int layerMaskBoardSurfaceLayer = 1 << 8;
    private static int layerMaskPiecesLayer = 1 << 9;
    private bool isDragging = false;
    private bool isValidMove = false;
    private GameObject draggingObject;
    private Vector3 originalPosition, targetPosition;

    private UIEventManager uIEventManager;

    private void Awake() {
        uIEventManager = UIEventManager.Instance;
    }

    void Update() {
        if(Input.GetMouseButton(0)) {
            if(!isDragging) {
                if(draggingObject = GetMarbleFromMouseRaycast()) {
                    originalPosition = draggingObject.transform.position;
                    UpdateIsDragging(true);
                }
            } else {
                Vector2? surfacePoint = GetBoardSurfacePositionFromMouseRaycast();
                targetPosition = new Vector3(surfacePoint.Value.x, originalPosition.y, surfacePoint.Value.y);
                isValidMove = surfacePoint.HasValue && IsValidMove(draggingObject, surfacePoint.Value);
            }
        }
        if(Input.GetMouseButtonUp(0)) {
            if(isValidMove && isDragging) {
                RegisterMove();
            }
            UpdateIsDragging(false);
        }
    }

    private void UpdateIsDragging(bool isDragging) {
        this.isDragging = isDragging;
        if(draggingObject) {
            draggingObject.GetComponent<Renderer>().material.color = isDragging ? pieceDraggingColor : defaultColor;
        }
    }

    private void RegisterMove() {
        Vector3 dir = targetPosition - originalPosition;
        Vector2Int moveDir = new Vector2(dir.x, dir.z).RoundToUnitVector();
        Vector2Int pos = Vector2Int.RoundToInt(draggingObject.transform.position.XZ());
        uIEventManager.FireMovePiece(draggingObject, pos, moveDir);
    }

    private GameObject GetMarbleFromMouseRaycast() {
        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layerMaskPiecesLayer)) {
            return hitInfo.collider.gameObject;
        }
        return null;
    }

    private Vector2? GetBoardSurfacePositionFromMouseRaycast() {
        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layerMaskBoardSurfaceLayer)) {
            return hitInfo.point.XZ();
        }
        return null;
    }

    private bool IsValidMove(GameObject marble, Vector2 surfacePoint) {
        float dx = Mathf.Abs(surfacePoint.x - marble.transform.position.x);
        float dz = Mathf.Abs(surfacePoint.y - marble.transform.position.z);
        dx = Mathf.Max(dirSensitivity / 2, dx);
        dz = Mathf.Max(dirSensitivity / 2, dz);
        return dx > 2 * dz + Mathf.Epsilon || dz > 2 * dx + Mathf.Epsilon;
    }
}
