using UnityEngine;

public class TurretAim : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;  // prefab del proiettile
    [SerializeField] private Transform _firePoint;          // punto da cui sparare
    [SerializeField] private float _fireRate = 1f;          // colpi al secondo
    [SerializeField] private float _projectileSpeed = 10f;  // velocità del proiettile

    [SerializeField] private Transform _player;             // player da seguire
    [SerializeField] private float _rotationSpeed = 5f;     // velocità rotazione
    [SerializeField] private float _detectionRadius = 10f;  // raggio di attivazione

    private float _nextFireTime = 0f;

    void Update()
    {
        if (_player == null || _firePoint == null || _projectilePrefab == null)
            return;

        // Controlla se il player è nel raggio
        bool playerInRange = Physics.CheckSphere(transform.position, _detectionRadius, LayerMask.GetMask("Player"));

        if (playerInRange)
        {
            RotateTowardsPlayer();
            TryFire();
        }
    }

    // Ruota solo sull'asse Y verso il player
    void RotateTowardsPlayer()
    {
        Vector3 targetDir = _player.position - transform.position;
        targetDir.y = 0f; // ignora altezza

        if (targetDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _rotationSpeed * Time.deltaTime);
        }
    }

    // Spara proiettile verso player
    void TryFire()
    {
        if (Time.time >= _nextFireTime)
        {
            Fire();
            _nextFireTime = Time.time + 1f / _fireRate;
        }
    }

    // Istanzia un proiettile e gli assegna velocità verso il player
    void Fire()
    {
        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (_player.position + Vector3.up * 1f - _firePoint.position).normalized;
            rb.velocity = dir * _projectileSpeed;
        }

        // SUONO sparo
        AudioManager.Instance?.PlayTurretShoot();
    }

    // Mostra il raggio in scena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}