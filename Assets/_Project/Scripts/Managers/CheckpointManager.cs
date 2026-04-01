using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private Vector3 _currentCheckpointPos;
    private int _coinsAtCheckpoint;
    private bool _hasCheckpoint = false;

    private readonly List<GameObject> _collectedCoinsAfterCheckpoint = new();

    private void Awake()
    {
        // Se esiste gia' un'altra istanza, distrugge questa
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Salva questa istanza come principale
        Instance = this;

        // Mantiene questo oggetto anche quando si cambia scena
        DontDestroyOnLoad(gameObject);
    }

    public void ActivateCheckpoint(Vector3 position, int coins)
    {
        // Salva la posizione del checkpoint
        _currentCheckpointPos = position;

        // Salva quante monete aveva il player in quel momento
        _coinsAtCheckpoint = coins;

        // Segna che ora esiste un checkpoint attivo
        _hasCheckpoint = true;

        // Svuota la lista delle monete raccolte dopo il checkpoint precedente
        _collectedCoinsAfterCheckpoint.Clear();

        Debug.Log("Checkpoint attivato!");
    }

    public void RegisterCollectedCoin(GameObject coin)
    {
        // Se la moneta esiste e non e' gia' nella lista, la aggiunge
        if (coin != null && !_collectedCoinsAfterCheckpoint.Contains(coin))
            _collectedCoinsAfterCheckpoint.Add(coin);
    }

    public void RestoreCollectedCoins()
    {
        // Riattiva tutte le monete raccolte dopo il checkpoint
        foreach (GameObject coin in _collectedCoinsAfterCheckpoint)
        {
            if (coin != null)
                coin.SetActive(true);
        }

        // Svuota la lista dopo aver ripristinato le monete
        _collectedCoinsAfterCheckpoint.Clear();
    }

    // Restituisce la posizione del checkpoint attuale
    public Vector3 GetCurrentCheckpointPosition() => _currentCheckpointPos;

    // Restituisce il numero di monete salvate nel checkpoint
    public int GetCoinsAtCheckpoint() => _coinsAtCheckpoint;

    // Restituisce true se esiste un checkpoint attivo
    public bool HasCheckpoint() => _hasCheckpoint;

    public void ResetCheckpoint()
    {
        // Riporta la posizione del checkpoint a zero
        _currentCheckpointPos = Vector3.zero;

        // Azzera il numero di monete salvate
        _coinsAtCheckpoint = 0;

        // Segna che non esiste piu' un checkpoint attivo
        _hasCheckpoint = false;

        // Svuota la lista delle monete raccolte
        _collectedCoinsAfterCheckpoint.Clear();
    }
}