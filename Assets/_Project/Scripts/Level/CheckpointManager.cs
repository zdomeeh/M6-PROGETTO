using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private Vector3 _currentCheckpointPos;
    private int _coinsAtCheckpoint;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ActivateCheckpoint(Vector3 position, int coins)
    {
        _currentCheckpointPos = position;
        _coinsAtCheckpoint = coins;
        Debug.Log("Checkpoint attivato!");
    }

    public Vector3 GetCurrentCheckpointPosition() => _currentCheckpointPos;
    public int GetCoinsAtCheckpoint() => _coinsAtCheckpoint;
}