using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class God : MonoBehaviour
{
    public static God I;
    public static int Count = 0;
    public static int Record = 0;
    public static int Aim = 10;
    public static int Level = 0;
    public Text cur;
    public Text rec;
    public Text next;
    public Text lev;
    void Start()
    {
        I = this;
    }
    
    void Update()
    {
        if (Count > Record)
        {
            Record = Count;
        }
        if (Count >= Aim)
        {
            Level++;
            Aim *= 2;
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView + 4,0, 150);
        }
        transform.Rotate(new Vector3(0, -10 * Time.deltaTime * Input.GetAxis("Slide")));
        cur.text = "Bricks: " + Count;
        rec.text = "Record: " + Record;
        next.text = "Aim: " + Aim;
        lev.text = "Level: " + Level;
    }
}
