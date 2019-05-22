using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Restarter : MonoBehaviour
{
    public Text WinText;
    // Start is called before the first frame update
    void Start()
    {
        WinText.text += Time.time + " seconds of our life ;)";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        God.Hp = 1000000;
        God.Mul = 1;
        God.IsNext = false;
        Starter.IsSkipped = false;
    }
}
