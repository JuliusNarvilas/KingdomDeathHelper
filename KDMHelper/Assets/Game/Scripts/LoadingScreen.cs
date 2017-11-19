using Common.IO;
using Game.IO.InfoDB;
using Game.Model;
using Game.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {

    public InfoDBController InfoDB;
    [SerializeField]
    private PopupDisplayList m_DisplayList;
    [SerializeField]
    private DisplayTransitionController m_DisplayController;

    private int m_LoadingFinished = 0;

    private IEnumerator UpdateDisplayState(InfoDBSource infoSource)
    {
        infoSource.Load();
        var state = infoSource.State;
        var displayElement = m_DisplayList.Add(state.ToString());
        displayElement.Label = infoSource.Name;
        while (state != InfoDBSource.EState.Ready)
        {
            while (state == infoSource.State)
            {
                yield return null;
            }
            state = infoSource.State;
            if(state == InfoDBSource.EState.Errored)
            {
                displayElement.Content = string.Format("{0}: {1}", state.ToString(), infoSource.Error);
                yield break;
            }
            displayElement.Content = state.ToString();
        }

        m_LoadingFinished++;
    }

    private IEnumerator WaitToFinishLoading()
    {
        while (m_LoadingFinished < InfoDB.Sources.Count)
        {
            yield return null;
        }
        
        m_DisplayController.TransitionTo("Main");
    }


    void Awake () {
        //force instantiate AssetReferenceUpdateRunner on main thread
        var temp = AssetReferenceUpdateRunner.Instance;

        if (InfoDB.Sources != null)
        {
            int count = InfoDB.Sources.Count;
            if (count > 0 && InfoDB.Sources[0].State == InfoDBSource.EState.Initial)
            {
                for (int i = 0; i < count; ++i)
                {
                    StartCoroutine(UpdateDisplayState(InfoDB.Sources[i]));
                }
                StartCoroutine(WaitToFinishLoading());
            }
        }

        m_DisplayController.TransitionTo("Loading");
    }
    
}
