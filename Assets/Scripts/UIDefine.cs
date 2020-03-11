using System;

namespace DM
{
    public enum BaseLayerState
    {
        None, // 初期値

        // ↓ invisible, untouchable
        InFading, // フェード終了待ち
        Loading, // 読み込み待ち
        Adding, // リストに追加待ち

        // ↓ visible
        InAnimation, // 登場アニメーション中

        // ↓ screen touchable
        Active, // 有効

        // ↓ screen untouchable
        OutAnimation, // 退場アニメーション中
        OutFading, // フェード終了待ち

        // ↓ invisible
        UselessLoading, // 無駄読み中
        Removing, // リストから削除待ち
    }

    [Flags]
    public enum UIPreset
    {
        None = 0,
        // 背面のレイヤを表示
        BackVisible = 1,
        // 背面のタッチ有効
        BackTouchable = (1 << 1),
        // レイヤのタッチ有効
        TouchEventCallable = (1 << 2),
        // レイヤのタッチ無効（レイヤ削除時に再タッチ可能通知を送らない）
        SystemUntouchable = (1 << 3),
        // フェード状態を無視してロード可能
        LoadingWithoutFade = (1 << 4),
        // フェード状態を無視してアクティブ化可能
        ActiveWithoutFade = (1 << 5),
        // 3Dグラフィックレイヤに配置
        View3D = (1 << 6),

        SystemIndicator = (BackVisible | BackTouchable | SystemUntouchable | LoadingWithoutFade | ActiveWithoutFade),
    }
    
    public enum UIGroup
    {
        None = 0,
        View3D,
        MainScene,
        Scene,
        Floater,
        Dialog,
        Debug,
        SystemFade,
        System,
    }
    
    public enum TouchType
    {
        None = 0,
        Click,
        Down,
        Up,
        Drag,
    }
}