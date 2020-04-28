using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DM
{
    public class HomeScrollerPart : UIPart
    {
        // 追加先のレイヤ
        private UIBase m_TargetLayer;
        private readonly HomeScrollerView m_HomeScrollerView;

        // public HomeScrollerPart() : base("HomeScroller") { }
        public HomeScrollerPart(HomeSceneView homeSceneView)
            : base(homeSceneView.m_HomeScrollerView.transform)
        {
            m_HomeScrollerView = homeSceneView.m_HomeScrollerView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            m_TargetLayer = targetLayer;
            InitRootTransform();
            InitHomeScrollerController();

            List<UIPart> parts = new List<UIPart>
            {
                new HomeTabPart(m_HomeScrollerView.m_TabObject, JumpToDataIndex)
            };

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);

            const int FIRST_INDEX = 2;
            m_HomeScrollerView.m_Scroller.JumpToDataIndex(FIRST_INDEX);
        }

        private void InitRootTransform()
        {
            // UIPartの追加先を決定する
            Transform layer = m_TargetLayer.RootTransform.Find("Layer");
            RootTransform.SetParent(layer);
            RootTransform.localPosition = new Vector3(0, 0, 0);
            RootTransform.localScale = Vector3.one;
        }

        private void InitHomeScrollerController()
        {
            m_HomeScrollerView.Init(CellViewInstantiated, ScrollerScrollingChanged,
                ScrollerBeginDrag, ScrollerEndDrag);
        }

        // 新規セルビュー追加時デリゲート
        private void CellViewInstantiated(EnhancedScroller scroller, EnhancedScrollerCellView cellView)
        {
            UIPart part = null;
            switch (cellView.cellIdentifier)
            {
                case "ShopCellView":
                    part = new ShopPart(cellView as ShopCellView);
                    break;
                case "BallCellView":
                    part = new BallPart(cellView as BallCellView, m_HomeScrollerView);
                    break;
                case "BattleCellView":
                    part = new BattlePart(cellView as BattleCellView);
                    break;
                case "SocialCellView":
                    part = new SocialPart(cellView as SocialCellView);
                    break;
                case "EventCellView":
                    part = new EventPart(cellView as EventCellView);
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
            UIController.Instance.AttachParts(m_TargetLayer, parts);
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
            int index = (int)(scroller.ScrollPosition / m_HomeScrollerView.CellSize + 0.5f);
            bool isPrevShift = (int)scroller.ScrollPosition % (int)m_HomeScrollerView.CellSize
                               > m_HomeScrollerView.CellSize / 2;

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
            if (m_HomeScrollerView.m_Scroller.IsTweening)
            {
                return;
            }

            m_HomeScrollerView.m_Scroller.JumpToDataIndex(index, 0, 0, true,
                EnhancedScroller.TweenType.easeOutQuart, 0.5f,
                () => m_HomeScrollerView.m_Scroller.Velocity = Vector2.zero);
        }
    }
}