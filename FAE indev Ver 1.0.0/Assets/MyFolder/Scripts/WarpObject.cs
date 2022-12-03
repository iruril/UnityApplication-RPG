using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpObject : MonoBehaviour
{
    public static WarpObject instance;

    public bool[] isActive;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
            isActive = new bool[5];
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }       
    }
}
