using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class VideoClipManager : MonoBehaviour
{
    public static VideoClipManager instance;

    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public VideoClip clip;


    public bool IsVideoPlaying { get => videoPlayer.isPlaying; }


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
        canvasGroup.blocksRaycasts = false;

        videoPlayer.clip = null;

        // 解放して前のテクスチャを削除
        videoPlayer.targetTexture.Release();
    }

    /// <summary>
    /// デバッグ用(Awake でやらないこと。DataBaseManager の準備が競合して間に合わない)
    /// </summary>
    /// <returns></returns>
    IEnumerator Start() {

        yield return null;

        // VideoClip の準備
        //PrepareVideoClip(1);
    }

    /// <summary>
    /// VideoClip の準備
    /// </summary>
    /// <param name="videoNo"></param>
    public void PrepareVideoClip(int setVideoNo, VideoClip sourceVideoClip = null) {

        // VideoClip が未設定なら
        if (videoPlayer.clip == null) {

            // VideoClip の引数情報がない場合
            if (sourceVideoClip == null) {

                // 対象の VideoClip を検索して設定
                videoPlayer.clip = DataBaseManager.instance.GetVideoData(setVideoNo).videoClip;
            } else {

                // VideoClip の情報がある場合は、それを使う
                videoPlayer.clip = sourceVideoClip;
            }
            

            // 読み込み後のイベントのコールバック登録
            videoPlayer.prepareCompleted += OnCompletePrepare;

            // 読み込み開始
            videoPlayer.Prepare();

            Debug.Log("VideoClip ロード開始");

            // TODO フェイドインと合わせる

        }

        /// <summary>
        /// Prepare 完了時に呼ばれるコールバック
        /// </summary>
        /// <param name="vp"></param>
        void OnCompletePrepare(VideoPlayer vp) {

            // イベントのコールバックから削除(残っていると次回も実行されるため)
            videoPlayer.prepareCompleted -= OnCompletePrepare;

            Debug.Log("VideoClip ロード完了");

            // 再生
            StartCoroutine(PlayVideo(DataBaseManager.instance.GetVideoData(setVideoNo).bgmType));
        }

        //// 読み込むまで待機(videoPlayer.prepareCompleted を使わない場合)
        //while (!videoPlayer.isPrepared)
        //    yield return null;
    }

    /// <summary>
    /// VideoClip の再生
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayVideo(SoundManager.BGM_Type bgmType = SoundManager.BGM_Type.SILENCE) {

        // TODO フェードインして再生(簡易。後でトランジションと合わせる)
        canvasGroup.DOFade(1.0f, 1.0f).OnComplete(() => canvasGroup.blocksRaycasts = true);   // OnComplete でPlay するとダメ

        // BGM 再生
        if (bgmType == SoundManager.BGM_Type.SILENCE) {
            // 設定がない場合には停止
            SoundManager.instance.MuteBGM();
        } else {
            SoundManager.instance.PlayBGM(bgmType);
        }

        videoPlayer.Play();

        Debug.Log("VideoClip 再生");

        // 再生が終了するまで待機
        while (videoPlayer.isPlaying) {

            // 再生中にタップしたら再生停止してスキップ
            if (Input.GetMouseButtonDown(0)) {
                PauseVideo();
            }
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

            // 一時停止(isPlayng が false になる)
            videoPlayer.Pause();

            Debug.Log("VideoClip 一時停止");
        }
    }

    /// <summary>
    /// VideoClip の停止
    /// </summary>
    public void StopVideo() {

        // 停止
        videoPlayer.Stop();

        // フェードアウトして初期化
        canvasGroup.DOFade(0, 1.0f)
            .OnComplete(() => {
                Initialize();
            });

        Debug.Log("VideoClip 停止");
    }
}