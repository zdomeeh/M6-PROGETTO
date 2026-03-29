using UnityEngine;

public class TurretStraight : TurretBase
{
    private bool playerInRange = false; // indica se il player è nel collider

    void Update()
    {
        // Se il player è nel range, prova a sparare
        if (!playerInRange) return;

        TryFire(firePoint.forward);
    }

    // Rileva quando il player entra nel collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    // Rileva quando il player esce dal collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}