using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;

    void Start ()
    {
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
    }
	
	void OnGUI ()
    {
        GUISkin skin = GUI.skin;
        skin.button.normal.textColor = Color.black;
        skin.label.normal.textColor = Color.black;
        skin.button.fontSize = 20;
        skin.label.fontSize = 20;
        GUI.skin = skin;
        int life = action.GetLife();
        if (Input.GetButtonDown("Fire1"))
            action.Hit(Input.mousePosition);

        if(life > 0)
        {
            string life_string = "";
            for (int i = 0; i < life; i++) life_string += "#";
            GUI.Label(new Rect(0, 0, Screen.width / 8, Screen.height / 16), "Life:" + life_string);
            GUI.Label(new Rect(0, Screen.height / 16, Screen.width / 8, Screen.height / 16), "Score:" + action.GetScore().ToString());
            GUI.Label(new Rect(0, Screen.height / 8, Screen.width / 8, Screen.height / 16), "Round:" + action.GetRound().ToString());
        }
        else
        {
            GUI.Label(new Rect(Screen.width/2-60, Screen.height*5/16, 120, Screen.height / 8), "Game Over!");
            GUI.Label(new Rect(Screen.width/2-75, Screen.height*7/16, 150, Screen.height / 8), "Your score is:"+ action.GetScore().ToString());
            if (GUI.Button(new Rect(Screen.width * 7 / 16, Screen.height * 9 / 16, Screen.width / 8, Screen.height / 8), "restart"))
            {
                action.ReStart();
                return;
            }
        }
    }
}
