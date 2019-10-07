using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class God : MonoBehaviour
{
    public Text Status;
    public bool IsLose;
    public static bool IsNext = false;
    public static int Hp = 1000000;
    public static int Mul = 1;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsLose)
        {
            
            Camera.main.backgroundColor = Color.red;
        }
        else if (Random.value < 0.001f)
        {
            IsLose = true;
            Status.text = "Invisible random death";
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            IsLose = true;
            Hp -= Mul;
            if (Mul != 5000 && Hp < 100000)
            {
                Hp = 100000;
                Mul = 5000;
                Status.text = Hp + " of my HP left. Int overflow (doesn't care it's 32bit). Now I can't deal so 'balanced' damage.";
            }
            else
            {
                Status.text = "Invisible " + Mul + " damage. " + Hp + " of my HP left. But I use AWP. Headshot. Now you are invisible for the thermal imager.";
            }
            if (Hp <= 25000)
            {
                Status.text = Hp + " left. Invisible climax of the story.";
                if (Hp <= 0)
                {
                    SceneManager.LoadScene(2);
                }
            }
            else if (Hp <= 100000)
            {
                Status.text = Hp + " left. Sometimes we waste our time on something useless. Invisible narrative.";
            }
            else
            {
                Mul *= 2;
            }
        }
    }

    public void Restart()
    {
        if (!IsLose)
        {
            IsLose = true;
            Status.text = "You can't see but you was still alive before you press. Now you are dead. Invisible irony.";
        }
        else
        {
            SceneManager.LoadScene(0);
        }
        
    }

    public void Skip()
    {
        if (!IsLose)
        {
            if (Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.LeftShift))
            {
                IsLose = true;
                Starter.IsSkipped = true;
                Status.text = "Invisible hope. You can't skip this level. Now you are dead. Invisible intrigue that it isn't the end and it doesn't even matter~♫.";
            }
            else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
            {
                IsLose = true;
                Status.text = "Invisible friend who should press another shift to help you";
            }
            else
            {
                IsLose = true;
                Status.text = "Invisible dead. You should press shift before you press.";
            }
        }
        
    }

    public void Think()
    {
        IsLose = true;
        Status.text = "Sorry but I don't give a *invisible word*!";
    }
}
