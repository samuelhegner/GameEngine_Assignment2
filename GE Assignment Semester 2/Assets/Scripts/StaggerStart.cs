using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggerStart : MonoBehaviour
{
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        Invoke("StartSource", Random.Range(0, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartSource() {
        source.Play();
    }
}
