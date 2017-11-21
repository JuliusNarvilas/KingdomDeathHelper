using Common.Display.Transition;
using Game.IO.InfoDB;
using Game.Model;
using Game.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Screen
{

    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField]
        private PopupDisplayList m_DisplayList;
        [SerializeField]
        private DisplayTransitionController m_DisplayController;
        private List<PopupDisplayElement> m_DisplayElements = new List<PopupDisplayElement>();

        private int m_LoadingFinished = 0;

        private IEnumerator UpdateDisplayState(InfoDBSource infoSource)
        {
            infoSource.Load();
            var state = infoSource.State;
            var displayElement = m_DisplayList.Add(state.ToString());
            displayElement.Label = infoSource.Name;
            m_DisplayElements.Add(displayElement);

            while (state != InfoDBSource.EState.Ready)
            {
                while (state == infoSource.State)
                {
                    yield return null;
                }
                state = infoSource.State;
                if (state == InfoDBSource.EState.Errored)
                {
                    displayElement.Content = string.Format("{0}: {1}", state.ToString(), infoSource.Error);
                    yield break;
                }
                displayElement.Content = state.ToString();
            }

            m_LoadingFinished++;
        }

        private IEnumerator MenuTransitionWaiter()
        {
            while (ApplicationManager.State != ApplicationManager.EState.Ready)
            {
                yield return null;
            }
            if (ApplicationManager.Instance.InfoDB == null || ApplicationManager.Instance.InfoDB.Sources == null)
            {
                yield break;
            }
            while (m_LoadingFinished < ApplicationManager.Instance.InfoDB.Sources.Count)
            {
                yield return null;
            }

            m_DisplayController.TransitionFromFadeToFade("Menu");
        }


        IEnumerator Start()
        {
            while(ApplicationManager.State != ApplicationManager.EState.Ready)
            {
                yield return null;
            }
            m_DisplayController.TransitionFromFadeFastToFadeFast("Loading");
            Load();
            TransitionToMenuWhenReady();
        }

        public void TransitionToMenuWhenReady()
        {
            StartCoroutine(MenuTransitionWaiter());
        }
        
        public void Load()
        {
            if (ApplicationManager.State == ApplicationManager.EState.Ready)
            {
                if (ApplicationManager.Instance.InfoDB != null && ApplicationManager.Instance.InfoDB.Sources != null)
                {
                    var infoDB = ApplicationManager.Instance.InfoDB;
                    infoDB.Reset();
                    m_LoadingFinished = 0;
                    int displayItemCount = m_DisplayElements.Count;
                    for (int i = 0; i < displayItemCount; ++i)
                    {
                        Destroy(m_DisplayElements[i].gameObject);
                    }
                    m_DisplayElements.Clear();
                    int count = infoDB.Sources.Count;
                    if (count > 0 && infoDB.Sources[0].State == InfoDBSource.EState.Initial)
                    {
                        for (int i = 0; i < count; ++i)
                        {
                            StartCoroutine(UpdateDisplayState(infoDB.Sources[i]));
                        }
                    }
                }
            }
        }
    }

}
