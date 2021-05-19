using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace mactinite.FloatingText
{
    public class FloatingText : MonoBehaviour
    {
        public float riseSpeed = 0.25f;
        public float fadeTime = 0.5f;
        public Vector3 riseFrom;
        private Vector3 origin;
        public float height;
        private TMP_Text uiText;

        private float alpha;
        private float currentHeight = 0;
        private float xOffset = 0;
        private float timer;
        public bool started;
        public Canvas canvas;
        private void OnEnable()
        {
            transform.localScale = Vector3.one;
        }

        public void Init()
        {
            canvas = GetComponentInParent<Canvas>();
            transform.localScale = Vector3.one;
            xOffset = Random.Range(-10f, 10f);
            uiText = GetComponent<TMP_Text>();
            origin = riseFrom;
            alpha = 1;
            currentHeight = transform.position.y;
            timer = 0;

            Color color = uiText.color;
            color.a = alpha;
            uiText.color = color;
            StartOnTransform(riseFrom);
            started = true;
        }

        private void Update()
        {
            if (started)
            {
                StartOnTransform(riseFrom);
                UpdatePosition();
                UpdateAlpha();
            }
        }


        public void SetText(string text, Color color)
        {
            uiText.text = text;
            uiText.color = color;
        }


        public void UpdatePosition()
        {
            Vector2 newPosition = transform.position;
            newPosition.y += riseSpeed * Time.deltaTime;
            
        }

        public void UpdateAlpha()
        {
            timer += Time.deltaTime;
            if (timer < fadeTime)
            {
                Color color = uiText.color;
                alpha = 1 - (timer / fadeTime);
                color.a = alpha;
                uiText.color = color;
            }
            else
            {
                Color color = uiText.color;
                color.a = 0;
                uiText.color = color;
                gameObject.SetActive(false);
            }
        }

        public void StartOnTransform(Vector3 start)
        {
            RectTransform UI_Element = this.GetComponent<RectTransform>();

            Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, transform.position);
            Vector3 screenTargetPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, start);
            screenTargetPosition.x += xOffset;
            // is on screen
            if (screenTargetPosition.x > -0.01f && screenTargetPosition.x < Screen.width && screenTargetPosition.y > -0.01f && screenTargetPosition.y < Screen.height)
            {
                var screenPoint = Vector3.Lerp(screenPosition, screenTargetPosition, Time.deltaTime * 500f);
                var canvasRect = transform.parent as RectTransform;
                var inRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, canvas.worldCamera, out var position);
                transform.localPosition = position;
            }
            else
            {
                var screenPoint = Vector3.Lerp(screenPosition, screenTargetPosition, Time.deltaTime * 500f);
                var inRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPoint, null, out var position);
                var canvasRect = transform.parent as RectTransform;
                position = Vector2.ClampMagnitude(position, canvasRect.sizeDelta.y * .45f);
                transform.localPosition = position;
            }

            uiText.enabled = true;

        }
    }
}
