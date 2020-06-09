using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance = null;

    private AudioSource EffectsSource = null;
    private AudioSource MusicsSource = null;

    public AudioClip ConstructBuilding = null;
    public AudioClip DestructBuilding = null;
    public AudioClip RepairBuilding = null;
    public AudioClip UpgradeBuilding = null;

    public enum Audio
    {
        Construct,
        Destruct,
        Repair,
        Upgrade
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
    }
}
