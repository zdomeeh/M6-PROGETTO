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
        _rb.isKinematic = true; // blocco si muove ma non reagisce alla fisica
        if (pointA != null)
            _rb.position = pointA.position;

        target = pointB != null ? pointB.position : _rb.position;
    }

    private void FixedUpdate()
    {
        if (pointA == null || pointB == null) return;

        Vector3 nextPos = Vector3.MoveTowards(_rb.position, target, speed * Time.fixedDeltaTime);
        _rb.MovePosition(nextPos);

        if (Vector3.Distance(nextPos, target) < 0.1f)
            target = target == pointA.position ? pointB.position : pointA.position;
    }
}