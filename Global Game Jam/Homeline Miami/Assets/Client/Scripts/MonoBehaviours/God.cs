using System.Collections;
using Client.Scripts.Algorithms.Legacy;
using Client.Scripts.Extensions;
using Client.Scripts.Scriptable;
using EZCameraShake;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


namespace Client.Scripts.MonoBehaviours
{
    public class God : MonoBehaviour
    {
        public float CameraMinSize;
        public float CameraBorder;

        public SettingsObject Settings;

        [HideInInspector]
        [Space]
        [Range(1, 2)]
        public int PlayerCount;

        [HideInInspector]
        public bool UseMouse;

        public RectTransform SecondPanel;

        [HideInInspector]
        public bool NoBlood;

        [HideInInspector]
        public bool GodMode;

        public int NextScore;
        public int Level;

        public float PlayerSpeed;
        public float FireCoolDown;

        [Space]
        public float MaxHp;

        public float HpRegenSpeed;

        public float MinMana;
        public float MaxMana;
        public float ManaSpendSpeed;
        public float ManaRegenSpeed;

        [Space]
        public float SkillSpeed;

        public float SkillRegenSpeed;
        public float SkillCap;
        public float SkillCoolDown;

        [Space]
        public float Difficulty;

        public float EnemyDelay;

        [Space]
        public Slider LevelSlider;

        public Slider HpSlider;
        public Slider ManaSlider;

        [Space]
        public string BulletPrefabPath;

        public string EnemyPrefabPath;

        public PlayerController Player1;
        public PlayerController Player2;
        public Transform BulletPoolTransform;
        public Transform EnemyPoolTransform;
        public Tilemap WallsMap;
        public int MapSize;

        public Transform Canvas;

        public GameObject GunPrefab;
        public GameObject LevelPrefab;
        public EnemyController EnemyController;

        public TextMeshProUGUI LevelText;

        public GunObject DefaultGun;
        
        [HideInInspector]
        public bool Lose;

        [HideInInspector]
        public PrefabPool<Bullet> BulletPool;

        [HideInInspector]
        public PrefabPool<Enemy> EnemyPool;

        [HideInInspector]
        public float CurrentHp;

        [HideInInspector]
        public float CurrentMana;

        public Camera Camera;

        public GameObject[] Bleed;

        [HideInInspector]
        public CameraShaker Shaker;

        [HideInInspector]
        public static God Instance;

        private string _skillAxis = "Skill";
        private float _skillCoolDown;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;

                //DontDestroyOnLoad(gameObject);
                Instance.Loaded();
            }
            else
            {
                Instance.Loaded();
                Destroy(gameObject);
            }
        }

        private void Loaded()
        {
            PlayerCount = Settings.PlayerCount;
            NoBlood = Settings.NoBlood;
            UseMouse = Settings.IsMouse;
            GodMode = Settings.GodMode;
            Shaker = Camera.GetComponent<CameraShaker>();
            //SplashText("Level " + Level + "!", Color.yellow);
            LevelSlider.maxValue = 1;
            HpSlider.maxValue = MaxHp;
            ManaSlider.maxValue = MaxMana;
            HpRegenSpeed += 0.5f;
            ManaSpendSpeed--;
            FireCoolDown -= 0.02f;
            BulletPool = new PrefabPool<Bullet>(BulletPrefabPath);
            EnemyPool = new PrefabPool<Enemy>(EnemyPrefabPath);
            CurrentHp = MaxHp;
            CurrentMana = MaxMana;
            LevelSlider.maxValue = NextScore;
            Lose = false;
            _skillCoolDown = 0;
            if (PlayerCount == 1)
            {
                Player2.gameObject.SetActive(false);
                SecondPanel.gameObject.SetActive(false);
            }
            Shaker.ShakeOnce(4f, 3f, 2f, 2f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
            float score = Player1.Score + Player2.Score;
            LevelSlider.value = Mathf.Lerp(LevelSlider.value, score, Time.deltaTime * 5);
            if (score >= NextScore)
            {
                EnemyDelay -= 0.2f;
                LevelSlider.minValue = NextScore;
                NextScore *= 2;
                Level++;
                PlayerSpeed++;
                CurrentHp = MaxHp;
                CurrentMana = MaxMana;
                SkillCap -= 0.02f;
                LevelText.text = "LVL " + Level;
                LevelSlider.maxValue = NextScore;
                SplashText("Level " + Level + "!", Color.yellow);
                for (int i = 0; i < Level; i++)
                {
                    EnemyController.SpawnEnemy();
                }
            }
            CurrentHp = Mathf.Min(CurrentHp + HpRegenSpeed * Time.deltaTime, MaxHp);
            HpSlider.value = Mathf.Lerp(HpSlider.value, CurrentHp, Time.deltaTime * 5);;
            ManaSlider.value = Mathf.Lerp(ManaSlider.value, CurrentMana, Time.deltaTime * 5);;
            if (CurrentHp < 0 && !Lose)
            {
                Lose = true;
                if (Player1.Score > Player2.Score)
                {
                    SplashText("YOU DIED", Color.red);
                }
                else
                {
                    SplashText("YOU DIED", Color.red);
                }
                StartCoroutine(Restart());
            }

            _skillCoolDown -= Time.deltaTime;
            if (_skillCoolDown < 0 && CurrentMana > MinMana && Input.GetAxis(_skillAxis) > 0)
            {
                Time.timeScale = Mathf.Max(SkillCap, Time.timeScale - SkillSpeed * Time.deltaTime);
                CurrentMana -= ManaSpendSpeed * Time.deltaTime;
                if (CurrentMana < MinMana)
                {
                    _skillCoolDown = SkillCoolDown;
                }
            }
            else
            {
                Time.timeScale = Mathf.Min(1, Time.timeScale + SkillRegenSpeed * Time.deltaTime);
                CurrentMana = Mathf.Min(MaxMana, CurrentMana + ManaRegenSpeed * Time.deltaTime);
            }

            if (PlayerCount == 1)
            {
                transform.position = Player1.transform.position.WithZ(-10);
            }
            else
            {
                Vector2 center = (Player1.transform.position + Player2.transform.position) / 2;
                transform.position = center.WithZ(-10);
                Camera.orthographicSize = CameraBorder
                    + Mathf.Max((Player1.transform.position - Player2.transform.position).magnitude / 2, CameraMinSize);
            }
        }

        private IEnumerator Restart()
        {
            yield return new WaitForSecondsRealtime(2.5f);
            SceneManager.LoadScene(1);
        }

        public void SplashText(string text, Color color)
        {
            GameObject go = Instantiate(LevelPrefab, Canvas);
            TextMeshProUGUI textMesh = go.GetComponent<TextMeshProUGUI>();
            textMesh.text = text;
            textMesh.color = color;
            Destroy(go, 3.3f);
        }
    }
}