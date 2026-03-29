using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _gameManager != null)
        {
            _gameManager.FinishLevel();
        }
    }
}