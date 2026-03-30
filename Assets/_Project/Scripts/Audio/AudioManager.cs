using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Clips")]
    [SerializeField] private AudioClip _gameOverClip;
    [SerializeField] private AudioClip _victoryClip;
    [SerializeField] private AudioClip _starSparkleClip;
    [SerializeField] private AudioClip _timeCoinClip;
    [SerializeField] private AudioClip _turretShootClip;
    [SerializeField] private AudioClip _checkpointClip;
    [SerializeField] private AudioClip _fireworkClip;

    [Header("AudioSources")]
    [SerializeField] private AudioSource _musicSource; // Musica di background
    private AudioSource _sfxSource; // SFX generali

    [Header("Volumes")]
    [Range(0f, 1f)] public float MusicVolume = 1f;
    [Range(0f, 1f)] public float SFXVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // AudioSource per SFX
            _sfxSource = gameObject.AddComponent<AudioSource>();
            _sfxSource.spatialBlend = 0f; // effetto 2D

            // Controllo musica
            if (_musicSource == null)
            {
                _musicSource = gameObject.AddComponent<AudioSource>();
                _musicSource.loop = true;
                _musicSource.spatialBlend = 0f;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Aggiorna volumi ogni frame per riflettere slider
        if (_musicSource != null)
            _musicSource.volume = MusicVolume;

        if (_sfxSource != null)
            _sfxSource.volume = SFXVolume;
    }

    // --- Metodi pubblici per slider ---
    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp01(value);
        if (_musicSource != null)
            _musicSource.volume = MusicVolume;
    }

    public void SetSFXVolume(float value)
    {
        SFXVolume = Mathf.Clamp01(value);
        if (_sfxSource != null)
            _sfxSource.volume = SFXVolume;
    }

    // --- SFX comuni ---
    public void PlayGameOver() { PlaySFX(_gameOverClip); }
    public void PlayVictory() { PlaySFX(_victoryClip); }
    public void PlayPerfectVictory()
    {
        StartCoroutine(PlayPerfectVictoryRoutine());
    }
    private IEnumerator PlayPerfectVictoryRoutine()
    {
        PlaySFX(_victoryClip);
        PlaySFX(_starSparkleClip);
        yield return null;
    }
    public void PlayTimeCoin() { PlaySFX(_timeCoinClip); }
    public void PlayTurretShoot() { PlaySFX(_turretShootClip); }
    public void PlayCheckpoint() { PlaySFX(_checkpointClip); }
    public void PlayFirework() { PlaySFX(_fireworkClip); }

    // Metodo generico per riprodurre SFX
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && _sfxSource != null)
            _sfxSource.PlayOneShot(clip, SFXVolume);
    }

    // --- Musica ---
    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (_musicSource == null || musicClip == null) return;
        _musicSource.clip = musicClip;
        _musicSource.loop = loop;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        if (_musicSource != null)
            _musicSource.Stop();
    }
}