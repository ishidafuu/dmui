﻿// [概要]
// UIレイヤーはOnUpdate()の呼び出しを明示すると、
// UIController コンポーネント の Update()と同フレームで
// OnUpdate()が呼び出されます。
// また、UIレイヤーは他のレイヤー全てに向けてイベントを発信することができます。
// [操作]
// 画面下のボタンを押す
// [結果]
// 緑画面下のボタンを押すと
// 黄四角内に、更新回数が表示されます。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample11 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator(), new LoadingCreator(), new ToastCreator());
			UIController.Instance.AddFront(new Sample11Scene());
		}
	}

	class DispachParams {
		public int count;
		public DispachParams(int c) {
			count = c;
		}
	}

	class Sample11Scene : UIBase {

		private int m_count = 0;

		public Sample11Scene() : base("UISceneA", EnumUIGroup.Scene) {
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);

			UIController.Instance.AddFront(new Sample11Dialog());

			IsScheduleUpdate = true;
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonBottom": {
					UIController.Instance.Dispatch("Sample", new DispachParams(m_count));
					return true;
				}
				default: {
					return false;
				}
			}
		}

		public override void OnUpdate() {
			m_count++;
		}
	}


	class Sample11Dialog : UIBase {

		public Sample11Dialog() : base("UIDialog", EnumUIGroup.Dialog, EnumUIPreset.BackVisible | EnumUIPreset.BackTouchable) {
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);
		}

		public override void OnDispatchedEvent(DispatchedEvent dispatchedEvent) {
			if (dispatchedEvent.EventName == "Sample") {
				Text text = RootTransform.Find("Layer/Text").GetComponent<Text>();
				text.text = ((DispachParams)dispatchedEvent.Param).count.ToString();
				Debug.Log("Scene11 : All Right");
			}
		}
	}
}
