using UnityEngine;

public class PlatformPathMover : MonoBehaviour
{
    [SerializeField] private Transform[] _points; // Punti del percorso della piattaforma
    [SerializeField] private float _speed = 2f;

    private int _currentIndex = 0;
    private Vector3 _lastPosition;

    private bool _playerOnPlatform = false;
    private Rigidbody _playerRb;

    void Start()
    {
        if (_points.Length > 0) // Se ci sono punti nel percorso, inizializza la posizione precedente
            _lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (_points.Length == 0) return; // Se non ci sono punti non fa nulla

        transform.position = Vector3.MoveTowards(transform.position, _points[_currentIndex].position, _speed * Time.fixedDeltaTime); // Muove la piattaforma verso il punto target corrente

        if (Vector3.Distance(transform.position, _points[_currentIndex].position) < 0.1f) // Quando raggiunge il target, passa al punto successivo
            _currentIndex = (_currentIndex + 1) % _points.Length;

        Vector3 deltaPos = transform.position - _lastPosition; // Calcola il delta movimento rispetto al frame precedente

        if (_playerOnPlatform && _playerRb != null) // Se il player è sopra, lo muove insieme alla piattaforma
            _playerRb.MovePosition(_playerRb.position + deltaPos);

        _lastPosition = transform.position; // Aggiorna l'ultima posizione per il prossimo frame
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) // Quando il player sale sulla piattaforma
        {
            _playerOnPlatform = true;
            _playerRb = collision.collider.GetComponent<Rigidbody>(); // Salva il Rigidbody per muovere il player insieme
            _lastPosition = transform.position; // Aggiorna l'ultima posizione per evitare scatti
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) // Quando il player lascia la piattaforma
        {
            _playerOnPlatform = false;
            _playerRb = null;
        }
    }
}
