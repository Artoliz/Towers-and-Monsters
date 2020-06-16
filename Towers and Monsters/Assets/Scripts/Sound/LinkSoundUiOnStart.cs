using UnityEngine;
using UnityEngine.UI;

public class LinkSoundUIOnStart : MonoBehaviour
{
    private void Start()
    {
        var sliders = Resources.FindObjectsOfTypeAll<Slider>();
        foreach (var slider in sliders)
        {
            switch (slider.name)
            {
                case "MusicsVolumeToggle":
                    SoundsManager.Instance.musicsToggle = slider;
                    break;
                case "EffectsVolumeToggle":
                    SoundsManager.Instance.effectsToggle = slider;
                    break;
                case "MusicsVolumeSlider":
                    SoundsManager.Instance.musicsSlider = slider;
                    break;
                case "EffectsVolumeSlider":
                    SoundsManager.Instance.effectsSlider = slider;
                    break;
            }
        }
    }
}