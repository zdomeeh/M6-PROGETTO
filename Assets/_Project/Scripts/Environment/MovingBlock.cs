using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;

    private Vector3 target;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true; // il blocco si muove ma non viene spinto da altri oggetti

        if (pointA != null)
            _rb.position = pointA.position; // inizio dal punto A

        target = pointB != null ? pointB.position : _rb.position; // prima destinazione
    }

    private void FixedUpdate()
    {
        if (pointA == null || pointB == null) return; // se mancano i punti, non fare nulla

        // calcola la prossima posizione verso il target
        Vector3 nextPos = Vector3.MoveTowards(_rb.position, target, speed * Time.fixedDeltaTime);
        _rb.MovePosition(nextPos); // muove il blocco

        // se siamo arrivati vicino al target, cambia direzione
        if (Vector3.Distance(nextPos, target) < 0.1f)
            target = target == pointA.position ? pointB.position : pointA.position;
    }
}