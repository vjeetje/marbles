using UnityEngine;

public class PieceBounceDropSound : MonoBehaviour {

    public float heightOnBoard;

    private bool isOnBoard;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if(gameObject.transform.position.y <= heightOnBoard && !isOnBoard) {
            isOnBoard = true;
            audioSource.Play();
        }

        if(gameObject.transform.position.y > heightOnBoard) {
            isOnBoard = false;
        }
    }
}
