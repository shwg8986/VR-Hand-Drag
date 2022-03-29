using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InsideJudge : MonoBehaviour
{
    //[SerializeField]
    //OVRHand LHand;

    [SerializeField]
    OVRHand RHand;


    [SerializeField]
    GameObject Object;

    GameManage gameManager;

    public GameObject Parent;


    //Start is called before the first frame update
    void Start()
    {

        Parent = transform.parent.gameObject; //親オブジェクト取得
        GetComponent<Renderer>().material.color = new Color(0.9f, 0.1f, 0.1f, 0.0f);//色の設定
        Parent.GetComponent<Renderer>().material.color = new Color(0.0f, 0.4f, 0.0f, 0.6f);//親の色の設定

        gameManager = GameObject.Find("GameManager").GetComponent<GameManage>(); //シーンの切り替え処理で使用



        //オブジェクトのサイズを取得 縦横奥行き
        Transform Object_T = Object.gameObject.GetComponent<Transform>();
        Vector3 ObjectScale = Object_T.localScale;
        //ObjectScale.x; 

        //親オブジェクトのサイズを取得 縦横奥行き
        Transform Parent_T = Parent.gameObject.GetComponent<Transform>();
        Vector3 ParentScale = Parent_T.localScale;
        //ParentScale.x;


        //insideオブジェクトのサイズを変更する

        Transform inside_T = gameObject.GetComponent<Transform>();
        Vector3 insideScale = inside_T.localScale;


        if (ObjectScale.x > (ParentScale.x * 0.5f))
        {
            insideScale.x = 0.001f;
            insideScale.y = 0.001f;
            insideScale.z = 0.001f;

            inside_T.localScale = insideScale;
        }
        else
        {
            insideScale.x = ParentScale.x - 2.0f * ObjectScale.x;
            insideScale.y = ParentScale.y - 2.0f * ObjectScale.y;
            insideScale.z = ParentScale.z - 2.0f * ObjectScale.z;

            inside_T.localScale = insideScale;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //if (OVRInput.GetDown(OVRInput.RawButton.Y) || (RHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) == true) )
        //{
        //    gameManager.NextStage();
        //}
    }



    //OntriggerStay関数 すり抜けている時
    public void OnTriggerStay(Collider other)
    {

        //接触したオブジェクトのタグが"Object"のとき
        if (other.CompareTag("Object"))
        //if(other.gameObject.tag == "Object")
        {
            GetComponent<Renderer>().material.color = new Color(0.1f, 0.9f, 0.1f, 0.0f);　//色を青の透明色に変える
            Parent.GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 0.0f, 0.3f); //色を緑の透明に
                                                                                                //}

            //if (RHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) < 1f || LHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) < 1f)
            if (RHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) < 0.4f)
            {
                //当たったオブジェクトを消す
                Destroy(other.gameObject);
                gameManager.NextStage();

            }

        }

    }

    //OnTriggerExit関数　すり抜け終わった時
    public void OnTriggerExit(Collider other)
    {

        //CubeButtonがCubeと離れた場合

        if (other.CompareTag("Object"))
        {
            GetComponent<Renderer>().material.color = new Color(0.4f, 0.0f, 0.0f, 0.0f); //色を元の色に戻す
            Parent.GetComponent<Renderer>().material.color = new Color(0.0f, 0.4f, 0.0f, 0.6f);
        }
    }

}