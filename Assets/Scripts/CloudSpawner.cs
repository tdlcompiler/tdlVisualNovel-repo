using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject clouds1;
    public GameObject clouds2;
    public int spawnerDelay;

    void Start()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnerDelay);
            GameObject cloud;
            float rand = Random.Range(257.5f, 258.8f);
            if (Random.value < 0.5f)
                cloud = Instantiate(clouds1, new Vector3(538.5f, rand, -0.5f), Quaternion.identity);
            else
                cloud = Instantiate(clouds2, new Vector3(538.5f, rand, -0.5f), Quaternion.identity);
            rand = Random.Range(0.7f, 1.5f);
            cloud.transform.localScale = new Vector3(cloud.transform.localScale.x * rand, cloud.transform.localScale.y * rand, 0);
        }
    }
}
