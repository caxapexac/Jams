using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.Effects;


namespace UnityStandardAssets.SceneUtils
{
    public class ParticleSceneControls : MonoBehaviour
    {
        public enum Mode
        {
            Activate,
            Instantiate,
            Trail
        }

        public enum AlignMode
        {
            Normal,
            Up
        }


        public DemoParticleSystemList DemoParticles;
        public float SpawnOffset = 0.5f;
        public float Multiply = 1;
        public bool ClearOnChange = false;
        public Text TitleText;
        public Transform SceneCamera;
        public Text InstructionText;
        public Button PreviousButton;
        public Button NextButton;
        public GraphicRaycaster GraphicRaycaster;
        public EventSystem EventSystem;


        private ParticleSystemMultiplier particleMultiplier;
        private List<Transform> currentParticleList = new List<Transform>();
        private Transform instance;
        private Vector3 camOffsetVelocity = Vector3.zero;
        private Vector3 lastPos;
        private static int s_SelectedIndex = 0;
        private static DemoParticleSystem selected;


        private void Awake()
        {
            Select(s_SelectedIndex);

            PreviousButton.onClick.AddListener(Previous);
            NextButton.onClick.AddListener(Next);
        }


        private void OnDisable()
        {
            PreviousButton.onClick.RemoveListener(Previous);
            NextButton.onClick.RemoveListener(Next);
        }


        private void Previous()
        {
            s_SelectedIndex--;
            if (s_SelectedIndex == -1)
            {
                s_SelectedIndex = DemoParticles.items.Length - 1;
            }
            Select(s_SelectedIndex);
        }


        public void Next()
        {
            s_SelectedIndex++;
            if (s_SelectedIndex == DemoParticles.items.Length)
            {
                s_SelectedIndex = 0;
            }
            Select(s_SelectedIndex);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Previous();

            if (Input.GetKeyDown(KeyCode.RightArrow))
                Next();

            SceneCamera.localPosition = Vector3.SmoothDamp(SceneCamera.localPosition, Vector3.forward * -selected.camOffset, ref camOffsetVelocity, 1);

            if (selected.mode == Mode.Activate)
            {
                // this is for a particle system that just needs activating, and needs no interaction (eg, duststorm)
                return;
            }

            if (CheckForGuiCollision()) return;

            bool oneShotClick = (Input.GetMouseButtonDown(0) && selected.mode == Mode.Instantiate);
            bool repeat = (Input.GetMouseButton(0) && selected.mode == Mode.Trail);

            if (oneShotClick || repeat)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var rot = Quaternion.LookRotation(hit.normal);

                    if (selected.align == AlignMode.Up)
                    {
                        rot = Quaternion.identity;
                    }

                    var pos = hit.point + hit.normal * SpawnOffset;

                    if ((pos - lastPos).magnitude > selected.minDist)
                    {
                        if (selected.mode != Mode.Trail || instance == null)
                        {
                            instance = (Transform)Instantiate(selected.transform, pos, rot);

                            if (particleMultiplier != null)
                            {
                                instance.GetComponent<ParticleSystemMultiplier>().multiplier = Multiply;
                            }

                            currentParticleList.Add(instance);

                            if (selected.maxCount > 0 && currentParticleList.Count > selected.maxCount)
                            {
                                if (currentParticleList[0] != null)
                                {
                                    Destroy(currentParticleList[0].gameObject);
                                }
                                currentParticleList.RemoveAt(0);
                            }
                        }
                        else
                        {
                            instance.position = pos;
                            instance.rotation = rot;
                        }

                        if (selected.mode == Mode.Trail)
                        {
                            var emission = instance.transform.GetComponent<ParticleSystem>().emission;
                            emission.enabled = false;
                            instance.transform.GetComponent<ParticleSystem>().Emit(1);
                        }

                        instance.parent = hit.transform;
                        lastPos = pos;
                    }
                }
            }
        }


        bool CheckForGuiCollision()
        {
            PointerEventData eventData = new PointerEventData(EventSystem);
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> list = new List<RaycastResult>();
            GraphicRaycaster.Raycast(eventData, list);
            return list.Count > 0;
        }

        private void Select(int i)
        {
            selected = DemoParticles.items[i];
            instance = null;
            foreach (var otherEffect in DemoParticles.items)
            {
                if ((otherEffect != selected) && (otherEffect.mode == Mode.Activate))
                {
                    otherEffect.transform.gameObject.SetActive(false);
                }
            }
            if (selected.mode == Mode.Activate)
            {
                selected.transform.gameObject.SetActive(true);
            }
            particleMultiplier = selected.transform.GetComponent<ParticleSystemMultiplier>();
            Multiply = 1;
            if (ClearOnChange)
            {
                while (currentParticleList.Count > 0)
                {
                    Destroy(currentParticleList[0].gameObject);
                    currentParticleList.RemoveAt(0);
                }
            }

            InstructionText.text = selected.instructionText;
            TitleText.text = selected.transform.name;
        }


        [Serializable]
        public class DemoParticleSystem
        {
            public Transform transform;
            public Mode mode;
            public AlignMode align;
            public int maxCount;
            public float minDist;
            public int camOffset = 15;
            public string instructionText;
        }

        [Serializable]
        public class DemoParticleSystemList
        {
            public DemoParticleSystem[] items;
        }
    }
}
