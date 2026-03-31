using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, 0f);

    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}