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

        #region EnhancedScroller Handlers

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _data.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return cellSize;
        }

        public CellView14_2 GetCell(int dataIndex)
        {
            return scroller.GetCellView(cellViewPrefab) as CellView14_2;
        }
        
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            CellView14_2 cellView142 = scroller.GetCellView(cellViewPrefab) as CellView14_2;
            // Set handlers for the cell views delegates.
            // Each handler will respond to a different type of event
            cellView142.cellButtonTextClicked = CellButtonTextClicked;
            cellView142.cellButtonFixedIntegerClicked = CellButtonFixedIntegerClicked;
            cellView142.cellButtonDataIntegerClicked = CellButtonDataIntegerClicked;
            cellView142.SetData(_data[dataIndex]);
            return cellView142;
        }

        #endregion

        /// <summary>
        /// Handler for when the cell view fires a fixed text button click event
        /// </summary>
        /// <param name="value">value of the text</param>
        private void CellButtonTextClicked(string value)
        {
            Debug.Log("Cell Text Button Clicked! Value = " + value);
        }

        /// <summary>
        /// Handler for when the cell view fires a fixed integer button click event
        /// </summary>
        /// <param name="value">value of the integer</param>
        private void CellButtonFixedIntegerClicked(int value)
        {
            Debug.Log("Cell Fixed Integer Button Clicked! Value = " + value);
        }

        /// <summary>
        /// Handler for when the cell view fires a data integer button click event
        /// </summary>
        /// <param name="value">value of the integer</param>
        private void CellButtonDataIntegerClicked(int value)
        {
            Debug.Log("Cell Data Integer Button Clicked! Value = " + value);
        }
    }
}
