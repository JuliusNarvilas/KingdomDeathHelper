using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model.Events
{
    [Serializable]
    public class SettlementTimelineRecord
    {
        public int LanternYear;
        public List<string> Events;
    }
}
