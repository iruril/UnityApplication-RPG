using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    // 0. NULL, 1. 원소 불 붙이기, 2. 횟불 붙이기 1.2 합칠수 있을거 같음. 3. 퍼즐 멀만들지 모르겟다.
    public int puzzleType; //퍼즐의 종류 설정
    public bool puzzleState; //퍼즐이 성공했는지 아닌지 판단 

    public GameObject[] etrigger; //원소 불 붙일거
    public GameObject treasureBox; //보물상자

    private void Awake()
    {
        puzzleState = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (puzzleType)
        {
            case 0:
                break;
            case 1:
                puzzleState = EelmentTriggerPuzzle();
                PuzzleClear(puzzleType);
                break;
            case 2:

                break;
            default:
                break;
        }
    }

    //퍼즐을 클리어 했을 때
    void PuzzleClear(int type)
    {
        if (puzzleState == true)
        {
            //원소 불 붙이기 일때
            if (type == 1)
            {
                if (treasureBox != null)
                {
                    //보물상자 활성화
                    treasureBox.GetComponent<TreasureBox>().boxState = true;
                }
                
            }
        }
    }

    //1번 퍼즐
    bool EelmentTriggerPuzzle()
    {
        for (int i = 0; i < etrigger.Length; i++)
        {
            if (etrigger[i].GetComponent<Etrigger>().state == true)
            {
                continue;
            }
            else
                return false;
        }

        return true;
    }
}
