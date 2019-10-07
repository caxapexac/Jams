using UnityEngine;
using UnityEngine.UI;


public class Diff : MonoBehaviour
{
    public static Diff Instance;

    public PlayerScript Player;
    public BossScript Boss;
    public Text Level;
    
    public int Difficulty = 0;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Instance.LevelUp();
        }
        else
        {
            Instance.LevelUp();
            Destroy(gameObject);
        }
    }

    private void LevelUp()
    {
        Level = GameObject.FindGameObjectWithTag("Level").GetComponent<Text>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        Boss = Player.Boss;
        Level.text = "Level: " + Difficulty;
        Player.BulletSpeed *= Difficulty / 2f;
        Player.Delay *= Difficulty / 2f;
        Player.BulletSpeedMultipiler *= Difficulty / 2f;
        Boss.BulletSpeed *= Difficulty;
        Boss.Delay /= Difficulty;
        Boss.BulletSpeedMultipiler *= Difficulty;
        Boss.Hp *= Difficulty;
        Boss.Speed *= (Difficulty + 10) / 10f;
    }
}