using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class LoadSound : MonoBehaviour
{
    public AudioMixer audioMixer;

    private void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "Settings", "SoundSettings.json");

        if (File.Exists(path) && audioMixer)
        {
            SoundSettings settings = JsonUtility.FromJson<SoundSettings>(File.ReadAllText(path));

            SetVolume(settings.MusicIsOn, "MusicsVolume", settings.MusicDecibel);
            SetVolume(settings.EffectsIsOn, "EffectsVolume", settings.EffectsDecibel);
        }
    }

    private void SetVolume(bool isOn, string volumeName, float value)
    {
        if (isOn)
            audioMixer.SetFloat(volumeName, Mathf.Log10(value) * 20);
        else
            audioMixer.SetFloat(volumeName, -80);
    }
}