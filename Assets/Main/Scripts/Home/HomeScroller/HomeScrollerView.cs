using System;
using UnityEngine;
using System.Collections.Generic;
using EnhancedScrollerDemos.CellEvents;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace DM
{
    public class HomeScrollerView : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private List<Data> m_Data;
        [SerializeField] public EnhancedScroller m_Scroller;
        [SerializeField] public TabObject m_TabObject;

        private float m_ScrollWidth;
        private Transform m_CursorTransform;
        
        // ヒエラルキー上ではなく、Resourceフォルダ内のPrefabを指定
        public EnhancedScrollerCellView[] m_CellViewPrefabs;

        public float CellSize { get; private set; }

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
            
            CellSize = UIController.Instance.m_CanvasScaler.referenceResolution.x;
            LoadData();
            m_Scroller.ReloadData();
        }
        
        private void Start()
        {
            m_ScrollWidth = (m_TabObject.transform as RectTransform).rect.width / 5 * 4;
            m_CursorTransform = m_TabObject.m_Cursor.transform;
        }
        
        private void Update()
        {
            // カーソル位置調整
            var localPosition = m_CursorTransform.localPosition;
            var horizontalNormalizedPosition = m_Scroller.ScrollRect.horizontalNormalizedPosition;
            float newX = (horizontalNormalizedPosition - 0.5f) * m_ScrollWidth;

            if (Math.Abs(newX - localPosition.x) < 0.01f)
            {
                return;
            }

            Vector3 newPosition = localPosition;
            newPosition.x = newX;
            m_CursorTransform.localPosition = newPosition;
        }

        private void LoadData()
        {
            m_Data = new List<Data>();

            for (var i = 0; i < m_CellViewPrefabs.Length; i++)
                m_Data.Add(new Data() {hour = i});
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return m_Data.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return CellSize;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            // Debug.Log($"dataindex{dataIndex} cellIndex{cellIndex}");
            switch (cellIndex)
            {
                case 0:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as ShopCellView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
                case 1:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as BallCellView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
                case 2:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as BattleCellView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
                case 3:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as SocialCellView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
                case 4:
                {
                    var cellView = scroller.GetCellView(m_CellViewPrefabs[cellIndex]) as EventCellView;
                    cellView.SetData(m_Data[dataIndex]);
                    return cellView;
                }
            }

            return null;
        }
    }
}