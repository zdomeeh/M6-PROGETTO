using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private RigidbodyCharacter _player;
    [SerializeField] private PlayerCoinCollector _playerCollector;
    [SerializeField] private LifeController _playerLife;

    [Header("UI")]
    [SerializeField] private VictoryUI _victoryUI;
    [SerializeField] private GameOverUI _gameOverUI;

    [Header("Level")]
    [SerializeField] private LevelTimer _levelTimer;
    [SerializeField] private int _requiredCoins = 100;

    private PlatformMoveOnPlayer[] _movingPlatforms;

    private void Awake()
    {
        // Trova tutte le piattaforme mobili presenti nella scena
        _movingPlatforms = FindObjectsOfType<PlatformMoveOnPlayer>();
    }

    private void OnEnable()
    {
        if (_levelTimer != null)
            _levelTimer.OnTimeEnded.AddListener(HandleTimeEnded);

        if (_playerLife != null)
            _playerLife.OnDefeated.AddListener(HandlePlayerDefeated);
    }

    private void OnDisable()
    {
        if (_levelTimer != null)
            _levelTimer.OnTimeEnded.RemoveListener(HandleTimeEnded);

        if (_playerLife != null)
            _playerLife.OnDefeated.RemoveListener(HandlePlayerDefeated);
    }

    private void HandleTimeEnded()
    {
        _gameOverUI?.Show();
    }

    private void HandlePlayerDefeated()
    {
        _gameOverUI?.Show();
    }

    public void FinishLevel()
    {
        if (_playerCollector != null && _victoryUI != null)
        {
            _victoryUI.ShowVictory(_playerCollector, _requiredCoins);
        }
    }

    public void RespawnAtCheckpoint()
    {
        if (_player == null) return;

        // Teletrasporto al checkpoint
        Vector3 spawnPos = CheckpointManager.Instance?.GetCurrentCheckpointPosition() ?? _player.transform.position;
        _player.transform.position = spawnPos;

        // Resetta tutte le piattaforme mobili
        foreach (var platform in _movingPlatforms)
        {
            platform.ResetPlatform();
        }

        int savedCoins = CheckpointManager.Instance?.GetCoinsAtCheckpoint() ?? 0;
        _playerCollector.SetCoins(savedCoins);

        _playerLife.SetHP(_playerLife.GetMaxHP());

        _levelTimer?.ResetTimer(120f);

        _player.enabled = true;

        _gameOverUI?.Hide();
    }
}