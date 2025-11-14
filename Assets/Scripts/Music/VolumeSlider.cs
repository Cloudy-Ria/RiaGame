using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] AudioMixer mainAudioMixer;
    private GameState gameState;
    private void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
        if (gameState.masterVolume != 0f)
        {
            GetComponent<Slider>().value = gameState.masterVolume;
        }
    }
    public void SetVolume(float sliderValue)
    {
        gameState.masterVolume = sliderValue;
        mainAudioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue)*20);
    }
}
