using UnityEngine;

public class PuzzleLifecycleManager : MonoBehaviour {
    public GameObject pieceTemplate;
    public int minNbPieces;
    public int maxNbPiecesStart;
    public int maxNbPiecesEnd;

    private ProblemGenerator problemGenerator;
    private GameEventManager gameEventManager;
    private TutorialProblemManager tutorialProblemManager;

    private int nbOfPiecesPreviousProblem;
    private int maxNbPieces;

    void Start() {
        problemGenerator = new ProblemGenerator();
        gameEventManager = GameEventManager.Instance;
        tutorialProblemManager = TutorialProblemManager.Instance;
        resetNbOfPiecesState();
    }

    void resetNbOfPiecesState() {
        minNbPieces = 4;
        maxNbPiecesStart = 7;
        maxNbPiecesEnd = 12;
        nbOfPiecesPreviousProblem = 0;
        maxNbPieces = maxNbPiecesStart;
    }

    public Puzzle GenerateFirstPuzzle(int width, int height) {
        resetNbOfPiecesState();
        tutorialProblemManager.Reset();
        return GenerateNextPuzzle(width, height);
    }

    public Puzzle GenerateNextPuzzle(int width, int height) {
        Problem problem;
        if(GameDataManager.Instance.GetGameState() == GameState.Game) {
            problem = GenerateNextProblem(width, height);
        } else {
            problem = tutorialProblemManager.GetNextProblem();
        }
        return CreatePuzzle(problem);
    }

    private Problem GenerateNextProblem(int width, int height) {
        nbOfPiecesPreviousProblem = nbOfPiecesPreviousProblem < minNbPieces ? minNbPieces : nbOfPiecesPreviousProblem + 1;
        if(nbOfPiecesPreviousProblem > maxNbPieces) {
            nbOfPiecesPreviousProblem = minNbPieces;
            maxNbPieces = ++maxNbPieces > maxNbPiecesEnd ? maxNbPiecesEnd : maxNbPieces;
        }
        return problemGenerator.GenerateProblem(width, height, nbOfPiecesPreviousProblem);
    }

    public Puzzle ResetPuzzle(Puzzle puzzle) {
        Puzzle originalPuzzle = CreatePuzzle(puzzle.originalProblem);
        DestroyPuzzle(puzzle);
        return originalPuzzle;
    }

    public void DestroyPuzzle(Puzzle puzzle) {
        if(puzzle == null) {
            return;
        }

        foreach(Vector2Int pos in puzzle.PositionsIterator()) {
            GameObject piece = puzzle.GetPiece(pos);
            if(piece) {
                DestroyPiece(puzzle, piece, pos);
            }
        }
    }

    private Puzzle CreatePuzzle(Problem problem) {
        Puzzle puzzle = new Puzzle(problem);
        foreach(Vector2Int pos in puzzle.PositionsIterator()) {
            if(problem.pieces[pos.y, pos.x]) {
                AddPiece(puzzle, pos);
            }
        }
        return puzzle;
    }

    private void AddPiece(Puzzle puzzle, Vector2Int pos) {
        GameObject piece = CreatePiece(pos);
        puzzle.AddPiece(piece, pos);
        gameEventManager.FirePieceAdded(piece, pos);
    }

    private GameObject CreatePiece(Vector2Int pos) {
        GameObject piece = Instantiate(pieceTemplate, pos.XYZ(), Quaternion.identity, transform);
        piece.name = "Marble(" + pos.x + ", " + pos.y + ")";
        return piece;
    }

    public void DestroyPiece(Puzzle puzzle, GameObject piece, Vector2Int pos) {
        Destroy(piece);
        puzzle.RemovePiece(pos);
        gameEventManager.FirePieceRemoved(piece, pos);
    }
}
