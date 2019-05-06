using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggerStart : MonoBehaviour
{
    //starts the audio in random timings
    AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
        Invoke("StartSource", Random.Range(0, 1f));
    }

    void Update()
    {
        
    }

    void StartSource() {
        source.Play();
    }
}
