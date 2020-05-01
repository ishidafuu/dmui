using System;
using EnhancedScrollerDemos.CellEvents;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;

namespace DM
{
    public class ElementLineItemCellView : EnhancedScrollerCellView
    {
        private Data _data;

        public Text someTextText;
        public ButtonObject m_ElementButton;
        public DragObject m_ElementDragObject;
        public void SetData(Data data)
        {
            _data = data;
            // someTextText.text = (_data.hour == 0 ? "Midnight" : string.Format("{0} 'o clock", _data.hour.ToString()));
        }

        public int GetHour()
        {
            return _data.index;
        }
    }
}