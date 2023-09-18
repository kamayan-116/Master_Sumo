using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // [SerializeField] private GameManager gameManager;  // GameManagerのスクリプト
    [SerializeField] private PlayerManager enemy;  // 相手のスクリプト
    [SerializeField] private FixedJoystick lFJoystick;  // 左足のJoyStick
    [SerializeField] private FixedJoystick rFJoystick;  // 右足のJoyStick
    [SerializeField] private GameObject playerObj;  // プレイヤーオブジェクト
    [SerializeField] private GameObject wholeObj;  //   全身のオブジェクト
    [SerializeField] private GameObject spineObj;  // 上半身のオブジェクト
    [SerializeField] private GameObject lfObj;  // 左足のオブジェクト
    [SerializeField] private GameObject rfObj;  // 右足のオブジェクト
    [SerializeField] private Vector3 lflocalPos;  // 左足のローカル座標
    [SerializeField] private Vector3 rflocalPos;  // 右足のローカル座標
    [SerializeField] private Vector3 lf;  // 左足のワールド座標
    [SerializeField] private Vector2 footInidis;  // 左右の足の初期距離(xが横方向、yが縦方向)
    [SerializeField] private int playerNum;  // プレイヤーナンバー
    private float playerScale;  // プレイヤーオブジェクトの大きさ
    [SerializeField] private float graFBNum = 0f;  // 前後方重心の値（重心耐久の最大値:10）
    [SerializeField] private float graLRNum = 0f;  // 左右方重心の値（重心耐久の最大値:10）
    private float moveFBGraMagNum = 0.04f;  // 重心前後移動の倍率数値
    private float moveLRGraMagNum = 0.034f;  // 重心左右移動の倍率数値
    [SerializeField] private float lFFBNum = 0f;  // 左足前後方の値（前方と後方の最大値:5）
    [SerializeField] private float lFLRNum = 0f;  // 左足左右の値（左と右の最大値:5）
    [SerializeField] private float rFFBNum = 0f;  // 右足前後方の値（前方と後方の最大値:5）
    [SerializeField] private float rFLRNum = 0f;  // 右足左右の値（左と右の最大値:5）
    private float footMax = 5f;  // 足の値の最大値
    private float moveLRDisMagNum;  // 左右移動距離の倍率数値
    private float moveFBDisMagNum;  // 前後移動距離の倍率数値
    private float moveSpeedMagNum = 5f;  // 移動スピードの倍率数値
    private Rigidbody rb;
    private Vector3 center;  // プレイヤーの重心初期座標
    [SerializeField] private Vector3 gravityPlace;  // プレイヤーの重心初期座標
    public Vector3 gravityWorldPos;  // プレイヤーの重心ワールド座標
    private Vector3 disFromTotalGra;  // 総合重心との距離ベクトル
    [SerializeField] private Vector3 spineAngle;  // 上半身の角度
    private float spineFBSlope = 5.5f;  // 上半身の前後の角度の傾き
    private float spineFBIntercept = 10f;  // 上半身の前後の角度の切片
    private float spineLRSlope = -4.5f;  // 上半身の左右の角度の傾き
    private float spineLRIntercept = 0f;  // 上半身の左右の角度の切片
    private float wholeY;  // 全身のワールドY座標
    private Vector3 force;  // 相手の胸元を押す力
    private Vector3 nowPos;  // 自身の座標
    private Vector3 target;  // 相手の座標

    // Start is called before the first frame update
    void Start()
    {
        rb = playerObj.GetComponent<Rigidbody>();
        playerScale = playerObj.transform.lossyScale.x;
        lflocalPos = lfObj.transform.localPosition;
        rflocalPos = rfObj.transform.localPosition;
        lf = lfObj.transform.position;
        footInidis = new Vector2(Mathf.Abs(lflocalPos.x - rflocalPos.x), Mathf.Abs(lflocalPos.z - rflocalPos.z)) * playerScale;
        // 足の長さに応じて移動距離を変更（例:長さ90⇒左右120,前後80：自分）
        moveLRDisMagNum = (((120f / 90f) * lf.y) - footInidis.x) / (footMax * playerScale * 2f);
        moveFBDisMagNum = (((80f / 90f) * lf.y) - footInidis.y) / (footMax * playerScale * 2f);
        wholeY = wholeObj.transform.position.y;
        center = new Vector3(-0.15f, wholeY+0.2f, 0.15f);
    }

    // Update is called once per frame
    void Update()
    {
        switch(playerNum)
        {
            case 1:
                // if(Input.GetAxis("Rotation1") < 0f && 
                //     (Input.GetAxis("LeftHorizontal1") != 0f || Input.GetAxis("LeftVertical1") != 0f))
                // {
                //     SetLeftFootNum(Input.GetAxis("LeftHorizontal1"), Input.GetAxis("LeftVertical1"));
                // }

                // if(Input.GetAxis("Rotation1") > 0f && 
                //     (Input.GetAxis("LeftHorizontal1") != 0f || Input.GetAxis("LeftVertical1") != 0f))
                // {
                //     SetRightFootNum(Input.GetAxis("LeftHorizontal1"), Input.GetAxis("LeftVertical1"));
                // }

                // if(Input.GetAxis("Rotation1") < 0f && Input.GetAxis("RotationButton1") != 0f)
                // {
                //     SetWholeAngle(lFObj, -Input.GetAxis("RotationButton1"));
                // }

                // if(Input.GetAxis("Rotation1") > 0f && Input.GetAxis("RotationButton1") != 0f)
                // {
                //     SetWholeAngle(rFObj, Input.GetAxis("RotationButton1"));
                // }

                if((Input.GetAxis("LeftHorizontal1") != 0f || Input.GetAxis("LeftVertical1") != 0f) &&
                    (Input.GetAxis("RightHorizontal1") == 0f && Input.GetAxis("RightVertical1") == 0f) &&
                    Input.GetAxis("Rotation1") == 0f
                    )
                {
                    SetLeftFootNum(Input.GetAxis("LeftHorizontal1"), Input.GetAxis("LeftVertical1"));
                }

                if((Input.GetAxis("LeftHorizontal1") == 0f && Input.GetAxis("LeftVertical1") == 0f) &&
                    (Input.GetAxis("RightHorizontal1") != 0f || Input.GetAxis("RightVertical1") != 0f) && 
                    Input.GetAxis("Rotation1") == 0f
                    )
                {
                    SetRightFootNum(Input.GetAxis("RightHorizontal1"), Input.GetAxis("RightVertical1"));
                }

                if(Input.GetAxis("Rotation1") < 0f)
                {
                    SetWholeAngle(lfObj, -Input.GetAxis("RightVertical1"));
                }

                if(Input.GetAxis("Rotation1") > 0f)
                {
                    SetWholeAngle(rfObj, Input.GetAxis("LeftVertical1"));
                }

                break;
            case 2:
                if((Input.GetAxis("LeftHorizontal2") != 0f || Input.GetAxis("LeftVertical2") != 0f) &&
                    (Input.GetAxis("RightHorizontal2") == 0f && Input.GetAxis("RightVertical2") == 0f) && 
                    Input.GetAxis("Rotation2") == 0f
                    )
                {
                    SetLeftFootNum(Input.GetAxis("LeftHorizontal2"), Input.GetAxis("LeftVertical2"));
                }

                if((Input.GetAxis("LeftHorizontal2") == 0f && Input.GetAxis("LeftVertical2") == 0f) &&
                    (Input.GetAxis("RightHorizontal2") != 0f || Input.GetAxis("RightVertical2") != 0f) && 
                    Input.GetAxis("Rotation2") == 0f
                    )
                {
                    SetRightFootNum(Input.GetAxis("RightHorizontal2"), Input.GetAxis("RightVertical2"));
                }

                if(Input.GetAxis("Rotation2") < 0f)
                {
                    SetWholeAngle(lfObj, -Input.GetAxis("RightVertical2"));
                }

                if(Input.GetAxis("Rotation2") > 0f)
                {
                    SetWholeAngle(rfObj, Input.GetAxis("LeftVertical2"));
                }

                break;
        }

        // nowPos = new Vector3(this.rb.position.x, 0, this.rb.position.z);
        // nowPos += new Vector3
        //     (
        //         (enemy.gameObject.transform.position.x - this.rb.position.x) * Time.deltaTime,
        //         0,
        //         (enemy.gameObject.transform.position.z - this.rb.position.z) * Time.deltaTime
        //     );
        // rb.position = nowPos;

        if(lFJoystick.Horizontal != 0f || lFJoystick.Vertical != 0f)
        {
            SetLeftFootNum(lFJoystick.Horizontal, lFJoystick.Vertical);
            rFJoystick.gameObject.SetActive(false);
        }
        else
        {
            rFJoystick.gameObject.SetActive(true);
        }

        if(rFJoystick.Horizontal != 0f || rFJoystick.Vertical != 0f)
        {
            SetRightFootNum(rFJoystick.Horizontal, rFJoystick.Vertical);
            lFJoystick.gameObject.SetActive(false);
        }
        else
        {
            lFJoystick.gameObject.SetActive(true);
        }

        SetGravityPlace();
        SetSpineAngle();
    }

    // 左足JoyStickによる入力値の変化を行う関数
    public void SetLeftFootNum(float rightPosi, float frontPosi)
    {
        if((rightPosi < 0f && -footMax < lFLRNum) || (rightPosi > 0f && lFLRNum < footMax))
        {
            rFLRNum -= Time.deltaTime * rightPosi * moveSpeedMagNum;
            lFLRNum += Time.deltaTime * rightPosi * moveSpeedMagNum;
            enemy.SetGravityNum(-rightPosi, 0);
            if((graLRNum < 0 && rightPosi < 0) || (graLRNum > 0 && rightPosi > 0))
            {
                SetGravityNum(-rightPosi, 0);
            }
            this.transform.Translate(Time.deltaTime * rightPosi * moveSpeedMagNum * moveLRDisMagNum * playerScale, 0, 0);
        }

        if((frontPosi < 0f && -footMax < lFFBNum) || (frontPosi > 0f && lFFBNum < footMax))
        {
            rFFBNum -= Time.deltaTime * frontPosi * moveSpeedMagNum;
            lFFBNum += Time.deltaTime * frontPosi * moveSpeedMagNum;
            enemy.SetGravityNum(0, -frontPosi);
            if((graFBNum < 0 && frontPosi < 0) || (graFBNum > 0 && frontPosi > 0))
            {
                SetGravityNum(0, -frontPosi);
            }
            this.transform.Translate(0, 0, Time.deltaTime * frontPosi * moveSpeedMagNum * moveFBDisMagNum * playerScale);
        }

        SetFootPlace();
    }

    // 右足JoyStickによる入力値の変化を行う関数
    public void SetRightFootNum(float rightPosi, float frontPosi)
    {
        if((rightPosi < 0f && -footMax < rFLRNum) || (rightPosi > 0f && rFLRNum < footMax))
        {
            rFLRNum += Time.deltaTime * rightPosi * moveSpeedMagNum;
            lFLRNum -= Time.deltaTime * rightPosi * moveSpeedMagNum;
            enemy.SetGravityNum(-rightPosi, 0);
            if((graLRNum < 0 && rightPosi < 0) || (graLRNum > 0 && rightPosi > 0))
            {
                SetGravityNum(-rightPosi, 0);
            }
            this.transform.Translate(Time.deltaTime * rightPosi * moveSpeedMagNum * moveLRDisMagNum * playerScale, 0, 0);
        }

        if((frontPosi < 0f && -footMax < rFFBNum) || (frontPosi > 0f && rFFBNum < footMax))
        {
            rFFBNum += Time.deltaTime * frontPosi * moveSpeedMagNum;
            lFFBNum -= Time.deltaTime * frontPosi * moveSpeedMagNum;
            enemy.SetGravityNum(0, -frontPosi);
            if((graFBNum < 0 && frontPosi < 0) || (graFBNum > 0 && frontPosi > 0))
            {
                SetGravityNum(0, -frontPosi);
            }
            this.transform.Translate(0, 0, Time.deltaTime * frontPosi * moveSpeedMagNum * moveFBDisMagNum * playerScale);
        }

        SetFootPlace();    
    }

    // 重心値の変化を行う関数
    public void SetGravityNum(float rightPosi, float frontPosi)
    {
        graFBNum += Time.deltaTime * frontPosi * moveSpeedMagNum;
        graLRNum += Time.deltaTime * rightPosi * moveSpeedMagNum;
    }

    // 足の位置の変更を行う関数
    private void SetFootPlace()
    {
        rfObj.transform.localPosition = 
            new Vector3
            (
                rFLRNum * moveLRDisMagNum + rflocalPos.x,
                rfObj.transform.localPosition.y,
                rFFBNum * moveFBDisMagNum + rflocalPos.z
            );

        lfObj.transform.localPosition = 
            new Vector3
            (
                lFLRNum * moveLRDisMagNum + lflocalPos.x,
                lfObj.transform.localPosition.y,
                lFFBNum * moveFBDisMagNum + lflocalPos.z
            );
    }

    // 重心の位置の変更を行う関数
    private void SetGravityPlace()
    {
        gravityPlace = 
            new Vector3
            (
                center.x + graLRNum * moveLRGraMagNum,
                center.y,
                center.z + graFBNum * moveFBGraMagNum
            );

        rb.centerOfMass = gravityPlace;
        gravityWorldPos = playerObj.transform.TransformPoint(gravityPlace);
        Debug.DrawLine (playerObj.transform.position , playerObj.transform.position + playerObj.transform.rotation * gravityPlace);
    }

    // 中心重心座標と自身の距離の計算を行う関数
    // private void SetDisGravity()
    // {
    //     disFromTotalGra = 
    //         new Vector3
    //         (
    //             this.transform.position.x - gameManager.totalCenter.x,
    //             this.transform.position.y - gameManager.totalCenter.y,
    //             this.transform.position.z - gameManager.totalCenter.z
    //         );
    // }

    // 上半身の角度の変更を行う関数
    private void SetSpineAngle()
    {
        spineAngle = spineObj.transform.localEulerAngles;
        spineAngle.x = spineFBSlope * graFBNum + spineFBIntercept;
        spineAngle.y = 0;
        spineAngle.z = spineLRSlope * graLRNum + spineLRIntercept;
        spineObj.transform.localEulerAngles = spineAngle;
    }

    // 全身の角度の回転を行う関数
    public void SetWholeAngle(GameObject target, float rotateSpeed)
    {
        this.transform.RotateAround
        (
            target.transform.position,
            Vector3.up,
            Time.deltaTime * rotateSpeed * moveSpeedMagNum * moveSpeedMagNum
        );
    }

    // 重心値の表示を行う関数
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere (playerObj.transform.position + playerObj.transform.rotation * gravityPlace, 0.1f);
    }
}
