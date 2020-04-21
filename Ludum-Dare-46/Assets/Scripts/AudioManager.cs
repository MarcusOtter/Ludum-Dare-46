using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    internal static AudioManager Instance { get; private set; }
    internal float MainVolume { get; private set; } = 1f;

    [Header("Audio sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _ambienceSource;

    [Header("Sound effects")]
    [SerializeField] private Transform _soundEffectsParent;
    [SerializeField] private float _sfxExtraDestroyDelaySeconds = 2f;

    private float _musicVolume = 0.5f;
    private float _ambienceVolume = 0.5f;
    private float _sfxVolume = 0.5f;

    private void Awake()
    {
        if (!IsAloneSingleton()) { return; }
        //UpdateAudioSourceVolume();
    }

    internal void PlaySoundEffect(SoundEffect soundEffect, float volumeScale = 1f)
    {
        if (soundEffect is null) { return; }

        if (soundEffect.AudioClips is null || soundEffect.AudioClips.Length is 0)
        {
            Debug.LogError($"Sound effect \"{soundEffect.name}\" does not have any audio clips.");
            return;
        }

        var pitch = soundEffect.HasRandomizedPitch
            ? Random.Range(soundEffect.MinPitch, soundEffect.MaxPitch)
            : soundEffect.MinPitch;

        var volume = soundEffect.HasRandomizedVolume
            ? Random.Range(soundEffect.MinVolume, soundEffect.MaxVolume)
            : soundEffect.MinVolume;

        var clip = soundEffect.HasRandomizedClips
            ? soundEffect.AudioClips[Random.Range(0, soundEffect.AudioClips.Length)]
            : soundEffect.AudioClips[0];

        var name = $"{soundEffect.name} ({DateTime.Now.ToString("HH:mm:ss")})";

        var sfxSource = new GameObject(name, typeof(AudioSource)).GetComponent<AudioSource>();
        sfxSource.transform.SetParent(_soundEffectsParent, true);

        sfxSource.pitch = pitch;
        sfxSource.volume = volume * _sfxVolume * MainVolume * volumeScale;
        sfxSource.clip = clip;
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;

        sfxSource.Play();
        Destroy(sfxSource.gameObject, clip.length + _sfxExtraDestroyDelaySeconds);
    }

    public void SetVolume(float? main = null, float? music = null, float? ambience = null, float? sfx = null)
    {
        MainVolume = main ?? MainVolume;
        _musicVolume = music ?? _musicVolume;
        _ambienceVolume = ambience ?? _ambienceVolume;
        _sfxVolume = sfx ?? _sfxVolume;

        UpdateAudioSourceVolume();
    }

    private void UpdateAudioSourceVolume()
    {
        if (_musicSource != null) { _musicSource.volume = _musicVolume * MainVolume; }
        if (_ambienceSource != null) { _ambienceSource.volume = _ambienceVolume * MainVolume; }
    }

    private bool IsAloneSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
            return true;
        }

        Debug.LogWarning($"Deleting duplicate singleton \"{name}\". Don't put singletons in your scene - use the singleton spawner!");
        Destroy(gameObject);
        return false;
    }
}
