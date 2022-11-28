using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour
{
    public void ChangeShader(GameObject[] objects, Shader shader)
    {
        foreach (GameObject obj in objects)
        {
            MeshRenderer mr = obj.GetComponent<MeshRenderer>();
            mr.material.shader = shader;
        }
    }
    public void ChangeMaterial(GameObject obj, Material material)
    { 
        MeshRenderer mr = obj.GetComponent<MeshRenderer>();
        mr.material = material;  
    }

    public void ChangeMaterials(GameObject obj, Material[] materials)
    {
        Renderer mr = obj.GetComponent<Renderer>();
        mr.materials = materials;
    }

    public void ChangeMaterials(GameObject[] objects, Material material)
    {
        foreach (GameObject obj in objects)
        {
            MeshRenderer mr = obj.GetComponent<MeshRenderer>();
            mr.material = material;
        }
    }

}
