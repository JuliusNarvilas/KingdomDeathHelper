using Game.DisplayHandler;
using Game.IO.InfoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Model.Display
{
    public class DisplaySetup
    {
        public List<DisplaySetupSection> Sections;

        public void Build(Transform destination)
        {
            int count = Sections.Count;
            for(int i = 0; i < count; ++i)
            {
                DisplaySetupSection section = Sections[i];
                NamedProperty property = null;
                IDataSource source = DataSource.FindDataSource(section.Source);
                if(source != null)
                {
                    object data = null;
                    string[] selectionParts = section.Selection.Split('.');
                    for (int selectionPartIndex = 0; i < selectionParts.Length; i++)
                    {
                        if(source == null)
                        {
                            data = null;
                            break;
                        }
                        data = source.GetData(selectionParts[selectionPartIndex]);
                        source = data as IDataSource;
                    }

                    if (data != null)
                    {
                        property = data as NamedProperty;
                        if (property == null)
                        {
                            property = new NamedProperty();
                            property.SetName(selectionParts[selectionParts.Length - 1]);
                            property.Property = data;
                        }
                    }
                }

                if(property != null)
                {
                    ValueDisplayHandler handler = ApplicationManager.Instance.DisplayHandlerDB.Find(property.Property.GetType(), section.DisplayMode, section.DisplayHandlerFilter);
                    ValueDisplayHandler instancedHandler = UnityEngine.Object.Instantiate(handler, destination);
                    instancedHandler.SetValue(property);
                }
            }
        }
    }
}
