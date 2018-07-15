using Game.Model;
using Game.Model.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.DisplayHandler
{
    public abstract class ValueDisplayHandler : MonoBehaviour
    {
        public abstract void SetValue(NamedProperty val);
        public abstract void Setup(ValueDisplayMode mode);
    }
}
