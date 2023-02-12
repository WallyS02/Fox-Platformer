using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class GeneratedPlatforms : MonoBehaviour
{
    public GameObject platformPrefab;
    public float a = 0.0f;
    public float b = 0.0f;
    int PLATFORMS_NUM = 8;
    GameObject[] platforms;
    Vector3[] positions;
    public float frequensy = 0.1f;
    public int amplitude = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x, y, z;

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            x = Mathf.Cos(Time.time * frequensy + 2 * Mathf.PI / PLATFORMS_NUM *i) * amplitude;
            y = Mathf.Sin(Time.time * frequensy + 2 * Mathf.PI / PLATFORMS_NUM * i) * amplitude;
            z = 0.0f;


            positions[i] =  new Vector3(x, y, z);
            platforms[i].transform.position = positions[i];
        }
    }

    void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];

        float alfa = 0.0f;
        float c = 0.0f;

        for (int i=0; i< PLATFORMS_NUM; i++)
        {
            a = Mathf.Sin(alfa);
            b = Mathf.Cos(alfa);
            positions[i] = new Vector3(b, a, c);
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);

            alfa += (2 * Mathf.PI / PLATFORMS_NUM);
        }
    }
}
