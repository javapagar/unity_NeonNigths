using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    //public static int score=0;

    public float turnSpeed = 1;
    public float MaxPlaneRotationDegrees = 45;

    public float maxSpeed = 10;
    public float acceleration = 1;

    public static float currentSpeed = 0;

    protected Transform plane;

    protected FollowTarget[] followers;

    public float secondsOnEnd = 2;
    public bool hasControl = true;

    protected Vector3 initialPos;

    public LayerMask stopMask;

    //particles
    public GameObject particlesPrefab;
    public float secondsWaitingOnDeath = 3;

    public GameManager manager;//Instanciamos el game player para manejar el incio y el fin

    


    // Use this for initialization
    void Start()
    {
        initialPos = transform.position;


        plane = transform.GetChild(0);

        followers = FindObjectsOfType<FollowTarget>();

        //stopMask = LayerMask.GetMask("Walls");//añadido para que reconociera los muros
    }

    // Update is called once per frame
    void Update()
    {
        if (hasControl)
        {
            manager.resPawn = false;
            var verti= Input.GetAxis("Vertical");//guardo las flechas verticales
            if(verti < 0)//si decelero
            {
                if(currentSpeed >0)currentSpeed -= Time.deltaTime * (acceleration * 2);//desaleracion
                if (currentSpeed < 0) currentSpeed = 0;
            }
            else if(verti >0)//acelero
            {
                if(currentSpeed < maxSpeed)currentSpeed += Time.deltaTime * (acceleration * 3);
            }
            else
            {
                currentSpeed += Time.deltaTime * acceleration; //aceleracion ormal
            }
            
            if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
                //currentSpeed = currentSpeed + Time.deltaTime * acceleration > MaxSpeed ? MaxSpeed : currentSpeed + Time.deltaTime * acceleration > MaxSpeed;
            }


            var horiz = Input.GetAxis("Horizontal");

            var direction = (transform.right * turnSpeed * horiz * Time.deltaTime).normalized;

            //Debug.Log(horiz);

            if (!Physics.Raycast(transform.position, direction, turnSpeed *  Time.deltaTime + 0.5f, stopMask))
            {
                transform.position = transform.position + transform.right * turnSpeed * horiz * Time.deltaTime;
            }


            //Rotamos la nave en función del giro
            var r = plane.rotation.eulerAngles;
            r.z= Mathf.Lerp(-MaxPlaneRotationDegrees, MaxPlaneRotationDegrees, (-horiz + 1) / 2);

            plane.rotation = Quaternion.Lerp(plane.rotation, Quaternion.Euler(r), 0.1f);

        }


        //move forward
        transform.position = transform.position + transform.forward * Time.deltaTime * currentSpeed;
        //Debug.Log(transform.position);

       
       
    }

    public void OnTriggerEnter(Collider other)
    {
        if (hasControl)//Para evitar que esto pase varias veces seguidas
        {
            if (other.tag == "TriggerEndLevel")
            {
                Debug.Log(other.tag);
                StartCoroutine(waitAndEnd());
                /*AudioSource audioSrc = manager.GetComponent<AudioSource>();
                audioSrc.Play();*/
                other.GetComponent<AudioSource>().Play();

            }
            else if (other.tag == "Obstacles" || other.tag == "SoftObstacles")
            {
                StartCoroutine(Respawn());
            }
        
            else if (other.tag == "SumaPuntos")
            {
                var min = currentSpeed / maxSpeed;
                if (min == 0) min = 0.1f;
                GameManager.score += (int) (min * 300);
                other.GetComponent<AudioSource>().Play();
                other.GetComponent<CapsuleCollider>().enabled = false;
                other.GetComponent<MeshRenderer>().enabled = false;
                Debug.Log(GameManager.score);
            }

        }
    }

    
    protected IEnumerator Respawn()
    {
        if (GameManager.score > 500)
        {
            GameManager.score -= 500;

        }
        else
        {
            GameManager.score = 0;
        }
        currentSpeed = 0;
        hasControl = false;
        plane.gameObject.SetActive(false);
        AudioSource audioSrc = gameObject.GetComponent<AudioSource>();
        audioSrc.Play();
        var particles = Instantiate(particlesPrefab, transform.position, transform.rotation);
        //reescalo la explosión
        particles.transform.localScale = new Vector3(10, 10, 10);
        manager.resPawn = true;
        
        yield return new WaitForSeconds(secondsWaitingOnDeath);

        Destroy(particles);
        
        transform.position = initialPos;
        plane.gameObject.SetActive(true);
        hasControl = true;
       
        yield return null;
    }

    protected IEnumerator waitAndEnd()
    {
        for (int i = 0; i < followers.Length; i++)
        {
            followers[i].enabled = false;
        }


        hasControl = false;
        yield return new WaitForSeconds(secondsOnEnd);

        //avisar fin de nivel/cargar nuevo nivel.
        if(manager != null)
        {
            manager.endLevel();
        }
    }

    public void initLevel()
    {
        currentSpeed = 0;
        hasControl = false;
       
    }
}
