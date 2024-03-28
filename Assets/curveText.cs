using UnityEngine;
using TMPro;

public class curveText : MonoBehaviour
{
    public AnimationCurve curve;
    public float curveMultiplier = 1f;
    private TMP_Text textMesh;

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        UpdateText();
    }

    void UpdateText()
    {
        string originalText = textMesh.text;
        string curvedText = "";
        for (int i = 0; i < originalText.Length; i++)
        {
            curvedText += originalText[i] + "\n";
        }
        textMesh.text = curvedText;
    }
}
