using Common.IO.FileHelpers.CSV;
using Common.IO.FileHelpers.FileLoadSpecializations;
using Common.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Common.IO.ContentProvider.Instances
{
    public class ContentInstanceCSV : ContentInstance
    {
        private static ContentInstance Create(ContentSettingsEntry entry)
        {
            return new ContentInstanceCSV(entry);
        }
        private static ContentInstance.Registration s_registration = new ContentInstance.Registration(EContentType.CSV, typeof(ContentInstanceCSV), Create);


        public ContentInstanceCSV(ContentSettingsEntry i_entry) : base(i_entry)
        { }

        object m_processingData;
        CSVData m_Result;

        private bool m_Dirty;
        private string m_Source;



        public override bool IsDirty()
        {
            return m_Dirty;
        }
        public override void MarkAsDirty()
        {
            m_Dirty = true;
        }


        CSVData GetCSV()
        {
            if (GetState() == EContentInstanceState.Ready)
                return m_Result;

            return null;
        }


        protected override void Fixup()
        {
            object source = LoadSource();
            if (source == null)
            {
                SetState(EContentInstanceState.Errored);
                return;
            }

            Type sourceType = source.GetType();
            if(sourceType == typeof(string))
            {
                m_Result = CSVData.CreateFromString(source as string);
                return;
            }
            else if(typeof(Stream).IsAssignableFrom(sourceType))
            {
                Stream sourceStream = source as Stream;
                using (var sr = new StreamReader(sourceStream))
                {
                    string contentText = sr.ReadToEnd();
                    m_Result = CSVData.CreateFromString(contentText);
                }
                sourceStream.Dispose();
                return;
            }

            m_ErrorMsg = string.Format("Type \"{0}\" could not handle data of type \"{1}\"", GetType().ToString(), sourceType.ToString());
            SetState(EContentInstanceState.Errored);
            return;
        }
    }
}
