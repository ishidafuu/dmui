// [概要]
// UIPartをUIBaseへ紐づける(YieldAttachParts())ことにより、
// UIPartとして個別に処理を分けることができます。
// 状況によって扱う数が変動するものをUIPartとして扱うと効果的です。
// 　例：リスト内の1項目を1つのUIPartとして扱う
// [操作]
// それぞれのボタンを押す
// [結果]
// 押したボタンに応じたログを表示します。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
				new Sample14_2Scroller(0)
			};


			// const int num = 4;
			// for (int i = 1; i <= num; i++) {
			// 	parts.Add(new Sample14_2Button(i));
			// }
			await UIController.Instance.YieldAttachParts(this, parts);
		}
	}

	class Sample14_2Scroller : UIPart {

		private int m_id = 0;

		public Sample14_2Scroller(int id) : base("Scroller") {
			m_id = id;
		}
		
		// public Sample14_2Button(int id, Transform buttonTransform) : base(buttonTransform) {
		// 	m_id = id;
		// }

		public override async UniTask OnLoadedPart(UIBase targetLayer) {

			Controller14_2 controller14_2 = targetLayer.RootTransform.GetComponent<Controller14_2>();
			controller14_2.scroller = RootTransform.GetComponent<EnhancedScroller>();
			controller14_2.Init();
			controller14_2.scroller.Update();
			
			// UIPartの追加先を決定する
			Transform layer = targetLayer.RootTransform.Find("Layer");
			RootTransform.SetParent(layer);
			RootTransform.localPosition = new Vector3(200, 500,0);
			RootTransform.localScale = Vector3.one;
			
			// cellview
			List<UIPart> parts = new List<UIPart>();
			for (int i = 0; i < 5; i++)
			{
				CellView14_2 cell = controller14_2.scroller.GetCellViewAtDataIndex(i) as CellView14_2;
				parts.Add(new Sample14_2CellViewButton(i, cell.textButton));
				parts.Add(new Sample14_2CellViewButton(i, cell.fixedIntegerButton));
				parts.Add(new Sample14_2CellViewButton(i, cell.dataIntegerButton));		
			}

			UIController.Instance.AttachParts(targetLayer, parts);	
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			Debug.Log("push Sample14_2Scroller: " + m_id);
			Debug.Log("Scene14 : All Right");

			return true;
		}
	}
	
	class Sample14_2CellViewButton : UIPart {

		private int m_id = 0;

		public Sample14_2CellViewButton(int id, GameObject buttonObject) : base(buttonObject.transform) {
			m_id = id;
		}

		public override async UniTask OnLoadedPart(UIBase targetLayer) {
			// UIPartの追加先を決定する
			// Transform layer = targetLayer.RootTransform.Find("Layer");
			// RootTransform.SetParent(layer);
			// RootTransform.localPosition = new Vector3(0, 0, 0);
			// RootTransform.localScale = Vector3.one;
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			Debug.Log("push Sample14_2CellViewButton: " + m_id);
			Debug.Log("Scene14 : All Right");

			return true;
		}
	}
	
	class Sample14_2Button : UIPart {

		private int m_id = 0;

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