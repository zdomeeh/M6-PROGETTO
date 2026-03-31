using UnityEngine;

public class PlatformRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 60f;

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.fixedDeltaTime, Space.World); // Ruota la piattaforma attorno all'asse Y a velocita' costante
    }
}