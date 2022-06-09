using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public int gameTime;

    [SerializeField]
    private GameManager gameManager;


    private void Start() {
        StartCoroutine(ObserveGameTime());
    }

    /// <summary>
    /// �Q�[���i�s�ɍ��킹���Q�[�����Ԃ̊Ď�
    /// </summary>
    /// <returns></returns>
    private IEnumerator ObserveGameTime() {
        float timer = 0;
        while (true) {
            // GameUp �ɂȂ�����Ď��I��
            if (gameManager.currentGameState == GameState.GameUp) {
                break;
            }

            // �~�b�V�������J�n���ꂽ��J�E���g�_�E���J�n
            if (gameManager.currentGameState == GameState.Play_Mission) {
                CountDown();
            }
            yield return null;
        }

        /// <summary>
        /// �Q�[�����Ԃ̃J�E���g�_�E��
        /// </summary>
        void CountDown() {
            // ���Ԃ̌v���J�n
            timer += Time.deltaTime;

            // 1�b���J�E���g�_�E��
            if (timer > 1.0f) {
                timer = 0;
                gameTime--;

                // �c�莞�Ԃ� 0 �ȉ��ɂȂ�����
                if (gameTime <= 0) {
                    // �Q�[���I��
                    gameManager.currentGameState = GameState.GameUp;
                }
            }
        }
    }

    /// <summary>
    /// �Q�[�����Ԃ̌v�Z
    /// </summary>
    /// <param name="amount"></param>
    public void CalcGameTime(int amount) {
        gameTime += amount;
    }
}
