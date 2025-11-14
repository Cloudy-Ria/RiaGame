using System.Runtime.CompilerServices;
using UnityEngine;

public class SceneBlackout : MonoBehaviour
{
    private UnityEngine.UI.Image blackoutImage;
    private float TargetAlpha;
    private float CurrentAlpha;
    public static float fadeSpeed = 4f;
    public static float immovableTime = 1/fadeSpeed;    
    

    private void Awake()
    {
        blackoutImage = GetComponent<UnityEngine.UI.Image>();
    }
    void Start()
    {
         
    }

    void Update()
    {
        if (TargetAlpha != CurrentAlpha)
        {
            CurrentAlpha = Mathf.Clamp(CurrentAlpha+Time.deltaTime * fadeSpeed * Mathf.Sign(TargetAlpha-CurrentAlpha), Mathf.Min(CurrentAlpha,TargetAlpha), Mathf.Max(CurrentAlpha, TargetAlpha));
            blackoutImage.color = new Color(0f, 0f, 0f, CurrentAlpha);
        }
    }

    public void FadeTo(float ratio)
    {
        TargetAlpha = ratio;
    }

    public void SetTo(float ratio)
    {
        CurrentAlpha = ratio;
        TargetAlpha = ratio;
        blackoutImage.color = new Color(0f, 0f, 0f, CurrentAlpha);
    }

}
