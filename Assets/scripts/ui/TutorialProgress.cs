using UnityEngine.UI;
using UnityEngine;

public class TutorialProgress : MonoBehaviour {

    public Slider tutorialProgressSlider;
    public Text progressText;

    private TutorialProblemManager tutorialProblemManager;

    void Awake() {
        tutorialProblemManager = TutorialProblemManager.Instance;
    }

    void Update() {
        tutorialProgressSlider.value = tutorialProblemManager.GetProgress();
        progressText.text = tutorialProblemManager.GetProgressAsText();
    }
}
