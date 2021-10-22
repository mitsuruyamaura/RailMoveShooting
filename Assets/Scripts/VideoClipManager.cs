using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;

public class VideoClipManager : MonoBehaviour
{
    public static VideoClipManager instance;

    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public VideoClip clip;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        // 初期化
        Initialize();
    }

    /// <summary>
    /// Video 処理の初期化
    /// </summary>
    private void Initialize() {
        canvasGroup.alpha = 0;

        videoPlayer.clip = null;

        // 解放して前のテクスチャを削除
        videoPlayer.targetTexture.Release();
    }

    /// <summary>
    /// デバッグ用
    /// </summary>
    /// <returns></returns>
    IEnumerator Start() {

        yield return null;

        PrepareVideo(1);
    }

    /// <summary>
    /// VideoClip の準備
    /// </summary>
    /// <param name="videoNo"></param>
    public void PrepareVideo(int setVideoNo) {

        // VideoClip が未設定なら
        if (videoPlayer.clip == null) {

            // 対象の VideoClip を検索して設定
            videoPlayer.clip = DataBaseManager.instance.GetVideoData(setVideoNo).videoClip;

            // 読み込み後のコールバック登録
            videoPlayer.prepareCompleted += OnCompletePrepare;

            // 読み込み開始
            videoPlayer.Prepare();

            // TODO フェイドインと合わせる
            
        }

        //// 読み込むまで待機(videoPlayer.prepareCompleted を使わない場合)
        //while (!videoPlayer.isPrepared)
        //    yield return null;

        /// <summary>
        /// Prepare 完了時に呼ばれるコールバック
        /// </summary>
        /// <param name="vp"></param>
        void OnCompletePrepare(VideoPlayer vp) {

            videoPlayer.prepareCompleted -= OnCompletePrepare;

            Debug.Log("ロード完了");

            // 再生
            StartCoroutine(PlayVideo());
        }
    }

    /// <summary>
    /// VideoClip の再生
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayVideo() {

        // TODO フェードインして再生(簡易。後でトランジションと合わせる)
        canvasGroup.DOFade(1.0f, 1.0f);   // OnComplete でPlay するとダメ

        videoPlayer.Play();

        Debug.Log("再生");

        // 再生が終了するまで待機
        while (videoPlayer.isPlaying) {
            yield return null;
        }

        // 停止
        StopVideo();
    }

    /// <summary>
    /// VideoClip の一時停止
    /// </summary>
    public void PauseVideo() {

        // 再生中の VideoClip がある場合
        if (videoPlayer.isPlaying) {

            // 一時停止
            videoPlayer.Pause();

            Debug.Log("一時停止");
        }
    }

    /// <summary>
    /// VideoClip の停止
    /// </summary>
    public void StopVideo() {

        // 停止
        videoPlayer.Stop();

        // フェードアウトして初期化
        canvasGroup.DOFade(0, 1.0f).OnComplete(() => { Initialize(); });       

        Debug.Log("停止");
    }
}
