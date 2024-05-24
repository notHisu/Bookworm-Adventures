using TMPro;
using UnityEngine;

public class CurveText : MonoBehaviour
{
    public AnimationCurve curve;
    public float curveMultiplier = 1f;
    private TMP_Text textMesh;

    private void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        StartCoroutine(Curve());
    }

    private System.Collections.IEnumerator Curve()
    {
        float elapsed = 0f;
        float duration = 1f; // You can adjust the duration as needed

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float curveValue = curve.Evaluate(elapsed / duration) * curveMultiplier;
            textMesh.characterSpacing = curveValue;
            yield return null;
        }
    }
}
