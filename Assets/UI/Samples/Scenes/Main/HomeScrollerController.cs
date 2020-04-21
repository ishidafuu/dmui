using UnityEngine;
using System.Collections.Generic;
using EnhancedScrollerDemos.CellEvents;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Serialization;

namespace DM {

    public class HomeScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private List<Data> m_Data;
        public EnhancedScroller m_Scroller;
        // ヒエラルキー上ではなく、Resourceフォルダ内のPrefabを指定
        public EnhancedScrollerCellView m_CellViewPrefab;
        public float m_CellSize;

        public void Init(EnhancedScroller scroller)
        {
            m_Scroller = scroller;
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
            HomeCellView cellView = scroller.GetCellView(m_CellViewPrefab) as HomeCellView;
            cellView.SetData(m_Data[dataIndex]);
            return cellView;
        }
    }
}
