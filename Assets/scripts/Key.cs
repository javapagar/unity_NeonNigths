using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject rigthDoor;
    public Vector3 rightDoorInitPos;
    public GameObject leftDoor;
    public Vector3 leftDoorInitPos;
    public GameObject text;
    private bool openDoor;
    public GameManager manager;
   
    // Start is called before the first frame update
    void Start()
    {
        openDoor = false;//inicializo el booleno a false para indicar que la puerta está cerrada
        //posDoor = lastDoor.transform.position;
        rightDoorInitPos = rigthDoor.transform.position;
        leftDoorInitPos = leftDoor.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!openDoor)//la puerta está cerrada
        {
            this.transform.eulerAngles = this.transform.eulerAngles + new Vector3(0, 0, 1f);//le doy el efecto de rotación a la llave
            
        }
        else//la puerta se abre
        {
            
            rigthDoor.transform.position = rigthDoor.transform.position + new Vector3(1f, 0, 0);//muevo puerta derecha
            leftDoor.transform.position = leftDoor.transform.position + new Vector3(-1f, 0, 0);//muevo puerta izquierda
        }
    }

    public void OnTriggerEnter(Collider other)//recoge el evento del disparador SphereCollider
    {
        if (other.tag == "Player")
        {
            openDoor = true;//cuando el player choca contra la llave pongo la variable boolean que controla la puerta a true

            AudioSource audioSrc = this.GetComponent<AudioSource>();//tomo el elemento audioSrc de la llave
            if(audioSrc!=null)audioSrc.Play();//si hay sonido asociado le ejecuto

            StartCoroutine(SetActive(false));//desactivo la llave
            
        }
    }

    public void resPawn()
    {
        openDoor = false;
        StartCoroutine(SetActive(true));//activo la llave
        leftDoor.transform.position = leftDoorInitPos;//coloco la puerta izquierda
        rigthDoor.transform.position = rightDoorInitPos;
    }

    public IEnumerator SetActive(bool setActive)//corrutina que activa o desactiva la llave
    {
        this.GetComponent<SphereCollider>().enabled = setActive;//seteo el sphere collider de la llave
        this.GetComponent<MeshRenderer>().enabled = setActive;//seteo que se visualice la llave

        if (text != null) text.GetComponent<MeshRenderer>().enabled = setActive;//Si hay texto lo seteo
        yield return null;
    }
}