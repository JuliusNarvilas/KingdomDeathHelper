using Common.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Common.IO.ContentProvider
{
    public abstract class ContentInstance
    {
        private static Dictionary<EContentType, Registration> s_InstanceRegistrations = new Dictionary<EContentType, Registration>();

        public class Registration
        {
            public Registration(EContentType i_contentType, Type i_instanceType, Func<ContentSettingsEntry, ContentInstance> i_construction)
            {
                ContentType = i_contentType;
                InstanceType = i_instanceType;
                Construction = i_construction;

                Assert.IsTrue(Enum.IsDefined(typeof(EContentType), ContentType));
                Assert.IsTrue(i_instanceType != null);
                Assert.IsTrue(i_construction != null);

                s_InstanceRegistrations.Add(ContentType, this);
            }

            public readonly EContentType ContentType;
            public readonly Type InstanceType;
            public readonly Func<ContentSettingsEntry, ContentInstance> Construction;
        }

        public enum EContentInstanceState
        {
            Initial,
            Loading,
            Parsing,
            Ready,
            Errored
        }

        public ContentInstance(ContentSettingsEntry i_entry)
        {
            m_State = EContentInstanceState.Initial;
            SettingsEntry = i_entry;
        }


        private ContentInstance.EContentInstanceState m_State;
        protected string m_ErrorMsg;
        protected object m_ProcessingData;
        public ContentSettingsEntry SettingsEntry;

        public string GetErrorMsg()
        {
            return m_ErrorMsg;
        }
        public ContentInstance.EContentInstanceState GetState()
        {
            return m_State;
        }
        protected void SetState(ContentInstance.EContentInstanceState newState)
        {
            m_State = newState;
        }

        protected virtual object LoadSource()
        {
            SetState(EContentInstanceState.Loading);

            switch (SettingsEntry.SourceType)
            {
                case EContentSourceType.External:
                    {
                        string absoluteFilePath = string.Format("{0}/{1}", Application.persistentDataPath, SettingsEntry.Source);

                        if (File.Exists(absoluteFilePath))
                        {
                            MemoryStream memStream = new MemoryStream();
                            try
                            {
                                using (var fileStream = File.OpenRead(absoluteFilePath))
                                {
                                    using (var bufferedStream = new BufferedStream(fileStream))
                                    {
                                        IOHelpers.CopyStream(bufferedStream, memStream);
                                        memStream.Position = 0;
                                        return memStream;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                memStream.Dispose();
                                m_ErrorMsg = e.Message;
                                SetState(EContentInstanceState.Errored);
                            }
                            return null;
                        }
                        else
                        {
                            m_ErrorMsg = string.Format("File \"{0}\" not found.", absoluteFilePath);
                            SetState(EContentInstanceState.Errored);
                        }
                        return null;
                    }
                case EContentSourceType.Resource:
                    {
                        string tempErrorMsg = null;
                        string result = null;
                        TextAsset asset = null;
                        try
                        {
                            asset = Resources.Load<TextAsset>(SettingsEntry.Source);
                            result = asset.text;
                        }
                        catch(Exception e)
                        {
                            tempErrorMsg = e.Message;
                        }

                        if(result == null && asset != null)
                        {
                            // must be background thread and we must use main thread ti get string out
                            Action mainThreadCallback = () => { result = asset.text; };
                            var handle = ThreadPool.Instance.AddTaskMainThread(mainThreadCallback);
                            while(handle.State <= EThreadedTaskState.InProgress)
                            {
                                System.Threading.Thread.Sleep(10);
                            }

                            if(handle.State != EThreadedTaskState.Succeeded)
                            {
                                m_ErrorMsg = handle.GetException().Message;
                                SetState(EContentInstanceState.Errored);
                            }
                        }
                        else
                        {
                            m_ErrorMsg = tempErrorMsg;
                            SetState(EContentInstanceState.Errored);
                        }

                        return result;
                    }
                default:
                    m_ErrorMsg = string.Format("Unknown CSV source type {0}", SettingsEntry.SourceType.ToString());
                    SetState(ContentInstance.EContentInstanceState.Errored);
                    return null;
            }
        }

        public virtual void MarkAsDirty()   { }
        public virtual bool IsDirty()       { return false; }
        protected virtual void StartAsyncFixup()
        {
            ThreadPool.Instance.AddTask(Fixup);
        }

        protected abstract void Fixup();

        public static ContentInstance CreateAsync(ContentSettingsEntry entry, Registration contentInstanceOverride = null)
        {
            if (entry == null || entry.IsValid())
                return null;

            Registration reg = contentInstanceOverride;
            if (reg == null)
            {
                if (!s_InstanceRegistrations.TryGetValue(entry.ContentType, out reg))
                    return null;
            }

            var result = reg.Construction(entry);
            if (result != null)
                result.StartAsyncFixup();

            return result;
        }
    }
}
