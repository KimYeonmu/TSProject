using UnityEngine;

public class TextData : MonoBehaviour
{
    public MeshRenderer TextRenderer;
    public TextMesh Text;

    public void Awake()
    {
        TextRenderer = GetComponent<MeshRenderer>();
        TextRenderer.sortingLayerName = "Card";

        Text = GetComponent<TextMesh>();
    }
}