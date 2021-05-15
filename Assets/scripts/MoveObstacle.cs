using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    
    public int moveSpeed=1;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = transform.position + new Vector3(moveSpeed, 0,0);
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("chocó");
        if (other.tag == "Wall")
        {
            moveSpeed = -1 * moveSpeed;
        }
    }
}
