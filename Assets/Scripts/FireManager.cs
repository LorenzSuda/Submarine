using UnityEngine;

public class FireManager : MonoBehaviour

{
    [SerializeField] private GameObject _bulletPrefab; // prefab del proiettile
    [SerializeField] private float firePower = 100; 
    [SerializeField] private float fireRate = 1; // tempo di attesa tra i colpi
    [SerializeField] private ForceMode fireMode = ForceMode.Impulse; // modalità di applicazione della forza (AddRelativeForce sotto)
    [SerializeField] private Transform[] firePositions; // posizioni di fuoco (bocche di fuoco) 
    [SerializeField] private Transform root; // inclinazione del sottomarino 


    float _fireTimer = 0;
    private void Update()
    {
        _fireTimer += Time.deltaTime;

        if (_fireTimer < fireRate) return; //in pratica: se non hai ancora sparato "esci"

        //se il tasto di fuoco è premuto
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log($"Fire1: {transform.eulerAngles}"); // stampa l'angolo di rotazione dell'oggetto
            for (int i = 0; i < firePositions.Length; i++)
            {
                GameObject bullet = Instantiate(_bulletPrefab, firePositions[i].position, _bulletPrefab.transform.rotation * root.rotation); // instanzia il proiettile ***
                Rigidbody rb = bullet.GetComponent<Rigidbody>(); // ottieni il componente Rigidbody del proiettile
                rb.AddRelativeForce(Vector3.down * firePower, fireMode);//or Vector3.down 
                                        //applica una forza al RB del proiettile in direzione del suo asse locale, del suo froward (in questo caso verso il basso)
                                        //Ecco perch� se va nel verso opposto devi mettere Vector3.up
                                                                      
            }
            _fireTimer = 0;
        }
    }
}
//***NOTE IMPORTANTI: prodotto fra quaternioni -> _bulletPrefab.transform.rotation * root.rotation
// _bulletPrefab.transform.rotation ->> rotazione del prefab del proiettile 
// root.rotation ->> rotazione del sottomarino
//IN PRATICA: ruoti il proiettile di 90� e poi lo ruoti in base allinclinazione del sottomarino (root)
