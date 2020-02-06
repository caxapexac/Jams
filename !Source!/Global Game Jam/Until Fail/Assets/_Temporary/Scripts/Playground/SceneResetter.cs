using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetter : MonoBehaviour
{
    public KeyCode ResetButton;

    private void Update()
    {
        // if we have forced a reset
        if (Input.GetKeyDown(ResetButton))
        {
            // reload the scene
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }
}
