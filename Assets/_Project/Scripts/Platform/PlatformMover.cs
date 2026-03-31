using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private bool _loop = true;
    [SerializeField] private bool _startOnPlayer = false;

    private int _currentIndex;
    private bool _active = false;

    private void Start()
    {
        // Posiziona piattaforma al primo waypoint
        if (_waypoints != null && _waypoints.Length > 0)
            transform.position = _waypoints[0].position;

        _currentIndex = Mathf.Min(1, _waypoints.Length - 1); // prepara prossimo waypoint

        if (!_startOnPlayer)
            _active = true; // piattaforma attiva subito se non deve aspettare il player
    }

    private void FixedUpdate()
    {
        if (!_active) return; // se non attiva, non muovere
        if (_waypoints == null || _waypoints.Length < 2) return; // serve almeno 2 waypoint

        Transform target = _waypoints[_currentIndex];

        // Muove piattaforma verso waypoint corrente
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            _speed * Time.fixedDeltaTime
        );

        // Se raggiunge il waypoint, passa al prossimo
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            _currentIndex++;
            if (_currentIndex >= _waypoints.Length)
                _currentIndex = _loop ? 0 : _waypoints.Length - 1; // loop o fermati
        }
    }

    public void ActivatePlatform()
    {
        _active = true; // attiva la piattaforma se era inattiva
    }

    public void ResetPlatform()
    {
        // Riporta piattaforma al primo waypoint
        if (_waypoints != null && _waypoints.Length > 0)
        {
            transform.position = _waypoints[0].position;
            _currentIndex = Mathf.Min(1, _waypoints.Length - 1);
        }
    }
}