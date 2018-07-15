using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public static class DataSource
    {
        private static Dictionary<string, IDataSource> s_DataSources = new Dictionary<string, IDataSource>();

        public static void AddDataSource(IDataSource dataSource)
        {
            s_DataSources[dataSource.GetKey()] = dataSource;
        }

        public static void RemoveDataSource(IDataSource dataSource)
        {
            IDataSource result = null;
            if(s_DataSources.TryGetValue(dataSource.GetKey(), out result))
            {
                if(result == dataSource)
                {
                    s_DataSources.Remove(dataSource.GetKey());
                }
            }
        }

        public static IDataSource FindDataSource(string key)
        {
            IDataSource result = null;
            s_DataSources.TryGetValue(key, out result);
            return result;
        }

    }

    public interface IDataSource
    {
        string GetKey();

        object GetData(string Selection);
    }
}
