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
        BackVisible = 1,
        BackTouchable = (1 << 1),
        TouchEventCallable = (1 << 2),
        SystemUntouchable = (1 << 3),
        LoadingWithoutFade = (1 << 4),
        ActiveWithoutFade = (1 << 5),
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