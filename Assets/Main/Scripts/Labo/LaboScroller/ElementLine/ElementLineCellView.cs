using EnhancedScrollerDemos.CellEvents;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;

namespace DM
{
    public class ElementLineCellView : EnhancedScrollerCellView
    {
        [SerializeField] public ElementLineScrollerView m_ElementLineScrollerView;
        [HideInInspector] public LaboScrollerView m_LaboScrollerView;

        public void SetData(Data data)
        {
            // _data = data;
            // someTextText.text = (_data.hour == 0 ? "Midnight" : string.Format("{0} 'o clock", _data.hour.ToString()));
        }
    }
}