using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.IO.Save
{
    public class CampaignSaveData
    {
        public Guid Id;
        public string Name;
        public string SettlementName;
        public string CampaignType;
        public List<string> Expansions;
        
    }
}
