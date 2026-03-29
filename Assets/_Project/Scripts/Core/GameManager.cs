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
        // Timer
        if (_levelTimer != null)
            _levelTimer.OnTimeEnded.AddListener(HandleTimeEnded);

        // Player HP
        if (_playerLife != null)
            _playerLife.OnDefeated.AddListener(HandlePlayerDefeated);

        // Opzionale: puoi ascoltare il numero di monete
        // if (_playerCollector != null)
        //     _playerCollector.OnCoinsChanged.AddListener(HandleCoinsChanged);
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
        // Mostra Game Over se finisce il tempo
        _gameOverUI?.Show();
    }

    private void HandlePlayerDefeated()
    {
        // Mostra Game Over se HP=0
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
}