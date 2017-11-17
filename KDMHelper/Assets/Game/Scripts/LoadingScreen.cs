using Common.IO;
using Game.Data;
using Game.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {

    public InfoDBController InfoDB;
    [SerializeField]
    private PopupDisplayList m_DisplayList;
    [SerializeField]
    private GameObject m_LoadingDisplay;
    [SerializeField]
    private GameObject m_MainDisplay;

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

        if(m_MainDisplay != null)
        {
            m_MainDisplay.SetActive(true);
        }

        if (m_LoadingDisplay != null) m_LoadingDisplay.SetActive(false);
        if (m_MainDisplay != null) m_MainDisplay.SetActive(true);
    }


    void Awake () {
        //force instantiate AssetReferenceUpdateRunner on main thread
        var temp = AssetReferenceUpdateRunner.Instance;
        bool isLoading = false;

        if (InfoDB.Sources != null)
        {
            int count = InfoDB.Sources.Count;
            if (count > 0 && InfoDB.Sources[0].State == InfoDBSource.EState.Initial)
            {
                isLoading = true;
                for (int i = 0; i < count; ++i)
                {
                    StartCoroutine(UpdateDisplayState(InfoDB.Sources[i]));
                }
                StartCoroutine(WaitToFinishLoading());
            }
        }

        if(m_LoadingDisplay != null) m_LoadingDisplay.SetActive(isLoading);
        if(m_MainDisplay != null) m_MainDisplay.SetActive(!isLoading);
    }
    
}
