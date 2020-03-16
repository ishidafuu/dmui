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
}