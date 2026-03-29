using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 5f;

    void Start()
    {
        // Distrugge il proiettile dopo un certo tempo
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Applica danno solo al player
        if (other.CompareTag("Player"))
        {
            LifeController life = other.GetComponent<LifeController>();
            if (life != null)
            {
                life.AddHP(-damage);
            }

            // Distrugge il proiettile dopo aver colpito
            Destroy(gameObject);
        }
    }
}