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

    // Metodo chiamato dalla ExitDoor
    public void FinishLevel()
    {
        if (_playerCollector != null && _victoryUI != null)
        {
            _victoryUI.ShowVictory(_playerCollector, _requiredCoins);
        }
    }

    // Metodo per respawn al checkpoint
    public void RespawnAtCheckpoint()
    {
        if (_player == null) return;

        // Recupera posizione e monete salvate
        Vector3 spawnPos = CheckpointManager.Instance?.GetCurrentCheckpointPosition() ?? _player.transform.position;
        _player.transform.position = spawnPos;

        int savedCoins = CheckpointManager.Instance?.GetCoinsAtCheckpoint() ?? 0;
        _playerCollector.SetCoins(savedCoins);

        // Ripristina vita piena
        _playerLife.SetHP(_playerLife.GetMaxHP());

        // Reset timer a 120 secondi
        _levelTimer?.ResetTimer(120f);

        // Riabilita il player
        _player.enabled = true;

        // Nasconde Game Over UI
        _gameOverUI?.Hide();
    }
}