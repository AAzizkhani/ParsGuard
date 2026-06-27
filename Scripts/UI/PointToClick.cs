
using UnityEngine;

public class PointToClick : MonoBehaviour
{
    [SerializeField] private float MDuration = 1f;
    private float MTimer;
    void Start()
    {
    }

    void Update()
    {
        MTimer += Time.deltaTime;
        
        transform.localScale = Reapetly();//Squishy();//
        
        if (MTimer >= MDuration)
            Destroy(gameObject);
    }

    Vector3 Reapetly()
    {
        float progress = MTimer / MDuration;
        progress = Mathf.Clamp01(progress);
        
        float squish = 1f + Mathf.Sin(MTimer * Mathf.PI * 8f) * 0.2f;
        
        float fadeMultiplier = 1f - Mathf.Pow(Mathf.Max(0, (progress - 0.8f) / 0.2f), 2f);
        
        squish *= fadeMultiplier;
        
        return Vector3.one * squish;
    }

    Vector3 Squishy()
    {
        float progress = MTimer / MDuration;
        progress = Mathf.Clamp01(progress);
        
        float squishSpeed = 4f;
        float squishAmount = 0.3f;
        
        float xScale = 1f + Mathf.Sin(progress * Mathf.PI * squishSpeed) * squishAmount;
        
        float yScale = 1f + Mathf.Cos(progress * Mathf.PI * squishSpeed) * squishAmount;
        
        if (progress > 0.8f)
        {
            float shrinkProgress = (progress - 0.8f) / 0.2f;
            xScale *= (1f - shrinkProgress);
            yScale *= (1f - shrinkProgress);
        }
        
        return new Vector3(xScale, yScale, 1f);
    }
}
