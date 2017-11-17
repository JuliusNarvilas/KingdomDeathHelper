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
    private int m_LoadingFinished = 0;

    private List<PopupDisplayElement> m_DisplayElements = new List<PopupDisplayElement>();

    private IEnumerator UpdateDisplayState(InfoDBSource infoSource)
    {
        infoSource.Load();
        var state = infoSource.State;
        var displayElement = m_DisplayList.Add(state.ToString());
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

    // Use this for initialization
    void Start () {
        //instantiate
        var temp = AssetReferenceUpdateRunner.Instance;
        if (InfoDB.Sources != null)
        {
            int count = InfoDB.Sources.Count;
            for(int i = 0; i < count; ++i)
            {
                StartCoroutine(UpdateDisplayState(InfoDB.Sources[i]));
            }
        }
    }


	
	// Update is called once per frame
	void Update () {
		if(m_LoadingFinished >= InfoDB.Sources.Count)
        {
            Debug.Log("Finished Loading");
        }
	}
}
