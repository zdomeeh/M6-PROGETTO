using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip _gameOverClip;
    [SerializeField] private AudioClip _victoryClip;
    [SerializeField] private AudioClip _starSparkleClip;
    [SerializeField] private AudioClip _timeCoinClip;
    [SerializeField] private AudioClip _turretShootClip;

    [SerializeField, Range(0f, 1f)] private float _turretShootVolume = 1f;

    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.spatialBlend = 0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // GAME OVER
    public void PlayGameOver()
    {
        if (_gameOverClip != null)
            _audioSource.PlayOneShot(_gameOverClip);
    }

    // NORMAL VICTORY
    public void PlayVictory()
    {
        if (_victoryClip != null)
            _audioSource.PlayOneShot(_victoryClip);
    }

    // PERFECT VICTORY
    public void PlayPerfectVictory()
    {
        StartCoroutine(PlayPerfectVictoryRoutine());
    }

    private IEnumerator PlayPerfectVictoryRoutine()
    {
        // Suono della vittoria normale
        if (_victoryClip != null)
            _audioSource.PlayOneShot(_victoryClip);

        // Suono dello sparkle parte subito insieme al fade della stella
        if (_starSparkleClip != null)
            _audioSource.PlayOneShot(_starSparkleClip);

        yield return null;
    }

    // TIME COIN
    public void PlayTimeCoin()
    {
        if (_timeCoinClip != null)
            _audioSource.PlayOneShot(_timeCoinClip);
    }

    // TURRET SHOOT
    public void PlayTurretShoot()
    {
        if (_turretShootClip != null)
            _audioSource.PlayOneShot(_turretShootClip, _turretShootVolume);
    }
}
