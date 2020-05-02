using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public class LaboScrollerBase : UIBase
    {
        private LaboScrollerView m_LaboScrollerView;
        private PreviewFieldBase m_PreviewFieldBase;
        public MixedBallTabBase m_MixedBallTabBase;
        
        public LaboScrollerBase() : base("Labo/LaboScrollerBase", EnumUIGroup.Floater, EnumUIPreset.Frame)
        {
            IsScheduleUpdate = true;
        }

        public override async UniTask OnLoadedBase()
        {
            UIBaseLayer previewFieldLayer = UIController.Instance.GetBaseLayer(typeof(PreviewFieldBase));
            m_PreviewFieldBase = previewFieldLayer.Base as PreviewFieldBase;
            
            UIBaseLayer mixedBallTabBase = UIController.Instance.GetBaseLayer(typeof(MixedBallTabBase));
            m_MixedBallTabBase = mixedBallTabBase.Base as MixedBallTabBase;
            
            m_LaboScrollerView = RootTransform.GetComponent<LaboScrollerView>();
            InitLaboScrollerController();

            List<UIPart> parts = new List<UIPart>
            {
                // new HomeTabPart(m_LaboScrollerView.m_TabView, JumpToDataIndex)
            };

            // 追加待ち
            await UIController.Instance.YieldAttachParts(this, parts);
            
            const int FIRST_INDEX = 2;
            m_LaboScrollerView.m_Scroller.JumpToDataIndex(FIRST_INDEX);
        }

        private void InitLaboScrollerController()
        {
            m_LaboScrollerView.Init(CellViewInstantiated, ScrollerScrollingChanged,
                ScrollerBeginDrag, ScrollerEndDrag);
        }

        // 新規セルビュー追加時デリゲート
        private void CellViewInstantiated(EnhancedScroller scroller, EnhancedScrollerCellView cellView)
        {
            UIPart part = null;
            switch (cellView.cellIdentifier)
            {
                case "MixedListCellView":
                    part = new MixedListPart(cellView as MixedListCellView);
                    break;
                case "MixedLineCellView":
                    part = new MixedLinePart(cellView as MixedLineCellView, m_LaboScrollerView);
                    break;
                case "PreviewCellView":
                    part = new PreviewPart(cellView as PreviewCellView);
                    break;
                case "ElementLineCellView":
                    part = new ElementLinePart(cellView as ElementLineCellView, m_LaboScrollerView, m_MixedBallTabBase.m_MixedBallTabView);
                    break;
                case "ElementListCellView":
                    part = new ElementListPart(cellView as ElementListCellView);
                    break;
                default:
                    Debug.LogError($"CellViewInstantiated Error cellIdentifier:{cellView.cellIdentifier}");
                    return;
            }

            List<UIPart> parts = new List<UIPart>
            {
                part
            };

            // 即時追加
            UIController.Instance.AttachParts(this, parts);
        }


        private void ScrollerScrollingChanged(EnhancedScroller scroller, bool scrolling) { }

        private void ScrollerBeginDrag(EnhancedScroller scroller, PointerEventData data)
        {
            scroller.CancelTweening();
        }

        private void ScrollerEndDrag(EnhancedScroller scroller, PointerEventData data)
        {
            // ScrollRectのInertiaはFalseにしておく
            // TweenはEaseInQuad

            const float BORDER = 0.01f;
            const int PAGE_NUM = 5;

            int index = 0;
            float pagePosition = 0;
            bool isPrevShift = false;
            for (int i = 0; i < PAGE_NUM; i++)
            {
                index = i;
                float cellSize = m_LaboScrollerView.GetCellSize(i);
                if (scroller.ScrollPosition < pagePosition + (cellSize / 2))
                {
                    isPrevShift = scroller.ScrollPosition - pagePosition < 0;
                    break;
                }

                pagePosition += cellSize;
            }

            if (data.delta.x < -BORDER && !isPrevShift)
            {
                index += 1;
            }
            else if (data.delta.x > BORDER && isPrevShift)
            {
                index -= 1;
            }

            JumpToDataIndex(index);
        }

        private void JumpToDataIndex(int index)
        {
            if (m_LaboScrollerView.m_Scroller.IsTweening)
            {
                return;
            }

            m_LaboScrollerView.m_Scroller.JumpToDataIndex(index, 0, 0, true,
                EnhancedScroller.TweenType.easeOutQuart, 0.5f,
                () => m_LaboScrollerView.m_Scroller.Velocity = Vector2.zero);
        }
        
        public void ClickMixedLineItem(int index)
        {
            m_MixedBallTabBase.ChangeMixedBall(index);
        }
    }
}