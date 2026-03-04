using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public enum SFX
{
    PlayerWalk,
    PlayerJump,
    PlayerAttack,
    PlayerHurt,
    PlayerDeath,
    EnemyAttack,
    EnemyHurt,
    EnemyDeath,
    ButtonClick,
    
}

public enum MixerGroup
{
    Master,
    SFX,
    Environment

}

[Serializable] struct SFXConfig
{
    public SFX Type;
    public AudioClip AudioClip;
    public float VolumeScale;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer AudioMixer;
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioSource EnvironmentAudioSource;
    [SerializeField] private SFXConfig[] SFXConfigs;

    private Dictionary<SFX, SFXConfig> SFXs;
    private Dictionary<MixerGroup, string> MixerGroups;

    private void Awake()
    {
        MixerGroups = new()
        {
            {MixerGroup.Master, "MasterVolume" },
            { MixerGroup.SFX, "SFXVolume" },
            { MixerGroup.Environment, "EnvironmentVolume" }
        };

        SFXs = SFXConfigs.ToDictionary(SFXConfig => SFXConfig.Type, SFXConfig => SFXConfig);
    }

    public void PlaySFX(SFX type)
    {
        if (!SFXs.ContainsKey(type))
        {
            Debug.LogWarning($"[AudioManager] SFX not configured: {type}");
            return;
        }

        SFXConfig config = SFXs[type];
        if (config.AudioClip == null)
        {
            Debug.LogWarning($"[AudioManager] AudioClip missing for SFX: {type}");
            return;
        }

        if (SFXAudioSource != null)
        {
            SFXAudioSource.PlayOneShot(config.AudioClip, config.VolumeScale);
        }
        else
        {
            
            var cam = Camera.main;
            if (cam != null)
                AudioSource.PlayClipAtPoint(config.AudioClip, cam.transform.position, config.VolumeScale);
            else
                Debug.LogWarning("[AudioManager] No SFXAudioSource and no Camera.main to PlayClipAtPoint.");
        }
    }

    public void SetMixerVolume(MixerGroup group, float normalizedValue)
    {
        string groupString = MixerGroups[group];
        float safeValue = Mathf.Clamp(normalizedValue, 0.0001f, 1f);
        float volume = Mathf.Log10(safeValue) * 20f;
        AudioMixer.SetFloat(groupString, volume);
    }

    public float GetMixerVolume(MixerGroup group, bool normalized = true)
    {
        string groupString = MixerGroups[group];
        AudioMixer.GetFloat(groupString, out float volume);
        if (normalized)
        {
            return Mathf.Pow(10, volume / 20);
        }
        return volume;
    }

}
