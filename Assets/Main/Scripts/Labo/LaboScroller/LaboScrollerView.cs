using System.Collections.Generic;
using EnhancedScrollerDemos.CellEvents;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

namespace DM
{
    public class LaboScrollerView : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private List<Data> m_Data;

        [SerializeField] public EnhancedScroller m_Scroller;

        // ヒエラルキー上ではなく、Resourceフォルダ内のPrefabを指定
        public EnhancedScrollerCellView[] m_CellViewPrefabs;

        private float m_CellSize;

        public float GetCellSize(int index)
        {
            // スクロールライン表示
            return (index == 1 || index == 2)
                ? m_CellSize / 4
                : m_CellSize;
        }

        public void Init(CellViewInstantiated cellViewInstantiated,
            ScrollerScrollingChangedDelegate scrollerScrollingChanged,
            OnBeginDragDelegate scrollerBeginDrag,
            OnEndDragDelegate scrollerEndDrag)
        {
            m_Scroller.Delegate = this;
            m_Scroller.cellViewInstantiated = cellViewInstantiated;
            m_Scroller.scrollerScrollingChanged = scrollerScrollingChanged;
            m_Scroller.scrollerBeginDrag = scrollerBeginDrag;
            m_Scroller.scrollerEndDrag = scrollerEndDrag;

            m_CellSize = UIController.Instance.m_CanvasScaler.referenceResolution.x;
            LoadData();
            m_Scroller.ReloadData();
        }

        private void LoadData()
        {
            m_Data = new List<Data>();

            for (var i = 0; i < m_CellViewPrefabs.Length; i++)
                m_Data.Add(new Data() {index = i});
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return m_Data.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return GetCellSize(dataIndex);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            // Debug.Log($"dataindex{dataIndex} cellIndex{cellIndex}");
            switch (cellIndex)
            {
                case 0:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as MixedListCellView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
                case 1:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as MixedLineCellView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
                case 2:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as PreviewCellView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
                case 3:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as ElementLineCellView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
                case 4:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as ElementListCellView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
            }

            return null;
        }
    }
}