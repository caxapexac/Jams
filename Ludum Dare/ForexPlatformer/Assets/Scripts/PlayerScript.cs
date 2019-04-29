using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum PlayerStates
{
    Air,
    Exchange,
    Jumping,
    Lose,
    Won,
    Idle
}


public class PlayerScript : MonoBehaviour
{
    public PlayerStates State = PlayerStates.Air;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI RecordText;
    public Slider Slider;
    public Camera Camera;
    public ParticleSystem Particles;
    public float Score;
    public float PlayerTime;
    public GameObject Platforms;

    private ExchangeScript _exchange;
    private Rigidbody2D _rigidbody2D;
    private CircleCollider2D _circleCollider2D;
    private Vector2 _target;
    private float _currentSpeed;
    private float _jumpTime;
    private bool _isJumped;
    private bool _isRecord;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _currentSpeed = 0;
        _jumpTime = 0;
        Score = 100;
        Slider.minValue = 0;
        Slider.maxValue = 1000;
        _isRecord = false;
        Camera = Camera.main;
        _isJumped = false;
        PlayerTime = Time.time;
        God.G.PlayerScript = this;
    }

    void Update()
    {
        Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize,
            8 + Mathf.Abs(_currentSpeed) * 3 + Convert.ToInt32(State == PlayerStates.Jumping) * 3, Time.deltaTime);
        ScoreText.text = Score.ToString("F3");
        TimeText.text = (Time.time - PlayerTime).ToString("F3") + "s";
        ScoreText.color = Color.Lerp(Color.red, Color.green, Score / 1000);
        Slider.value = Mathf.Clamp(Score, 0, 1000);
        float axis = Input.GetAxis("Horizontal");
        if (Mathf.Abs(axis) > 0.001f)
        {
            _currentSpeed = Mathf.Clamp(God.G.S.MoveSpeed * axis * Time.deltaTime, -God.G.S.MaxSpeed, God.G.S.MaxSpeed);
        }
        if (transform.position.y < -50)
        {
            transform.position = transform.position + Vector3.up * 150;
        }
        switch (State)
        {
            case PlayerStates.Air:
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (1 + Score / 1000), Time.deltaTime * 10);
                _rigidbody2D.velocity += Vector2.right * _currentSpeed;
                break;
            case PlayerStates.Exchange:

                //Debug.Log("Exchange");
                transform.position = Vector3.Lerp(transform.position, _target, Time.deltaTime * 2);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 0.3f, Time.deltaTime * 2);
                if (Input.GetAxis("Jump") > 0)
                {
                    _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    State = PlayerStates.Jumping;
                    _rigidbody2D.velocity = Vector2.right * _currentSpeed;
                    _jumpTime = Time.time;
                    _isJumped = false;
                }
                if (_exchange)
                {
                    Score += (_exchange.Current - 0.5f) * Time.deltaTime * _exchange.Course.Difficulty * 3;
                    if (Score <= 0)
                    {
                        Score = 0;
                        State = PlayerStates.Lose;
                        break;
                    }
                    else if (Score >= 1000)
                    {
                        Score = 1000;
                        State = PlayerStates.Won;
                        break;
                    }
                    _exchange.Obuse += Time.deltaTime;
                    _exchange.Current -= Time.deltaTime / 10f;
                    if ((_exchange.Obuse > 10 || _exchange.Current <= 0) && _exchange.Course.Chance != 0)
                    {
                        _exchange.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        _exchange.Particles.Play();
                        _exchange.GetComponent<BoxCollider2D>().enabled = false;
                        _exchange = null;
                        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                        State = PlayerStates.Jumping;
                        _rigidbody2D.velocity = Vector2.right * _currentSpeed;
                        _jumpTime = Time.time;
                        _isJumped = false;
                    }
                }
                break;
            case PlayerStates.Jumping:

                //Debug.Log("Jumping");
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 10);
                _rigidbody2D.velocity += Vector2.right * _currentSpeed;
                if (Time.time - _jumpTime < God.G.S.JumpTime)
                {
                    if (!_isJumped && Input.GetKeyDown(KeyCode.Space))
                    {
                        //_jumpTime = Time.time;
                        _isJumped = true;
                    }
                    _rigidbody2D.velocity += Vector2.up
                        * God.G.S.JumpSpeed
                        * (Mathf.Sqrt(God.G.S.JumpTime - Time.time + _jumpTime) / God.G.S.JumpTime)
                        * Time.deltaTime;
                }
                else
                {
                    State = PlayerStates.Air;
                }
                break;
            case PlayerStates.Lose:
                //Debug.Log("LOSE");
                Particles.Play();
                God.G.Record = Time.time - PlayerTime;
                God.G.ShowScores();
                State = PlayerStates.Idle;
                break;
            case PlayerStates.Won:
                God.G.Record = Time.time - PlayerTime;
                God.Database.SetValueAsync(new List<object> {God.G.NameIn.text == "" ? "%username%" : God.G.NameIn.text, God.G.Record.ToString("F3")});

                State = PlayerStates.Idle;
                //Debug.Log("WON");
                
                StartCoroutine(Win());
                break;
            case PlayerStates.Idle:
                TimeText.text = "";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator Win()
    {
        float tick = Time.time;
        foreach (Rigidbody2D rigid in Platforms.gameObject.GetComponentsInChildren<Rigidbody2D>())
        {
            rigid.bodyType = RigidbodyType2D.Dynamic;
        }
        God.G.ShowScores();
        while (Time.time - tick < 5)
        {
            Camera.orthographicSize += Time.deltaTime;
            yield return null;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (State == PlayerStates.Idle)
        {
            return;
        }
        _exchange = other.gameObject.GetComponent<ExchangeScript>();
        if (_exchange)
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            State = PlayerStates.Exchange;
            _target = other.transform.position;
        }
    }
}