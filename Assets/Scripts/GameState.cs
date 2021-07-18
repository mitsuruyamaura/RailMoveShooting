/// <summary>
/// ゲームの進行状態の種類
/// </summary>
public enum GameState {
    Debug,           // AR 時にエディターでデバッグを行う際に適用
    Tracking,        // AR のトラッキング中
    Wait,            // トラッキング完了後、ゲームの準備
    Play_Move,       // 移動中
    Play_Mission,    // ミッション中
    GameUp,          // ゲーム終了(ゲームオーバー、ゲームクリア)
}
