using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    private Shield shield;
    [SerializeField] private bool setShield;

    private void Start()
    {
        shield = this.gameObject.transform.Find("Shield").GetComponent<Shield>();
        setShield = false;
    }

    private void Update()
    {
        if (setShield)
        {
            shield.SetActive(true);
            setShield = false;
        }
    }
}
