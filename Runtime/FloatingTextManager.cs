using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mactinite.FloatingText
{
    public class FloatingTextManager : SingletonMonobehavior<FloatingTextManager>
    {

        public Transform textPrefab;
        public Transform parent;
        public int preload = 25;

        private List<Transform> floatingTextPool = new List<Transform>();
        private List<Transform> sleepingText = new List<Transform>();
        private void Awake()
        {
            for (int i = 0; i < preload; i++)
            {
                var text = Instantiate(textPrefab);
                text.transform.position = Vector2.zero;
                text.transform.SetParent(parent);
                text.gameObject.SetActive(false);
                floatingTextPool.Add(text.transform);
            }
        }

        private void FixedUpdate()
        {
            GetSleepingText();
        }

        public void GetSleepingText()
        {
            foreach (var obj in floatingTextPool)
            {
                if (!obj.gameObject.activeSelf)
                {
                    sleepingText.Add(obj);
                }
            }
        }

        public static void SpawnText(string text, Color textColor, Vector3 spawnOn)
        {
            FloatingTextManager.Instance.InstantiateText(text, textColor, spawnOn);
        }

        public void InstantiateText(string text, Color textColor, Vector3 spawnOn)
        {
            Transform textTransform = null;
            if (sleepingText.Count > 0)
            {
                textTransform = sleepingText[0];
                sleepingText.RemoveAt(0);
            }
            else
            {
                textTransform = Instantiate(textPrefab);
                floatingTextPool.Add(textTransform);
            }

            if (textTransform != null)
            {
                textTransform.position = Vector3.zero;
                textTransform.SetParent(parent, false);
                FloatingText floatingText = textTransform.GetComponent<FloatingText>();
                floatingText.riseFrom = spawnOn;
                floatingText.Init();
                floatingText.SetText(text, textColor);
                floatingText.started = true;
                floatingTextPool.Add(textTransform);
                textTransform.gameObject.SetActive(true);

            }

        }
    }
}
