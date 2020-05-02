using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MixedBallItemView : DropObject
{
    [SerializeField] private Text m_Text;
    public int Slot { get; set; }

    protected void Reset()
    {
        // base.Reset();
        m_Text = transform.Find("Text")?.GetComponent<Text>();
    }
    
    public void SetText(int ballNo)
    {
        m_Text.text = $"BallNo:{ballNo}\nSlot:{Slot}";
    }

}