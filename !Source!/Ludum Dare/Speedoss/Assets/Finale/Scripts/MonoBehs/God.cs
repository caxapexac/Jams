using System;
using System.Collections;
using System.Threading;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MonoBehs
{
    public enum GameState
    {
        Menu,
        Start,
        Walk,
        Wheel,
        Playing,
        Finish,
        CutScene
    }

    public class God : MonoBehaviour
    {
        public static God I;

        public int Score;

        public AudioSource BackAudio;
        public AudioSource DejaVu;
        public AudioSource CarSounds;
        public AudioSource Signals;
        public AudioSource MenuMusic;

        public TextMeshProUGUI ScoreText;
        public TextMeshProUGUI CountText;
        public TextMeshProUGUI NameText;
        public Transform NameT;
        public Transform WheelPrefab;
        public Transform WheelInstance;

        public Transform AnchorMenu;
        public Transform AnchorThird;
        public Transform AnchorFirst;
        public Transform AnchorSecond;
        public Transform AnchorPlayer;
        [Space] public Transform Viewer;
        public Transform Player;
        [HideInInspector] public Transform CameraT;

        public GameState State;
        [HideInInspector] public CameraScript CameraScript;

        [HideInInspector] public PlayerScript PlayerScript;
        [HideInInspector] public CarPartScript CarPartScript;
        public QTE Quick;

        public float Speed;
        public float BoostUp;
        public float BoostDown;
        public float MaxSpeed;
        public float Deviation;

        public float HorizontalSpeed;
        public float HorizontalBoost;
        public float MaxHorizontalSpeed;

        public Vector3 StartingPos;

        private void Awake()
        {
            Score = 0;
            if (I)
            {
                I = this;
                //Destroy(gameObject);
            }
            else
            {
                I = this;
            }
        }

        private void Start()
        {
            MenuMusic.Play();
            CameraT = Camera.main.transform;
            CameraScript = CameraT.GetComponent<CameraScript>();
            PlayerScript = Player.GetComponent<PlayerScript>();
            CarPartScript = Player.GetComponent<CarPartScript>();
            Viewer = GetComponent<EndlessTerrain>().viewer;
            Speed = 0;
        }

        private void Update()
        {
            ScoreText.text = Score + " PTS";
            var localPosition = AnchorPlayer.transform.localPosition;
            localPosition = new Vector3(PlayerScript.DeLorean.localPosition.x / 2, localPosition.y, localPosition.z);
            AnchorPlayer.transform.localPosition = localPosition;

            switch (State)
            {
                case GameState.Menu:
                    CameraScript.Aim = AnchorMenu;
                    NameText.gameObject.SetActive(true);
                    Viewer.Translate(Time.deltaTime * 5 * Vector3.forward);
                    if (Input.anyKeyDown)
                    {
                        StartCoroutine(MusicOff(MenuMusic));
                        StartCoroutine(FadeOut(NameT, 2));
                        StartCoroutine(Countdown());
                        State = GameState.CutScene;
                    }

                    break;
                case GameState.Start:
                    StartCoroutine(AimDelay(AnchorPlayer, 6));
                    MaxSpeed = 5;
                    StartingPos = Player.transform.position;
                    WheelInstance = Instantiate(WheelPrefab, Player.position + Vector3.forward * 40,
                        Quaternion.Euler(0, 90, 0));
                    //CountText.gameObject.SetActive(true);
                    //CountText.text = "W";
                    BackAudio.volume = 1 - Mathf.Clamp01((Viewer.transform.position.z - StartingPos.z) / 40);
                    DejaVu.volume = Mathf.Clamp01(Mathf.Pow((Viewer.transform.position.z - StartingPos.z) / 40, 2));
                    BackAudio.PlayDelayed(2);
                    DejaVu.time = 60;
                    //DejaVu.Play();
                    //StartCoroutine(FalloutText(CountText, 1));
                    State = GameState.Walk;
                    break;
                case GameState.Walk:
                    CameraScript.axes = CameraScript.RotationAxes.MouseXAndY;
                    BackAudio.volume = 1 - Mathf.Clamp01((Player.transform.position.z - StartingPos.z) / 60);
                    DejaVu.volume = Mathf.Clamp01(Mathf.Pow((Viewer.transform.position.z - StartingPos.z) / 40, 3));
                    if (Input.GetKey(KeyCode.W))
                    {
                        //Speed += 2 * Time.deltaTime;
                        Viewer.Translate(Time.deltaTime * 2 * Vector3.forward);
                        if (Player.transform.position.z > StartingPos.z + 40)
                        {
                            BackAudio.Stop();
                            DejaVu.volume = 0f;
                            DejaVu.Play();
                            //DejaVu.PlayScheduled(AudioSettings.dspTime + 1f);
                            StartingPos = Player.transform.position;
                            CameraScript.Speed /= 6f;
                            State = GameState.Wheel;
                            MaxSpeed = 10;
                        }
                    }
                    else
                    {
                        Speed *= Time.deltaTime;
                    }

                    break;
                case GameState.Wheel:
                    DejaVu.volume = Mathf.Lerp(DejaVu.volume, 1f, Time.deltaTime);
                    PlayerScript.Cat1.gameObject.SetActive(false);
                    PlayerScript.Cat2.gameObject.SetActive(true);
                    if (WheelInstance)
                    {
                        WheelInstance.gameObject.SetActive(false);
                        WheelInstance = null;
                    }

                    CameraScript.Aim = AnchorSecond;
                    CameraScript.Cam.fieldOfView = 60 + (20 * (Speed / MaxSpeed));
                    if (Input.GetKey(KeyCode.W))
                    {
                        Speed += 2 * Time.deltaTime;
                        Viewer.Translate(Time.deltaTime * Speed * Vector3.forward);
                        if (Player.transform.position.z > StartingPos.z + 100)
                        {
                            //CameraScript.Speed *= 3f;
                            CameraScript.maximumX = 3360;
                            CameraScript.minimumX = -3360;
                            State = GameState.Playing;
                        }
                    }
                    else
                    {
                        Speed *= Time.deltaTime;
                    }

                    break;
                case GameState.Playing:
                    
                    ScoreText.gameObject.SetActive(true);
                    PlayerScript.Cat2.gameObject.SetActive(false);
                    PlayerScript.DeLorean.gameObject.SetActive(true);
                    MaxSpeed = Mathf.Lerp(MaxSpeed, 10 + 5 * CarPartScript.CurrentPart, 0.3f);
                    CameraScript.Aim = AnchorThird;
                    if (WheelInstance)
                    {
                        if (!Quick.Started && Player.transform.position.z > StartingPos.z + (CarPartScript.CurrentPart + 1) * 50)
                        {
                            Quick.StartAnimation();
                            
                        }
                    }
                    else
                    {
                        StartingPos = Player.position;
                        WheelInstance = Instantiate(CarPartScript.CarParts[CarPartScript.CurrentPart],
                            Player.position + (CarPartScript.CurrentPart + 1) * 50 * Vector3.forward,
                            Quaternion.Euler(0, 90, 0));

                        //Spawn
                    }

                    if (Input.GetAxis("Vertical") > 0)
                    {
                        Speed = Mathf.Clamp(Speed + BoostUp * Input.GetAxis("Vertical") * Time.deltaTime, 0, MaxSpeed);
                    }
                    else
                    {
                        Speed = Mathf.Clamp(Speed - BoostDown * Time.deltaTime, 0, MaxSpeed);
                    }

                    Viewer.Translate(Time.deltaTime * Speed * Vector3.forward);
                    CameraScript.Cam.fieldOfView = 60 + (30 * (Speed / MaxSpeed));
                    if (Input.GetAxis("Horizontal") != 0)
                    {
                        HorizontalSpeed = Mathf.Clamp(
                            HorizontalSpeed + Input.GetAxis("Horizontal") * HorizontalBoost * Time.deltaTime + (Random.value - 0.5f) * Deviation * Time.deltaTime,
                            -MaxHorizontalSpeed,
                            MaxHorizontalSpeed);
                    }
                    else
                    {
                        HorizontalSpeed *= 0.9f * Time.deltaTime;
                    }

                    if (PlayerScript.DeLorean.localPosition.x > 4.5f)
                    {
                        HorizontalSpeed = Mathf.Clamp(HorizontalSpeed - Time.deltaTime, -MaxHorizontalSpeed, 0);
                    }
                    else if (PlayerScript.DeLorean.localPosition.x < -4.5f)
                    {
                        HorizontalSpeed = Mathf.Clamp(HorizontalSpeed + Time.deltaTime, 0, MaxHorizontalSpeed);
                    }

                    PlayerScript.DeLorean.Translate(Time.deltaTime * Mathf.Sqrt(Mathf.Abs(Speed)) * HorizontalSpeed *
                                                    Vector3.right);
                    break;
                case GameState.Finish:
                    ScoreText.gameObject.SetActive(false);
                    //CameraScript.Aim = CameraScript.transform;
                    StartCoroutine(Restart());
                    State = GameState.Playing;
                    break;
                case GameState.CutScene:
                    //CameraScript.Aim = AnchorSecond;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator MusicOff(AudioSource source)
        {
            while (source.volume > 0.01f)
            {
                source.volume = Mathf.Lerp(source.volume, 0, Time.deltaTime);
                yield return null;
            }
            source.Stop();
        }

        public void Points(float f)
        {
            if (f > 0)
            {
                Score += (int)f;
                //ScoreText.text = Score.ToString();
                WheelInstance.gameObject.SetActive(false);
                CarPartScript.CarParts[CarPartScript.CurrentPart].gameObject.SetActive(true);
            }
            CarPartScript.CurrentPart++;
            WheelInstance = null;
        }

        private IEnumerator Restart()
        {
            float t = Time.time;
            while (Time.time - t < 10)
            {
                AnchorThird.Translate(Vector3.up * Time.deltaTime);
                yield return null;
            }
            SceneManager.LoadScene(0);
        }

        private IEnumerator Countdown()
        {
            PlayerScript.Wheel.gameObject.SetActive(true);
            CameraScript.Speed += 1;
            CameraScript.Aim = AnchorFirst;
            yield return new WaitForSeconds(3);
            CountText.gameObject.SetActive(true);
            CameraScript.Speed -= 1;
            Signals.Play();
            CountText.text = "3";
            yield return new WaitForSeconds(0.95f);
            CountText.text = "2";
            yield return new WaitForSeconds(0.95f);
            CountText.text = "1";
            yield return new WaitForSeconds(1.05f);
            CountText.text = "GO";
            CarSounds.Play();
            StartCoroutine(FalloutText(CountText, 1));
            State = GameState.Start;
        }

        private IEnumerator Effect()
        {
            yield return null;
        }
        private IEnumerator FalloutText(TextMeshProUGUI text, float t)
        {
            yield return new WaitForSeconds(t);
            text.gameObject.SetActive(false);
        }

        private IEnumerator FadeOut(Transform obj, float t)
        {
            float cur = Time.time;
            Vector3 start = obj.localScale;
            while (obj.localScale.x > 0.001f)
            {
                obj.localScale = Vector3.Lerp(start, Vector3.zero, (Time.time - cur) / t);
                yield return null;
            }

            obj.gameObject.SetActive(false);
        }

        private IEnumerator AimDelay(Transform t, float f)
        {
            yield return new WaitForSeconds(f);
            CameraScript.Aim = t;
        }
    }
}