using UnityEngine;
public class ObstaclePlacer : MonoBehaviour
{
    [SerializeField] GameObject[] obstaclePrefabs; 
    [SerializeField][Range(0, 5)] private float randomVerticalDisplacement = 1; //sposta gli ostacoli in verticale (y)
    [SerializeField][Range(0, 30)] private float distance = 5; //sposta gli ostacoli in orizzontale (x)
    [SerializeField] private Vector3 displacement = Vector3.zero; //sposta gli ostacoli in orizzontale (x) e verticale (y) in modo random
    [SerializeField] private Vector3 movementDirection = Vector3.right; //sposta gli ostacoli in orizzontale (x) e verticale (y) in modo random

    private Vector3 _currentPosition = Vector3.zero;

    [SerializeField][Range(0.5f, 10f)] private float repeatingRatio = 1;
    [SerializeField][Range(0.5f, 10f)] private float startDelay = 1;
    [SerializeField][Range(0.5f, 30f)] private float destroyDelay = 10;

    Vector3 _startPosition;

    void Start()
    {
        //posizione dell'oggetto iniziale: okkio "qualsiasi" oggetto infatti vedi numeri...
        _startPosition = transform.position;

        _currentPosition = _startPosition;
        //1 dai un "secondo" di attesa prima di iniziare a spawnare gli ostacoli 
        InvokeRepeating(nameof(PlaceObstacles), startDelay, repeatingRatio);
    }

    void PlaceObstacles()
    {
        //2. prendi un prefab a caso tra quelli che hai messo nell'array (obstaclePrefabs)
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];


        //3. lo istanzi perché Instantiate ha bisogno di una rotazione che di solito è Quaternion.identity
        //ma questa andrebbe in conflitto con la rotazione della mina (già a 90°)...

        GameObject obstacle = Instantiate(prefab,_currentPosition + distance * movementDirection + displacement +
                                          new Vector3(0, Random.Range(0, randomVerticalDisplacement), 0),
                                          prefab.transform.rotation, //4...quindi con la rotazione dell'oggetto originale (che sarebbe Quaternion.identity: metti mouse su Istaziate)
                                          transform);

        //5.qualunque sia la rotazione dell'oggetto originale, la posizione di spawn è quella del prefab.
        //6. OKKIO: transform alla fine è la posizione dell'oggetto padre che la spawna (vedi più avanti se c'è qualche manager che lo fa)

        //7. Registra la posizione dell'ostacolo spawnato
        _currentPosition = new Vector3(obstacle.transform.position.x, _startPosition.y, obstacle.transform.position.z);

        Destroy(obstacle, destroyDelay);


        //NOTE: vedi schema pdf -> in pratica gli passi delle posizioni random agli oggetti mantendendo la posizione y costante a _startPosition.y (okkio a "punto y")
        //così ogni volta che spwani un ostacolo, lui lo sposta in una posizione random (x e z) ma non in altezza (y) ->> IL PERCHé PERò TI è IGNOTO AL MOMENTO ^^
        //cioè si claro non esce in verticale e "a ventaglio" dallo schermo ma allora lo stesso non vale anche per x e z? 
    }
}
