using UnityEngine;

public class PlatformRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 60f;

    private bool _playerOnPlatform = false; // Flag se il player è sopra la piattaforma
    private Rigidbody _playerRb;
    private Quaternion _lastRotation; // Rotazione della piattaforma nel frame precedente

    void Start()
    {
        _lastRotation = transform.rotation; // Salva la rotazione iniziale per calcolare il delta della rotazione
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.fixedDeltaTime, Space.World); // Ruota la piattaforma attorno all'asse Y

        if (_playerOnPlatform && _playerRb != null) // Se il player è sopra, applica la rotazione anche al player
        {
            Quaternion deltaRot = transform.rotation * Quaternion.Inverse(_lastRotation); // Calcola la rotazione applicata dalla piattaforma rispetto all'ultimo frame
            Vector3 direction = _playerRb.position - transform.position;
            direction = deltaRot * direction; // Applica la rotazione al player
            _playerRb.MovePosition(transform.position + direction); // Muove il player insieme alla piattaforma
        }

        _lastRotation = transform.rotation; // Aggiorna l'ultima rotazione per il prossimo frame
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _playerOnPlatform = true;
            _playerRb = collision.collider.GetComponent<Rigidbody>(); // Salva il Rigidbody del player
            _lastRotation = transform.rotation; // Aggiorna l'ultima rotazione per evitare scatti
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _playerOnPlatform = false;
            _playerRb = null;
        }
    }
}
