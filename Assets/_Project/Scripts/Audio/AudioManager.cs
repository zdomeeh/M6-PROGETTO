using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Clips")]
    [SerializeField] private AudioClip _mainMenuMusic;
    [SerializeField] private AudioClip _levelMusic;
    [SerializeField] private AudioClip _gameOverClip;
    [SerializeField] private AudioClip _victoryClip;
    [SerializeField] private AudioClip _starSparkleClip;
    [SerializeField] private AudioClip _timeCoinClip;
    [SerializeField] private AudioClip _turretShootClip;
    [SerializeField] private AudioClip _checkpointClip;
    [SerializeField] private AudioClip _fireworkClip;

    [Header("Player SFX")]
    [SerializeField] private AudioClip _playerJumpClip;
    [SerializeField] private AudioClip _playerCoinClip;
    [SerializeField] private AudioClip _playerDamageClip;

    [Header("AudioSources")]
    [SerializeField] private AudioSource _musicSource;
    private AudioSource _sfxSource;

    [Header("Volumes")]
    [Range(0f, 1f)] public float MusicVolume = 1f;
    [Range(0f, 1f)] public float SFXVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _sfxSource = gameObject.AddComponent<AudioSource>();
            _sfxSource.spatialBlend = 0f;

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
            return;
        }
    }

    private void Start()
    {
        PlaySceneMusic();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic();
    }

    private void PlaySceneMusic()
    {
        if (_musicSource == null) return;

        AudioClip clipToPlay = null;
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainMenu") clipToPlay = _mainMenuMusic;
        else clipToPlay = _levelMusic;

        if (clipToPlay != null)
        {
            _musicSource.clip = clipToPlay;
            _musicSource.loop = true;
            _musicSource.Play();
        }
    }

    public void StopMusic()
    {
        _musicSource?.Stop();
    }

    public void RestartMusicForScene()
    {
        PlaySceneMusic();
    }

    private void Update()
    {
        if (_musicSource != null) _musicSource.volume = MusicVolume;
        if (_sfxSource != null) _sfxSource.volume = SFXVolume;
    }

    // --- Setter per slider PauseManager ---
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

    // --- Player SFX ---
    public void PlayPlayerJump() => PlaySFX(_playerJumpClip);
    public void PlayPlayerCoin() => PlaySFX(_playerCoinClip);
    public void PlayPlayerDamage() => PlaySFX(_playerDamageClip);

    // --- SFX generici ---
    public void PlayGameOver() => PlaySFX(_gameOverClip);
    public void PlayVictory() => PlaySFX(_victoryClip);
    public void PlayPerfectVictory() => StartCoroutine(PlayPerfectVictoryRoutine());
    private IEnumerator PlayPerfectVictoryRoutine()
    {
        PlaySFX(_victoryClip);
        PlaySFX(_starSparkleClip);
        yield return null;
    }
    public void PlayTimeCoin() => PlaySFX(_timeCoinClip);
    public void PlayTurretShoot() => PlaySFX(_turretShootClip);
    public void PlayCheckpoint() => PlaySFX(_checkpointClip);
    public void PlayFirework() => PlaySFX(_fireworkClip);

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && _sfxSource != null)
            _sfxSource.PlayOneShot(clip, SFXVolume);
    }
}