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
        if (CheckpointManager.Instance == null || !CheckpointManager.Instance.HasCheckpoint())
        {
            Debug.Log("Non sei arrivato in nessun checkpoint");
            return;
        }

        Vector3 spawnPos = CheckpointManager.Instance.GetCurrentCheckpointPosition();
        int savedCoins = CheckpointManager.Instance.GetCoinsAtCheckpoint();

        if (_player != null)
        {
            _player.transform.position = spawnPos;
            _player.enabled = true;
        }

        if (_playerCollector != null)
            _playerCollector.SetCoins(savedCoins);

        if (_playerLife != null)
            _playerLife.SetHP(_playerLife.GetMaxHP());

        if (_levelTimer != null)
            _levelTimer.ResetTimer(120f);

        // Reset piattaforme mobili
        foreach (var platform in _movingPlatforms)
            platform.ResetPlatform();

        _gameOverUI?.Hide();
    }
}