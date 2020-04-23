using EnhancedScrollerDemos.CellEvents;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Serialization;

namespace DM
{
    public class LaboCellView : EnhancedScrollerCellView
    {
        [SerializeField] public LaboScrollerView m_LaboScrollerView;
        [SerializeField] public LaboScrollerController m_LaboScrollerController;
        
        public void SetData(Data data)
        {
            // _data = data;
            // someTextText.text = (_data.hour == 0 ? "Midnight" : string.Format("{0} 'o clock", _data.hour.ToString()));
        }
    }
}