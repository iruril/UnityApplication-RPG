using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer : WeakAnimal
{
    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();
    }

    private void RandomAction()
    {
        //RandomSound();

        int _random = Random.Range(0, 4); // 대기, 풀뜯기, 두리번, 걷기

        if (_random == 0 || _random == 1 || _random == 2)
            Wait();
        //else if (_random == 1)
        //    Eat();
        //else if (_random == 2)
        //    Peek();
        else if (_random == 3)
            TryWalk();
    }

    private void Wait()  // 대기
    {
        currentTime = waitTime;
        anim.SetTrigger("idle");
        //Debug.Log("대기");
    }

    private void Eat()  // 대기모션
    {
        currentTime = waitTime;
        anim.SetTrigger("idle");
        //Debug.Log("풀 뜯기");
    }

    private void Peek()  // 두리번
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        //Debug.Log("두리번");
    }
}
