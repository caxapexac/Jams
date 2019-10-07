using System;
using System.Collections;
using MonoBehs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class QTE : MonoBehaviour
    {
        public Image Sprite;
        public TextMeshProUGUI ScoreBonus;
        public Transform Panel;
        public Transform SpacePanel;

        public float StartTime;
        public float MaxTime;
        public float Dist;
        public bool Started;
        public bool IsFirst;
        public float DebugTime;

        private void Start()
        {
            IsFirst = true;
        }

        private void Update()
        {
            DebugTime = Time.time;
            if (Input.GetKeyDown(KeyCode.Space) && Started)
            {
                God.I.Points(StopAnimation());
            }

            if (Started)
            {
                if (Time.time - StartTime < MaxTime)
                {
                    Sprite.color = Color.white;
                    Sprite.transform.localPosition =
                        Vector2.Lerp(-Vector2.up * Dist / 2, Vector2.up * Dist / 2, (Time.time - StartTime) / MaxTime);
                }
                else
                {
                    God.I.Points(0);
                }
            }
            else
            {
                Sprite.color = Color.red;
            }
        }

        public void StartAnimation()
        {
            StopAllCoroutines();
            MaxTime *= 0.9f;
            if (IsFirst)
            {
                SpacePanel.gameObject.SetActive(true);
            }

            Panel.gameObject.SetActive(true);
            Started = true;
            StartTime = Time.time;
        }

        public float StopAnimation()
        {
            Panel.gameObject.SetActive(false);
            Started = false;
            if (IsFirst)
            {
                SpacePanel.gameObject.SetActive(false);
                IsFirst = false;
            }

            float total = Time.time - StartTime;
            float res = 100f - 90 * Mathf.Abs(MaxTime / 2 - total) / (MaxTime / 2);
            StartCoroutine(ShowBonus(res));
            return res;
        }

        private IEnumerator ShowBonus(float t)
        {
            ScoreBonus.gameObject.SetActive(true);
            ScoreBonus.text = "+ " + t.ToString("F0");
            yield return new WaitForSeconds(1);
            ScoreBonus.gameObject.SetActive(false);
        }
    }
}