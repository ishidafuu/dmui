using UnityEngine;
using System.Collections.Generic;
using EnhancedScrollerDemos.CellEvents;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;

namespace DM {
    /// <summary>
    /// This demo shows how you can respond to events from your cells views using delegates
    /// </summary>
    public class Controller14_2 : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private List<Data> _data;
        public EnhancedScroller scroller;
        // ヒエラルキー上ではなく、Resourceフォルダ内のPrefabを指定
        public EnhancedScrollerCellView cellViewPrefab;
        public float cellSize;

        public void Init()
        {
            scroller.Delegate = this;
            LoadData();
        }

        private void LoadData()
        {
            _data = new List<Data>();

            for (var i = 0; i < 24; i++)
                _data.Add(new Data() { hour = i });
        }
        
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _data.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return cellSize;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            CellView14_2 cellView142 = scroller.GetCellView(cellViewPrefab) as CellView14_2;
            cellView142.SetData(_data[dataIndex]);
            return cellView142;
        }
    }
}
