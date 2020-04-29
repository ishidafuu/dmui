using System;
using System.Collections.Generic;

namespace DM
{
    public class UIBaseLayerController
    {
        private readonly List<UIBaseLayer> m_List = new List<UIBaseLayer>();

        public static int GetCountInGroup(EnumUIGroup enumUIGroup, IEnumerable<UIBaseLayer> list)
        {
            int count = 0;
            foreach (UIBaseLayer item in list)
            {
                if (enumUIGroup == item.Base.Group)
                {
                    count++;
                }
            }

            return count;
        }

        public int GetCountInGroup(EnumUIGroup enumUIGroup)
        {
            return GetCountInGroup(enumUIGroup, m_List);
        }

        public void AddOrInsert(UIBaseLayer layer)
        {
            int index = FindInsertPosition(layer.Base.Group);
            if (index < 0)
            {
                m_List.Add(layer);
            }
            else
            {
                m_List.Insert(index, layer);
            }
        }

        public void Remove(UIBaseLayer targetLayer)
        {
            int index = m_List.FindIndex(layer => (layer == targetLayer));
            if (index >= 0)
            {
                m_List.RemoveAt(index);
            }
        }

        public void ForEachOnlyActive(Action<UIBaseLayer> action)
        {
            foreach (UIBaseLayer item in m_List)
            {
                if (item.State != EnumLayerState.Active)
                {
                    continue;
                }

                action(item);
            }
        }
        
        public int FindUntouchableIndex()
        {
            int index = -1;
            foreach (UIBaseLayer item in m_List)
            {
                if (item.State != EnumLayerState.Active)
                {
                    continue;
                }

                if (index >= 0)
                {
                    break;
                }

                if (item.Base.IsSystemUntouchable())
                {
                    index = item.SiblingIndex - 1;
                }
            }

            return index;
        }

        public void ForEachAnything(Action<UIBaseLayer> action)
        {
            List<UIBaseLayer> list = new List<UIBaseLayer>(m_List);
            foreach (UIBaseLayer item in list)
            {
                action(item);
            }
        }

        public UIBaseLayer Find(UIBase uiBase)
        {
            return m_List.Find(layer => (layer.Base == uiBase));
        }
        
        public UIBaseLayer Find(Type type)
        {
            return m_List.Find(layer => (layer.Base.GetType() == type));
        }

        public IEnumerable<UIBaseLayer> FindLayers(EnumUIGroup enumUIGroup)
        {
            List<UIBaseLayer> list = new List<UIBaseLayer>();
            foreach (UIBaseLayer layer in m_List)
            {
                if (layer.Base.Group == enumUIGroup)
                {
                    list.Add(layer);
                }
            }

            return list;
        }

        public UIBaseLayer FindFrontLayerInGroup(EnumUIGroup enumUIGroup)
        {
            return m_List.Find(layer => (layer.Base.Group == enumUIGroup));
        }

        public bool Has(string baseName)
        {
            return m_List.Exists(layer => (layer.Base.Name == baseName));
        }

        private int FindInsertPosition(EnumUIGroup enumUIGroup)
        {
            while (true)
            {
                if (enumUIGroup == EnumUIGroup.None)
                {
                    return -1;
                }

                int index = FindFrontIndexInGroup(enumUIGroup);
                if (index > -1)
                {
                    return index;
                }

                enumUIGroup -= 1;
            }
        }

        private int FindFrontIndexInGroup(EnumUIGroup enumUIGroup)
        {
            return m_List.FindIndex(layer => layer.Base.Group == enumUIGroup);
        }
        
    }
}