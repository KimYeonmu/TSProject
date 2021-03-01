using UnityEngine;

/// <summary>카드에 들어가는 문자 데이터 클래스 </summary>
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