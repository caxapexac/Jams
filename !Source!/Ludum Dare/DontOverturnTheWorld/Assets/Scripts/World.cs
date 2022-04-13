using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public sealed class World : MonoBehaviour
{
    [Header("Settings")]
    public float Timespan = 1;
    public float LoseDot = 0.5f;
    public int WinCount = 100;

    [Header("References")]
    public CanvasGroup LoseCanvas;
    public AudioSource LoseSound;
    public AudioSource WinSound;
    public Image ProgressBar;

    [Header("Read Only")]
    public float Utc;
    public bool IsGameOver = false;
    public bool IsWon = false;
    public bool IsAlreadyWon = false;
    public bool IsStartedLosing = false;

    [HideInInspector]
    public Earth Earth;
    private EllipseAnimator _ellipseAnimator;
    private List<Side> _sides = new List<Side>();

    private void Start()
    {
        Earth = FindObjectOfType<Earth>();
        _ellipseAnimator = GetComponent<EllipseAnimator>();
        _sides = GetComponentsInChildren<Side>().ToList();
        StartCoroutine(PlayCoroutine());
    }

    private void Update()
    {
        Utc = (Time.time * Timespan) % 360;
        _ellipseAnimator.Angle = -Utc;

        if (Vector3.Dot(Earth.transform.up, Vector3.up) < LoseDot)
        {
            IsGameOver = true;
            foreach (Side side in FindObjectsOfType<Side>())
            {
                side.Lose();
            }
        }

        int count = 0;
        foreach (Side side in _sides)
        {
            count += side.Humans.Count;
        }

        float winAmount = (float)count / WinCount;
        ProgressBar.fillAmount = Mathf.Clamp01(winAmount);
        if (winAmount > 1)
        {
            if (!IsWon)
            {
                IsWon = true;
                WinSound.Play();
            }
        }

        if (!IsWon)
        {
            if (!IsStartedLosing && IsGameOver)
            {
                IsStartedLosing = true;
                LoseSound.Play();
                StartCoroutine(LoseCoroutine());
            }
        }
    }

    private IEnumerator PlayCoroutine()
    {
        LoseCanvas.alpha = 1;
        LoseCanvas.DOFade(0, 2f);
        yield break;
    }

    private IEnumerator LoseCoroutine()
    {
        yield return new WaitForSeconds(3f);
        float startTime = Time.time;
        while (Time.time < startTime + 5f)
        {
            LoseCanvas.alpha = (Time.time - startTime) / 5f;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(0);
    }
}
