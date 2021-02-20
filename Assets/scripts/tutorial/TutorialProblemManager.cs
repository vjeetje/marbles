using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TutorialProblemManager : Singleton<TutorialProblemManager> {

    public int currentProblem;
    public List<Problem> problems;

    private GameDataManager gameDataManager;

    void Awake() {
        gameDataManager = GameDataManager.Instance;
    }

    void Start() {
        problems = new List<Problem>();
        AddProblem(CreateProblem1());
        AddProblem(CreateProblem2());
        AddProblem(CreateProblem3());
        AddProblem(CreateProblem4());
        AddProblem(CreateProblem5());
    }

    public Problem GetNextProblem() {
        if(currentProblem < problems.Count) {
            return problems.ElementAt(currentProblem++);
        } else {
            gameDataManager.SetGameState(GameState.TutorialCompleteScreen);
            return Problem.EMPTY_PROBLEM;
        }
    }

    public float GetProgress() {
        return currentProblem * 1.0f / problems.Count;
    }

    public string GetProgressAsText() {
        return currentProblem + "/" + problems.Count;
    }

    public void Reset() {
        currentProblem = 0;
    }

    public bool isFinished() {
        return problems.Count == currentProblem;
    }

    private Problem CreateProblem1() {
        List<Vector2Int> positionOfPieces = new List<Vector2Int> {
            new Vector2Int(1, 3),
            new Vector2Int(5, 3)
        };
        List<Move> solution = new List<Move> {
            new Move(new Vector2Int(1,3), Vector2Int.right)
        };
        return CreateProblem(7, 7, positionOfPieces, solution);
    }

    private Problem CreateProblem2() {
        List<Vector2Int> positionOfPieces = new List<Vector2Int> {
            new Vector2Int(1, 2),
            new Vector2Int(3, 2),
            new Vector2Int(2, 5)
        };
        List<Move> solution = new List<Move> {
            new Move(new Vector2Int(1,2), Vector2Int.right),
            new Move(new Vector2Int(2,2), Vector2Int.up)
        };
        return CreateProblem(7, 7, positionOfPieces, solution);
    }

    private Problem CreateProblem3() {
        List<Vector2Int> positionOfPieces = new List<Vector2Int> {
            new Vector2Int(1, 3),
            new Vector2Int(3, 3),
            new Vector2Int(4, 3)
        };
        List<Move> solution = new List<Move> {
            new Move(new Vector2Int(3,3), Vector2Int.left),
            new Move(new Vector2Int(4,3), Vector2Int.left)
        };
        return CreateProblem(7, 7, positionOfPieces, solution);
    }

    private Problem CreateProblem4() {
        List<Vector2Int> positionOfPieces = new List<Vector2Int> {
            new Vector2Int(2, 2),
            new Vector2Int(2, 4),
            new Vector2Int(4, 2),
            new Vector2Int(4, 4),
        };
        List<Move> solution = new List<Move> {
            new Move(new Vector2Int(2,2), Vector2Int.right),
            new Move(new Vector2Int(4,2), Vector2Int.right),
            new Move(new Vector2Int(3,2), Vector2Int.up),
        };
        return CreateProblem(7, 7, positionOfPieces, solution);
    }

    private Problem CreateProblem5() {
        List<Vector2Int> positionOfPieces = new List<Vector2Int> {
            new Vector2Int(0, 2),
            new Vector2Int(2, 2),
            new Vector2Int(3, 2),
            new Vector2Int(4, 2),
            new Vector2Int(1, 4),
            new Vector2Int(2, 4),
            new Vector2Int(3, 4),
            new Vector2Int(6, 4),
        };
        List<Move> solution = new List<Move> {
            new Move(new Vector2Int(2,2), Vector2Int.left),
            new Move(new Vector2Int(3,2), Vector2Int.left),
            new Move(new Vector2Int(4,2), Vector2Int.left),
            new Move(new Vector2Int(3,4), Vector2Int.right),
            new Move(new Vector2Int(2,4), Vector2Int.right),
            new Move(new Vector2Int(1,4), Vector2Int.right),
            new Move(new Vector2Int(3,4), Vector2Int.up),
        };
        return CreateProblem(7, 7, positionOfPieces, solution);
    }

    private Problem CreateProblem(int width, int height, List<Vector2Int> positionOfPieces, List<Move> solution) {
        Problem problem = new Problem(width, height);
        foreach(Vector2Int pos in positionOfPieces) {
            problem.AddPiece(pos);
        }
        problem.solution.AddRange(solution);
        return problem;
    }

    private void AddProblem(Problem problem) {
        problems.Add(problem);
    }
}
