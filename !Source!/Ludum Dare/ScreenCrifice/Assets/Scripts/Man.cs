
using UnityEngine;
using UnityEngine.UI;

public class Man : MonoBehaviour
{
    public bool Alive = false;
    public bool SkipScenes = true;
    public bool AutoClick = true;
    public int WorldSize = 128;
    public int PixelSize = 16;
    public float BlockSize = 0.32f;
    public int Octaves = 5;
    public float Persistance = 0.2f;
    public int DistanceW = 30;
    public int DistanceH = 20;
    [HideInInspector]
    public float DistanceWMax;
    [HideInInspector]
    public float DistanceHMax;
    [HideInInspector]
    public int ScreenH;
    [HideInInspector]
    public int ScreenW;
    public float RenderTick = 1;

    public float GlobalTime;
    public float Coal;
    public float Iron;
    public float Gold;
    public float Rocks;
    public float Uran;
    public float Wood;
    public int CoalSize;
    public int IronSize;
    public int GoldSize;
    public int RocksSize;
    public int UranSize;
    public int WoodSize;
    
    public int Saeed = 0;
    public float Difficulty = 10;
    public float DiffMultipiler = 0.99f;
    
    public float SpeedMultipiler = 2;
    public float SlowMultipiler = 0.6f;
    public bool SlowedDown = false;
    public int IdleMiner;
    public float HoldTick;
    public float HoldTime;
    
    public int[] Costs;
    public float ResourceMultipiler;
    public int InventMultipiler;
    public int InventCost;
    public float InventSpeed;
    public float RocketDelay;
    public float RocketFrequency;
    public float RocketSpeed;
    public int RocketDamage;
    public Slider Progress;
    public Button[] Sacrifs;

    public Vector2 MousePos = Vector2.zero;

    public Text[] Inventory;
    
    public Text TextRes;
    public Text ProgText;

    public AudioSource ErrSound;
    public GameObject ErrImage;

    public GameObject Tutorial;
    public GameObject Res;
    public RectTransform ResTransform;
    public GameObject ParticleBreak;
    public GameObject TextPrefab;
    public GameObject HeadPrefab;
    public GameObject BlockPrefab;
    public InputManager InputM;
    public MouseManager MouseM;
    public DeathManager DeathM;
    public PlayerManager PlayerM;
    public WorldManager WorldM;
    public RocketManager RocketM;

    private float _tick = 0;
    
    private void Awake()
    {
        InputM = GetComponent<InputManager>();
        MouseM = GetComponent<MouseManager>();
        WorldM = GetComponent<WorldManager>();
        PlayerM = GetComponent<PlayerManager>();
        DeathM = GetComponent<DeathManager>();
        RocketM = GetComponent<RocketManager>();
        PlayerM.Player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerM.Animator = PlayerM.Player.GetComponentInChildren<Animator>();
        PlayerM.PlayerHead = Instantiate(HeadPrefab, PlayerM.Player.transform).transform;
        PlayerM.Res = new[] {0, 0, 0, 0, 0, 0};
        if (Saeed == 0) Saeed = (int) (Random.value * 100000);
        ScreenW = Screen.width + PixelSize / 2;
        ScreenH = Screen.height + PixelSize / 2;
        DistanceWMax = DistanceW * BlockSize;
        DistanceHMax = DistanceH * BlockSize;
        ResTransform = Res.GetComponent<RectTransform>();

    }
    

    void Update()
    {
        _tick += Time.deltaTime;
        if (_tick >= InventSpeed)
        {
            if (GlobalTime > 20)
            {
                Tutorial.SetActive(false);
            }
            ProgText.text = Progress.value + " / " + Progress.maxValue + " for " + InventMultipiler + " pixels";
            for (int i = 0; i < PlayerM.Res.Length; i++)
            {
                PlayerM.Res[i] += IdleMiner;
                if (PlayerM.Res[i] > InventCost)
                {
                    PlayerM.Res[i] -= InventCost;
                    Inventory[i].text = PlayerM.Res[i].ToString();
                    Progress.value += 1;
                    if (Progress.value >= Progress.maxValue)
                    {
                        for (int j = 0; j < InventMultipiler; j++)
                        {
                            DeathM.FixPixel();
                        }

                        InventCost += 1;
                        Progress.value = 0;
                        Progress.maxValue += 1;
                    }
                }
            }
            _tick = 0;
        }
    }
}
