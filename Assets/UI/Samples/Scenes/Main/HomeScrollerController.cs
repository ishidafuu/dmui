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
        public EnhancedScrollerCellView[] m_CellViewPrefabs;
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

            for (var i = 0; i < m_CellViewPrefabs.Length; i++)
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
            Debug.Log($"dataindex{dataIndex} cellIndex{cellIndex}");
            switch (cellIndex)
            {
                case 0:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as HomeCell0ShopView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;  
                }
                case 1:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as HomeCell1LaboratoryView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
                case 2:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as HomeCell2BattleView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
                case 3:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as HomeCell3SocialView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
                case 4:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as HomeCell4EventView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
            }

            return null;
        }
    }
}
