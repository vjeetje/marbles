using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ProblemGenerator {

    private int width;
    private int height;

    public Problem GenerateProblem(int width, int height, int nbOfPieces) {
        this.width = width;
        this.height = height;
        Problem problem = new Problem(height, width);
        AddRandomPiece(problem);
        Move move;
        while(--nbOfPieces > 0 && (move = GetValidMove(problem)) != null) {
            AddMove(move, problem);
        }
        return problem;
    }

    private void AddRandomPiece(Problem problem) {
        int x = Random.Range(1, width - 1);
        int y = Random.Range(1, height - 1);
        problem.pieces[y, x] = true;
    }

    private Move GetValidMove(Problem problem) {
        List<Move> validMoves = new List<Move>();
        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                if(problem.pieces[y, x]) {
                    foreach(Vector2Int dir in Problem.DIRECTIONS) {
                        Move move = new Move(new Vector2Int(x, y), dir);
                        if(IsValidMove(problem, move)) {
                            validMoves.Add(move);
                        }
                    }
                }
            }
        }

        if(validMoves.Count > 0) {
            return validMoves.ElementAt(Random.Range(0, validMoves.Count));
        } else {
            return null;
        }
    }

    private bool IsValidMove(Problem problem, Move move) {
        bool pushOffBorder = true;

        // no pieces in direction dir
        Vector2Int pos = move.pos + move.dir;
        while(problem.IsValidPosition(pos)) {
            if(problem.pieces[pos.y, pos.x]) {
                return false;
            }
            pos += move.dir;
            pushOffBorder = false;
        }

        if(pushOffBorder) {
            return false;
        }

        // at least one piece in direction -dir
        pos = move.pos - move.dir;
        while(problem.IsValidPosition(pos)) {
            if(!problem.pieces[pos.y, pos.x]) {
                return true;
            }
            pos -= move.dir;
        }

        return false;
    }

    private void AddMove(Move move, Problem problem) {
        Vector2Int pos, prevPos;
        pos = move.pos + move.dir;
        problem.pieces[pos.y, pos.x] = true;

        // pick an open position in direction -dir as action position
        List<Vector2Int> openPositions = new List<Vector2Int>();
        pos = move.pos - move.dir;
        while(problem.IsValidPosition(pos)) {
            if(!problem.pieces[pos.y, pos.x]) {
                openPositions.Add(pos);
            }
            pos -= move.dir;
        }
        Vector2Int actionPos = openPositions.ElementAt(Random.Range(0, openPositions.Count));

        // make action applicable to puzzle
        prevPos = move.pos;
        pos = move.pos - move.dir;
        while(problem.IsValidPosition(pos) || pos == actionPos) {
            if(!problem.pieces[pos.y, pos.x]) {
                problem.pieces[prevPos.y, prevPos.x] = false;
                problem.pieces[pos.y, pos.x] = true;
            }
            prevPos = pos;
            pos -= move.dir;
        }

        problem.solution.Add(new Move(actionPos, move.dir));
    }
}
