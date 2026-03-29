using UnityEngine;

public class TurretStraight : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _projectileSpeed = 5f;

    private float _nextFireTime = 0f; // Timestamp del prossimo sparo
    private bool _playerInRange = false; // indica se il player è nel collider

    void Update()
    {
        if (_playerInRange && Time.time >= _nextFireTime) // Se il player è nel range e il cooldown è passato, spara
        {
            Fire();
            _nextFireTime = Time.time + 1f / _fireRate; // Aggiorna il tempo del prossimo sparo basato sul fire rate
        }
    }

    void Fire()
    {
        if (_projectilePrefab != null && _firePoint != null) // Controlla che prefab e fire point siano assegnati
        {
            GameObject proj = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation); // Istanzia il proiettile
            Rigidbody rb = proj.GetComponent<Rigidbody>(); // Imposta la velocità del proiettile tramite Rigidbody
            if (rb != null)
            {
                rb.velocity = _firePoint.forward * _projectileSpeed;
            }

            // SUONO sparo
            AudioManager.Instance?.PlayTurretShoot();
        }
    }

    // Rileva quando il player entra nel collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInRange = true;
    }

    // Rileva quando il player esce dal collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInRange = false;
    }
}
