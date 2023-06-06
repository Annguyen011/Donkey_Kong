using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spanwe : MonoBehaviour
{
    [SerializeField] private GameObject prefabs;
    [SerializeField] private float minTime = 2f;
    [SerializeField] private float maxTime = 4f;

    
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }
    public void Spawn()
    {
        Instantiate(prefabs, transform.position, Quaternion.identity);
        Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
