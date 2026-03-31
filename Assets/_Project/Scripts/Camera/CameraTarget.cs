using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 1.5f, 0f);
    [SerializeField] private bool _followRotation = false;

    private void LateUpdate()
    {
        if (_player == null) // Se non c'è il giocatore, non fare nulla
        { return;}

        transform.position = _player.position + _offset; // Posiziona la camera sopra il giocatore, usando l'offset

        if (_followRotation)
        {
            Vector3 flatForward = _player.forward; // Ignora l'altezza
            flatForward.y = 0f;

            if (flatForward.sqrMagnitude > 0.001f) // Se la direzione è valida, fai guardare la camera in quella direzione
                transform.rotation = Quaternion.LookRotation(flatForward);
        }
    }
}