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
    public class UIBaseLayerController
    {
        private readonly List<UIBaseLayer> m_List = new List<UIBaseLayer>();

        public static int GetCountInGroup(UIGroup uiGroup, IEnumerable<UIBaseLayer> list)
        {
            int count = 0;
            foreach (UIBaseLayer item in list)
            {
                if (uiGroup == item.Base.Group)
                {
                    ++count;
                }
            }

            return count;
        }

        public int GetCountInGroup(UIGroup uiGroup)
        {
            return GetCountInGroup(uiGroup, m_List);
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

        public UIBaseLayer Find(UIBase uiBase)
        {
            return m_List.Find(layer => (layer.Base == uiBase));
        }

        public IEnumerable<UIBaseLayer> FindLayers(UIGroup uiGroup)
        {
            List<UIBaseLayer> list = new List<UIBaseLayer>();
            foreach (UIBaseLayer layer in m_List)
            {
                if (layer.Base.Group == uiGroup)
                {
                    list.Add(layer);
                }
            }

            return list;
        }

        public UIBaseLayer FindFrontLayerInGroup(UIGroup uiGroup)
        {
            return m_List.Find(layer => (layer.Base.Group == uiGroup));
        }

        public bool Has(string baseName)
        {
            return m_List.Exists(layer => (layer.Base.Name == baseName));
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
            return m_List.FindIndex(layer => layer.Base.Group == uiGroup);
        }
    }
}