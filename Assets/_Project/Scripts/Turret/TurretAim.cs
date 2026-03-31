using UnityEngine;

public class TurretAim : TurretBase
{
    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float detectionRadius = 10f;

    void Update()
    {
        if (player == null || firePoint == null)
            return;

        // Controlla distanza dal player
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            // Ruota verso il player
            RotateTowardsPlayer();

            // Calcola direzione verso il player
            Vector3 dir = (player.position + Vector3.up - firePoint.position).normalized;

            // Prova a sparare
            TryFire(dir);
        }
    }

    // Ruota solo sull'asse Y verso il player
    void RotateTowardsPlayer()
    {
        Vector3 targetDir = player.position - transform.position;
        targetDir.y = 0f; // ignora altezza

        if (targetDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(targetDir);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    // Mostra il raggio in scena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}