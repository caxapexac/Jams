using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NameScript : MonoBehaviour
{
    public InputField InputField;
    public void Ok()
    {
        PlayerPrefs.SetString("Name", InputField.text);
        SceneManager.LoadScene(0);
    }
}
