using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangMaterials : MonoBehaviour
{
    public Material[] mat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void ChangeMyMaterials(int element)
    {
        if (element == 1)
            gameObject.GetComponent<SkinnedMeshRenderer>().materials[1] = mat[0];
        else if (element == 2)
            gameObject.GetComponent<SkinnedMeshRenderer>().materials[1] = mat[1];
        else if (element == 3)
            gameObject.GetComponent<SkinnedMeshRenderer>().materials[1] = mat[2];
    }
}
