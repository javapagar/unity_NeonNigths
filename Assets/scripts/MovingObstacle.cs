using UnityEngine;
using System.Collections;

public class MovingObstacle : MonoBehaviour
{
    public float maxMovementMeters;
    public float timeToMove;

    protected Vector3 LBorder, RBorder;
    // Use this for initialization
    void Start()
    {
        LBorder = transform.position + Vector3.left * maxMovementMeters;
        RBorder = transform.position + Vector3.right * maxMovementMeters;

        StartCoroutine(moveLoop());
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected IEnumerator moveLoop()
    {
        var initial = LBorder;
        var end = RBorder;

        while (true)
        {
            var accumTime = 0.0f;

            while (accumTime <= 1)
            {
                accumTime += Time.deltaTime / timeToMove;

                transform.position = Vector3.Lerp(initial, end, Mathf.SmoothStep(0, 1, accumTime));
                yield return null;
            }
            //change direction
            if (initial == LBorder)
            {
                initial = RBorder;
                end = LBorder;
            }
            else
            {
                end = RBorder;
                initial = LBorder;
            }
        }
    }
}
