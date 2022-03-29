using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour
{

    [System.NonSerialized]
    public int currentStageNum = 0; //現在のステージ番号（0始まり）

    [SerializeField]
    string[] stageName; //ステージ名

    //[SerializeField]
    //GameObject fadeCanvasPrefab;

    [SerializeField]
    float fadeWaitTime = 0.5f; //フェード時の待ち時間

    //GameObject fadeCanvasClone;
    //FadeCanvas fadeCanvas;

    //最初の処理
    void Start()
    {
        //シーンを切り替えてもこのゲームオブジェクトを削除しないようにする
        DontDestroyOnLoad(gameObject);
    }

    //毎フレームの処理
    void Update()
    {

    }

    //次のステージに進む処理
    public void NextStage()
    {
        currentStageNum += 1;

        //コルーチンを実行
        StartCoroutine(WaitForLoadScene());
    }


    //シーンの読み込みと待機を行うコルーチン
    IEnumerator WaitForLoadScene()
    {

        ////フェードオブジェクトを生成
        //fadeCanvasClone = Instantiate(fadeCanvasPrefab);

        ////コンポーネントを取得
        //fadeCanvas = fadeCanvasClone.GetComponent<FadeCanvas>();

        ////フェードインさせる
        //fadeCanvas.fadeIn = true;

        yield return new WaitForSeconds(fadeWaitTime);

        //シーンを非同期で読込し、読み込まれるまで待機する
        yield return SceneManager.LoadSceneAsync(stageName[currentStageNum]);

        ////フェードアウトさせる
        //fadeCanvas.fadeOut = true;
    }


}