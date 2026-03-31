using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private Vector3 _currentCheckpointPos;
    private int _coinsAtCheckpoint;
    private bool _hasCheckpoint = false;

    private void Awake()
    {
        // Assicura che ci sia solo un CheckpointManager (singleton)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // distruggi duplicati
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // non distruggerlo tra le scene
    }

    public void ActivateCheckpoint(Vector3 position, int coins)
    {
        _currentCheckpointPos = position; // salva posizione checkpoint
        _coinsAtCheckpoint = coins; // salva numero monete raccolte
        _hasCheckpoint = true; // segnala che esiste un checkpoint attivo
        Debug.Log("Checkpoint attivato!");
    }

    public Vector3 GetCurrentCheckpointPosition() => _currentCheckpointPos; // ritorna posizione checkpoint
    public int GetCoinsAtCheckpoint() => _coinsAtCheckpoint; // ritorna monete salvate
    public bool HasCheckpoint() => _hasCheckpoint; // verifica se esiste checkpoint

    public void ResetCheckpoint()
    {
        _currentCheckpointPos = Vector3.zero;
        _coinsAtCheckpoint = 0;
        _hasCheckpoint = false; // resetta checkpoint
    }
}