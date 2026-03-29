using System.Collections;
using UnityEngine;

public class PlatformDisappear : MonoBehaviour
{
    [SerializeField] private float _timeBeforeDisappear = 3f;
    [SerializeField] private float _respawnTime = 5f;

    private Renderer _renderer;
    private Collider _collider;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Se il player e' sopra, inizia la routine
            StartCoroutine(DisappearRoutine());
        }
    }

    private IEnumerator DisappearRoutine()
    {
        // Aspetta prima di scomparire
        yield return new WaitForSeconds(_timeBeforeDisappear);

        // toglie fisica e renderer
        if (_renderer != null)
            _renderer.enabled = false;

        if (_collider != null)
        {
            // Disattiva il collider della piattaforma
            _collider.enabled = false;

            // Sposta leggermente la piattaforma sotto il terreno cosi' non interferisce con il player
            transform.position += Vector3.down * 5f;
        }

        // Aspetta il tempo di respawn
        yield return new WaitForSeconds(_respawnTime);

        // Riporta piattaforma al posto originale
        if (_collider != null)
        {
            _collider.enabled = true;
            transform.position -= Vector3.down * 5f;
        }

        if (_renderer != null)
            _renderer.enabled = true;
    }
}
