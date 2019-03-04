using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
    public GameObject ball;
    public float speed = 3;
    public float viewDistance = 8;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 vec = ball.transform.position - transform.position; // gives vector pointing twards part A from part B
        vec = vec.normalized; // set magnitude to 1

        float  dis = vec.magnitude;

        Debug.Log("my way:" + dis); //  proof

        dis = Vector3.Distance(transform.position, ball.transform.position); // your way less open

        Debug.Log("dis way:" + dis);


        if (dis <= viewDistance)
        {
            transform.LookAt(ball.transform);

            //Quaternion dirQ = Quaternion.LookRotation(ball.transform.position); // what direction i want
            //Quaternion slerp = Quaternion.Slerp(dirQ, transform.rotation, speed * Time.deltaTime); // rotates to it over time
            //
            //transform.rotation = slerp;

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else if (gameObject.GetComponent<Rigidbody>().angularVelocity != Vector3.zero)
        {
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        else if (gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        
    }
}
