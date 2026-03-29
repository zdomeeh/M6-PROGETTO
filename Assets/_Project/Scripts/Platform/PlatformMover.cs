using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed = 2f;

    private Vector3 _target;
    private Vector3 _lastPosition;

    private bool _playerOnPlatform = false;
    private Rigidbody _playerRb;

    void Start()
    {
        _target = _pointB.position;
        _lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Muove la piattaforma
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.fixedDeltaTime);

        // Cambia direzione se raggiunge il target
        if (Vector3.Distance(transform.position, _target) < 0.1f)
            _target = (_target == _pointA.position) ? _pointB.position : _pointA.position;

        // Calcola delta posizione
        Vector3 deltaPos = transform.position - _lastPosition;

        // Muove il player insieme alla piattaforma
        if (_playerOnPlatform && _playerRb != null)
            _playerRb.MovePosition(_playerRb.position + deltaPos);

        _lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _playerOnPlatform = true;
            _playerRb = collision.collider.GetComponent<Rigidbody>();
            _lastPosition = transform.position; // evita scatti
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
