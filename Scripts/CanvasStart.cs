using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class CanvasStart : MonoBehaviour
{

    //[SerializeField]
    //OVRHand LHand;

    //GameManage gameManager;

    //bool isfirst = true;

    public AudioClip metal_sliding_door_close_01a;
    public AudioClip AISound;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManage>(); //シーンの切り替え処理で使用

        AudioSource.PlayClipAtPoint(AISound, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //if (LHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) >= 0.8f)
        //{
        //    gameManager.NextStage(); //次のシーン(条件)に遷移する

        //    if (isfirst == true)
        //    {
        //        AudioSource.PlayClipAtPoint(metal_sliding_door_close_01a, transform.position);
        //        isfirst = false;
        //    }
        //}
    }
}
