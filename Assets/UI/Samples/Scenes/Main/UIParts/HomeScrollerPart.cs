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
        private HomeScrollerController m_HomeScrollerController;
        private HomeScrollerView m_HomeScrollerView;

        // public HomeScrollerPart() : base("HomeScroller") { }
        public HomeScrollerPart(HomeScrollerView homeScrollerView) : base(homeScrollerView.transform)
        {
            m_HomeScrollerView = homeScrollerView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            m_TargetLayer = targetLayer;
            InitRootTransform();

            m_HomeScrollerController = InitHomeScrollerController();
            // m_HomeScrollerController.m_HomeTabControl.Init((int)UIController.Instance.m_CanvasScaler.referenceResolution.x / 5);

            List<UIPart> parts = new List<UIPart>
            {
                new HomeTabPart(m_HomeScrollerView.m_TabView)
            };
            // int cellCount = m_HomeScrollerController.m_Scroller.GetActiveCellViewsCount();
            // for (int i = 0; i < cellCount; i++)
            // {
            //     HomeCellView cell = controller.m_Scroller.GetCellViewAtDataIndex(i) as HomeCellView;
            //     if (cell == null)
            //     {
            //         continue;
            //     }
            //
            //     parts.Add(new Sample14_2CellViewButton(cell, cell.textButton));
            //     parts.Add(new Sample14_2CellViewButton(cell, cell.fixedIntegerButton));
            //     parts.Add(new Sample14_2CellViewButton(cell, cell.dataIntegerButton));
            // }

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }

        private void InitRootTransform()
        {
            // UIPartの追加先を決定する
            Transform layer = m_TargetLayer.RootTransform.Find("Layer");
            RootTransform.SetParent(layer);
            RootTransform.localPosition = new Vector3(0, 0, 0);
            RootTransform.localScale = Vector3.one;
        }

        private HomeScrollerController InitHomeScrollerController()
        {
            HomeScrollerController controller = m_TargetLayer.RootTransform.GetComponent<HomeScrollerController>();
            controller.Init(RootTransform.GetComponent<EnhancedScroller>());
            controller.m_Scroller.ReloadData();
            // 新規セルビュー追加時デリゲート
            controller.m_Scroller.cellViewInstantiated = CellViewInstantiated;
            controller.m_Scroller.scrollerScrollingChanged = ScrollerScrollingChanged;
            controller.m_Scroller.scrollerBeginDrag = ScrollerBeginDrag;
            controller.m_Scroller.scrollerEndDrag = ScrollerEndDrag;
            // controller.m_Scroller.scrollerSnapped = ScrollerSnapped;
            return controller;
        }

        public override bool OnClick(TouchEvent touch, UISound uiSound)
        {
            Debug.Log("push Sample14_2Scroller: ");
            Debug.Log("Scene14 : All Right");


            return true;
        }

        // 新規セルビュー追加時デリゲート
        private void CellViewInstantiated(EnhancedScroller scroller, EnhancedScrollerCellView cellView)
        {
            UIPart part = null;

            Debug.Log($"CellViewInstantiated{cellView.cellIndex}");
            // switch (cellView.cellIndex)
            // {
            //     case 0:
            //         part = new HomeCell0ShopPart(cellView.GetComponent<HomeCell0ShopView>());
            //         break;
            //     case 1:
            //         part = new HomeCell1LaboratoryPart(cellView.GetComponent<HomeCell1LaboratoryView>());
            //         break;
            //     case 2:
            //         part = new HomeCell2BattlePart(cellView.GetComponent<HomeCell2BattleView>());
            //         break;
            //     case 3:
            //         part = new HomeCell3SocialPart(cellView.GetComponent<HomeCell3SocialView>());
            //         break;
            //     case 4:
            //         part = new HomeCell4EventPart(cellView.GetComponent<HomeCell4EventView>());
            //         break;
            // }
            //
            // List<UIPart> parts = new List<UIPart>
            // {
            //     part
            // };
            //
            // // 即時追加
            // UIController.Instance.AttachParts(m_TargetLayer, parts);
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

            if (scroller.IsTweening)
                return;

            const int BORDER = 0;
            var index = (int)(scroller.ScrollPosition / m_HomeScrollerController.CellSize + 0.5f);
            bool isPrevShift = (int)scroller.ScrollPosition % (int)m_HomeScrollerController.CellSize
                               > m_HomeScrollerController.CellSize / 2;

            if (data.delta.x < -BORDER && !isPrevShift)
            {
                index += 1;
            }
            else if (data.delta.x > BORDER && isPrevShift)
            {
                index -= 1;
            }

            scroller.JumpToDataIndex(index, 0, 0, true,
                EnhancedScroller.TweenType.easeOutQuart, 0.5f,
                () => scroller.Velocity = Vector2.zero);
        }

        // private void ScrollerSnapped(EnhancedScroller scroller, int cellIndex, int dataIndex,
        //     EnhancedScrollerCellView cellView)
        // {
        //     scroller.snapping = false;
        // }
    }
}