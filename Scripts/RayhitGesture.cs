using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RayhitGesture : MonoBehaviour
{

    [SerializeField]
    OVRCameraRig Camera;

    [SerializeField]
    OVRHand RHand;
    [SerializeField]
    OVRHand LHand;



    [SerializeField]
    OVRHand NowHand;

    [SerializeField]
    GameObject Object;

    [SerializeField]
    GameObject Target;

    Color ObjectColor;

    LineRenderer lineRenderer;

    Vector3 hitPosR;
    Vector3 tmpPosR;

    //Vector3 hitPosL;
    //Vector3 tmpPosL;

    Vector3 hitPos;
    Vector3 tmpPos;

    float lazerDistanceR = 5f;
    float lazerStartPointDistanceR = 0.1f;
    float lineWidthR = 0.01f;

    //float lazerDistanceL = 5f;
    //float lazerStartPointDistanceL = 0.1f;
    //float lineWidthL = 0.01f;

    float lazerDistance = 5f;
    float lazerStartPointDistance = 0.1f;
    float lineWidth = 0.01f;


    //サウンド
    public AudioClip SoundGet;
    public AudioClip SoundRelease;

    bool isFitst = true;


    void Reset()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;

    }

    void Start()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;


        ObjectColor = Object.GetComponent<Renderer>().material.color;//最初のオブジェクトの色を取得



        ////オブジェクトとターゲットをHMD側にむける
        //Vector3 aim_T = Camera.transform.position - Target.transform.position;
        //Quaternion look_T = Quaternion.LookRotation(aim_T);//Vector3.up
        //Target.transform.rotation = look_T;

        //Vector3 aim_O = Camera.transform.position - Object.transform.position;
        //Quaternion look_O = Quaternion.LookRotation(aim_O);//Vector3.up
        //Object.transform.rotation = look_O;

        Object.transform.rotation = Quaternion.Euler(0, 0, 0);
        Target.transform.rotation = Quaternion.Euler(0, 0, 0);



    }


    void Update()
    {
        OnRay();

        //Vector3 _parent = Object.transform.parent.transform.localRotation.eulerAngles;

        //Object.transform.rotation = Quaternion.Euler(0, 0, 0);
        //Vector3 aim = Camera.transform.position - Object.transform.position;
        //Quaternion look = Quaternion.LookRotation(aim, Vector3.up);//Vector3.up

        //Object.transform.rotation = look;

        //Object.transform.localRotation = Quaternion.Euler(0, 0, 0);
        //Object.transform.rotation = 0.0f;

        Object.transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    void OnRay()
    {

        //汎用
        Vector3 direction = NowHand.PointerPose.forward * lazerDistance;
        Vector3 rayStartPosition = NowHand.PointerPose.forward * lazerStartPointDistance;
        Vector3 pos = NowHand.PointerPose.position;
        RaycastHit hit;
        Ray ray = new Ray(pos + rayStartPosition, NowHand.PointerPose.forward);

        lineRenderer.SetPosition(0, pos + rayStartPosition);



        if (Physics.Raycast(ray, out hit, lazerDistance))
        {
            hitPos = hit.point;
            lineRenderer.SetPosition(1, hitPos);
        }
        else
        {
            lineRenderer.SetPosition(1, pos + direction);

        }

        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0.1f);




        //オブジェクトの色変更

        //オブジェクト判定に使う用途
        //右
        Vector3 directionR = RHand.PointerPose.forward * lazerDistanceR;
        Vector3 rayStartPositionR = RHand.PointerPose.forward * lazerStartPointDistanceR;
        Vector3 posR = RHand.PointerPose.position;
        RaycastHit hitR;
        Ray rayR = new Ray(posR + rayStartPositionR, RHand.PointerPose.forward);


        ////左
        //Vector3 directionL = LHand.PointerPose.forward * lazerDistanceL;
        //Vector3 rayStartPositionL = LHand.PointerPose.forward * lazerStartPointDistanceL;
        //Vector3 posL = LHand.PointerPose.position;
        //RaycastHit hitL;
        //Ray rayL = new Ray(posL + rayStartPositionL, LHand.PointerPose.forward);


        if (Physics.Raycast(rayR, out hitR, lazerDistanceR))
        {
            if ((hit.collider.tag == "Object"))
            {
                Object.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 1.0f, 1.0f); //Rayがヒットした時にオブジェクトの色を変える   
            }
        }
        else
        {
            Object.GetComponent<Renderer>().material.color = ObjectColor; //Rayのヒットが外れたらオブジェクトの色を戻す
        }




        //Nowコントローラー


        //レイに当たった物体をつかむ処理　
        if (RHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) >= 0.9f)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(RHand.PointerPose.position, RHand.PointerPose.forward, 4.0f);
            foreach (var ahit in hits)
            {
                if (ahit.collider.tag == "Object")
                {
                    ahit.collider.transform.parent = RHand.PointerPose;

                    Object.GetComponent<Renderer>().material.color = new Color(0.1f, 0.6f, 0.75f, 1.0f);//掴んだ時にオブジェクトの色を変える 

                }

            }
        }

        //レイに当たった物体をつかむ処理　//音を鳴らすだけ
        if (RHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) >= 0.8f)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(RHand.PointerPose.position, RHand.PointerPose.forward, 4.0f);
            foreach (var ahit in hits)
            {
                if (ahit.collider.tag == "Object")
                {
                    if (isFitst == true)
                    {
                        AudioSource.PlayClipAtPoint(SoundGet, transform.position); //サウンド
                        isFitst = false;
                    }
                }

            }
        }


        //物体を離す処理
        if (RHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) < 0.9f)
        {
            for (int i = 0; i < RHand.PointerPose.childCount; i++)
            {
                var child = RHand.PointerPose.GetChild(i);
                if (child.tag == "Object")
                {
                    child.parent = null;

                }
            }
        }

        //物体を離す処理  //音を鳴らすだけ
        if (RHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) < 0.8f)
        {
            if (isFitst == false)
            {
                AudioSource.PlayClipAtPoint(SoundRelease, transform.position); //サウンド
                isFitst = true;
            }

        }
    }
}