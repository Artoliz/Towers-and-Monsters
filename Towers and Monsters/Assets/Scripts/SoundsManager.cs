using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    #region PublicVariables

    public static SoundsManager Instance = null;

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
    public AudioClip AOE = null;
    public AudioClip AOELoop = null;

    [Header("Enemies Sounds Effects")]
    public AudioClip Death = null;
    public AudioClip SwordHit = null;
    public AudioClip CannonShot = null;

    [Header("Game Sounds Effects")]
    public AudioClip WaveStart = null;
    public AudioClip EndGameLoose = null;
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
        AOE,
        AOELoop,
        WaveStart,
        EndGameLoose,
        EndGameWin
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
        else if (audio == Audio.AOE)
            EffectsSource.PlayOneShot(AOE);
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
        else if (audio == Audio.EndGameLoose)
            EffectsSource.PlayOneShot(EndGameLoose);
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
        if (audio == Audio.AOELoop)
        {
            EffectsSourceLoop.clip = AOELoop;
            EffectsSourceLoop.Play();
        }
    }
}
