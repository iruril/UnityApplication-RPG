using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public interface IDamgeable
{
    void TakeHit(float damage);
    void TakeElementHit(float damage, ElementControl.ElementType elementType);
    void TakeElementHit(float damage, EnemyElement element); //나중에 삭제될 가능성 있음.
}
