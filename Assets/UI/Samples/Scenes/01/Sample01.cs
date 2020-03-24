// [概要]
// AddFront()によるUIレイヤーの追加を行ない、prefab内のラベルを書き換えます。
// 通信処理などを想定したWaitForSeconds()により時間を空けています。
// [操作]
// 操作なし
// [結果]
// 緑の画面が表示され、中央に"Scene"と表示されます。

using System.Collections;
using System.Collections.Generic;
using DMUIFramework.Samples;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace DM {

	public class Sample01 : MonoBehaviour {

		void Start () {
			UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator(), new LoadingCreator());
			UIController.Instance.AddFront(new Sample01Scene());
		}
	}

	class Sample01Scene : UIBase {

		public Sample01Scene() : base("UISceneA", EnumUIGroup.Scene) {
		}

		public override async UniTask OnLoadedBase() {
			
			await UniTask.DelayFrame(120); 

			Text text = RootTransform.Find("Layer/Text").GetComponent<Text>();
			text.text = "Scene";

			RootTransform.Find("Layer/ButtonTop"   ).gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);
			RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);

			Debug.Log("Scene01 : All Right");
		}
	}
}
