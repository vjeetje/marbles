using UnityEngine;

public class DevInputController : MonoBehaviour {

    private PuzzleController puzzleController;

    void Start () {
        puzzleController = GetComponentInChildren<PuzzleController>();
	}
	
	void Update () {
        if (Input.GetKeyUp(KeyCode.R))
        {
            StartCoroutine(puzzleController.CreateNextPuzzle());
        }
    }
}
