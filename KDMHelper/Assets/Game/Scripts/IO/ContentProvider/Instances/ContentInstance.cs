using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace Assets.Game.Scripts.IO.ContentProvider.Instances
{

    public abstract class ContentInstance
    {
        private static Dictionary<string, Registration> s_InstanceRegistrations;

        public class Registration
        {
            public Registration(string i_typeName, Type i_instanceType, Func<ContentEntry, ContentInstance> i_construction)
            {
                TypeName = i_typeName;
                InstanceType = i_instanceType;
                Construction = i_construction;

                Assert.IsTrue(!string.IsNullOrEmpty(TypeName));
                Assert.IsTrue(i_instanceType != null);
                Assert.IsTrue(i_construction != null);

                s_InstanceRegistrations.Add(TypeName, this);
            }

            public readonly string TypeName;
            public readonly Type InstanceType;
            public readonly Func<ContentEntry, ContentInstance> Construction;
        }

        public enum State
        {
            Initial,
            Loading,
            Parsing,
            Ready,
            Errored
        }

        public ContentInstance(ContentEntry i_entry)
        {
            m_State = State.Initial;
            Entry = i_entry;
        }


        private ContentInstance.State m_State;
        public ContentEntry Entry;

        public ContentInstance.State GetState()
        {
            return m_State;
        }

        public abstract bool IsDirty();
        protected abstract void StartAsyncSetup();


        public static ContentInstance CreateAsync(ContentEntry entry)
        {
            if (entry == null || entry.IsValid())
                return null;

            Registration reg;
            if(!s_InstanceRegistrations.TryGetValue(entry.Type, out reg))
                return null;

            var result = reg.Construction(entry);
            if(result != null)
                result.StartAsyncSetup();

            return result;
        }
    }
}
