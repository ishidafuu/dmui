using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;

namespace DM {
    class CellViewScrollerPart : UIPart
    {
        private UIBase m_TargetLayer;

        public CellViewScrollerPart() : base("CellViewScroller") { }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            m_TargetLayer = targetLayer;
            Controller14_2 controller14_2 = targetLayer.RootTransform.GetComponent<Controller14_2>();
            controller14_2.scroller = RootTransform.GetComponent<EnhancedScroller>();
            controller14_2.Init();
            controller14_2.scroller.ReloadData();
            // 新規セルビュー追加時デリゲート
            controller14_2.scroller.cellViewInstantiated = CellViewInstantiated;

            // UIPartの追加先を決定する
            Transform layer = targetLayer.RootTransform.Find("Layer");
            RootTransform.SetParent(layer);
            RootTransform.localPosition = new Vector3(200, 500, 0);
            RootTransform.localScale = Vector3.one;

            // cellview
            List<UIPart> parts = new List<UIPart>();
            int cellCount = controller14_2.scroller.GetActiveCellViewsCount();
            for (int i = 0; i < cellCount; i++)
            {
                CellView14_2 cell = controller14_2.scroller.GetCellViewAtDataIndex(i) as CellView14_2;
                if (cell == null)
                {
                    continue;
                }

                parts.Add(new Sample14_2CellViewButton(cell, cell.textButton));
                parts.Add(new Sample14_2CellViewButton(cell, cell.fixedIntegerButton));
                parts.Add(new Sample14_2CellViewButton(cell, cell.dataIntegerButton));
            }

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
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
            CellView14_2 cell = cellView as CellView14_2;
            if (cell == null)
            {
                return;
            }

            List<UIPart> parts = new List<UIPart>
            {
                new Sample14_2CellViewButton(cell, cell.textButton),
                new Sample14_2CellViewButton(cell, cell.fixedIntegerButton),
                new Sample14_2CellViewButton(cell, cell.dataIntegerButton)
            };
            // 即時追加
            UIController.Instance.AttachParts(m_TargetLayer, parts);
        }
    }
}