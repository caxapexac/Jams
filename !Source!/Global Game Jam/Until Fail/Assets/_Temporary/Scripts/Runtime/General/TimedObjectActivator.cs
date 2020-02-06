using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace UnityStandardAssets.Utility
{
    public class TimedObjectActivator : MonoBehaviour
    {
        public enum Action
        {
            Activate,
            Deactivate,
            Destroy,
            ReloadLevel,
            Call,
        }


        [Serializable]
        public class Entry
        {
            public GameObject target;
            public Action action;
            public float delay;
        }

        public List<Entry> entries;

        private void Awake()
        {
            foreach (Entry entry in entries)
            {
                switch (entry.action)
                {
                    case Action.Activate:
                        StartCoroutine(Activate(entry));
                        break;
                    case Action.Deactivate:
                        StartCoroutine(Deactivate(entry));
                        break;
                    case Action.Destroy:
                        Destroy(entry.target, entry.delay);
                        break;
                    case Action.ReloadLevel:
                        StartCoroutine(ReloadLevel(entry));
                        break;
                }
            }
        }

        private IEnumerator Activate(Entry entry)
        {
            yield return new WaitForSeconds(entry.delay);
            entry.target.SetActive(true);
        }

        private IEnumerator Deactivate(Entry entry)
        {
            yield return new WaitForSeconds(entry.delay);
            entry.target.SetActive(false);
        }

        private IEnumerator ReloadLevel(Entry entry)
        {
            yield return new WaitForSeconds(entry.delay);
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }
}
