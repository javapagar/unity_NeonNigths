using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bala;
    public float coolDown = 0;
    private float currentCoolDown = 0;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if((Input.GetKeyDown(KeyCode.Space) &&  currentCoolDown < 0) )
        {
            currentCoolDown = coolDown;
     
            Instantiate(bala, transform.position + new Vector3(0, 1, 0), bala.transform.rotation);
        }
        currentCoolDown -= Time.deltaTime;
    }
}
