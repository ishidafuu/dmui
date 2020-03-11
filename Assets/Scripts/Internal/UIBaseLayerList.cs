// ----------------------------------------------------------------------
// DMUIFramework
// Copyright (c) 2018 Takuya Nishimura (tnishimu)
//
// This software is released under the MIT License.
// https://opensource.org/licenses/mit-license.php
// ----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DM
{
    public class UIBaseLayerList
    {
        private readonly List<UIBaseLayer> m_List = new List<UIBaseLayer>();

        public static int GetNumInGroup(UIGroup group, IEnumerable<UIBaseLayer> list)
        {
            int count = 0;
            foreach (var item in list)
            {
                if (group == item.Base.Group)
                {
                    ++count;
                }
            }

            return count;
        }

        public int GetNumInGroup(UIGroup group)
        {
            return GetNumInGroup(group, m_List);
        }
        public int Count => m_List.Count;

        public void Insert(UIBaseLayer layer)
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

        public void Eject(UIBaseLayer layer)
        {
            int index = m_List.FindIndex(l => (l == layer));
            if (index >= 0)
            {
                m_List.RemoveAt(index);
            }
        }

        public void ForEachOnlyActive(Action<UIBaseLayer> action)
        {
            foreach (UIBaseLayer item in m_List)
            {
                if (item.State != BaseLayerState.Active)
                {
                    continue;
                }
                
                action(item);
            }
        }

        public void ForEachAnything(Action<UIBaseLayer> action)
        {
            List<UIBaseLayer> list = new List<UIBaseLayer>(m_List);
            foreach (var item in list)
            {
                action(item);
            }
        }

        public UIBaseLayer Find(UIBase ui)
        {
            return m_List.Find(l => (l.Base == ui));
        }

        public List<UIBaseLayer> FindLayers(UIGroup uiGroup)
        {
            List<UIBaseLayer> list = new List<UIBaseLayer>();
            foreach (var item in m_List)
            {
                if (item.Base.Group == uiGroup)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public UIBaseLayer FindFrontLayerInGroup(UIGroup group)
        {
            return m_List.Find(l => (l.Base.Group == @group));
        }

        public bool Has(string baseName)
        {
            return m_List.Exists(l => (l.Base.Name == baseName));
        }

        private int FindInsertPosition(UIGroup uiGroup)
        {
            while (true)
            {
                if (uiGroup == UIGroup.None)
                {
                    return -1;
                }

                int index = FindFrontIndexInGroup(uiGroup);
                if (index > -1) return index;
                uiGroup -= 1;
            }
        }

        private int FindFrontIndexInGroup(UIGroup uiGroup)
        {
            return m_List.FindIndex(l => l.Base.Group == uiGroup);
        }
    }
}