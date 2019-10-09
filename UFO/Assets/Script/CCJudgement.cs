using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SituationType : int { Continue, Loss }

public class CCJudgement : MonoBehaviour
{
    public int life;
    public int score; 
    private int round2 = 10;
    private int round3 = 20;

    void Start ()
    {
        life = 5;
        score = 0;
    }

    public SituationType GetSituation()
    {
        if (life <= 0) return SituationType.Loss;
        return SituationType.Continue;
    }

    public void Record(GameObject disk)
    {
        score += disk.GetComponent<UfoProperty>().score;
    }

    public void Reset()
    {
        life = 5;
        score = 0;
    }
    public int Get_round()
    {
        if (score < round2) return 1;
        else if (score < round3) return 2;
        else return 3;
    }
    public void Miss()
    {
        life -= 1;
    }
}
