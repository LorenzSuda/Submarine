//using GLTFast.Schema;
using UnityEngine;

public class FloatingAnimation : MonoBehaviour
//fa muovere gli oggetti ondeggiando in alto e in basso
{
    [SerializeField] Vector3 direction = Vector3.up; //direzione di movimento (es. Vector3.up per muoversi in alto) 
    [SerializeField] float speed = 0.1f; //velocità di movimento
    [SerializeField] float delta = 1; //ampiezza del movimento (es. 1 per un'oscillazione completa)
    private Vector3 _startPosition; //posizione iniziale dell'oggetto

    private void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        // con la funzione seno si ottiene un movimento ondeggiante (vedi slides)
        transform.position = _startPosition + delta * Mathf.Sin(speed * Time.time) * direction;

        //_startPosition ->> dal punto in cui si trova l'oggetto
        //delta ->> ampiezza del movimento
        //Mathf.Sin(speed * Time.time) ->> oscillazione fra -1 e 1
        //direction ->> direzione del movimento
        //OKKIO siccome � .up -> il movimento � verticale ossia Vetor3.up = (0,1,0) [posiz. di default di un oggetto nel suo up]

    }

}
