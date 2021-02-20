using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Puzzle {

    public GameObject[,] pieces;
    public Problem problem;
    public Problem originalProblem;

    public Puzzle(Problem problem) {
        this.problem = new Problem(problem);
        originalProblem = new Problem(problem);
        pieces = new GameObject[problem.height, problem.width];
    }

    public bool IsSolved() {
        return problem.IsSolved();
    }

    public bool HasValidMoves() {
        return problem.HasValidMoves();
    }

    public bool HasMadeMoves() {
        return originalProblem.GetNumberOfPieces() > problem.GetNumberOfPieces();
    }

    public bool IsKissing(Vector2Int pos, Vector2Int dir) {
        return problem.IsKissing(pos, dir);
    }

    public bool IsValidPosition(Vector2Int pos) {
        return problem.IsValidPosition(pos);
    }

    public IEnumerable<Movement> PushPiece(Vector2Int posOriginal, Vector2Int dir) {
        foreach(Movement m in problem.PushPiece(posOriginal, dir)) {
            yield return new Movement(GetPiece(m.posStart), m.posStart, m.posEnd);
            if(IsValidPosition(m.posEnd)) {
                TranslatePiece(m.posStart, m.posEnd);
            } else {
                RemovePiece(m.posStart);
            }
        }
    }

    public Vector2Int GetPositionAfterMovingPiece(Vector2Int pos, Vector2Int dir) {
        return problem.GetPositionAfterMovingPiece(pos, dir);
    }

    public bool HitsOtherPiece(Vector2Int pos, Vector2Int dir) {
        return problem.HitsOtherPiece(pos, dir);
    }

    public void AddPiece(GameObject piece, Vector2Int pos) {
        pieces[pos.y, pos.x] = piece;
    }

    public GameObject GetPiece(Vector2Int pos) {
        return pieces[pos.y, pos.x];
    }

    public GameObject GetChildPiece(Vector2Int pos) {
        return GetPiece(pos).transform.GetChild(0).gameObject;
    }

    public bool HasPiece(Vector2Int pos) {
        return problem.HasPiece(pos);
    }

    public void RemovePiece(Vector2Int pos) {
        pieces[pos.y, pos.x] = null;
        problem.RemovePiece(pos);
    }

    public void TranslatePiece(Vector2Int posFrom, Vector2Int posTo) {
        if(posFrom == posTo) {
            return;
        }
        pieces[posTo.y, posTo.x] = GetPiece(posFrom);
        pieces[posFrom.y, posFrom.x] = null;
        problem.TranslatePiece(posFrom, posTo);
    }

    public IEnumerable PositionsIterator() {
        return problem.PositionsIterator();
    }
}
