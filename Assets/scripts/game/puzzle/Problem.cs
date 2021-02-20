using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Problem {
    public static readonly Vector2Int[] DIRECTIONS = { Vector2Int.down, Vector2Int.right, Vector2Int.up, Vector2Int.left };
    public static readonly Problem EMPTY_PROBLEM = new Problem(7, 7);

    public int width;
    public int height;
    public bool[,] pieces;
    public List<Move> solution;

    public Problem(int width, int height) {
        this.width = width;
        this.height = height;
        pieces = new bool[height, width];
        solution = new List<Move>();
    }

    public Problem(Problem problem) {
        width = problem.width;
        height = problem.height;
        pieces = new bool[height, width];
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                pieces[y, x] = problem.pieces[y, x];
            }
        }
        solution = new List<Move>(problem.solution);
    }

    public bool HasValidMoves() {
        foreach(Vector2Int pos in PositionsIterator()) {
            if(!HasPiece(pos)) {
                continue;
            }
            foreach(Vector2Int dir in DIRECTIONS) {
                if(HitsOtherPiece(pos, dir) && !IsKissing(pos, dir)) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsValidPosition(Vector2Int pos) {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    public bool IsSolved() {
        return GetNumberOfPieces() == 1;
    }

    public int GetNumberOfPieces() {
        return pieces.Cast<bool>().Where(v => v == true).Count();
    }

    public bool IsKissing(Vector2Int pos, Vector2Int direction) {
        return IsValidPosition(pos + direction) && HasPiece(pos + direction);
    }

    public IEnumerable<Movement> PushPiece(Vector2Int posOriginal, Vector2Int dir) {
        Vector2Int posFrom = posOriginal;
        while(IsValidPosition(posFrom)) {
            if(HasPiece(posFrom)) {
                Vector2Int posTo = GetPositionAfterMovingPiece(posFrom, dir);
                yield return new Movement(posFrom, posTo);

                posFrom = posTo;
            }
            posFrom += dir;
        }
    }

    public Vector2Int GetPositionAfterMovingPiece(Vector2Int pos, Vector2Int dir) {
        Vector2Int p = pos + dir;
        while(IsValidPosition(p)) {
            if(HasPiece(p)) {
                return p - dir;
            }
            p += dir;
        }
        return p;
    }

    public bool HitsOtherPiece(Vector2Int pos, Vector2Int direction) {
        return IsValidPosition(GetPositionAfterMovingPiece(pos, direction));
    }

    public bool HasPiece(Vector2Int pos) {
        return IsValidPosition(pos) && pieces[pos.y, pos.x];
    }

    public void AddPiece(Vector2Int pos) {
        pieces[pos.y, pos.x] = true;
    }

    public void RemovePiece(Vector2Int pos) {
        pieces[pos.y, pos.x] = false;
    }

    public void TranslatePiece(Vector2Int posFrom, Vector2Int posTo) {
        pieces[posFrom.y, posFrom.x] = false;
        pieces[posTo.y, posTo.x] = true;
    }

    public IEnumerable PositionsIterator() {
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                yield return new Vector2Int(x, y);
            }
        }
    }
}
