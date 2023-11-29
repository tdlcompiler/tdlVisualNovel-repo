using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float speed;
    public bool randomSpeed;

    void Start()
    {
        if (randomSpeed)
        {
            System.Random rand = new System.Random();
            speed = rand.Next(1, 8);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
        if (transform.position.x > 550f)
            Destroy(gameObject);
    }
}
