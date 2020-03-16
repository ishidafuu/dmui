// [概要]
// UIPartをUIBaseへ紐づける(YieldAttachParts())ことにより、
// UIPartとして個別に処理を分けることができます。
// 状況によって扱う数が変動するものをUIPartとして扱うと効果的です。
// 　例：リスト内の1項目を1つのUIPartとして扱う
// [操作]
// それぞれのボタンを押す
// [結果]
// 押したボタンに応じたログを表示します。

using DMUIFramework.Samples;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace DM
{
    public class Sample14_2 : MonoBehaviour
    {
        void Start()
        {
            UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator());
            UIController.Instance.AddFront(new Sample14_2Scene());
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

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }

        public override bool OnClick(TouchEvent touch, UISound uiSound)
        {
            // TouchListenerを継承してGetComponentせずに済むようなクラスを作ってもいいかも
            Debug.Log($"{m_cellView14_2.someTextText.text}");
            return true;
        }
    }
}