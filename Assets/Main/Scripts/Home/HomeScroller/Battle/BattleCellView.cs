using EnhancedScrollerDemos.CellEvents;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;

namespace DM
{
    public class BattleCellView : EnhancedScrollerCellView
    {
        private Data _data;

        public Text someTextText;

        public GameObject textButton;
        public GameObject fixedIntegerButton;
        public GameObject dataIntegerButton;
        [SerializeField] public ButtonObject m_LaboButtonObject;
        
        public void SetData(Data data)
        {
            _data = data;
            someTextText.text = (_data.index == 0 ? "Midnight" : string.Format("{0} 'o clock", _data.index.ToString()));
        }
    }
}