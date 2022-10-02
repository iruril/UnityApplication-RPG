using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="ItemSO",menuName ="Scriptable Object/ItemSO")]
public class itemSO : ScriptableObject
{
    public List<itemData> items;
}
