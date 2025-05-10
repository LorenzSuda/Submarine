using UnityEngine;
//using static UnityEditor.Rendering.CoreEditorDrawer<TData>;
public class SubmarineManager : MonoBehaviour

{  
    [SerializeField] float fuel = 100f; //benzina che si consuma*
    [SerializeField] float maxFuel = 100f;
    [SerializeField] float fuelUsageSpeed = 1f; //consumo di benzina
    [SerializeField] float mineFuelReduction = 5f;//benzina che si consuma quando prendi una mina
    
    //tipi di forza di spostamento e loro direzioni 
    [SerializeField] Vector3 impulseForce = Vector3.up * 10;
    [SerializeField] Vector3 constantForce = Vector3.up * 20;
    [SerializeField] Vector3 forwardForce = Vector3.right * 20;
    
    // a seconda del tipo di forza che scegli (clicca su ForceMode) hai effetti diversi:
    //A - Force = applica una forza continua al rigidbody, usando la sua massa
    //B - Impulse = applica un impulso istantaneo al rigidbody, usando la sua massa
    [SerializeField] ForceMode forceMode = ForceMode.Force;

    bool _thrust; // attiva/disattiva propulsione
    Rigidbody rb; 

    [SerializeField] float minRotation = 35;// per ruotare il sottomarino figlio
    [SerializeField] float maxRotation = -35; //okkio lui ha scritto rotaion: vedi se incide sennò cancella
    [SerializeField] float pitchSpeed = 1;//beccheggio:
    [SerializeField] float speed = 1;
    [SerializeField] Transform ship;//entità estetica del sottomarino
    //quindi, l'oggetto fisico scende la parte estetica si allinea averso l basso: 1h:00 ??? rivedi sto commento
    
    private bool resetted = false; //evita che il sottomarino si muova quando non è in movimento

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    
    void Update()
    {
        fuel -= Time.deltaTime * fuelUsageSpeed;
        if (fuel <= 0)
        {
            enabled = false; //disabilita lo script quando il carburante è finito: il sommergibile non si muove più
        }
        if (Input.GetButtonDown("Jump")) //se premi il tasto di Jump [spazio]
        {
            _thrust = true; // attiva la propulsione
            resetted = false; 
        }
        else if (Input.GetButton("Jump"))
        {
            //se il tasto di Jump è premuto, ruota il sottomarino verso L'ALTO perché:
            //maxRotation viene assegnato alla prima componente del Vector3, ovvero l'asse X.
            //Quindi, imposta l’angolo di rotazione X a maxRotation, mentre lascia Y e Z invariati.
            Vector3 dest = new Vector3(maxRotation, ship.transform.localRotation.eulerAngles.y, ship.transform.localRotation.eulerAngles.z);
            
            //Lerp calcola il punto intermedio tra due valori: rotazione attuale e destinazione.
            //quindi con la rotazione attuale e la dest ruoti "la nave in gradi" e la sposti alla velocità di pitchSpeed, velocità di beccheggio
            ship.transform.localRotation = Quaternion.Lerp(ship.transform.localRotation, Quaternion.Euler(dest), Time.deltaTime * pitchSpeed);
        }
        else if (Input.GetButtonUp("Jump")) //okkio! ste stai drogato come quando lo hai copiato, il codice qui dice UP non DOWN!
        {
            //se il tasto di Jump non è più premuto, disattiva la propulsione
            _thrust = false;
        }
        else
        {
            //se il tasto di Jump non è premuto, ruota il sottomarino verso IL BASSO:
            Vector3 dest = new Vector3(minRotation, ship.transform.localRotation.eulerAngles.y, ship.transform.localRotation.eulerAngles.z);
            ship.transform.localRotation = Quaternion.Lerp(ship.transform.localRotation, Quaternion.Euler(dest), Time.deltaTime * pitchSpeed);
        }

    }

    private void FixedUpdate() 
    {
        if (_thrust)
        {
            if (forceMode == ForceMode.Impulse) //se la forza è di tipo impulso
            {
                //disattiva la propulsione
                _thrust = false;
                resetted = true;
                
                //applica un impulso al RB del sottomarino in direzione dell'asse Y (up) 
                rb.AddForce(impulseForce, forceMode);
                
                //ruota il sottomarino verso l'alto
                ship.transform.localEulerAngles = new Vector3(maxRotation,
                ship.transform.localRotation.eulerAngles.y, ship.transform.localRotation.eulerAngles.z);
            }
            // else if (forceMode == ForceMode.Acceleration) //se la forza è di tipo accelerazione
            // {
            //     _thrust = false;
            //     resetted = true;
            //
            //     rb.AddForce(impulseForce, forceMode);
            //
            //     ship.transform.localEulerAngles = new Vector3(maxRotation,
            //
            //     ship.transform.localRotation.eulerAngles.y, ship.transform.localRotation.eulerAngles.z);
            //
            // }
            else
            {
                // applica una forza costante al submarine in su (up)
                rb.AddForce(constantForce, forceMode);
            }
        } 
        //e spinge in avanti il sottomarino
        rb.AddForce(forwardForce, forceMode);
    }

    private void OnTriggerEnter(Collider other)
    //rileva le collisioni (ricorda: "trasparenti", trigger) con Box e Mine
    {
        Debug.Log($"Trigger by: {other.gameObject}", other.gameObject);
    
        if (other.gameObject.CompareTag("Box")) //se sei entrato all'interno "dell'area del Box"
        {
            Destroy(other.gameObject);
            fuel = Mathf.Clamp(fuel + fuelUsageSpeed, 0, maxFuel); //okkio solo che è fuelUsageSpeed che ti riaggiunge la benzina vera e propria su sto clamp!
            Debug.Log($"Fuel gained: {fuel}");
        }
        else if (other.gameObject.CompareTag("Mine"))
        {
            Destroy(other.gameObject);
            fuel = Mathf.Clamp(fuel - mineFuelReduction, 0, maxFuel); //sennò la levi 
            Debug.Log($"Fuel lost: {fuel}");
            if (fuel <= 0)
            {
                enabled = false;
            }
        }
    }

    private void OnCollisionEnter(Collision other) //se collidi con le colonne o il pavimento
    {
        rb.isKinematic = true;
        enabled = false;
    }
}
