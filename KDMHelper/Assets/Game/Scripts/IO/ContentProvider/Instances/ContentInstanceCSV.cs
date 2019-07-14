using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace Assets.Game.Scripts.IO.ContentProvider.Instances
{

    public class ContentInstanceCSV : ContentInstance
    {
        private static ContentInstance Create(ContentEntry entry)
        {
            return new ContentInstanceCSV(entry);
        }
        private static ContentInstance.Registration s_registration = new ContentInstance.Registration("CSV", typeof(ContentInstanceCSV), Create);


        public ContentInstanceCSV(ContentEntry i_entry) : base(i_entry)
        { }



        private bool m_Dirty;
        private string m_Source;

        public override bool IsDirty()
        {
            return m_Dirty;
        }


        protected override void StartAsyncSetup()
        {

        }
    }
}
