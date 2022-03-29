using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Diagnostics;
using System.Threading;

using UnityEngine.SceneManagement; //現在のsceneの名前を取得する。

public class ObjectLogGesture : MonoBehaviour
{

    [SerializeField]
    OVRHand RHand;

    [SerializeField]
    GameObject Target1;


    bool isFirst = true; // 最初の一回を判定するフラグ
    bool isFirstTransform = true; // 最初の一回を判定するフラグ

    StreamWriter sw;

    Stopwatch time = new Stopwatch();

    string item = "実験条件番号" + "," + "ドラッグ操作時間(ミリ秒)" + "," + "ドラッグ完了時のオブジェクトのx座標" + "," + "ドラッグ完了時のオブジェクトのy座標" + "," + "ドラッグ完了時のオブジェクトのz座標" + "," + "スタート時のオブジェクトのx座標" + "," + "スタート時のオブジェクトのy座標" + "," + "スタート時のオブジェクトのz座標" + "," + "ターゲットのx座標" + "," + "ターゲットのy座標" + "," + "ターゲットのz座標" + "," + "Dx[m]" + "," + "CTD[m]" + "," + "D[m]" + "," + "D_dash[m]" + "," + "ID=log2(D/W+1)" + "," + "W[m]" + "," + "O[m]" + "," + "W/0 (ターゲットとオブジェクトの辺の長さ比)";


    //最初のオブジェクトとターゲットのトランスフォーム取得
    Transform Object;
    Transform Target;

    Vector3 ObjectP;
    float O_X;
    float O_Y;
    float O_Z;

    // ワールド座標を基準に、座標を取得
    Vector3 TargetP;
    float T_X;
    float T_Y;
    float T_Z;

    //文字列に変換
    string strO_X;
    string strO_Y;
    string strO_Z;

    //文字列に変換
    string strT_X;
    string strT_Y;
    string strT_Z;

    //サイズ取得

    Vector3 ObjectSize;
    Vector3 TargetSize;

    float SizePortion;
    string strSizePortion;

    string TS;//ターゲットのサイズ(一辺の長さ)
    string OS;//オブジェクトのサイズ

    float DistanceX; //Dxを求める
    float absDistanceX;

    float DistanceZ; //CTDを求める
    float absDistanceZ;

    float Dist; //Dを求める
    float absDistanceD;


    Double Dx;
    Double CTD;
    Double D;

    Double DT;

    string strDx;
    string strCTD;
    string strD;


    Double D_dash;
    string DS_dash;

    Double ID;            //IDのログ計算
    string strID;



    void Start()
    {

        string fname = "Gesture_drag.csv";
        string path = Path.Combine(Application.persistentDataPath, fname);

        if (File.Exists(path))
        {
            sw = new StreamWriter(path, true);
        }
        else
        {
            sw = new StreamWriter(path, false);
            sw.WriteLine(item);
        }



    }


    // Update is called once per frame
    void Update()
    {
        // 一回だけ呼ばれる
        if (isFirstTransform == true)
        {

            //最初のオブジェクトとターゲットのトランスフォーム取得
            Object = this.transform;
            Target = Target1.transform;

            //座標取得

            // ワールド座標を基準に、座標を取得
            ObjectP = Object.position;
            O_X = ObjectP.x;
            O_Y = ObjectP.y;
            O_Z = ObjectP.z;

            // ワールド座標を基準に、座標を取得
            TargetP = Target.position;
            T_X = TargetP.x;
            T_Y = TargetP.y;
            T_Z = TargetP.z;

            //文字列に変換
            strO_X = O_X.ToString();
            strO_Y = O_Y.ToString();
            strO_Z = O_Z.ToString();

            //文字列に変換
            strT_X = T_X.ToString();
            strT_Y = T_Y.ToString();
            strT_Z = T_Z.ToString();

            //サイズ取得

            ObjectSize = Object.localScale;
            TargetSize = Target.localScale;

            OS = ObjectSize.x.ToString();
            TS = TargetSize.x.ToString();

            SizePortion = TargetSize.x / ObjectSize.x;
            strSizePortion = SizePortion.ToString();


            DistanceX = O_X - T_X; //Dxを求める
            DistanceZ = O_Z - T_Z; //CTDを求める

            if (DistanceX >= 0)
            {
                absDistanceX = DistanceX;
            }
            else
            {
                absDistanceX = -1.0f * DistanceX;
            }

            if (DistanceZ >= 0)
            {
                absDistanceZ = DistanceZ;
            }
            else
            {
                absDistanceZ = -1.0f * DistanceZ;
            }




            Dx = (Double)absDistanceX;
            CTD = (Double)absDistanceZ;

            DT = (Double)TargetSize.x; //ターゲットのサイズW


            //3次元座標での距離取得(引数はVector3)
            Dist = Vector3.Distance(TargetP, ObjectP);

            if (Dist >= 0)
            {
                absDistanceD = Dist;
            }
            else
            {
                absDistanceD = -1.0f * Dist;
            }

            D = (Double)absDistanceD;




            ID = Math.Log((D / DT + 1), 2);            //IDのログ計算

            strID = ID.ToString();


            strDx = absDistanceX.ToString();
            strCTD = absDistanceZ.ToString();
            strD = absDistanceD.ToString();


            isFirstTransform = false;  // 一回はすぎた

            //break;

        }

        if ((isFirst == true) && (RHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) >= 0.9f))
        {

            time.Start();
            isFirst = false;  // 一回はすぎた
                              //break;

        }


        if ((isFirst == false) && (RHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) < 0.9f))
        {

                time.Stop(); //ストップウォッチ停止


                long Mili = time.ElapsedMilliseconds;
                string MiliSt = Mili.ToString();

                Transform Object_T = this.transform;

                // ワールド座標を基準に、座標を取得
                Vector3 Object_P = Object_T.position;
                float x = Object_P.x;
                float y = Object_P.y;
                float z = Object_P.z;

                //文字列に変換
                string strX = x.ToString();

                string strY = y.ToString();
                string strZ = z.ToString();

                //sw = fi.AppendText();

                D_dash = Math.Sqrt(Math.Pow((O_X - x), 2) + Math.Pow((O_Y - y), 2) + Math.Pow((O_Z - z), 2));
                DS_dash = D_dash.ToString();

                string str = SceneManager.GetActiveScene().name + "," + MiliSt + "," + strX + "," + strY + "," + strZ + "," + strO_X + "," + strO_Y + "," + strO_Z + "," + strT_X + "," + strT_Y + "," + strT_Z + "," + strDx + "," + strCTD + "," + strD + "," + DS_dash + "," + strID + "," + TS + "," + OS + "," + strSizePortion;


                sw.WriteLine(str);
                sw.Flush();
                sw.Close();

                time.Reset();

                //if (isFirst == false)
                //{
                //    isFirst = true;  // 一回はすぎた
                //                     //break;
                //}

            
        }
    }
}
