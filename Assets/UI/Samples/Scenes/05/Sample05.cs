// [概要]
// UIPreset.TouchEventCallable使用した最前面に透明の画面全体のタッチ領域をAddFront()します。
// 画面を押すとタッチの反応としてログが流れます。ボタンを押すと緑の画面は削除されます。
// また、背後のボタンが反応するので、UIPreset.BackTouchableの利用例となります。
// [操作]
// 適宜画面をタッチ操作する
// [結果]
// 操作挙動に応じたログが表示されます。
// ボタンを押すと画面に何も表示されなくなります。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DM {

	public class Sample05 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator(), new LoadingCreator());
			UIController.Instance.AddFront(new Sample05Scene());
		}
	}

	class Sample05Scene : UIBase {

		public Sample05Scene() : base("UISceneA", EnumUIGroup.Scene) {
			UIController.Instance.AddFront(new UISample05TouchLayer1());
		}

		public override async UniTask OnLoadedBase() {
			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);
		}

		public override bool OnClick(TouchEvent touch, UISound uiSound) {
			switch (touch.Listener.name) {
				case "ButtonCenter": {
					UIController.Instance.Remove(this);
					return true;
				}
				default: {
					return false;
				}
			}
		}

		protected override void OnDestroy() {
			Debug.Log("Scene05 : All Right");
		}
	}

	class UISample05TouchLayer1 : UIBase {

		public UISample05TouchLayer1()
		: base("", EnumUIGroup.System, EnumUIPreset.BackVisible | EnumUIPreset.BackTouchable | EnumUIPreset.TouchEventCallable) {

		}

		public override bool OnTouchDown(TouchEvent touch) {
			Debug.Log("touch down " + touch.Listener.name + ": " + touch.Pointer.position);
			return false;
		}

		public override bool OnTouchUp(TouchEvent touch) {
			Debug.Log("touch up " + touch.Listener.name + ": " + touch.Pointer.position);
			return false;
		}

		public override bool OnDrag(TouchEvent touch) {
			Debug.Log("touch drag " + touch.Listener.name + ": " + touch.Pointer.position);
			return false;
		}
	}
}
