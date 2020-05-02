using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DM 
{
    public class MixedBallItemPart : UIPart
    {
        private readonly MixedBallItemView m_MixedBallItemView;
        private readonly Action m_ActionClick;
        private readonly Action m_ActionDrop;

        public MixedBallItemPart(MixedBallItemView mixedBallItemView, Action actionClick = null,
            Action actionDrop = null) 
            : base(mixedBallItemView.transform)
        {
            m_MixedBallItemView = mixedBallItemView;
            m_ActionClick = actionClick;
            m_ActionDrop = actionDrop;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            // // 追加待ち
            List<UIPart> parts = new List<UIPart>()
            {
                new DropPart(m_MixedBallItemView, Drop),
            };

            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }
        
        private void Drop(PointerEventData pointerEventData)
        {
            Debug.Log($"Drop{m_MixedBallItemView.Slot}");
            Debug.Log(ElementLineItemPart.s_DraggingItem != null
                ? $"Element Index:{ElementLineItemPart.s_DraggingItem.m_ElementLineItemCellView.GetHour()}"
                : $"Element null");

            ElementLineItemPart.s_DraggingItem = null;
        }
    }
}