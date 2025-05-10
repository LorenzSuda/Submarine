using UnityEngine;
using Random = UnityEngine.Random;

public class RotateAroundPivot : MonoBehaviour
//fa ruotare un oggetto attorno ad un pivot (un altro oggetto) in modo random (es. lo squalo!)
{
    [SerializeField] private float speed = 15; //velocità di rotazione
    [SerializeField] private Vector3 axis = Vector3.down; //asse attorno al quale ruotare
    [SerializeField] private bool randomAngle = false; //se true ruota randomicamente all'inizio
    [SerializeField] private Space rotationSpace = Space.Self; //spazio di rotazione (locale o globale)
    private bool _animate; //se true ruota l'oggetto

    private void Start()
    {
        if (randomAngle)
        {
            transform.Rotate(axis, Random.Range(0, 360), rotationSpace);
        }
    }
    private void Update()
    {
        if (_animate)
           transform.Rotate(axis, Time.deltaTime * speed, rotationSpace);
    }

    //ricordate: questo è quel discorso "delle pale eoliche del suo proggetto a lezione" -> quando non guardi con la cam sugli oggetti, li tieni OnBecame-In-visible
    private void OnBecameVisible()
    {
        _animate = true;
    }

    private void OnBecameInvisible()
    {
        _animate = false;
    }

}