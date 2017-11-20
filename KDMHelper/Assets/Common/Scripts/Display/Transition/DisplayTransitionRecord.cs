using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Common.Display.Transition
{
    [Serializable]
    public class DisplayTransitionTarget : IEquatable<DisplayTransitionTarget>
    {
        public string Key;
        public Animator Anim;
        public float SpeedIn = 1.0f;
        public float SpeedOut = 1.0f;

        public bool Equals(DisplayTransitionTarget other)
        {
            return Key == other.Key;
        }
    }
}
