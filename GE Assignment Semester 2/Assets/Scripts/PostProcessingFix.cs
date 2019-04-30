using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingFix : MonoBehaviour
{
    public PostProcessLayer layer;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("TurnOn", 0.5f);
    }

    void TurnOn() {
        layer.enabled = true;
    }
}
