using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    [SerializeField] Sprite fullHeart, emptyHeart;
    Image heartImage;
    void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Empty:
                if (emptyHeart != null)
                {
                    heartImage.sprite = emptyHeart;
                }
                break;
            case HeartStatus.Full:
                if (fullHeart != null)
                {
                    heartImage.sprite = fullHeart;
                }
                break;
        }
    }

    public enum HeartStatus
    {
        Empty = 0,
        Full = 1
    }
}
