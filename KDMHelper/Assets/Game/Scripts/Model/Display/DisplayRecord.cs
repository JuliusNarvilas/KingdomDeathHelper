using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model.Display
{
    public class DisplayRecord
    {
        private Dictionary<string, IDataSource> m_Context = new Dictionary<string, IDataSource>();

        public IDataSource GetDataSourceInContext(string contextProperty)
        {
            IDataSource result = null;
            m_Context.TryGetValue(contextProperty, out result);
            return result;
        }



    }
}
