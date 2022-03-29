using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RayHit : MonoBehaviour
{

    [SerializeField]
    OVRHand _hand;

    LineRenderer lineRenderer;
    Vector3 hitPos;
    Vector3 tmpPos;

    float lazerDistance = 5f;
    float lazerStartPointDistance = 0.03f;
    float lineWidth = 0.01f;

    void Reset()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
    }

    void Start()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
    }


    public void Update()
    {
        OnRay();
    }

    public void OnRay()
    {
        Vector3 direction = _hand.PointerPose.forward * lazerDistance;
        Vector3 rayStartPosition = _hand.PointerPose.forward * lazerStartPointDistance;
        Vector3 pos = _hand.PointerPose.position;
        RaycastHit hit;
        Ray ray = new Ray(pos + rayStartPosition, _hand.PointerPose.forward);

        lineRenderer.SetPosition(0, pos + rayStartPosition);

        if (Physics.Raycast(ray, out hit, lazerDistance))
        {
            hitPos = hit.point;
            lineRenderer.SetPosition(1, hitPos);

            //試用運転
            //if (hit.collider.tag == "Target")
            //{
            //    hit.collider.GetComponent<Renderer>().material.color = new Color(200, 20, 20, 100);
            //}

        }
        else
        {
            lineRenderer.SetPosition(1, pos + direction);

            //GameObject.Find("CubeButton").GetComponent<Renderer>().material.color = new Color(20, 200, 20, 100);
        }

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0.1f);




        //  ピンチされているか判定
        if (_hand.GetFingerPinchStrength(OVRHand.HandFinger.Index) >= 1f)
        {

            //  レイの衝突判定
            //var ray = new Ray(_hand.PointerPose.position, _hand.PointerPose.forward * _rayDistance);
            //if (Physics.Raycast(ray, out var hitinfo, _rayDistance))
            //{
            //    //var hitObject = hitinfo.collider.gameObject;
            //    //var distance = _hand.PointerPose.position - hitObject.transform.position;
            //    //var velocity = distance.normalized;

            //    //hitObject.GetComponent<Rigidbody>().AddForce(velocity * _speed);

            //}
            RaycastHit[] hits;
            hits = Physics.RaycastAll(_hand.PointerPose.position, _hand.PointerPose.forward, 4.0f);
            foreach (var ahit in hits)
            {
                if (ahit.collider.tag == "Cube")
                {
                    ahit.collider.transform.parent = _hand.PointerPose;
                    break;
                }
            }

        }
        //物体を離す処理
        if (_hand.GetFingerPinchStrength(OVRHand.HandFinger.Index) < 1f)
        {
            for (int i = 0; i < _hand.PointerPose.childCount; i++)
            {
                var child = _hand.PointerPose.GetChild(i);
                if (child.tag == "Cube")
                {
                    child.parent = null;
                }
            }
        }


    }
}
