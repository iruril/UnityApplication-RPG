using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private ShaderManager shaderManager;

    [SerializeField] private bool isDissolve;
    [SerializeField] private Material dissolve;

    // Start is called before the first frame update
    void Start()
    {
        shaderManager = this.gameObject.GetComponent<ShaderManager>();

        isDissolve = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDissolve == true && Mathf.Sin(Time.time) <= -0.99)
        {
            StartCoroutine(Act());
        }
    }

    IEnumerator Act()
    {
        isDissolve = false;
        shaderManager.ChangeMaterial(this.gameObject, dissolve);

        yield return new WaitForSeconds(3.1f);

        this.gameObject.SetActive(false);
    }
}
