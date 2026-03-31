using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _gameManager != null) // Se chi entra ha il tag "Player" e il GameManager esiste
        {
            _gameManager.FinishLevel(); // termina il livello
        }
    }
}