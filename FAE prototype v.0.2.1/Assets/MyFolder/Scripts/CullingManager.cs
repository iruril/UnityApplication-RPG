using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingManager : MonoBehaviour
{
    void Start()
    {
        Camera camera = this.gameObject.GetComponent<Camera>();
        float[] distances = new float[32];
        distances[9] = 100;
        camera.layerCullDistances = distances;
    }
}
