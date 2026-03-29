using UnityEngine;

public abstract class TurretBase : MonoBehaviour
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float projectileSpeed = 10f;

    protected float nextFireTime = 0f;

    // Controlla se può sparare (cooldown) e poi spara
    protected void TryFire(Vector3 direction)
    {
        if (Time.time < nextFireTime) return;

        Fire(direction);
        nextFireTime = Time.time + 1f / fireRate; // aggiorna cooldown
    }

    // Gestione generica dello sparo (usata da tutte le torrette)
    protected virtual void Fire(Vector3 direction)
    {
        // Controlla che prefab e fire point siano assegnati
        if (projectilePrefab == null || firePoint == null)
            return;

        // Istanzia il proiettile
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        // Imposta la velocità del proiettile tramite Rigidbody
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        // SUONO sparo
        AudioManager.Instance?.PlayTurretShoot();
    }
}