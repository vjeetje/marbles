using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Image))]
public class ColorChanger : MonoBehaviour {

    public Color colorFrom;
    public Color colorTo;
    public float time;

    private Image image;
    private float lerpTime;

    void Awake() {
        image = GetComponent<Image>();
    }
    void Update() {
        image.color = Color.Lerp(colorFrom, colorTo, Mathf.Clamp01(lerpTime / time));
        lerpTime += Time.deltaTime;
    }

    void OnEnable() {
        image.color = colorFrom;
        lerpTime = 0;
    }
}
