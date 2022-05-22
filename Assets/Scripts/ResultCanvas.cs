using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultCanvas : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Button btnFilterSubmit;

    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private Text txtSecretPoint;


    public void SetUpResultCanvas(int score, int secretPoint) {

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        canvasGroup.DOFade(1.0f, 1.0f).SetEase(Ease.InQuart).OnComplete(() => 
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(txtScore.DOCounter(0, score, 1.0f).SetEase(Ease.Linear));
            sequence.Append(txtSecretPoint.DOCounter(0, secretPoint, 1.0f).SetEase(Ease.Linear))
                .OnComplete(() => canvasGroup.blocksRaycasts = true);
        });
    }
}
