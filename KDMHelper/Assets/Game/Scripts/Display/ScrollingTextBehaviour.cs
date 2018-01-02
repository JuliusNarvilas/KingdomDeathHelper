using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Display
{
    [RequireComponent(typeof(UnityEngine.UI.Text), typeof(RectTransform))]
    public class ScrollingTextBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float m_RepeatDelay = 1.0f;
        [SerializeField]
        private float m_ScrollSpeed = 0.1f;
        [SerializeField]
        private float m_FadeSpeed = 0.5f;
        [SerializeField]
        private bool m_ScrollHorizontally = true;
        [SerializeField]
        private bool m_ScrollVertically = false;

        private UnityEngine.UI.Text m_ScrollingText;
        private RectTransform m_RectTransform;

        private Coroutine m_ScrollingTextCoroutine = null;
        private bool m_Reset = false;


        private void Start()
        {
            ApplicationManager.Instance.OnScreenSizeChange += Reset;
        }

        private void OnDestroy()
        {
            if (ApplicationManager.Instance != null)
            {
                ApplicationManager.Instance.OnScreenSizeChange -= Reset;
            }
        }

        // Use this for initialization
        private void OnEnable()
        {
            m_ScrollingText = GetComponent<UnityEngine.UI.Text>();
            m_RectTransform = GetComponent<RectTransform>();

            m_ScrollingTextCoroutine = StartCoroutine(ScrollCoroutine());
        }

        private void OnDisable()
        {
            if(m_ScrollingTextCoroutine != null)
            {
                StopCoroutine(m_ScrollingTextCoroutine);
                m_ScrollingTextCoroutine = null;
            }
            m_Reset = false;
        }

        private IEnumerator ScrollCoroutine()
        {
            Vector2 scrollExtents = new Vector2();
            Vector2 startOffset = new Vector2();
            m_Reset = true;

            while (true)
            {
                if(m_Reset)
                {
                    //attempting to wait for layouts to update transforms
                    yield return null;
                    yield return null;
                    yield return null;

                    bool cancelScrolling = true;
                    Vector2 textRectSize = m_RectTransform.rect.size;
                    if (m_ScrollHorizontally)
                    {
                        scrollExtents.x = -m_ScrollingText.cachedTextGenerator.GetPreferredWidth(m_ScrollingText.text, m_ScrollingText.GetGenerationSettings(textRectSize));
                        cancelScrolling = cancelScrolling && (-scrollExtents.x < textRectSize.x);
                    }
                    if (m_ScrollVertically)
                    {
                        scrollExtents.y = -m_ScrollingText.cachedTextGenerator.GetPreferredHeight(m_ScrollingText.text, m_ScrollingText.GetGenerationSettings(textRectSize));
                        cancelScrolling = cancelScrolling && (-scrollExtents.y < textRectSize.y);
                    }
                    if(cancelScrolling)
                    {
                        break;
                    }
                    m_Reset = false;
                }

                yield return new WaitForSeconds(m_RepeatDelay);

                //scrolling move
                float progress = 0;
                while (progress < 1.0f && !m_Reset)
                {
                    Vector2 currentOffset = Vector3.Lerp(startOffset, scrollExtents, progress);
                    m_RectTransform.offsetMin = currentOffset;

                    progress += m_ScrollSpeed * Time.deltaTime;
                    yield return null;
                }


                //reset and fade in
                m_RectTransform.offsetMin = startOffset;
                var renderer = m_ScrollingText.canvasRenderer;
                float currentAlpha = 0.0f;
                while (currentAlpha < 1.0f && !m_Reset)
                {
                    renderer.SetAlpha(currentAlpha);
                    currentAlpha += m_FadeSpeed * Time.deltaTime;
                    yield return null;
                }
                renderer.SetAlpha(1.0f);
            }

            m_ScrollingTextCoroutine = null;
        }

        public void Reset()
        {
            m_Reset = true;
            if (m_ScrollingTextCoroutine == null && isActiveAndEnabled)
            {
                m_ScrollingTextCoroutine = StartCoroutine(ScrollCoroutine());
            }
        }


        public void SetText(string i_Text)
        {
            m_ScrollingText.text = i_Text;
            Reset();
        }
    }

}