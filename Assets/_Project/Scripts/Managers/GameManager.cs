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

    private PlatformMover[] _movingPlatforms;

    private void Awake()
    {
        // Trova tutte le piattaforme mobili presenti nella scena
        _movingPlatforms = FindObjectsOfType<PlatformMover>();
    }

    private void OnEnable()
    {
        // Collega eventi del timer e della vita del giocatore
        if (_levelTimer != null)
            _levelTimer.OnTimeEnded.AddListener(HandleTimeEnded);

        if (_playerLife != null)
            _playerLife.OnDefeated.AddListener(HandlePlayerDefeated);
    }

    private void OnDisable()
    {
        // Rimuove listener quando il GameManager viene disattivato
        if (_levelTimer != null)
            _levelTimer.OnTimeEnded.RemoveListener(HandleTimeEnded);

        if (_playerLife != null)
            _playerLife.OnDefeated.RemoveListener(HandlePlayerDefeated);
    }

    private void HandleTimeEnded()
    {
        _gameOverUI?.Show(); // mostra game over se finisce il tempo
    }

    private void HandlePlayerDefeated()
    {
        _gameOverUI?.Show(); // mostra game over se il giocatore muore
    }

    public void FinishLevel()
    {
        // Mostra vittoria se il player ha raccolto abbastanza monete
        if (_playerCollector != null && _victoryUI != null)
        {
            _victoryUI.ShowVictory(_playerCollector, _requiredCoins);
        }
    }

    public void RespawnAtCheckpoint()
    {
        // Controlla se esiste un checkpoint valido
        if (CheckpointManager.Instance == null || !CheckpointManager.Instance.HasCheckpoint())
        {
            Debug.Log("Non sei arrivato in nessun checkpoint");
            return;
        }

        // Recupera posizione e monete salvate al checkpoint
        Vector3 spawnPos = CheckpointManager.Instance.GetCurrentCheckpointPosition();
        int savedCoins = CheckpointManager.Instance.GetCoinsAtCheckpoint();

        // Riposiziona il giocatore
        if (_player != null)
        {
            _player.transform.position = spawnPos;
            _player.enabled = true;
        }

        // Ripristina monete
        if (_playerCollector != null)
            _playerCollector.SetCoins(savedCoins);

        // Ripristina vita del giocatore
        if (_playerLife != null)
            _playerLife.SetHP(_playerLife.GetMaxHP());

        // Resetta timer
        if (_levelTimer != null)
            _levelTimer.ResetTimer(120f);

        // Reset piattaforme mobili
        foreach (var platform in _movingPlatforms)
            platform.ResetPlatform();

        _gameOverUI?.Hide(); // nasconde UI game over se presente
    }
}