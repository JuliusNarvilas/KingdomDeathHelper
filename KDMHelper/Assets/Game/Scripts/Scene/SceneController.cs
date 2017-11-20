using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Game.Scripts.Scene
{
    public class SceneController : MonoBehaviour
    {
        private void Start()
        {
            ApplicationManager.Initialise();
        }
    }
}
