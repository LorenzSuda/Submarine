using UnityEngine;
public class ObstaclePlacer : MonoBehaviour
{
    [SerializeField] GameObject[] obstaclePrefabs; 
    
    [SerializeField] private Vector3 displacement = Vector3.zero; //controlla la distanza tra ostacoli con un valore """costante"""
    [SerializeField][Range(0, 5)] private float randomVerticalDisplacement = 1; //sposta gli ostacoli in verticale (y) in modo random
    //infatti (vedi anche Istanziate) prima gli da una "costante" (displacement) e poi gli da un valore randomico tra 0 e randomVerticalDisplacement 
    
    [SerializeField][Range(0, 30)] private float distance = 5; //Distanza orizzontale (asse X) tra un ostacolo e il successivo: tra 0 e 30
    [SerializeField] private Vector3 spawnDirection = Vector3.right; //sposta gli ostacoli in orizzontale (x) e verticale (y) in modo random

    [SerializeField][Range(0.5f, 10f)] private float repeatingRatio = 1;
    [SerializeField][Range(0.5f, 10f)] private float startDelay = 1;
    [SerializeField][Range(0.5f, 30f)] private float destroyDelay = 10;

    Vector3 _startPosition;
    private Vector3 _currentPosition = Vector3.zero;

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
        
        //3. lo istanzi ma okkio perché***
        GameObject obstacle = Instantiate(prefab,_currentPosition + distance * spawnDirection + displacement +
                                          new Vector3(0, Random.Range(0, randomVerticalDisplacement), 0),
                                          prefab.transform.rotation, 
                                          transform);               
        //***4. Instantiate ha bisogno di una rotazione iniziale che di solito è Quaternion.identity
        //quindi, siccome stai usando la rotazione dell'oggetto originale su 3 ogg. diversi(mine,scatole, colonne) 
        //che è Quaternion.identity (metti mouse su Istaziate) questa andrebbe in conflitto con la rotazione della mina (già a 90°).
        
        //5. Invece così: qualunque sia la rotazione dell'oggetto originale, la posizione di spawn è:
        //quella del prefab: _currentPosition + distance * spawnDirection + displacement + new Vector3(0, Random.Range(0, randomVerticalDisplacement), 0)
        //6. OKKIO: transform alla fine è la posizione dell'oggetto padre che la spawna (vedi anche space invaders...)

        //7. Registra la posizione dell'ostacolo spawnato
        _currentPosition = new Vector3(obstacle.transform.position.x, _startPosition.y, obstacle.transform.position.z);

        Destroy(obstacle, destroyDelay);
    }
}
