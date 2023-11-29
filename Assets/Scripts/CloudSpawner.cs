using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject clouds1;
    public GameObject clouds2;
    public int spawnerDelay;

    // Start is called before the first frame update
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
                cloud = Instantiate(clouds1, new Vector3(540, rand, -1), Quaternion.identity);
            else
                cloud = Instantiate(clouds2, new Vector3(540, rand, -1), Quaternion.identity); 
        }
    }
}
