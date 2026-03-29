using UnityEngine;

public class PlatformMoveOnPlayer : MonoBehaviour
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed = 2f;

    private bool _playerOnPlatform = false;
    private Rigidbody _playerRb;
    private Vector3 _target;
    private Vector3 _lastPosition;

    private void Start()
    {
        transform.position = _pointA.position; // Imposta la piattaforma al punto A all'inizio
        _target = _pointB.position; // Imposta il target iniziale al punto B
        _lastPosition = transform.position; // Salva la posizione iniziale per calcolare delta movimento
    }

    private void FixedUpdate()
    {
        if (!_playerOnPlatform)
        {
            // Aggiorna solo lastPosition per evitare scatti quando il player sale
            _lastPosition = transform.position;
            return;
        }

        // Movimento piattaforma verso il target
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.fixedDeltaTime);

        // Cambia direzione se raggiunge il target
        if (Vector3.Distance(transform.position, _target) < 0.05f)
            _target = (_target == _pointA.position) ? _pointB.position : _pointA.position;

        // Calcola delta movimento
        Vector3 deltaPos = transform.position - _lastPosition;

        // Muove il player insieme alla piattaforma
        if (_playerRb != null)
            _playerRb.MovePosition(_playerRb.position + deltaPos);

        // Aggiorna ultima posizione
        _lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _playerOnPlatform = true;
            _playerRb = collision.collider.GetComponent<Rigidbody>(); // Prende il Rigidbody del player per muoverlo insieme
            _lastPosition = transform.position; // evita scatti all'ingresso
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
