using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    #region PublicVariables

    public static SoundsManager Instance = null;

    [Header("Towers / Walls Sounds Effects")]
    public AudioClip ConstructBuilding = null;
    public AudioClip DestructBuilding = null;
    public AudioClip RepairBuilding = null;
    public AudioClip UpgradeBuilding = null;

    [Header("Enemies Sounds Effects")]
    public AudioClip Death = null;
    public AudioClip SwordHit = null;
    public AudioClip CannonShot = null;

    #endregion

    #region PrivateVariables

    private AudioSource EffectsSource = null;
    private AudioSource MusicsSource = null;

    #endregion

    public enum Audio
    {
        Construct,
        Destruct,
        Repair,
        Upgrade,
        Death,
        SwordHit,
        CannonShot
    }

    private void Awake()
    {
        MusicsSource = GetComponents<AudioSource>()[0];
        EffectsSource = GetComponents<AudioSource>()[1];

        if (Instance != null)
        {
            Instance = null;
            Destroy(this.gameObject);
        } else
            Instance = this;
    }

    public void PlaySound(Audio audio)
    {
        if (audio == Audio.Construct)
            EffectsSource.PlayOneShot(ConstructBuilding);
        else if (audio == Audio.Destruct)
            EffectsSource.PlayOneShot(DestructBuilding);
        else if (audio == Audio.Repair)
            EffectsSource.PlayOneShot(RepairBuilding);
        else if (audio == Audio.Upgrade)
            EffectsSource.PlayOneShot(UpgradeBuilding);
        else if (audio == Audio.Death)
            EffectsSource.PlayOneShot(Death);
        else if (audio == Audio.SwordHit)
            EffectsSource.PlayOneShot(SwordHit);
        else if (audio == Audio.CannonShot)
            EffectsSource.PlayOneShot(CannonShot);
    }
}
