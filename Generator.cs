using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Generate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Generate()
    {
        for (int i = 0; i < 45; i++)
        {
            yield return new WaitForSeconds(1f);
            Instantiate(obj, transform.position + Random.insideUnitSphere, Quaternion.identity);
        }

    }
}
