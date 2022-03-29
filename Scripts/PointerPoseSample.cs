using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerPoseSample : MonoBehaviour
{
    [SerializeField] float _rayDistance = 5.0f;
    OVRHand _hand;
    LineRenderer _lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _hand = GetComponent<OVRHand>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //  レイをLineRendererで描画
        var positions = new Vector3[]{
            _hand.PointerPose.position,
            _hand.PointerPose.position + _hand.PointerPose.forward * _rayDistance
        };

        _lineRenderer.SetPositions(positions);

        //  PointerPoseが有効な時のみLineRendererを表示
        _lineRenderer.enabled = _hand.IsPointerPoseValid;


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
            hits = Physics.RaycastAll(_hand.PointerPose.position, _hand.PointerPose.forward, 1.0f);
            foreach (var hit in hits)
            {
                if (hit.collider.tag == "Cube")
                {
                    hit.collider.transform.parent = _hand.PointerPose;
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
