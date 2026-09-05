using UnityEngine;
using System.Collections;

public class VTuberController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer faceMeshRenderer;
    [SerializeField] private string joyBlendShapeName = "Joy";
    [SerializeField] private string angryBlendShapeName = "Angry";

    private int joyIndex = -1;
    private int angryIndex = -1;
    private Coroutine currentExpressionCoroutine;

   private void Awake()
{
    if (faceMeshRenderer == null || faceMeshRenderer.sharedMesh == null)
    {
        Debug.LogError("[VTuberController] FaceMeshRenderer がセットされていません");
        return;
    }

    //メッシュに入っている BlendShape 名を Console に表示
    int count = faceMeshRenderer.sharedMesh.blendShapeCount;
    Debug.Log($"--- [VTuberController] BlendShape 総数: {count} ---");
    for (int i = 0; i < count; i++)
    {
        string shapeName = faceMeshRenderer.sharedMesh.GetBlendShapeName(i);
        Debug.Log($"Index [{i}]: {shapeName}");
    }

    
    joyIndex = faceMeshRenderer.sharedMesh.GetBlendShapeIndex(joyBlendShapeName);
    angryIndex = faceMeshRenderer.sharedMesh.GetBlendShapeIndex(angryBlendShapeName);
}

    public void OnClickJoy() => SetExpression(joyIndex);
    public void OnClickAngry() => SetExpression(angryIndex);
    public void OnClickReset() => ResetAllExpressions();

    private void SetExpression(int targetIndex)
    {
        if (targetIndex == -1) return;
        if (currentExpressionCoroutine != null) StopCoroutine(currentExpressionCoroutine);
        currentExpressionCoroutine = StartCoroutine(SmoothExpressionRoutine(targetIndex, 100f, 0.25f));
    }

    private void ResetAllExpressions()
    {
        if (currentExpressionCoroutine != null) StopCoroutine(currentExpressionCoroutine);
        for (int i = 0; i < faceMeshRenderer.sharedMesh.blendShapeCount; i++)
        {
            faceMeshRenderer.SetBlendShapeWeight(i, 0f);
        }
    }

    private IEnumerator SmoothExpressionRoutine(int targetIndex, float targetWeight, float duration)
    {
        float startWeight = faceMeshRenderer.GetBlendShapeWeight(targetIndex);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float current = Mathf.Lerp(startWeight, targetWeight, elapsed / duration);
            faceMeshRenderer.SetBlendShapeWeight(targetIndex, current);
            yield return null;
        }

        faceMeshRenderer.SetBlendShapeWeight(targetIndex, targetWeight);
    }
}
