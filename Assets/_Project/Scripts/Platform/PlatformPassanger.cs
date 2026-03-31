using UnityEngine;

public class PlatformPassenger : MonoBehaviour
{
    private Rigidbody _playerRb;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    private bool _playerOnPlatform;

    private void Start()
    {
        // Salva posizione e rotazione iniziale della piattaforma
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (_playerOnPlatform && _playerRb != null)
        {
            // Calcola quanto la piattaforma si e' spostata e ruotata
            Vector3 deltaPosition = transform.position - _lastPosition;
            Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(_lastRotation);

            // Aggiorna posizione del player in base al movimento della piattaforma
            Vector3 relativePosition = _playerRb.position - transform.position;
            relativePosition = deltaRotation * relativePosition;
            Vector3 newPosition = transform.position + relativePosition + deltaPosition;

            _playerRb.MovePosition(newPosition); // muove il player insieme alla piattaforma
        }

        // Aggiorna riferimento alla posizione e rotazione attuali
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        _playerRb = collision.collider.GetComponent<Rigidbody>();
        _playerOnPlatform = _playerRb != null;

        // Se la piattaforma ha PlatformMover, attivala
        PlatformMover mover = GetComponent<PlatformMover>();
        if (mover != null)
            mover.ActivatePlatform();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        // Player lascia la piattaforma
        _playerOnPlatform = false;
        _playerRb = null;
    }
}