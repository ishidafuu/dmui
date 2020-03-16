// [概要]
// UIPartをUIBaseへ紐づける(YieldAttachParts())ことにより、
// UIPartとして個別に処理を分けることができます。
// 状況によって扱う数が変動するものをUIPartとして扱うと効果的です。
// 　例：リスト内の1項目を1つのUIPartとして扱う
// [操作]
// それぞれのボタンを押す
// [結果]
// 押したボタンに応じたログを表示します。

using System.Collections.Generic;
using DMUIFramework.Samples;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace DM {

	public class Sample14_2 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator());
			UIController.Instance.AddFront(new Sample14_2Scene());
		}
	}

	class Sample14_2Scene : UIBase {

		public Sample14_2Scene() : base("UISceneA_2", EnumUIGroup.Scene) {
		}

		public override async UniTask OnLoadedBase()
		{
			// var controller14_2 = RootTransform.GetComponent<Controller14_2>();
			// controller14_2.Init();
			
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);


			List<UIPart> parts = new List<UIPart>
			{
				new Sample14_2Scroller()
			};

			await UIController.Instance.YieldAttachParts(this, parts);
		}
	}

	class Sample14_2Scroller : UIPart
	{
		private UIBase m_TargetLayer;

		public Sample14_2Scroller() : base("Scroller") { }
		
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
			RootTransform.localPosition = new Vector3(200, 500,0);
			RootTransform.localScale = Vector3.one;
			
			// cellview
			List<UIPart> parts = new List<UIPart>();
			int cellCount = controller14_2.scroller.GetActiveCellViewsCount();
			for (int i = 0; i < cellCount; i++)
			{
				CellView14_2 cell = controller14_2.scroller.GetCellViewAtDataIndex(i) as CellView14_2;
				parts.Add(new Sample14_2CellViewButton(cell, cell.textButton));
				parts.Add(new Sample14_2CellViewButton(cell, cell.fixedIntegerButton));
				parts.Add(new Sample14_2CellViewButton(cell, cell.dataIntegerButton));		
			}

			// 追加待ち
			await UIController.Instance.YieldAttachParts(targetLayer, parts);	
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			Debug.Log("push Sample14_2Scroller: ");
			Debug.Log("Scene14 : All Right");

			return true;
		}

		// 新規セルビュー追加時デリゲート
		private void CellViewInstantiated(EnhancedScroller scroller, EnhancedScrollerCellView cellView)
		{
			List<UIPart> parts = new List<UIPart>();

			CellView14_2 cell = cellView as CellView14_2;
			parts.Add(new Sample14_2CellViewButton(cell, cell.textButton));
			parts.Add(new Sample14_2CellViewButton(cell, cell.fixedIntegerButton));
			parts.Add(new Sample14_2CellViewButton(cell, cell.dataIntegerButton));

			// 即時追加
			UIController.Instance.AttachParts(m_TargetLayer, parts);	
		}
	}
	
	class Sample14_2CellViewButton : UIPart
	{
		private readonly CellView14_2 m_cellView14_2;

		public Sample14_2CellViewButton(CellView14_2 cellView14_2, GameObject buttonObject) 
			: base(buttonObject.transform)
		{
			m_cellView14_2 = cellView14_2;
		}

		public override async UniTask OnLoadedPart(UIBase targetLayer) {　}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			// TouchListenerを継承してGetComponentせずに済むようなクラスを作ってもいいかも
			Debug.Log($"{m_cellView14_2.someTextText.text}");
			return true;
		}
	}
	
	class Sample14_2Button : UIPart {

		private readonly int m_id = 0;

		public Sample14_2Button(int id) : base("UIButton_2") {
			m_id = id;
		}

		public override async UniTask OnLoadedPart(UIBase targetLayer) {
			Text text = RootTransform.Find("Text").GetComponent<Text>();
			text.text = m_id.ToString();

			// UIPartの追加先を決定する
			Transform layer = targetLayer.RootTransform.Find("Layer");
			RootTransform.SetParent(layer);
			RootTransform.localPosition = new Vector3(426, 100 * m_id, 0);
			RootTransform.localScale = Vector3.one;
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			Debug.Log("push button: " + m_id);
			Debug.Log("Scene14 : All Right");

			return true;
		}
	}
}