using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager instance;

    public int gameTime;

    [SerializeField]
    private GameManager gameManager;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }   
    }

    void Start() {
        StartCoroutine(ObserveGameTime());
    }

    /// <summary>
    /// ゲーム進行に合わせたゲーム時間の監視
    /// </summary>
    /// <returns></returns>
    private IEnumerator ObserveGameTime() {
        float timer = 0;
        while (true) {
            // GameUp になったら監視終了
            if (gameManager.currentGameState == GameState.GameUp) {
                Debug.Log("ゲーム終了");
                break;
            }

            // ミッションが開始されたらカウントダウン開始
            if (gameManager.currentGameState == GameState.Play_Mission) {
                CountDown();
                Debug.Log("ミッション開始にあわせてゲーム時間のカウントダウン開始");
            }
            yield return null;
        }

        /// <summary>
        /// ゲーム時間のカウントダウン
        /// </summary>
        void CountDown() {
            // 時間の計測開始
            timer += Time.deltaTime;

            // 1秒ずつカウントダウン
            if (timer > 1.0f) {
                timer = 0;
                gameTime--;

                // 残り時間が 0 以下になったら
                if (gameTime <= 0) {
                    // ゲーム終了
                    gameManager.currentGameState = GameState.GameUp;
                }
            }
        }
    }

    /// <summary>
    /// ゲーム時間の計算
    /// </summary>
    /// <param name="amount"></param>
    public void CalcGameTime(int amount) {
        gameTime += amount;
    }
}
