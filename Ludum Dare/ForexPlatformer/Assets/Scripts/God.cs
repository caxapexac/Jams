using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class user
{
    public string name;
    public int score;
    public float time;

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> res = new Dictionary<string, object>()
        {
            {"name", name},
            {"score", score},
            {"time", time}
        };
        return res;
    }
}


public class God : MonoBehaviour
{
    public Settings S;
    public static God G = null;
    public static DatabaseReference Database;
    public GameObject Panel;
    public UserScript User;
    public GameObject Times;
    public GameObject Chats;
    public GameObject ChatPanel;
    public GameObject Scores;
    public PlayerScript PlayerScript;
    public TextMeshProUGUI EventText;
    public TextMeshProUGUI MessText;
    public InputField MessIn;
    public InputField NameIn;
    public bool Ended = false;
    public bool ListReady = false;
    public List<string> ListNames;
    public List<string> ListScores;
    public string Username;
    public float Record;
    public bool isHide;


    private void Awake()
    {
        G = this;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://currency-ludum.firebaseio.com/");
        Database = FirebaseDatabase.DefaultInstance.RootReference.Child("leaderboard").Push();
        Database.Root.Child("leaderboard").ChildAdded += (sender, args) =>
        {
            List<object> elems = (List<object>)args.Snapshot.GetValue(false);
            EventText.text = "Last record: " + (string)elems[0] + " " + (string)elems[1] + "!";
            //StartCoroutine(CloseEvent());
        };
        Database.Root.Child("chat").ChildAdded += (sender, args) =>
        {
            List<object> elems = (List<object>)args.Snapshot.GetValue(false);
            TextMeshProUGUI t = Instantiate(MessText, Vector3.zero, Quaternion.identity, Chats.transform);
            t.text = (string)elems[0] + ": " + (string)elems[1];
        };
        /*Database.Child("HelloWorld")
            .SetValueAsync(System.DateTime.Now.DayOfYear
                + "_"
                + System.DateTime.Now.Hour
                + "_"
                + System.DateTime.Now.Minute
                + "_"
                + System.DateTime.Now.Second);*/

        /*if (Username == null)
        {
            if (!PlayerPrefs.HasKey("Name"))
            {
                SceneManager.LoadScene(1);
            }
            Username = PlayerPrefs.GetString("Name");
            Database.Child("name").SetValueAsync(Username);
        }*/
        ListScores = new List<string>();
        ListNames = new List<string>();
    }

    public void Hide()
    {
        ChatPanel.transform.position = ChatPanel.transform.position -= isHide ? -Vector3.up * 300 : Vector3.up * 300;
        isHide = !isHide;
    }

    public void Send()
    {
        if (MessIn.text != "")
        {
            Database.Root.Child("chat")
                .Push()
                .SetValueAsync(new List<object> {NameIn.text == "" ? "%username%" : NameIn.text, MessIn.text});
            MessIn.text = "";
            MessIn.Select();
            MessIn.ActivateInputField();
        }
    }

    private IEnumerator CloseEvent()
    {
        yield return new WaitForSeconds(8);
        EventText.text = "";
    }

    void Start()
    {
        //ClearDatabase();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Send();
        }
        if (ListReady)
        {
            ListReady = false;
            int j;
            int n = ListNames.Count;
            for (j = n - 1; j > 0; j--)
            {
                int k;
                for (k = 0; k < j; k++)
                {
                    if (float.Parse(ListScores[k]) > float.Parse(ListScores[k + 1]))
                    {
                        string temporary = ListScores[k];
                        ListScores[k] = ListScores[k + 1];
                        ListScores[k + 1] = temporary;
                        temporary = ListNames[k];
                        ListNames[k] = ListNames[k + 1];
                        ListNames[k + 1] = temporary;
                    }
                }
            }
            int i = 1;
            for (j = 0; j < ListScores.Count; j++)
            {
                UserScript us = Instantiate(User, Vector3.zero, Quaternion.identity, Scores.transform);
                us.ScoreText.text = ListScores[j];
                us.NameText.text = ListNames[j];
                us.NumberText.text = i++.ToString();
            }
            Panel.SetActive(true);
            Debug.Log("Records");
        }
    }

    private void ClearDatabase()
    {
        Database.Root.SetRawJsonValueAsync("");
    }

    public void ShowScores()
    {
        if (Ended) return;
        Ended = true;
        Database.Root.Child("leaderboard")
            .GetValueAsync()
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("Failed");
                }
                else if (task.IsCompleted)
                {
                    foreach (var user in task.Result.Children)
                    {
                        List<object> elems = (List<object>)user.GetValue(false);
                        ListNames.Add((string)elems[0]);
                        ListScores.Add((string)elems[1]);
                    }
                }
                ListReady = true;
            });
    }

    public void Restart()
    {
        StopAllCoroutines();
        PlayerScript.StopAllCoroutines();
        /*Database.Child("Restart")
            .Child(System.DateTime.Now.DayOfYear
                + "_"
                + System.DateTime.Now.Hour
                + "_"
                + System.DateTime.Now.Minute
                + "_"
                + System.DateTime.Now.Second)
            .SetValueAsync(((int)PlayerScript.Score).ToString());*/
        SceneManager.LoadScene(0);
    }
}