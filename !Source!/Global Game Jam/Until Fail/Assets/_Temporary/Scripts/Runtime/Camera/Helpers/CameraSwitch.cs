using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviour
{
    public GameObject[] Cameras;
    public KeyCode NextCameraKey;
    public Text TextGui;

    private int currentActiveObject;

    private void OnEnable()
    {
        TextGui.text = Cameras[currentActiveObject].name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(NextCameraKey))
        {
            NextCamera();
        }
    }

    public void NextCamera()
    {
        int nextactiveobject = currentActiveObject + 1 >= Cameras.Length ? 0 : currentActiveObject + 1;

        for (int i = 0; i < Cameras.Length; i++)
        {
            Cameras[i].SetActive(i == nextactiveobject);
        }

        currentActiveObject = nextactiveobject;
        TextGui.text = Cameras[currentActiveObject].name;
    }
}
