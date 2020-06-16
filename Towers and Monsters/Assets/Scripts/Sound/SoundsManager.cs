using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundsManager : MonoBehaviour
{
    #region PublicVariables

    public static SoundsManager Instance = null;
    public SoundSettings soundSettings;

    [Header("Volume managers")]
    public AudioMixer audioMixer;

    public Slider musicsToggle;
    public Slider effectsToggle;

    public Slider musicsSlider;
    public Slider effectsSlider;

    [Header("Menus Sounds Effects")]
    public AudioClip MenuFirstScreenTransition = null;
    public AudioClip ButtonUiClick = null;
    public AudioClip ButtonUiBack = null;
    public AudioClip ButtonUiValidation = null;

    [Header("Towers / Walls Sounds Effects")]
    public AudioClip ConstructBuilding = null;

    public AudioClip DestructBuilding = null;
    public AudioClip RepairBuilding = null;
    public AudioClip UpgradeBuilding = null;
    public AudioClip LongRange = null;
    public AudioClip Effect = null;
    public AudioClip Bullet = null;

    [Header("Enemies Sounds Effects")]
    public AudioClip Death = null;
    public AudioClip SwordHit = null;
    public AudioClip CannonShot = null;
    public AudioClip Run = null;

    [Header("Game Sounds Effects")]
    public AudioClip WaveStart = null;
    public AudioClip EndGameLose = null;
    public AudioClip EndGameWin = null;

    #endregion

    #region PrivateVariables

    private AudioSource EffectsSource = null;
    private AudioSource EffectsSourceLoop = null;
    private AudioSource MusicsSource = null;

    #endregion

    public enum Audio
    {
        MenuFirstScreenTransition,
        ButtonUiClick,
        ButtonUiBack,
        ButtonUiValidation,
        Construct,
        Destruct,
        Repair,
        Upgrade,
        Death,
        SwordHit,
        CannonShot,
        LongRange,
        Effect,
        Bullet,
        WaveStart,
        EndGameLose,
        EndGameWin,
        Run
    }

    private void Awake()
    {
        MusicsSource = GetComponents<AudioSource>()[0];
        EffectsSource = GetComponents<AudioSource>()[1];
        EffectsSourceLoop = GetComponents<AudioSource>()[2];

        if (Instance != null)
        {
            Instance = null;
            Destroy(this.gameObject);
        } else
            Instance = this;
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            soundSettings = new SoundSettings();

            musicsSlider.onValueChanged.AddListener(delegate { OnVolumeMusicsChange(); });
            effectsSlider.onValueChanged.AddListener(delegate { OnVolumeEffectsChange(); });

            musicsToggle.onValueChanged.AddListener(delegate { OnMusicsVolumeToggle(); });
            effectsToggle.onValueChanged.AddListener(delegate { OnEffectsVolumeToggle(); });

            LoadSettings();
        }
    }

    public void SaveSettings()
    {
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Settings")))
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Settings"));

        soundSettings.MusicDecibel = musicsSlider.value;
        soundSettings.EffectsDecibel = effectsSlider.value;

        if (musicsToggle.value == 0)
            soundSettings.MusicIsOn = false;
        else
            soundSettings.MusicIsOn = true;
        if (effectsToggle.value == 0)
            soundSettings.EffectsIsOn = false;
        else
            soundSettings.EffectsIsOn = true;

        var path = Path.Combine(Application.persistentDataPath, "Settings", "SoundSettings.json");
        File.WriteAllText(path, JsonUtility.ToJson(soundSettings, true));
    }

    private void LoadSettings()
    {
        string path = Path.Combine(Application.persistentDataPath, "Settings", "SoundSettings.json");

        if (File.Exists(path))
        {
            soundSettings = JsonUtility.FromJson<SoundSettings>(File.ReadAllText(path));

            if (soundSettings.MusicIsOn)
                musicsToggle.value = 1;
            else
                musicsToggle.value = 0;
            musicsSlider.value = soundSettings.MusicDecibel;

            if (soundSettings.EffectsIsOn)
                effectsToggle.value = 1;
            else
                effectsToggle.value = 0;
            effectsSlider.value = soundSettings.EffectsDecibel;
        }
    }

    private void SetVolumeToggle(bool isOn, string volumeName, Slider slider)
    {
        if (isOn)
        {
            audioMixer.SetFloat(volumeName, Mathf.Log10(slider.value) * 20);
            slider.interactable = true;
        }
        else
        {
            audioMixer.SetFloat(volumeName, -80);
            slider.interactable = false;
        }
    }

    private void OnMusicsVolumeToggle()
    {
        bool isOn = musicsToggle.value == 1;

        soundSettings.MusicIsOn = isOn;
        SetVolumeToggle(isOn, "MusicsVolume", musicsSlider);
    }

    private void OnEffectsVolumeToggle()
    {
        bool isOn = effectsToggle.value == 1;

        soundSettings.EffectsIsOn = isOn;
        SetVolumeToggle(isOn, "EffectsVolume", effectsSlider);
    }

    public void OnVolumeMusicsChange()
    {
        soundSettings.MusicDecibel = musicsSlider.value;
        var value = Mathf.Log10(soundSettings.MusicDecibel) * 20;

        audioMixer.SetFloat("MusicsVolume", value);
    }

    public void OnVolumeEffectsChange()
    {
        soundSettings.EffectsDecibel = effectsSlider.value;
        var value = Mathf.Log10(soundSettings.EffectsDecibel) * 20;

        audioMixer.SetFloat("EffectsVolume", value);
    }

    public void PlaySound(Audio audio)
    {
        //Menus
        if (audio == Audio.MenuFirstScreenTransition)
            EffectsSource.PlayOneShot(MenuFirstScreenTransition);
        else if (audio == Audio.ButtonUiClick)
            EffectsSource.PlayOneShot(ButtonUiClick);
        else if (audio == Audio.ButtonUiBack)
            EffectsSource.PlayOneShot(ButtonUiBack);
        else if (audio == Audio.ButtonUiValidation)
            EffectsSource.PlayOneShot(ButtonUiValidation);
        //Towers / Walls
        else if (audio == Audio.Construct)
            EffectsSource.PlayOneShot(ConstructBuilding);
        else if (audio == Audio.Destruct)
            EffectsSource.PlayOneShot(DestructBuilding);
        else if (audio == Audio.Repair)
            EffectsSource.PlayOneShot(RepairBuilding);
        else if (audio == Audio.Upgrade)
            EffectsSource.PlayOneShot(UpgradeBuilding);
        else if (audio == Audio.LongRange)
            EffectsSource.PlayOneShot(LongRange);
        else if (audio == Audio.Effect)
            EffectsSource.PlayOneShot(Effect);
        else if (audio == Audio.Bullet)
            EffectsSource.PlayOneShot(Bullet);
        // Monsters
        else if (audio == Audio.Death)
            EffectsSource.PlayOneShot(Death);
        else if (audio == Audio.SwordHit)
            EffectsSource.PlayOneShot(SwordHit);
        else if (audio == Audio.CannonShot)
            EffectsSource.PlayOneShot(CannonShot);
        // Game
        else if (audio == Audio.WaveStart)
            EffectsSource.PlayOneShot(WaveStart);
        else if (audio == Audio.EndGameLose)
            EffectsSource.PlayOneShot(EndGameLose);
        else if (audio == Audio.EndGameWin)
            EffectsSource.PlayOneShot(EndGameWin);
    }

    public void PlaySound(string audio)
    {
        if (audio == Audio.ButtonUiClick.ToString())
            EffectsSource.PlayOneShot(ButtonUiClick);
        else if (audio == Audio.ButtonUiBack.ToString())
            EffectsSource.PlayOneShot(ButtonUiBack);
        else if (audio == Audio.ButtonUiValidation.ToString())
            EffectsSource.PlayOneShot(ButtonUiValidation);
    }

    public void PlaySoundLoop(Audio audio)
    {
        if (audio == Audio.Run)
        {
            EffectsSourceLoop.clip = Run;
            EffectsSourceLoop.Play();
        }
    }

    public void StopSoundLoop(Audio audio)
    {
        if (audio == Audio.Run)
        {
            EffectsSourceLoop.Stop();
            EffectsSourceLoop.clip = null;
        }
    }
}