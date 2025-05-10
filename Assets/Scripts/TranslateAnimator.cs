using UnityEngine;

public class TranslateAnimator : MonoBehaviour
//sposta/trasla un oggetto in una direzione specifica (es. il pesce che nuota)
{
    [SerializeField] private float speed; //velocità di movimento
    [SerializeField] private Vector3 direction = Vector3.right; //direzione di movimento (es. Vector3.right per muoversi a destra)

    void Update()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
    }
}
