using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model.Display
{
    public class DisplayRecord
    {
        private Dictionary<string, IDataSource> m_Context = new Dictionary<string, IDataSource>();

        /// <summary>
        /// Gets <see cref="IDataSource"/> that could be different based on current context.
        /// Should be used to get data based on concepts like "current survivor" or "current campaign"
        /// </summary>
        /// <param name="contextProperty"></param>
        /// <returns></returns>
        public IDataSource GetDataSourceInContext(string contextProperty)
        {
            IDataSource result = null;
            m_Context.TryGetValue(contextProperty, out result);
            return result;
        }



    }
}
