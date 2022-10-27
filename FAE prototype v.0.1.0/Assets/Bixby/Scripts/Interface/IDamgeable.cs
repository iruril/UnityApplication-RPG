using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public interface IDamgeable
{
    void TakeHit(float damage);
    void TakeElementHit(float damage, ElementRule.ElementType elementType);
}
