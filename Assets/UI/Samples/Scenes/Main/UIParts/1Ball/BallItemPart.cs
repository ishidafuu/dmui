using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;

namespace DM
{
    class BallItemPart : UIPart
    {
        private UIBase m_TargetLayer;

        public BallItemPart() : base("BallItem") { }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            m_TargetLayer = targetLayer;
            BallScrollerController ballScrollerController =
                targetLayer.RootTransform.GetComponent<BallScrollerController>();
            // laboScrollerController.m_Scroller = RootTransform.GetComponent<EnhancedScroller>();
            // laboScrollerController.Init();
            // laboScrollerController.m_Scroller.ReloadData();
            // // 新規セルビュー追加時デリゲート
            // laboScrollerController.m_Scroller.cellViewInstantiated = CellViewInstantiated;

            // UIPartの追加先を決定する
            // Transform layer = targetLayer.RootTransform.Find("Layer");
            // RootTransform.SetParent(layer);
            // RootTransform.localPosition = new Vector3(200, 500, 0);
            // RootTransform.localScale = Vector3.one;
            //
            // // cellview
            // List<UIPart> parts = new List<UIPart>();
            // int cellCount = laboScrollerController.m_Scroller.GetActiveCellViewsCount();
            // for (int i = 0; i < cellCount; i++)
            // {
            //     LaboItemCellView laboItemCell = laboScrollerController.m_Scroller.GetCellViewAtDataIndex(i) as LaboItemCellView;
            //     if (laboItemCell == null)
            //     {
            //         continue;
            //     }
            //
            //     parts.Add(new LaboItemButtonPart(laboItemCell, laboItemCell.textButton));
            //     parts.Add(new LaboItemButtonPart(laboItemCell, laboItemCell.fixedIntegerButton));
            //     parts.Add(new LaboItemButtonPart(laboItemCell, laboItemCell.dataIntegerButton));
            // }
            //
            // // 追加待ち
            // await UIController.Instance.YieldAttachParts(targetLayer, parts);
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
            BallItemCellView ballItemCell = cellView as BallItemCellView;
            if (ballItemCell == null)
            {
                return;
            }

            List<UIPart> parts = new List<UIPart>
            {
                new BallItemButtonPart(ballItemCell, ballItemCell.textButton),
                new BallItemButtonPart(ballItemCell, ballItemCell.fixedIntegerButton),
                new BallItemButtonPart(ballItemCell, ballItemCell.dataIntegerButton)
            };
            // 即時追加
            UIController.Instance.AttachParts(m_TargetLayer, parts);
        }
    }
}