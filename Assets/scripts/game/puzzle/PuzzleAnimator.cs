using UnityEngine;
using System.Collections;

public class PuzzleAnimator : MonoBehaviour {
    public float dropOffBoardAngle, dropOffBoardDistance;
    public float pieceSpeed;
    public float dropAirHeight, dropAirTime, dropAirTimeDelta;
    public float minimumAnimationTime;
    public Color defaultColor, errorColor;
    public float errorColorTransitionTime;

    public IEnumerator MovePieceOnBoard(GameObject piece, Vector2 from, Vector2 to) {
        float time = Mathf.Max(minimumAnimationTime, Vector2.Distance(from, to) / pieceSpeed);
        iTween.MoveTo(piece, iTween.Hash("position", to.XYZ(), "time", time, "easeType", iTween.EaseType.easeInOutCubic));
        
        yield return new WaitForSeconds(time);
    }

    public IEnumerator MovePieceOffBoard(GameObject piece, Vector2 from, Vector2 to) {
        yield return new WaitForSeconds(_MovePieceOffBoard(piece, from, to));
    }

    private float _MovePieceOffBoard(GameObject piece, Vector2 from, Vector2 to) {
        Vector2 dir = (to - from).normalized;
        Vector3 dirDrop = Vector3.Lerp(dir.XYZ(), Vector3.down, dropOffBoardAngle);
        Vector3 endPos = to.XYZ() + dropOffBoardDistance * dirDrop;
        float timeOfMove = Vector2.Distance(from, to) / pieceSpeed;
        float timeOfDrop = Vector3.Distance(to.XYZ(), endPos) / pieceSpeed;
        iTween.MoveTo(piece, iTween.Hash("position", to.XYZ(), "time", timeOfMove, "easeType", iTween.EaseType.linear));
        iTween.MoveTo(piece, iTween.Hash("position", endPos, "time", timeOfDrop, "delay", timeOfMove, "easeType", iTween.EaseType.linear));
        return timeOfMove + timeOfDrop;
    }

    public IEnumerator DropPiecesToBoard(GameObject[,] pieces) {
        foreach(GameObject piece in pieces) {
            if(piece) {
                Vector3 pos = piece.transform.position + dropAirHeight * Vector3.up;
                float time = dropAirTime + Random.Range(-dropAirTimeDelta / 2, dropAirTimeDelta / 2);
                iTween.MoveFrom(piece, iTween.Hash("position", pos, "time", time, "easeType", iTween.EaseType.easeOutBounce));
            }
        }
        yield return new WaitForSeconds(dropAirTime + dropAirTimeDelta / 2);
    }

    public IEnumerator AnimateKissingError(GameObject pieceFrom, GameObject pieceTo) {
        iTween.ColorTo(pieceFrom, iTween.Hash("color", errorColor, "time", errorColorTransitionTime, "easeType", iTween.EaseType.linear));
        iTween.ColorTo(pieceFrom, iTween.Hash("color", defaultColor, "time", errorColorTransitionTime, "delay", errorColorTransitionTime, "easeType", iTween.EaseType.linear));
        iTween.ColorTo(pieceTo, iTween.Hash("color", errorColor, "time", errorColorTransitionTime, "easeType", iTween.EaseType.linear));
        iTween.ColorTo(pieceTo, iTween.Hash("color", defaultColor, "time", errorColorTransitionTime, "delay", errorColorTransitionTime, "easeType", iTween.EaseType.linear));
        yield return new WaitForSeconds(errorColorTransitionTime * 2);
    }

    public IEnumerator AnimateNoHitMovingPieceError(GameObject piece, Vector2 from, Vector2 to) {
        float movePieceOffBoardTime = _MovePieceOffBoard(piece, from, to);
        iTween.MoveTo(piece, iTween.Hash("position", from.XYZ(), "time", 0, "delay", movePieceOffBoardTime, "easeType", iTween.EaseType.linear));
        yield return new WaitForSeconds(movePieceOffBoardTime);
    }
}
