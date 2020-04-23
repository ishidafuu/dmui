using UnityEngine;
using System.Collections.Generic;
using EnhancedScrollerDemos.CellEvents;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Serialization;

namespace DM {
    /// <summary>
    /// This demo shows how you can respond to events from your cells views using delegates
    /// </summary>
    public class LaboScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private List<Data> m_Data;
        public EnhancedScroller m_Scroller;
        // ヒエラルキー上ではなく、Resourceフォルダ内のPrefabを指定
        public EnhancedScrollerCellView m_CellViewPrefab;
        public float m_CellSize;

        public void Init()
        {
            m_Scroller.Delegate = this;
            LoadData();
        }

        private void LoadData()
        {
            m_Data = new List<Data>();

            for (var i = 0; i < 24; i++)
                m_Data.Add(new Data() { hour = i });
        }
        
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return m_Data.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return m_CellSize;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            LaboItemCellView laboItemCellView142 = scroller.GetCellView(m_CellViewPrefab) as LaboItemCellView;
            laboItemCellView142.SetData(m_Data[dataIndex]);
            return laboItemCellView142;
        }
    }
}
