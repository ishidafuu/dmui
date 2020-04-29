using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MixedBallItemView : ButtonObject
{
    public Image iconImage;
    private Sprite nowSprite;

    void Start()
    {
        nowSprite = null;
    }
    
}