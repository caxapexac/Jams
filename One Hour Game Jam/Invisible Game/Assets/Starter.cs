using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Starter : MonoBehaviour
{
    public static bool IsSkipped = false;

    public Text TextKill;
    // Start is called before the first frame update
    void Start()
    {
        if (IsSkipped)
        {
            God.IsNext = true;
            TextKill.text = "with spaces that kills me";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBut()
    {
        SceneManager.LoadScene(1);
    }
}
