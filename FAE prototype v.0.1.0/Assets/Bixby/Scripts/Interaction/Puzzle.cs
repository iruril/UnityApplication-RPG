using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    // 0. NULL, 1. 원소 불 붙이기, 2. 횟불 붙이기 1.2 합칠수 있을거 같음. 3. 패턴퍼즐, 4. 오브젝트 연결 퍼즐
    public int puzzleType; //퍼즐의 종류 설정
    public bool puzzleState; //퍼즐이 성공했는지 아닌지 판단 

    public GameObject[] etrigger; //원소 불 붙일거
    public GameObject[] pattern;  //패턴 오브젝트
    public GameObject[] connect;  //연결 오브젝트

    public Transform patternRotation; //패턴 성공 시의 오브젝트 로테이션

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
                //원소 불 붙이기 퍼즐
                puzzleState = EelmentTriggerPuzzle();
                PuzzleClear(puzzleType);
                break;
            case 2:

                break;
            case 3:
                //패턴퍼즐
                puzzleState = PatternPuzzle();
                PuzzleClear(puzzleType);
                break;
            case 4:
                //오브젝트 연결 퍼즐
                puzzleState = ConnectPuzzle();
                PuzzleClear(puzzleType);
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
            //패턴 퍼즐일때
            else if (type == 3)
            {
                if (treasureBox != null)
                {
                    //보물상자 활성화
                    treasureBox.GetComponent<TreasureBox>().boxState = true;
                }
            }
            else if (type == 4)
            {
                if (treasureBox != null)
                {
                    //보물상자 활성화
                    treasureBox.GetComponent<TreasureBox>().boxState = true;
                }
            }
        }
        else if (puzzleState == false)
        {
            //원소 불 붙이기 일때
            if (type == 1)
            {
                if (treasureBox != null)
                {
                    //보물상자 활성화
                    treasureBox.GetComponent<TreasureBox>().boxState = false;
                }
            }
            //패턴 퍼즐일때
            else if (type == 3)
            {
                if (treasureBox != null)
                {
                    //보물상자 활성화
                    treasureBox.GetComponent<TreasureBox>().boxState = false;
                }
            }
            else if (type == 4)
            {
                if (treasureBox != null)
                {
                    //보물상자 활성화
                    treasureBox.GetComponent<TreasureBox>().boxState = false;
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

    //3번 퍼즐
    bool PatternPuzzle()
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i].GetComponent<Pattern>().state == true)
            {
                continue;
            }
            else
                return false;
        }

        return true;
    }


    //4번 퍼즐
    bool ConnectPuzzle()
    {
        for (int i = 0; i < connect.Length; i++)
        {
            if (connect[i].GetComponent<Connect>().state == true)
            {
                continue;
            }
            else
                return false;
        }

        return true;
    }
}
