using System.Linq;
using UnityEngine;

public class FirstMoveIndicator : MonoBehaviour {

    public GameObject indicator;
    public Color colorFrom;
    public Color colorTo;
    public float time;

    private new Renderer renderer;
    private GameDataManager gameDataManager;
    private Game game;
    private bool isAnimating;

    void Awake() {
        renderer = indicator.GetComponent<Renderer>();
        gameDataManager = GameDataManager.Instance;
        game = gameDataManager.game;
        isAnimating = false;
    }

    void Update() {
        renderer.enabled = isAnimating;
        bool animate = gameDataManager.GetGameState() == GameState.Tutorial && game.puzzle != null && !game.puzzle.HasMadeMoves();
        if(animate && !isAnimating) {
            Move move = game.puzzle.problem.solution.ElementAt(0);
            Vector3 posTo = game.puzzle.GetPositionAfterMovingPiece(move.pos, move.dir).XYZ();
            gameObject.transform.position = move.pos.XYZ();
            renderer.material.color = colorFrom;
            iTween.ColorTo(indicator, iTween.Hash("color", colorTo, "time", time, "easeType", iTween.EaseType.linear));
            iTween.MoveTo(gameObject, iTween.Hash("position", posTo, "time", time, "easeType", iTween.EaseType.easeInOutCubic,
                "oncomplete", "OnAnimationComplete"));
            isAnimating = true;
        }
    }

    private void OnAnimationComplete() {
        isAnimating = false;
    }
}
