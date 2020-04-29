using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;

namespace DM
{
    class BallItemPart : UIPart
    {
        private UIBase m_TargetLayer;
        private BallItemCellView m_BallItemCellView;

        public BallItemPart(BallItemCellView ballItemCellView)
            : base(ballItemCellView.transform)
        {
            m_BallItemCellView = ballItemCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            m_TargetLayer = targetLayer;
            BallScrollerView mixedLineScrollerView =
                targetLayer.RootTransform.GetComponent<BallScrollerView>();
            
            // List<UIPart> parts = new List<UIPart>()
            // {
            //     new BallItemButtonPart(m_BallItemCellView)
            // };
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