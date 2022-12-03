using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private ShaderManager shaderManager;

    [SerializeField] private Material dissolve;
    private Material[] materials;
    [ColorUsage(true, true)] [SerializeField] private Color color;
    [SerializeField] private float[] scales;
    private float time;
    void Start()
    {
        shaderManager = this.gameObject.GetComponent<ShaderManager>();

        time = -Mathf.PI * 0.3f;
        CreateMaterial();
    }

    public IEnumerator Act(GameObject go)
    {
        shaderManager.ChangeMaterials(this.gameObject, materials);

        Material[] mats = this.gameObject.GetComponent<Renderer>().materials;

        while (time < Mathf.PI*0.2f)
        {
            yield return null;

            foreach(Material mat in mats)
                mat.SetFloat("_Value", Mathf.Sin(time));
            time += Time.deltaTime;
        }

        this.gameObject.SetActive(false);
        Destroy(go);

        yield break;
    }
    private void CreateMaterial()
    {
        Material[] mats = this.gameObject.GetComponent<Renderer>().materials;

        //Set Color
        if (transform.parent.gameObject.tag == "Enemy")
            color = transform.parent.GetComponent<Enemy>().GetMyElementColor();
        else if (transform.parent.gameObject.tag == "DungeonBoss")
            color = transform.parent.GetComponent<DungeonBoss>().GetMyElementColor();
        else if (transform.parent.gameObject.tag == "FinalBoss")
            color = transform.parent.GetComponent<FinalBoss>().GetMyElementColor();

        dissolve.SetColor("_Color", color);
        dissolve.SetFloat("_Value", Mathf.Sin(time));

        materials = new Material[mats.Length];
        for (int i = 0; i < mats.Length; i++)
        {
            materials[i] = Instantiate(dissolve);

            //Set Texture
            materials[i].SetTexture("_MainTexture", mats[i].mainTexture);

            //Set Scale
            materials[i].SetFloat("_Scale", scales[i]);
        }
    }
}
