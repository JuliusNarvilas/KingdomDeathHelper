using Common;
using Game.IO.InfoDB;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class ApplicationManager : SingletonMonoBehaviour<ApplicationManager>
    {
        public enum EState
        {
            None,
            Loading,
            Ready
        }

        private static EState s_State = EState.None;
        public static EState State { get { return s_State; } }

        public static void Initialise()
        {
            if(s_State == EState.None)
            {
                s_State = EState.Loading;
                SceneManager.LoadSceneAsync("GlobalAdditiveScene", LoadSceneMode.Additive);
            }
        }

        [SerializeField]
        private InfoDBController m_InfoDB;
        public InfoDBController InfoDB { get { return m_InfoDB; } }

        protected new void Awake()
        {
            base.Awake();
            s_State = EState.Ready;
        }

        private void OnApplicationQuit()
        {
            //TODO: autosave
        }
    }
}
