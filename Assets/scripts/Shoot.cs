using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float aceleracion = 500;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * aceleracion * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "SoftObstacles")
        {
            GameManager.score += 50;
            other.GetComponentInParent<AudioSource>().Play();//esta almacenado en una carpeta
            Destroy(other.gameObject);
            Destroy(gameObject);
        }else if(other.tag!="Player")
        {
            Debug.Log(other.tag);
            Destroy(gameObject);
        }
        

    }
}
