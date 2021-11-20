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

        // ������
        Initialize();
    }

    /// <summary>
    /// Video �����̏�����
    /// </summary>
    private void Initialize() {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        videoPlayer.clip = null;

        // ������đO�̃e�N�X�`�����폜
        videoPlayer.targetTexture.Release();
    }

    /// <summary>
    /// �f�o�b�O�p(Awake �ł��Ȃ����ƁBDataBaseManager �̏������������ĊԂɍ���Ȃ�)
    /// </summary>
    /// <returns></returns>
    IEnumerator Start() {

        yield return null;

        // VideoClip �̏���
        //PrepareVideoClip(1);
    }

    /// <summary>
    /// VideoClip �̏���
    /// </summary>
    /// <param name="videoNo"></param>
    public void PrepareVideoClip(int setVideoNo) {

        // VideoClip �����ݒ�Ȃ�
        if (videoPlayer.clip == null) {

            // �Ώۂ� VideoClip ���������Đݒ�
            videoPlayer.clip = DataBaseManager.instance.GetVideoData(setVideoNo).videoClip;

            // �ǂݍ��݌�̃C�x���g�̃R�[���o�b�N�o�^
            videoPlayer.prepareCompleted += OnCompletePrepare;

            // �ǂݍ��݊J�n
            videoPlayer.Prepare();

            Debug.Log("VideoClip ���[�h�J�n");

            // TODO �t�F�C�h�C���ƍ��킹��

        }

        /// <summary>
        /// Prepare �������ɌĂ΂��R�[���o�b�N
        /// </summary>
        /// <param name="vp"></param>
        void OnCompletePrepare(VideoPlayer vp) {

            // �C�x���g�̃R�[���o�b�N����폜(�c���Ă���Ǝ�������s����邽��)
            videoPlayer.prepareCompleted -= OnCompletePrepare;

            Debug.Log("VideoClip ���[�h����");

            // �Đ�
            StartCoroutine(PlayVideo());
        }

        //// �ǂݍ��ނ܂őҋ@(videoPlayer.prepareCompleted ���g��Ȃ��ꍇ)
        //while (!videoPlayer.isPrepared)
        //    yield return null;
    }

    /// <summary>
    /// VideoClip �̍Đ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayVideo() {

        // TODO �t�F�[�h�C�����čĐ�(�ȈՁB��Ńg�����W�V�����ƍ��킹��)
        canvasGroup.DOFade(1.0f, 1.0f).OnComplete(() => canvasGroup.blocksRaycasts = true);   // OnComplete ��Play ����ƃ_��

        videoPlayer.Play();

        Debug.Log("VideoClip �Đ�");

        // �Đ����I������܂őҋ@
        while (videoPlayer.isPlaying) {
            yield return null;
        }

        // ��~
        StopVideo();
    }

    /// <summary>
    /// VideoClip �̈ꎞ��~
    /// </summary>
    public void PauseVideo() {

        // �Đ����� VideoClip ������ꍇ
        if (videoPlayer.isPlaying) {

            // �ꎞ��~
            videoPlayer.Pause();

            Debug.Log("VideoClip �ꎞ��~");
        }
    }

    /// <summary>
    /// VideoClip �̒�~
    /// </summary>
    public void StopVideo() {

        // ��~
        videoPlayer.Stop();

        // �t�F�[�h�A�E�g���ď�����
        canvasGroup.DOFade(0, 1.0f)
            .OnComplete(() => {
                Initialize();
            });

        Debug.Log("VideoClip ��~");
    }
}
