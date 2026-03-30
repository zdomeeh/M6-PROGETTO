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
        ResetPlatform();
    }

    public void ResetPlatform()
    {
        transform.position = _pointA.position;
        _target = _pointB.position;

        _playerOnPlatform = false;
        _playerRb = null;
        _lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (!_playerOnPlatform)
        {
            _lastPosition = transform.position;
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            _target,
            _speed * Time.fixedDeltaTime
        );

        if (Vector3.Distance(transform.position, _target) < 0.05f)
            _target = (_target == _pointA.position) ? _pointB.position : _pointA.position;

        Vector3 deltaPos = transform.position - _lastPosition;

        if (_playerRb != null)
            _playerRb.MovePosition(_playerRb.position + deltaPos);

        _lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _playerOnPlatform = true;
            _playerRb = collision.collider.GetComponent<Rigidbody>();
            _lastPosition = transform.position;
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