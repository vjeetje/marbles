using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureOffsetDisplacer : MonoBehaviour
{

    public float offsetXSpeed;
    public float offsetYSpeed;

    public bool randomise = false;

    private Renderer rend;
    private string textureName = "_MainTex";

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        if (randomise)
        {
            SetTextureOffset(new Vector2(Random.value, Random.value));
        }
    }

    void Update()
    {
        IncreaseTextureOffset(GetDeltaOffset());
    }

    private void IncreaseTextureOffset(Vector2 offset)
    {
        SetTextureOffset(rend.material.GetTextureOffset(textureName) + offset);
    }

    private void SetTextureOffset(Vector2 offset)
    {
        rend.material.SetTextureOffset(textureName, offset);
    }

    private Vector2 GetDeltaOffset()
    {
        return new Vector2(offsetXSpeed * Time.deltaTime * Time.timeScale, offsetYSpeed * Time.deltaTime * Time.timeScale);
    }
}
