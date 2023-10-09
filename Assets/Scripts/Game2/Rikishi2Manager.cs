using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Rikishi2Manager : MonoBehaviour
{
    #region 変数宣言
    private enum PlayStyle
    {
        Yothu = 1,
        Mawashi = 2,
        Oshi = 3,
        Hataki = 4
    };
    private PlayStyle playStyle;  // 現在の攻撃状態

    [Header("各オブジェクト")]
    [SerializeField] private Rikishi2UIManager rikishiUI;  // プレイヤーのUIを表示するプログラム
    [SerializeField] private Rikishi2Manager enemy;  // 相手のスクリプト
    [SerializeField] private GameObject dohyoObj;  // 土俵オブジェクト
    [SerializeField] private Camera playerCamera;  // プレイヤーカメラオブジェクト
    [SerializeField] private GameObject playerObj;  // プレイヤーオブジェクト
    [SerializeField] private GameObject wholeObj;  //   全身のオブジェクト
    [SerializeField] private GameObject spineObj;  // 上半身のオブジェクト
    [SerializeField] private GameObject lsObj;  // 左肩のオブジェクト
    [SerializeField] private GameObject rsObj;  // 右肩のオブジェクト
    [SerializeField] private GameObject lfObj;  // 左足のオブジェクト
    [SerializeField] private GameObject rfObj;  // 右足のオブジェクト
    [SerializeField] private GameObject lhObj;  // 左手のオブジェクト
    [SerializeField] private GameObject rhObj;  // 右手のオブジェクト
    [SerializeField] private GameObject lThumbObj;  // 左親指のオブジェクト
    [SerializeField] private GameObject rThumbObj;  // 右親指のオブジェクト
    [SerializeField] private GameObject lIndexObj;  // 左人差し指のオブジェクト
    [SerializeField] private GameObject rIndexObj;  // 右人差し指のオブジェクト
    [SerializeField] private GameObject lMiddleObj;  // 左中指のオブジェクト
    [SerializeField] private GameObject rMiddleObj;  // 右中指のオブジェクト
    [SerializeField] private GameObject lRingObj;  // 左薬指のオブジェクト
    [SerializeField] private GameObject rRingObj;  // 右薬指のオブジェクト
    [SerializeField] private GameObject lLittleObj;  // 左小指のオブジェクト
    [SerializeField] private GameObject rLittleObj;  // 右小指のオブジェクト

    [SerializeField] private GameObject viewObj;  // 視線ベクトルオブジェクト
    [SerializeField] private GameObject lOFObj;  // 左足外前座標オブジェクト
    [SerializeField] private GameObject lOBObj;  // 左足外後座標オブジェクト
    [SerializeField] private GameObject lIFObj;  // 左足内前座標オブジェクト
    [SerializeField] private GameObject lIBObj;  // 左足内後座標オブジェクト
    [SerializeField] private GameObject rOFObj;  // 右足外前座標オブジェクト
    [SerializeField] private GameObject rOBObj;  // 右足外後座標オブジェクト
    [SerializeField] private GameObject rIFObj;  // 右足内前座標オブジェクト
    [SerializeField] private GameObject rIBObj;  // 右足内後座標オブジェクト
    [Header("基本情報")]
    public int playerNum;  // プレイヤーナンバー
    [SerializeField] private float lossyScaleNum;  // プレイヤーオブジェクトの全体の大きさ
    [SerializeField] private float localScaleNum;  // プレイヤーオブジェクトのローカルの大きさ
    [SerializeField] private float scaleYNum;  // プレイヤーオブジェクトの身長の大きさ
    [SerializeField] private Vector3 scaleVector;  // プレイヤーオブジェクトの大きさVector
    [SerializeField] private float footLengNum;  // 足の長さの値
    private Rigidbody rb;
    [Header("足や座標の情報")]
    [SerializeField] private Vector3 lf;  // 左足のワールド座標
    [SerializeField] private Vector3 rf;  // 右足のワールド座標
    [SerializeField] private Vector2 footInidis;  // 左右の足の初期距離(xが横方向、yが縦方向)
    [SerializeField] private Vector2 footDis;  // 左右の足の距離(xが横方向、yが縦方向)
    [SerializeField] private float enemyDis;  // 敵プレイヤーとの距離
    private float hatakiMax = 2.5f;  // はたきができる最大距離
    private float attackMax = 1.6f;  // はたき以外の攻撃ができる最大距離
    [SerializeField] private float dohyoLDis;  // 土俵の中心とプレイヤーの左足との距離
    [SerializeField] private float dohyoRDis;  // 土俵の中心とプレイヤーの左足との距離
    private float dohyoRadius = 4.85f;  // 土俵の半径
    [Header("立会い")]
    [SerializeField] private float startPushTime = 0f;  // 立会いのボタンを押す時間
    [SerializeField] private int penaltyNum = 0;  // 立会いの反則回数
    [SerializeField] private float pushTimeLag = 0f;  // 立会いのボタンを押した時間差
    private float pushMaxLag = 6f;  // 立会いの最大時間差
    [SerializeField] private Vector3 startPos;  // プレイヤーの立会いによる開始座標
    private float startPosXSlope = 0.5f;  // 立会い開始X座標の傾き
    private float startPosXIntercept = 0.75f;  // 立会い開始X座標の切片
    private float tachiaiSpeedMag = 2.5f;  // 立会いのスピード倍率
    [Header("抵抗")]
    [SerializeField] private float dragNum = 0f;  // 倒れる際の抵抗値
    [SerializeField] private float maxAngle;  // 倒れている時の最大角度
    private float angleMagNum = 20f;  // 角度による抵抗値減速係数
    [SerializeField] private float minusPerSecond;  // 抵抗値の減る秒速値
    [Header("体重")]
    [SerializeField] private float weightNum;  // 体重の数値
    [SerializeField] private float weightMin = 80f;  // 体重の最小値
    [SerializeField] private float weightMax = 220f;  // 体重の最大値
    [SerializeField] private float powerMagNum;  // 体重パワーの倍率
    [SerializeField] private float speedMagNum;  // スピード倍率
    [Header("足入力値")]
    [SerializeField] private float lFFBNum = 0f;  // 左足前後方の値（前方と後方の最大値:5）
    [SerializeField] private float lFLRNum = 0f;  // 左足左右の値（左と右の最大値:5）
    [SerializeField] private float rFFBNum = 0f;  // 右足前後方の値（前方と後方の最大値:5）
    [SerializeField] private float rFLRNum = 0f;  // 右足左右の値（左と右の最大値:5）
    private float footMax = 5f;  // 足の値の最大値
    private float clossMax = 2f;  // クロスの時の最大値
    [Header("移動倍率数値")]
    [SerializeField] private float moveLRDisMagNum;  // 左右移動距離の倍率数値
    [SerializeField] private float moveFBDisMagNum;  // 前後移動距離の倍率数値
    [SerializeField] private float moveFBGraMagNum;  // 重心前後移動の倍率数値
    [SerializeField] private float moveLRGraMagNum;  // 重心左右移動の倍率数値
    private float graFBMagSlope;  // 重心前後移動の倍率の傾き
    private float graFBMagIntercept;  // 重心前後移動の倍率の切片
    private float graLRMagSlope;  // 重心左右移動の倍率の傾き
    private float graLRMagIntercept = 0f;  // 重心左右移動の倍率の切片
    private float moveSpeedMagNum = 5f;  // 移動スピードの倍率数値
    [Header("重心")]
    [SerializeField] private float graFBNum = 0f;  // 前後方重心の値（重心耐久の最大値:15）
    [SerializeField] private float graLRNum = 0f;  // 左右方重心の値（重心耐久の最大値:15）
    public float graMax = 15f;  // 重心耐久の最大値
    private Vector3 center;  // プレイヤーの重心初期座標
    [SerializeField] private Vector3 gravityPlace;  // プレイヤーの重心初期座標
    public Vector3 gravityWorldPos;  // プレイヤーの重心ワールド座標
    [SerializeField] private float attackLLR = 0f;  // 左スティックからの相手への左右の重心移動値
    [SerializeField] private float attackLFB = 0f;  // 左スティックからの相手への前後の重心移動値
    [SerializeField] private float attackMoveLLR = 0f;  // 左足移動時の相手への左右の重心移動値
    [SerializeField] private float attackMoveLFB = 0f;  // 左足移動時の相手への前後の重心移動値
    [SerializeField] private float attackMoveRLR = 0f;  // 右足移動時の相手への左右の重心移動値
    [SerializeField] private float attackMoveRFB = 0f;  // 右足移動時の相手への前後の重心移動値
    [SerializeField] private float leftLR = 0f;  // 左スティックからの自身の左右の重心移動値
    [SerializeField] private float leftFB = 0f;  // 左スティックからの自身の前後の重心移動値
    [SerializeField] private float rightLR = 0f;  // 右スティックからの自身の左右の重心移動値
    [SerializeField] private float rightFB = 0f;  // 右スティックからの自身の前後の重心移動値
    [SerializeField] private float moveLFLR = 0f;  // 左足移動時の自身の左右の重心移動値
    [SerializeField] private float moveLFFB = 0f;  // 左足移動時の自身の前後の重心移動値
    [SerializeField] private float moveRFLR = 0f;  // 右足移動時の自身の左右の重心移動値
    [SerializeField] private float moveRFFB = 0f;  // 右足移動時の自身の前後の重心移動値
    [SerializeField] private float right1 = 0f;  // プレイヤー１からの左右の重心移動値の合計
    [SerializeField] private float front1 = 0f;  // プレイヤー１からの前後の重心移動値の合計
    [SerializeField] private float right2 = 0f;  // プレイヤー２からの左右の重心移動値の合計
    [SerializeField] private float front2 = 0f;  // プレイヤー２からの前後の重心移動値の合計
    private float  inputMin = 0.1f;  // 入力値の最小値
    [SerializeField] private float inputLLR = 0f;  // 左スティックからの左右の重心移動入力値
    [SerializeField] private float inputLFB = 0f;  // 左スティックからの前後の重心移動入力値
    [SerializeField] private float inputMoveLLR = 0f;  // 左足移動時からの左右の重心移動入力値
    [SerializeField] private float inputMoveLFB = 0f;  // 左足移動時からの前後の重心移動入力値
    [SerializeField] private float inputMoveRLR = 0f;  // 右足移動時からの左右の重心移動入力値
    [SerializeField] private float inputMoveRFB = 0f;  // 右足移動時からの前後の重心移動入力値
    [Header("角度計算")]
    [SerializeField] private Vector3 spineAngle;  // 上半身の角度
    private float spineFBSlope = 4.5f;  // 上半身の前後の角度の傾き
    private float spineFBIntercept = 10f;  // 上半身の前後の角度の切片
    private float spineLRSlope = -4.5f;  // 上半身の左右の角度の傾き
    private float spineLRIntercept = 0f;  // 上半身の左右の角度の切片
    private float wholeY;  // 全身のワールドY座標
    [SerializeField] private float angleY;  // 全身のY方向角度
    [SerializeField] private float enemyAngleY;  // 相手の全身のY方向角度
    [SerializeField] private Vector3 viewPos;  // 視線の空オブジェクトの座標
    [SerializeField] private Vector3 viewDir;  // 視線方向のベクトル
    [SerializeField] private float angularDif = 0;  // 相手の方向と自身の向きの角度差
    [SerializeField] private float angDifAbs = 0;  // 相手方向と自身の向きの角度差の絶対値

    [Header("bool判定")]
    [SerializeField] private bool gameStart = false;  // 操作方法の決定ボタンを押したか否か
    [SerializeField] private bool playerModeDecide = false;  // プレイヤー人数決定ボタンを押したか否か
    [SerializeField] private bool levelModeDecide = false;  // レベルモード決定ボタンを押したか否か
    [SerializeField] private bool weightStick = false;  // 体重入力したか否か
    [SerializeField] private bool weightInput = false;  // 体重決定したか否か
    [SerializeField] private bool isStartPush = false;  // 立会いのスタートを押したか否か
    [SerializeField] private bool isTachiaiMove = false;  // 立会いの開始位置への移動可能か否か
    [SerializeField] private bool isTachiaiEnd = false;  // 立会いが終わったか否か
    [SerializeField] private bool isLRPush = false;  // 左スティックで相手の重心を左右に操作しているか
    [SerializeField] private bool isFBPush = false;  // 左スティックで相手の重心を前後に操作しているか
    [SerializeField] private bool lFOpeInput = false;  // 左足の操作をしているか否か
    [SerializeField] private bool rFOpeInput = false;  // 右足の操作をしているか否か
    [SerializeField] private bool  isCollision = false;  // 相手と当たっているか否か
    [SerializeField] private bool  isInColl = false;  // 足の内側が当たっているか否か
    [SerializeField] private bool  isOutColl = false;  // 足の外側が当たっているか否か
    [SerializeField] private bool isAttack = false;  // はたき以外の攻撃ができるか否か
    [SerializeField] private bool isHataki = false;  // はたきができるか否か
    [SerializeField] private bool  isEnd = false;  // 勝敗決着しているか否か
    [SerializeField] private bool  isResult;  // 勝敗結果の表示（true:勝ち,false:負け）
    [SerializeField] private bool  isFallDown = false;  // 土俵に倒れたか否か
    [SerializeField] private bool  isOutDohyo = false;  // 土俵から出たか否か
    [SerializeField] bool isReplay = false;  // Replayボタンを押せるか否か
    [Header("自動移動計算")]
    [SerializeField] private Vector3 target;  // 相手の胸元方向へのベクトル
    [SerializeField] private Vector2 moveDir;  // 相手の胸元方向への単位ベクトル
    private float moveDirSpeed = 0.375f;  // 相手の胸元方向への単位ベクトルの倍率
    [SerializeField] private Vector3 nowPos;  // 自身の座標
    [SerializeField] private Vector3 enemyPos;  // 相手の座標
    [Header("初期情報")]
    [SerializeField] private Vector3 thisInitialPos;  // プレイヤー全体の初期座標
    [SerializeField] private Quaternion thisInitialRot;  // プレイヤー全体の初期角度
    [SerializeField] private Vector3 playerInitialPos;  // プレイヤーオブジェクトの初期座標
    [SerializeField] private Quaternion playerInitialRot;  // プレイヤーオブジェクトの初期角度
    [SerializeField] private Quaternion lsInitialRot;  // 左肩オブジェクトの初期角度
    [SerializeField] private Quaternion rsInitialRot;  // 右肩オブジェクトの初期角度
    [SerializeField] private Vector3 lfInitialPos;  // 左足の初期ローカル座標
    [SerializeField] private Vector3 rfInitialPos;  // 右足の初期ローカル座標
    [SerializeField] private Quaternion lhInitialRot;  // 左手オブジェクトの初期角度
    [SerializeField] private Quaternion rhInitialRot;  // 右手オブジェクトの初期角度
    [SerializeField] private Quaternion lThumbInitialRot;  // 左親指オブジェクトの初期角度
    [SerializeField] private Quaternion rThumbInitialRot;  // 右親指オブジェクトの初期角度
    [SerializeField] private Quaternion lIndexInitialRot;  // 左人差し指オブジェクトの初期角度
    [SerializeField] private Quaternion rIndexInitialRot;  // 右人差し指オブジェクトの初期角度
    [SerializeField] private Quaternion lMiddleInitialRot;  // 左中指オブジェクトの初期角度
    [SerializeField] private Quaternion rMiddleInitialRot;  // 右中指オブジェクトの初期角度
    [SerializeField] private Quaternion lRingInitialRot;  // 左薬指オブジェクトの初期角度
    [SerializeField] private Quaternion rRingInitialRot;  // 右薬指オブジェクトの初期角度
    [SerializeField] private Quaternion lLittleInitialRot;  // 左小指オブジェクトの初期角度
    [SerializeField] private Quaternion rLittleInitialRot;  // 右小指オブジェクトの初期角度
    [SerializeField] private Vector3 playerInitialScale;  // プレイヤーオブジェクトの初期スケール
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rikishiUI.SetWeightMaxMin(weightMax, weightMin);
        rikishiUI.SetGraUIMoveMagNum(graMax);
        rb = playerObj.GetComponent<Rigidbody>();
        SetInitialNum();
        SetPlayStyle(PlayStyle.Yothu);
    }

    // Update is called once per frame
    void Update()
    {
        switch(Game2Manager.Instance.gameState)
        {
            case Game2Manager.GameState.BeforePlay:
                #region 操作方法
                if(!gameStart)
                {
                    if(Input.GetButtonDown("Decide1"))
                    {
                        gameStart = true;
                        Game2Manager.Instance.PushGameStart();
                    }
                }
                #endregion
                #region モード選択
                else
                {
                    if(!levelModeDecide)
                    {
                        if(!playerModeDecide)
                        {
                            if(Input.GetAxisRaw("ModeChange") < 0)
                            {
                                Game2Manager.Instance.SelectOnePlayer();
                            }
                            if(Input.GetAxisRaw("ModeChange") > 0)
                            {
                                Game2Manager.Instance.SelectTwoPlayer();
                            }
                            if(Input.GetButtonDown("Decide1"))
                            {
                                playerModeDecide = true;
                                Game2Manager.Instance.DecidePlayerDown();
                            }
                        }
                        else
                        {
                            if(Input.GetAxisRaw("ModeChange") < 0)
                            {
                                Game2Manager.Instance.SelectEasyMode();
                            }
                            if(Input.GetAxisRaw("ModeChange") > 0)
                            {
                                Game2Manager.Instance.SelectNormalMode();
                            }
                            if(Input.GetButtonDown("Decide1"))
                            {
                                levelModeDecide = true;
                                Game2Manager.Instance.DecideModeDown();
                                SetCameraPlace();
                                Game2Manager.Instance.SetMainCamera();
                                rikishiUI.SetUIPlace(playerNum);
                            }
                        }
                    }
                #endregion
                #region 体重入力
                    else
                    {
                        if(!weightInput)
                        {
                            rikishiUI.SetWeightText(weightNum);

                            switch(playerNum)
                            {
                                case 1:
                                    if(Input.GetAxisRaw("WeightBigChange1") != 0 && !weightStick)
                                    {
                                        weightStick = true;
                                        rikishiUI.SetWeightSliderNum(10 * Input.GetAxisRaw("WeightBigChange1"));
                                    }
                                    if(Input.GetAxisRaw("WeightSmallChange1") != 0 && !weightStick)
                                    {
                                        weightStick = true;
                                        rikishiUI.SetWeightSliderNum(1 * Input.GetAxisRaw("WeightSmallChange1"));
                                    }
                                    if(Input.GetAxisRaw("WeightBigChange1") == 0 && Input.GetAxisRaw("WeightSmallChange1") == 0)
                                    {
                                        weightStick = false;
                                    }
                                    if(Input.GetButtonDown("Decide1"))
                                    {
                                        rikishiUI.SetWeightInput();
                                    }
                                    break;
                                case 2:
                                    if(Input.GetAxisRaw("WeightBigChange2") != 0 && !weightStick)
                                    {
                                        weightStick = true;
                                        rikishiUI.SetWeightSliderNum(10 * Input.GetAxisRaw("WeightBigChange2"));
                                    }
                                    if(Input.GetAxisRaw("WeightSmallChange2") != 0 && !weightStick)
                                    {
                                        weightStick = true;
                                        rikishiUI.SetWeightSliderNum(1 * Input.GetAxisRaw("WeightSmallChange2"));
                                    }
                                    if(Input.GetAxisRaw("WeightBigChange2") == 0 && Input.GetAxisRaw("WeightSmallChange2") == 0)
                                    {
                                        weightStick = false;
                                    }
                                    if(Input.GetButtonDown("Decide2"))
                                    {
                                        rikishiUI.SetWeightInput();
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            switch(playerNum)
                            {
                                case 1:
                                    if(Input.GetButtonDown("Decide1"))
                                    {
                                        SetPenalty();
                                        rikishiUI.SetTachiaiBActive(true);
                                    }
                                    break;
                                case 2:
                                    if(Input.GetButtonDown("Decide2"))
                                    {
                                        SetPenalty();
                                        rikishiUI.SetTachiaiBActive(true);
                                    }
                                    break;
                            }
                        }
                    }
                }
                #endregion
                break;
            case Game2Manager.GameState.Play:
                #region 立会い入力
                if(!isTachiaiEnd)
                {
                    switch(playerNum)
                    {
                        case 1:
                            if(Input.GetButtonDown("Decide1"))
                            {
                                TachiaiInput();
                                rikishiUI.SetTachiaiInput();
                            }
                            break;
                        case 2:
                            if(Input.GetButtonDown("Decide2"))
                            {
                                TachiaiInput();
                                rikishiUI.SetTachiaiInput();
                            }
                            break;
                    }
                    SetTimeMeasure();
                    SetTachiaiMove();
                }
                #endregion
                #region 重心移動対戦中
                else
                {
                    
                    SetEnemyTransform();
                    SetEnemyAngle();
                    SetFootInput();
                    SetShoulderRot();
                    SetEnemyDis();
                    SetGravityNum(right1 + right2, front1 + front2);
                    SetGravityPlace();
                    rikishiUI.SetGravityUI(graLRNum, graFBNum);
                    rikishiUI.SetArrowSprite((int)playStyle, angDifAbs, isAttack, isHataki);
                    SetGraPanelNum();
                    // SetVibration();
                    SetSpineAngle();
                    SetInDohyo();
                    maxAngle = SetMaxAngle();
                    SetDragNum(0, maxAngle);

                    SetRayCast();

                    switch(playerNum)
                    {
                        #region プレイヤー1の入力
                        case 1:
                            if(Input.GetAxis("LeftHorizontal1") != 0f || Input.GetAxis("LeftVertical1") != 0f)
                            {
                                SetEnemyGraInput(Input.GetAxis("LeftHorizontal1"), Input.GetAxis("LeftVertical1"));
                            }

                            if((Input.GetAxis("RightHorizontal1") != 0f || Input.GetAxis("RightVertical1") != 0f) && 
                                Input.GetAxis("MoveFoot1") == 0f
                                )
                            {
                                SetOwnGravity(2, Input.GetAxis("RightHorizontal1"), Input.GetAxis("RightVertical1"));
                            }

                            if(Input.GetAxis("MoveFoot1") < 0f &&
                                (Input.GetAxis("RightHorizontal1") != 0f || Input.GetAxis("RightVertical1") != 0f)
                                )
                            {
                                SetLeftFootNum(Input.GetAxis("RightHorizontal1"), Input.GetAxis("RightVertical1"));
                            }

                            if(Input.GetAxis("MoveFoot1") > 0f &&
                                (Input.GetAxis("RightHorizontal1") != 0f || Input.GetAxis("RightVertical1") != 0f)
                                )
                            {
                                SetRightFootNum(Input.GetAxis("RightHorizontal1"), Input.GetAxis("RightVertical1"));
                            }

                            if(Input.GetAxis("RightHorizontal1") == 0f && Input.GetAxis("RightVertical1") == 0f)
                            {
                                SetLeftFootNum(0, 0);
                                SetRightFootNum(0, 0);
                            }

                            if(Input.GetAxis("Rotation1") != 0f)
                            {
                                SetWholeAngle(enemy.gameObject, -Input.GetAxis("Rotation1"));
                            }

                            if(Input.GetAxis("MyRotation1") != 0f)
                            {
                                SetWholeAngle(this.gameObject, Input.GetAxis("MyRotation1"));
                            }

                            if(Input.GetAxis("LeftHorizontal1") == 0f && Input.GetAxis("LeftVertical1") == 0f)
                            {
                                SetEnemyGraInput(0, 0);
                            }

                            if(Input.GetAxis("RightHorizontal1") == 0f && Input.GetAxis("RightVertical1") == 0f && Input.GetAxis("MoveFoot1") == 0f ||
                                (Input.GetAxis("RightHorizontal1") != 0f || Input.GetAxis("RightVertical1") != 0f) && Input.GetAxis("MoveFoot1") != 0f
                                )
                            {
                                SetOwnGravity(2, 0 ,0);
                            }

                            if(Input.GetAxis("LeftHorizontal1") == 0f && Input.GetAxis("LeftVertical1") == 0f &&
                                Input.GetAxis("RightHorizontal1") == 0f && Input.GetAxis("RightVertical1") == 0f &&
                                Input.GetAxis("Rotation1") == 0f &&
                                !isCollision && !isEnd
                                )
                            {
                                moveDir = FindTransform();
                                // SetCollisionMove(moveDir.x, moveDir.y);
                            }

                            if(Input.GetButtonDown("Decide1"))
                            {
                                SetDragNum(1f, 0);
                            }

                            if(Input.GetButtonDown("Mawashi1"))
                            {
                                SetPlayStyle(PlayStyle.Mawashi);
                            }

                            if(Input.GetButtonDown("Oshi1"))
                            {
                                SetPlayStyle(PlayStyle.Oshi);
                            }

                            if(Input.GetButtonDown("Hataki1"))
                            {
                                SetPlayStyle(PlayStyle.Hataki);
                            }
                            break;
                        #endregion
                        #region プレイヤー2の入力
                        case 2:
                            if(Input.GetAxis("LeftHorizontal2") != 0f || Input.GetAxis("LeftVertical2") != 0f)
                            {
                                SetEnemyGraInput(Input.GetAxis("LeftHorizontal2"), Input.GetAxis("LeftVertical2"));
                            }

                            if((Input.GetAxis("RightHorizontal2") != 0f || Input.GetAxis("RightVertical2") != 0f) && 
                                Input.GetAxis("MoveFoot2") == 0f
                                )
                            {
                                SetOwnGravity(2, Input.GetAxis("RightHorizontal2"), Input.GetAxis("RightVertical2"));
                            }

                            if(Input.GetAxis("MoveFoot2") < 0f &&
                                (Input.GetAxis("RightHorizontal2") != 0f || Input.GetAxis("RightVertical2") != 0f)
                                )
                            {
                                SetLeftFootNum(Input.GetAxis("RightHorizontal2"), Input.GetAxis("RightVertical2"));
                            }

                            if(Input.GetAxis("MoveFoot2") > 0f &&
                                (Input.GetAxis("RightHorizontal2") != 0f || Input.GetAxis("RightVertical2") != 0f)
                                )
                            {
                                SetRightFootNum(Input.GetAxis("RightHorizontal2"), Input.GetAxis("RightVertical2"));
                            }

                            if(Input.GetAxis("RightHorizontal2") == 0f && Input.GetAxis("RightVertical2") == 0f)
                            {
                                SetLeftFootNum(0, 0);
                                SetRightFootNum(0, 0);
                            }

                            if(Input.GetAxis("Rotation2") != 0f)
                            {
                                SetWholeAngle(enemy.gameObject, -Input.GetAxis("Rotation2"));
                            }

                            if(Input.GetAxis("MyRotation2") != 0f)
                            {
                                SetWholeAngle(this.gameObject, Input.GetAxis("MyRotation2"));
                            }

                            if(Input.GetAxis("LeftHorizontal2") == 0f && Input.GetAxis("LeftVertical2") == 0f)
                            {
                                SetEnemyGraInput(0, 0);
                            }

                            if(Input.GetAxis("RightHorizontal2") == 0f && Input.GetAxis("RightVertical2") == 0f && Input.GetAxis("MoveFoot2") == 0f ||
                                (Input.GetAxis("RightHorizontal2") != 0f || Input.GetAxis("RightVertical2") != 0f) && Input.GetAxis("MoveFoot2") != 0f
                                )
                            {
                                SetOwnGravity(2, 0 ,0);
                            }

                            if(Input.GetAxis("LeftHorizontal2") == 0f && Input.GetAxis("LeftVertical2") == 0f &&
                                Input.GetAxis("RightHorizontal2") == 0f && Input.GetAxis("RightVertical2") == 0f &&
                                Input.GetAxis("Rotation2") == 0f &&
                                !isCollision && !isEnd
                                )
                            {
                                moveDir = FindTransform();
                                // SetCollisionMove(moveDir.x, moveDir.y);
                            }

                            if(Input.GetButtonDown("Decide2"))
                            {
                                SetDragNum(1f, 0);
                            }

                            if(Input.GetButtonDown("Mawashi2"))
                            {
                                SetPlayStyle(PlayStyle.Mawashi);
                            }

                            if(Input.GetButtonDown("Oshi2"))
                            {
                                SetPlayStyle(PlayStyle.Oshi);
                            }

                            if(Input.GetButtonDown("Hataki2"))
                            {
                                SetPlayStyle(PlayStyle.Hataki);
                            }
                            break;
                        #endregion
                    }
                }
                #endregion
                break;
            case Game2Manager.GameState.End:
                if(Input.GetButtonDown("Decide1") && isReplay)
                {
                    Game2Manager.Instance.PushReplayDown();
                }
                break;
        }

        lf = lfObj.transform.position;
        rf = rfObj.transform.position;
        footDis = new Vector2(Mathf.Abs(lf.x - rf.x), Mathf.Abs(lf.z - rf.z));
    }

    #region ゲーム設定に関するスクリプト
    // プレイヤー人数におけるカメラ設定
    private void SetCameraPlace()
    {
        switch(Game2Manager.Instance.gamePlayer)
        {
            case Game2Manager.GamePlayer.One:
                switch(playerNum)
                {
                    case 1:
                        playerCamera.gameObject.SetActive(true);
                        playerCamera.rect = new Rect(0, 0, 1, 1);
                        break;
                    case 2:
                        playerCamera.gameObject.SetActive(false);
                        break;
                }
                break;
            case Game2Manager.GamePlayer.Two:
                switch(playerNum)
                {
                    case 1:
                        playerCamera.gameObject.SetActive(true);
                        playerCamera.rect = new Rect(0, 0, 0.5f, 1);
                        break;
                    case 2:
                        playerCamera.gameObject.SetActive(true);
                        playerCamera.rect = new Rect(0.5f, 0, 1, 1);
                        break;
                }
                break;
        }
    }
    
    // 体重によるScaleの変化
    private void SetBodyScale()
    {
        scaleVector = playerObj.transform.localScale;
        scaleVector.y = scaleYNum;
        scaleVector.x = localScaleNum;
        scaleVector.z = localScaleNum;
        playerObj.transform.localScale = scaleVector;
        lossyScaleNum = playerObj.transform.lossyScale.x;
        SetMoveDisMagNum();
        SetMoveGraMagNum();
    }

    // 移動距離倍率の数値の計算を行う関数
    private void SetMoveDisMagNum()
    {
        footInidis = new Vector2(Mathf.Abs(lfInitialPos.x - rfInitialPos.x), Mathf.Abs(lfInitialPos.z - rfInitialPos.z)) * lossyScaleNum;
        // 足の長さに応じて移動距離を変更（例:長さ90⇒左右120,前後75：自分）
        moveLRDisMagNum = (((120f / 90f) * footLengNum) - footInidis.x) / (footMax * lossyScaleNum * 2f);
        moveFBDisMagNum = (((75f / 90f) * footLengNum) - footInidis.y) / (footMax * lossyScaleNum * 2f);
    }

    // 重心移動倍率の数値の計算を行う関数
    private void SetMoveGraMagNum()
    {
        switch(Game2Manager.Instance.gameMode)
        {
            case Game2Manager.GameMode.Easy:
                break;
            case Game2Manager.GameMode.Normal:
                graLRMagSlope = 0.2f / graMax;
                if(localScaleNum < 1.5f)
                {
                    if(graFBNum < 0f)
                    {
                        graFBMagSlope = 0.22f / graMax;
                        graFBMagIntercept = -0.15f / graMax;
                    }
                    else
                    {
                        graFBMagSlope = 0.19f / graMax;
                        graFBMagIntercept = -0.11f / graMax;
                    }
                }
                else
                {
                    if(graFBNum < 0f)
                    {
                        graFBMagSlope = 0.14f / graMax;
                        graFBMagIntercept = -0.03f / graMax;
                    }
                    else
                    {
                        graFBMagSlope = 0.1f / graMax;
                        graFBMagIntercept = 0.025f / graMax;
                    }
                }
                break;
        }
        
        moveFBGraMagNum = graFBMagSlope * localScaleNum + graFBMagIntercept;
        moveLRGraMagNum = graLRMagSlope * localScaleNum + graLRMagIntercept;
    }

    // 初期状態の記録
    private void SetInitialNum()
    {
        playerInitialScale = playerObj.transform.localScale;
        thisInitialPos = this.transform.position;
        thisInitialRot = this.transform.rotation;
        playerInitialPos = playerObj.transform.localPosition;
        playerInitialRot = playerObj.transform.localRotation;
        lsInitialRot = lsObj.transform.localRotation;
        rsInitialRot = rsObj.transform.localRotation;
        lfInitialPos = lfObj.transform.localPosition;
        rfInitialPos = rfObj.transform.localPosition;
        lhInitialRot = lhObj.transform.localRotation;
        rhInitialRot = rhObj.transform.localRotation;
        lThumbInitialRot = lThumbObj.transform.localRotation;
        rThumbInitialRot = rThumbObj.transform.localRotation;
        lIndexInitialRot = lIndexObj.transform.localRotation;
        rIndexInitialRot = rIndexObj.transform.localRotation;
        lMiddleInitialRot = lMiddleObj.transform.localRotation;
        rMiddleInitialRot = rMiddleObj.transform.localRotation;
        lRingInitialRot = lRingObj.transform.localRotation;
        rRingInitialRot = rRingObj.transform.localRotation;
        lLittleInitialRot = lLittleObj.transform.localRotation;
        rLittleInitialRot = rLittleObj.transform.localRotation;
        scaleYNum = playerObj.transform.localScale.y;
        footLengNum = lfObj.transform.position.y;
        wholeY = wholeObj.transform.position.y;
        center = new Vector3(0f, wholeY, -0.03f);
    }
    #endregion

    #region 体重に関するスクリプト
    // 体重の数値の入力
    public void SetWeightNum(float _weightNum)
    {
        weightNum = _weightNum;
    }

    // 体重の決定
    public void WeightInput()
    {
        weightInput = true;
        Game2Manager.Instance.GameStart(playerNum);
        localScaleNum = (weightNum + weightMax - 2 * weightMin) / (weightMax - weightMin);
        powerMagNum = localScaleNum;
        speedMagNum = 3f - localScaleNum;
        rb.mass = powerMagNum;
        SetBodyScale();
    }
    #endregion

    #region 立会いに関するスクリプト
    // 立会いのペナルティ回数
    private void SetPenalty()
    {
        penaltyNum++;
        startPushTime += 0.5f;
        rikishiUI.SetPenaltyText(penaltyNum);
    }

    // 立会いの入力時間の計測
    private void SetTimeMeasure()
    {
        if(!isStartPush)
        {
            startPushTime += Time.deltaTime;
        }
    }

    // 立会いの入力
    private void TachiaiInput()
    {
        isStartPush = true;
        rikishiUI.SetTachiaiBActive(false);
        Game2Manager.Instance.TachiaiStart(playerNum, startPushTime);
    }

    // 立会いの入力時間差の入力と開始位置の決定
    public void SetLagStartPos(int _playerNumMag, float _lag)
    {
        if(pushMaxLag < Mathf.Abs(_lag))
        {
            if(_lag > 0)
            {
                _lag = pushMaxLag;
            }
            else
            {
                _lag = -pushMaxLag;
            }
        }
        pushTimeLag = _lag;
        startPos = 
            new Vector3
            (
                (startPosXSlope * pushTimeLag + startPosXIntercept) * _playerNumMag,
                0,
                0
            );
        isTachiaiMove = true;
    }

    // 立会いの開始位置に移動する関数
    private void SetTachiaiMove()
    {
        if(isTachiaiMove)
        {
            if(pushTimeLag < 1.5f)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, startPos, Time.deltaTime * tachiaiSpeedMag);
            }
            else
            {
                if(isCollision)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, startPos, Time.deltaTime * tachiaiSpeedMag);
                }
            }
            if(this.transform.position == startPos)
            {
                isTachiaiMove = false;
                Game2Manager.Instance.TachiaiEnd(playerNum);
            }
        }
    }

    // 立会いの終了を示す関数
    public void SetTachiaiEnd()
    {
        isTachiaiEnd = true;
        rikishiUI.SetPlayImage(1);
    }
    #endregion

    #region プレイヤーの入力に関するスクリプト
    // プレイ状態の変化入力を行う関数
    private void SetPlayStyle(PlayStyle _playStyle)
    {
        if(playStyle != _playStyle)
        {
            playStyle = _playStyle;
        }
        else
        {
            playStyle = PlayStyle.Yothu;
        }
        rikishiUI.SetPlayImage((int)playStyle);
    }

    // 左JoyStickによる重心値の変化入力を行う関数
    private void SetEnemyGraInput(float rightPosi, float frontPosi)
    {
        angleY = this.transform.eulerAngles.y;
        enemyAngleY = enemy.gameObject.transform.eulerAngles.y;
        float graChaELRNum = 0;
        float graChaEFBNum = 0;
        float graChaMLRNum = 0;
        float graChaMFBNum = 0;

        SetEnemyArrow(1, rightPosi, frontPosi);
        if(rightPosi != 0 || frontPosi != 0)
        {
            if(angDifAbs <= 120f)
            {
                isLRPush = true;
            }
            else
            {
                isLRPush = false;
            }

            if(angDifAbs <= 60f || 
                (60f <= angDifAbs && angDifAbs <= 120f && frontPosi > 0f) ||
                (120f <= angDifAbs && angDifAbs <= 180f && frontPosi < 0f)
                )
            {
                isFBPush = true;
            }
            else
            {
                isFBPush = false;
            }

            if((playStyle == PlayStyle.Yothu || playStyle == PlayStyle.Mawashi) && !isAttack)
            {
                rightPosi = 0;
                frontPosi = 0;
            }

            if(playStyle == PlayStyle.Oshi)
            {
                if(angDifAbs <= 60f && isAttack)
                {
                    if(frontPosi <= 0)
                    {
                        frontPosi = 0;
                    }
                }
                else
                {
                    rightPosi = 0;
                    frontPosi = 0;
                }
            }

            if(playStyle == PlayStyle.Hataki)
            {
                if(angDifAbs <= 60f && isHataki)
                {
                    if(frontPosi >= 0)
                    {
                        frontPosi = 0;
                    }
                }
                else
                {
                    rightPosi = 0;
                    frontPosi = 0;
                }
            }

            if(isLRPush)
            {
                graChaELRNum += rightPosi * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                graChaEFBNum += rightPosi * Mathf.Cos((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
            }
            else
            {
                graChaMLRNum += rightPosi;
            }

            if(isFBPush)
            {
                graChaELRNum += frontPosi * Mathf.Cos((180f - (angleY + (90f - enemyAngleY))) * Mathf.Deg2Rad);
                graChaEFBNum += frontPosi * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
            }
            else
            {
                graChaMFBNum += frontPosi;
            }

            if(isFBPush && isLRPush)
            {
                SetEnemyGravity(1, graChaELRNum, graChaEFBNum);
            }
            else if(!isFBPush && !isLRPush)
            {
                SetOwnGravity(1, graChaMLRNum, graChaMFBNum);
            }
            else
            {
                SetEnemyGravity(1, graChaELRNum, graChaEFBNum);
                SetOwnGravity(1, graChaMLRNum, graChaMFBNum);
            }
        }
        else
        {
            SetEnemyGravity(1, 0, 0);;
            SetOwnGravity(1, 0, 0);
        }
    }

    // 左足の入力値の変化を行う関数
    private void SetLeftFootNum(float rightPosi, float frontPosi)
    {
        angleY = this.transform.eulerAngles.y;
        enemyAngleY = enemy.gameObject.transform.eulerAngles.y;
        float enemyRight = rightPosi;
        float enemyFront = frontPosi;
        float myLFLRGra = 0;
        float myLFFBGra = 0;
        float myLFLRPos = 0;
        float myLFFBPos = 0;
        float lFLRGra = 0;
        float lFFBGra = 0;
        float lFLRPos = 0;
        float lFFBPos = 0;
        bool lFLRInput = false;
        bool lFFBInput = false; 

        SetEnemyArrow(2, enemyRight, enemyFront);
        if((playStyle == PlayStyle.Yothu || playStyle == PlayStyle.Mawashi) && !isAttack)
        {
            enemyRight = 0;
            enemyFront = 0;
        }

        if(playStyle == PlayStyle.Oshi)
        {
            if(angDifAbs <= 60f && isAttack)
            {
                if(enemyFront <= 0)
                {
                    enemyFront = 0;
                }
            }
            else
            {
                enemyRight = 0;
                enemyFront = 0;
            }
        }

        if(playStyle == PlayStyle.Hataki)
        {
            if(angDifAbs <= 60f && isHataki)
            {
                if(enemyFront >= 0)
                {
                    enemyFront = 0;
                }
            }
            else
            {
                enemyRight = 0;
                enemyFront = 0;
            }
        }

        if((rightPosi < 0f && -footMax < lFLRNum) || (rightPosi > 0f && lFLRNum < clossMax))
        {
            lFLRInput = true;
            rFLRNum -= Time.deltaTime * rightPosi * moveSpeedMagNum * speedMagNum;
            lFLRNum += Time.deltaTime * rightPosi * moveSpeedMagNum * speedMagNum;
            myLFLRPos += rightPosi;
            if((graLRNum < 0 && rightPosi < 0) || (graLRNum > 0 && rightPosi > 0))
            {
                myLFLRGra += -rightPosi;
            }
            if(angDifAbs <= 120f)
            {
                if(playStyle == PlayStyle.Hataki)
                {
                    lFLRGra += enemyRight * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                    lFFBGra += enemyRight * Mathf.Cos((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                }
                else
                {
                    lFLRGra += enemyRight * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                    lFFBGra += enemyRight * Mathf.Cos((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                    lFLRPos += enemyRight * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                    lFFBPos += enemyRight * Mathf.Cos((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                }
            }
        }
        else
        {
            lFLRInput = false;
        }

        if((frontPosi < 0f && -footMax < lFFBNum) || (frontPosi > 0f && lFFBNum < footMax))
        {
            lFFBInput = true;
            rFFBNum -= Time.deltaTime * frontPosi * moveSpeedMagNum * speedMagNum;
            lFFBNum += Time.deltaTime * frontPosi * moveSpeedMagNum * speedMagNum;
            myLFFBPos += frontPosi;
            if((graFBNum < 0 && frontPosi < 0) || (graFBNum > 0 && frontPosi > 0))
            {
                myLFFBGra += -frontPosi;
            }
            if(angDifAbs <= 60f || 
                (60f <= angDifAbs && angDifAbs <= 120f && enemyFront > 0f) ||
                (120f <= angDifAbs && angDifAbs <= 180f && enemyFront < 0f)
                )
            {
                if(playStyle == PlayStyle.Hataki)
                {
                    lFLRGra += enemyFront * Mathf.Cos((180f - (angleY + (90f - enemyAngleY))) * Mathf.Deg2Rad);
                    lFFBGra += enemyFront * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                }
                else
                {
                    lFLRGra += enemyFront * Mathf.Cos((180f - (angleY + (90f - enemyAngleY))) * Mathf.Deg2Rad);
                    lFFBGra += enemyFront * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                    lFLRPos += enemyFront * Mathf.Cos((180f - (angleY + (90f - enemyAngleY))) * Mathf.Deg2Rad);
                    lFFBPos += enemyFront * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                }
            }
        }
        else
        {
            lFFBInput = false;
        }

        if(!lFLRInput && !lFFBInput)
        {
            lFOpeInput = false;
            SetEnemyGravity(2, 0, 0);
            SetOwnGravity(3, 0, 0);
        }
        else
        {
            lFOpeInput = true;
            SetOwnGravity(3, myLFLRGra, myLFFBGra);
            SetPlayerPos(myLFLRPos, myLFFBPos);
            SetFootPlace();
            SetEnemyGravity(2, lFLRGra, lFFBGra);
            enemy.SetPlayerPos(lFLRPos, lFFBPos);
        }
    }

    // 右足の入力値の変化を行う関数
    private void SetRightFootNum(float rightPosi, float frontPosi)
    {
        angleY = this.transform.eulerAngles.y;
        enemyAngleY = enemy.gameObject.transform.eulerAngles.y;
        float enemyRight = rightPosi;
        float enemyFront = frontPosi;
        float myRFLRGra = 0;
        float myRFFBGra = 0;
        float myRFLRPos = 0;
        float myRFFBPos = 0;
        float rFLRGra = 0;
        float rFFBGra = 0;
        float rFLRPos = 0;
        float rFFBPos = 0;
        bool rFLRInput = false;
        bool rFFBInput = false;

        SetEnemyArrow(3, enemyRight, enemyFront);
        if((playStyle == PlayStyle.Yothu || playStyle == PlayStyle.Mawashi) && !isAttack)
        {
            enemyRight = 0;
            enemyFront = 0;
        }

        if(playStyle == PlayStyle.Oshi)
        {
            if(angDifAbs <= 60f && isAttack)
            {
                if(enemyFront <= 0)
                {
                    enemyFront = 0;
                }
            }
            else
            {
                enemyRight = 0;
                enemyFront = 0;
            }
        }

        if(playStyle == PlayStyle.Hataki)
        {
            if(angDifAbs <= 60f && isHataki)
            {
                if(enemyFront >= 0)
                {
                    enemyFront = 0;
                }
            }
            else
            {
                enemyRight = 0;
                enemyFront = 0;
            }
        }

        if((rightPosi < 0f && -clossMax < rFLRNum) || (rightPosi > 0f && rFLRNum < footMax))
        {
            rFLRNum += Time.deltaTime * rightPosi * moveSpeedMagNum * speedMagNum;
            lFLRNum -= Time.deltaTime * rightPosi * moveSpeedMagNum * speedMagNum;
            rFLRInput = true;
            myRFLRPos += rightPosi;
            if((graLRNum < 0 && rightPosi < 0) || (graLRNum > 0 && rightPosi > 0))
            {
                myRFLRGra += -rightPosi;
            }
            if(angDifAbs <= 120f)
            {
                if(playStyle == PlayStyle.Hataki)
                {
                    rFLRGra += enemyRight * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                    rFFBGra += enemyRight * Mathf.Cos((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                }
                else
                {
                    rFLRGra += enemyRight * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                    rFFBGra += enemyRight * Mathf.Cos((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                    rFLRPos += enemyRight * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                    rFFBPos += enemyRight * Mathf.Cos((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                }
            }
        }
        else
        {
            rFLRInput = false;
        }

        if((frontPosi < 0f && -footMax < rFFBNum) || (frontPosi > 0f && rFFBNum < footMax))
        {
            rFFBNum += Time.deltaTime * frontPosi * moveSpeedMagNum * speedMagNum;
            lFFBNum -= Time.deltaTime * frontPosi * moveSpeedMagNum * speedMagNum;
            rFFBInput = true;
            myRFFBPos += frontPosi;
            if((graFBNum < 0 && frontPosi < 0) || (graFBNum > 0 && frontPosi > 0))
            {
                myRFFBGra += -frontPosi;
            }
            if(angDifAbs <= 60f || 
                (60f <= angDifAbs && angDifAbs <= 120f && enemyFront > 0f) ||
                (120f <= angDifAbs && angDifAbs <= 180f && enemyFront < 0f)
                )
            {
                if(playStyle == PlayStyle.Hataki)
                {
                    rFLRGra += enemyFront * Mathf.Cos((180f - (angleY + (90f - enemyAngleY))) * Mathf.Deg2Rad);
                    rFFBGra += enemyFront * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                }
                else
                {
                    rFLRGra += enemyFront * Mathf.Cos((180f - (angleY + (90f - enemyAngleY))) * Mathf.Deg2Rad);
                    rFFBGra += enemyFront * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                    rFLRPos += enemyFront * Mathf.Cos((180f - (angleY + (90f - enemyAngleY))) * Mathf.Deg2Rad);
                    rFFBPos += enemyFront * Mathf.Sin((angleY + (90f - enemyAngleY)) * Mathf.Deg2Rad);
                }
            }
        }
        else
        {
            rFFBInput = false;
        }

        if(!rFLRInput && !rFFBInput)
        {
            rFOpeInput = false;
            SetEnemyGravity(3, 0, 0);
            SetOwnGravity(4, 0, 0);
        }
        else
        {
            rFOpeInput = true;
            SetOwnGravity(4, myRFLRGra, myRFFBGra);
            SetPlayerPos(myRFLRPos, myRFFBPos);
            SetFootPlace();
            SetEnemyGravity(3, rFLRGra, rFFBGra);
            enemy.SetPlayerPos(rFLRPos, rFFBPos);
        }
    }

    // 各足の入力状態の確認を行う関数
    private void SetFootInput()
    {
        if(!lFOpeInput && !rFOpeInput)
        {
            rikishiUI.SetFootOperateColor(0);
            rikishiUI.SetFootOpeActive(false, false);
        }
        else if(lFOpeInput && !rFOpeInput)
        {
            rikishiUI.SetFootOperateColor(1);
            rikishiUI.SetFootOpeActive(true, false);
        }
        else if(!lFOpeInput && rFOpeInput)
        {
            rikishiUI.SetFootOperateColor(2);
            rikishiUI.SetFootOpeActive(false, true);
        }
    }

    // 互いの足の当たり判定を行う関数
    private void SetRayCast()
    {
        #region 左足外側の判定
        Vector3 loOrigin = lOBObj.transform.position;
        Vector3 loDirection = new Vector3(
            lOFObj.transform.position.x - lOBObj.transform.position.x,
            lOFObj.transform.position.y - lOBObj.transform.position.y,
            lOFObj.transform.position.z - lOBObj.transform.position.z
            );
        Ray loRay = new Ray(loOrigin, loDirection);
        // Debug.DrawRay(loRay.origin, loRay.direction * 0.53f, Color.red);

        RaycastHit lohit;
        bool loCol = false;

        if(Physics.Raycast(loRay, out lohit, 0.53f))
        {
            switch(playerNum)
            {
                case 1:
                    if(lohit.collider.CompareTag("Rikishi2"))
                    {
                        loCol = true;
                    }
                    else
                    {
                        loCol = false;
                    }
                    break;
                case 2:
                    if(lohit.collider.CompareTag("Rikishi1"))
                    {
                        loCol = true;
                    }
                    else
                    {
                        loCol = false;
                    }
                    break;
            }
        }
        else
        {
            loCol = false;
        }
        #endregion

        #region 左足内側の判定
        Vector3 liOrigin = lIBObj.transform.position;
        Vector3 liDirection = new Vector3(
            lIFObj.transform.position.x - lIBObj.transform.position.x,
            lIFObj.transform.position.y - lIBObj.transform.position.y,
            lIFObj.transform.position.z - lIBObj.transform.position.z
            );
        Ray liRay = new Ray(liOrigin, liDirection);
        // Debug.DrawRay(liRay.origin, liRay.direction * 0.53f, Color.blue);

        RaycastHit lihit;
        bool liCol = false;

        if(Physics.Raycast(liRay, out lihit, 0.53f))
        {
            switch(playerNum)
            {
                case 1:
                    if(lihit.collider.CompareTag("Rikishi2"))
                    {
                        liCol = true;
                    }
                    else
                    {
                        liCol = false;
                    }
                    break;
                case 2:
                    if(lihit.collider.CompareTag("Rikishi1"))
                    {
                        liCol = true;
                    }
                    else
                    {
                        liCol = false;
                    }
                    break;
            }
        }
        else
        {
            liCol = false;
        }
        #endregion

        #region 右足外側の判定
        Vector3 roOrigin = rOBObj.transform.position;
        Vector3 roDirection = new Vector3(
            rOFObj.transform.position.x - rOBObj.transform.position.x,
            rOFObj.transform.position.y - rOBObj.transform.position.y,
            rOFObj.transform.position.z - rOBObj.transform.position.z
            );
        Ray roRay = new Ray(roOrigin, roDirection);
        // Debug.DrawRay(roRay.origin, roRay.direction * 0.53f, Color.green);

        RaycastHit rohit;
        bool roCol = false;

        if(Physics.Raycast(roRay, out rohit, 0.53f))
        {
            switch(playerNum)
            {
                case 1:
                    if(rohit.collider.CompareTag("Rikishi2"))
                    {
                        roCol = true;
                    }
                    else
                    {
                        roCol = false;
                    }
                    break;
                case 2:
                    if(rohit.collider.CompareTag("Rikishi1"))
                    {
                        roCol = true;
                    }
                    else
                    {
                        roCol = false;
                    }
                    break;
            }
        }
        else
        {
            roCol = false;
        }
        #endregion

        #region 右足内側の判定
        Vector3 riOrigin = rIBObj.transform.position;
        Vector3 riDirection = new Vector3(
            rIFObj.transform.position.x - rIBObj.transform.position.x,
            rIFObj.transform.position.y - rIBObj.transform.position.y,
            rIFObj.transform.position.z - rIBObj.transform.position.z
            );
        Ray riRay = new Ray(riOrigin, riDirection);
        // Debug.DrawRay(riRay.origin, riRay.direction * 0.53f, Color.yellow);

        RaycastHit rihit;
        bool riCol = false;

        if(Physics.Raycast(riRay, out rihit, 0.53f))
        {
            switch(playerNum)
            {
                case 1:
                    if(rihit.collider.CompareTag("Rikishi2"))
                    {
                        riCol = true;
                    }
                    else
                    {
                        riCol = false;
                    }
                    break;
                case 2:
                    if(rihit.collider.CompareTag("Rikishi1"))
                    {
                        riCol = true;
                    }
                    else
                    {
                        riCol = false;
                    }
                    break;
            }
        }
        else
        {
            riCol = false;
        }
        #endregion

        if(liCol || riCol)
        {
            isInColl = true;
        }
        else
        {
            isInColl = false;
        }

        if(loCol || roCol)
        {
            isOutColl = true;
        }
        else
        {
            isOutColl = false;
        }
    }
    #endregion
    
    #region 重心に関するスクリプト
    // 相手の重心値の変化入力値の合計と矢印変化を行う関数
    private void SetEnemyGravity(int _lrNum, float rightPosi, float frontPosi)
    {
        switch(_lrNum)
        {
            case 1:
                attackLLR = rightPosi;
                attackLFB = frontPosi;
                break;
            case 2:
                attackMoveLLR = rightPosi;
                attackMoveLFB = frontPosi;
                break;
            case 3:
                attackMoveRLR = rightPosi;
                attackMoveRFB = frontPosi;
                break;
        }
        
        enemy.SetGraChangeNum(
            playerNum,
            attackLLR + attackMoveLLR + attackMoveRLR,
            attackLFB + attackMoveLFB + attackMoveRFB
            );
    }

    // 相手の重心値の変化入力における矢印変化を行う関数
    private void SetEnemyArrow(int inputNum, float rightPosi, float frontPosi)
    {
        float inputSumLR = 0;
        float inputSumFB = 0;

        switch(inputNum)
        {
            case 1:
                inputLLR = rightPosi;
                inputLFB = frontPosi;
                break;
            case 2:
                inputMoveLLR = rightPosi;
                inputMoveLFB = frontPosi;
                break;
            case 3:
                inputMoveRLR = rightPosi;
                inputMoveRFB = frontPosi;
                break;
        }

        inputSumLR = inputLLR + inputMoveLLR + inputMoveRLR;
        inputSumFB = inputLFB + inputMoveLFB + inputMoveRFB;

        if(inputSumFB < -inputMin)
        {
            rikishiUI.SetArrowActive(0, false);
            rikishiUI.SetArrowActive(1, true);
        }
        else if(inputMin < inputSumFB)
        {
            rikishiUI.SetArrowActive(0, true);
            rikishiUI.SetArrowActive(1, false);
        }
        else
        {
            rikishiUI.SetArrowActive(0, false);
            rikishiUI.SetArrowActive(1, false);
        }

        if(inputSumLR < -inputMin)
        {
            rikishiUI.SetArrowActive(2, true);
            rikishiUI.SetArrowActive(3, false);
        }
        else if(inputMin < inputSumLR)
        {
            rikishiUI.SetArrowActive(2, false);
            rikishiUI.SetArrowActive(3, true);
        }
        else
        {
            rikishiUI.SetArrowActive(2, false);
            rikishiUI.SetArrowActive(3, false);
        }
    }

    // 自身の重心値の変化入力値の合計を行う関数
    private void SetOwnGravity(int _lrNum, float rightPosi, float frontPosi)
    {
        switch(_lrNum)
        {
            case 1:
                leftLR = rightPosi;
                leftFB = frontPosi;
                break;
            case 2:
                rightLR = rightPosi;
                rightFB = frontPosi;
                break;
            case 3:
                moveLFLR = rightPosi;
                moveLFFB = frontPosi;
                break;
            case 4:
                moveRFLR = rightPosi;
                moveRFFB = frontPosi;
                break;
        }
        
        SetGraChangeNum(
            playerNum,
            leftLR + rightLR + moveLFLR + moveRFLR,
            leftFB + rightFB + moveLFFB + moveRFFB
            );
    }

    // 重心値の変化量の計算を行う関数
    public void SetGraChangeNum(int _playerNum, float rightPosi, float frontPosi)
    {
        switch(_playerNum)
        {
            case 1:
                right1 = rightPosi;
                front1 = frontPosi;
                break;
            case 2:
                right2 = rightPosi;
                front2 = frontPosi;
                break;
        }
    }

    // 重心値の変化を行う関数
    private void SetGravityNum(float rightPosi, float frontPosi)
    {
        graFBNum += Time.deltaTime * frontPosi * moveSpeedMagNum * powerMagNum;
        graLRNum += Time.deltaTime * rightPosi * moveSpeedMagNum * powerMagNum;
        SetMoveGraMagNum();
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
        gravityWorldPos = playerObj.transform.TransformPoint(gravityPlace / scaleYNum);
        Debug.DrawLine (playerObj.transform.position , playerObj.transform.position + playerObj.transform.rotation * gravityPlace);
        // Debug.DrawLine (playerObj.transform.position , playerObj.transform.position + playerObj.transform.rotation * (gravityPlace / playerScale));
    }

    // 重心のUI管理に関する関数
    private void SetGraPanelNum()
    {
        if(Mathf.Abs(right1) < inputMin && Mathf.Abs(front1) < inputMin && Mathf.Abs(right2) < inputMin && Mathf.Abs(front2) < inputMin)
        {
            rikishiUI.SetGraPanelColor(0);
        }
        if((Mathf.Abs(right1) > inputMin || Mathf.Abs(front1) > inputMin) && Mathf.Abs(right2) < inputMin && Mathf.Abs(front2) < inputMin)
        {
            rikishiUI.SetGraPanelColor(1);
        }
        if(Mathf.Abs(right1) < inputMin && Mathf.Abs(front1) < inputMin && (Mathf.Abs(right2) > inputMin || Mathf.Abs(front2) > inputMin))
        {
            rikishiUI.SetGraPanelColor(2);
        }
        if((Mathf.Abs(right1) > inputMin || Mathf.Abs(front1) > inputMin) && (Mathf.Abs(right2) > inputMin || Mathf.Abs(front2) > inputMin))
        {
            rikishiUI.SetGraPanelColor(3);
        }
    }

    // 重心の移動によるバイブレーション
    private void SetVibration()
    {
        switch(playerNum)
        {
            case 1:
                if((right1 >= 0 && (right1 + right2 < -inputMin)) || (right1 <= 0 && (right1 + right2 > inputMin)) ||
                    (front1 >= 0 && (front1 + front2 < -inputMin)) || (front1 <= 0 && (front1 + front2 > inputMin))
                    )
                {
                    GamePad.SetVibration(PlayerIndex.Two, 1, 1);
                }
                else
                {
                    GamePad.SetVibration(PlayerIndex.Two, 0, 0);
                }
                break;
            case 2:
                if((right2 >= 0 && (right1 + right2 < -inputMin)) || (right2 <= 0 && (right1 + right2 > inputMin)) ||
                    (front2 >= 0 && (front1 + front2 < -inputMin)) || (front2 <= 0 && (front1 + front2 > inputMin))
                    )
                {
                    GamePad.SetVibration(PlayerIndex.One, 1, 1);
                }
                else
                {
                    GamePad.SetVibration(PlayerIndex.One, 0, 0);
                }
                break;
        }
    }
    #endregion

    #region オブジェクトの座標に関するスクリプト
    // 敵との距離を測る関数
    public void SetEnemyDis()
    {
        Vector2 playerPlace =  new Vector2(this.transform.position.x, this.transform.position.z);
        Vector2 enemyPlace =  new Vector2(enemy.gameObject.transform.position.x, enemy.gameObject.transform.position.z);
        enemyDis = Vector2.Distance(playerPlace, enemyPlace);
        if(enemyDis < attackMax)
        {
            isAttack = true;
            if(playStyle == PlayStyle.Yothu || playStyle == PlayStyle.Mawashi || playStyle == PlayStyle.Oshi)
            {
                rikishiUI.SetBlinkPlayEnd();
            }
        }
        else
        {
            isAttack = false;
            if(playStyle == PlayStyle.Yothu || playStyle == PlayStyle.Mawashi || playStyle == PlayStyle.Oshi)
            {
                rikishiUI.SetBlinkPlay();
            }
        }
        if(enemyDis < hatakiMax)
        {
            isHataki = true;
            if(playStyle == PlayStyle.Hataki)
            {
                rikishiUI.SetBlinkPlayEnd();
            }
        }
        else
        {
            isHataki = false;
            if(playStyle == PlayStyle.Hataki)
            {
                rikishiUI.SetBlinkPlay();
            }
        }
    }

    // 全身の位置の変更を行う関数
    private void SetPlayerPos(float rightPosi, float frontPosi)
    {
        this.transform.Translate(
            Time.deltaTime * rightPosi * moveSpeedMagNum * moveLRDisMagNum * lossyScaleNum * speedMagNum,
            0f,
            Time.deltaTime * frontPosi * moveSpeedMagNum * moveFBDisMagNum * lossyScaleNum * speedMagNum
        );
    }

    // 足の位置の変更を行う関数
    private void SetFootPlace()
    {
        lfObj.transform.localPosition = 
            new Vector3
            (
                lFLRNum * moveLRDisMagNum + lfInitialPos.x,
                lfInitialPos.y,
                lFFBNum * moveFBDisMagNum + lfInitialPos.z
            );

        rfObj.transform.localPosition = 
            new Vector3
            (
                rFLRNum * moveLRDisMagNum + rfInitialPos.x,
                rfInitialPos.y,
                rFFBNum * moveFBDisMagNum + rfInitialPos.z
            );

        rikishiUI.SetFootOpeUIPlace(lfObj.transform.position, rfObj.transform.position);
    }
    #endregion

    #region オブジェクトの角度に関するスクリプト  
    // 相手の向きとの角度計算を行う関数
    private void SetEnemyAngle()
    {
        angularDif = Vector3.SignedAngle(target, viewDir, Vector3.up);
        angDifAbs = Mathf.Abs(angularDif);
    }

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
    private void SetWholeAngle(GameObject target, float rotateSpeed)
    {
        this.transform.RotateAround
        (
            target.transform.position,
            Vector3.up,
            Time.deltaTime * rotateSpeed * moveSpeedMagNum * moveSpeedMagNum * speedMagNum
        );
    }

    // プレイスタイルに応じて肩の角度を変更する関数
    private void SetShoulderRot()
    {
        switch(playStyle)
        {
            case PlayStyle.Yothu:
                SetHandRot();
                if(angDifAbs <= 60)
                {
                    lsObj.transform.localRotation = lsInitialRot;
                    rsObj.transform.localRotation = rsInitialRot;
                }
                else if(angDifAbs <= 120)
                {
                    if(angularDif > 0)
                    {
                        lsObj.transform.localEulerAngles = new Vector3(-320.629f, 148.527f, 125.262f);
                        rsObj.transform.localEulerAngles = new Vector3(-233.385f, 130.717f, -8.995f);
                    }
                    else
                    {
                        lsObj.transform.localEulerAngles = new Vector3(-233.385f, 130.717f, -8.995f);
                        rsObj.transform.localEulerAngles = new Vector3(-320.629f, 148.527f, -234.738f);
                    }
                }
                else
                {
                    lsObj.transform.localEulerAngles = new Vector3(-112.308f, 298.164f, -30.504f);
                    rsObj.transform.localEulerAngles = new Vector3(-67.692f, 118.164f, -210.504f);
                }
                break;
            case PlayStyle.Mawashi: 
                SetHandRot();
                if(angDifAbs <= 60)
                {
                    lsObj.transform.localEulerAngles = new Vector3(-219.864f, 56.919f, -59.877f);
                    rsObj.transform.localEulerAngles = new Vector3(-219.864f, 56.919f, -59.877f);
                }
                else if(angDifAbs <= 120)
                {
                    if(angularDif > 0)
                    {
                        lsObj.transform.localEulerAngles = new Vector3(-208.801f, 308.66f, -79.346f);
                        rsObj.transform.localEulerAngles = new Vector3(43.183f, 250.925f, -259.626f);
                    }
                    else
                    {
                        lsObj.transform.localEulerAngles = new Vector3(43.183f, 250.925f, -259.626f);
                        rsObj.transform.localEulerAngles = new Vector3(-208.801f, 308.66f, -79.346f);
                    }
                }
                else
                {
                    lsObj.transform.localEulerAngles = new Vector3(-126.877f, 26.653f, -116.695f);
                    rsObj.transform.localEulerAngles = new Vector3(-53.123f, 206.653f, -296.695f);
                }
                break;
            case PlayStyle.Oshi:
                if(angDifAbs <= 60)
                {
                    lsObj.transform.localEulerAngles = new Vector3(-201.884f, 100.532f, -3.473f);
                    rsObj.transform.localEulerAngles = new Vector3(-338.116f, -79.468f, -183.473f);
                    lhObj.transform.localEulerAngles = new Vector3(21.684f, 32.586f, 102.046f);
                    rhObj.transform.localEulerAngles = new Vector3(21.684f, 32.586f, 102.046f);
                    lThumbObj.transform.localEulerAngles = new Vector3(7.322f, 151.168f, -19.634f);
                    rThumbObj.transform.localEulerAngles = new Vector3(7.322f, 151.168f, -19.634f);
                    lIndexObj.transform.localEulerAngles = new Vector3(14.306f, 177.402f, 7.608f);
                    rIndexObj.transform.localEulerAngles = new Vector3(14.306f, 177.402f, 7.608f);
                    lMiddleObj.transform.localEulerAngles = new Vector3(23.152f, -175.871f, 6.675f);
                    rMiddleObj.transform.localEulerAngles = new Vector3(23.152f, -175.871f, 6.675f);
                    lRingObj.transform.localEulerAngles = new Vector3(27.452f, -168.91f, -1.89f);
                    rRingObj.transform.localEulerAngles = new Vector3(27.452f, -168.91f, -1.89f);
                    lLittleObj.transform.localEulerAngles = new Vector3(29.109f, -163.104f, -8.058f);
                    rLittleObj.transform.localEulerAngles = new Vector3(29.109f, -163.104f, -8.058f);
                }
                else
                {
                    lsObj.transform.localEulerAngles = new Vector3(-178.919f, 10.02f, -91.011f);
                    rsObj.transform.localEulerAngles = new Vector3(-361.081f, -190.02f, -271.01f);
                    SetHandRot();
                }
                break;
            case PlayStyle.Hataki:
                SetHandRot();
                if(angDifAbs <= 60)
                {
                    lsObj.transform.localEulerAngles = new Vector3(-219.3f, 112.495f, 30.527f);
                    rsObj.transform.localEulerAngles = new Vector3(39.3f, -67.505f, -149.473f);
                }
                else
                {
                    lsObj.transform.localEulerAngles = new Vector3(-178.919f, 10.02f, -91.011f);
                    rsObj.transform.localEulerAngles = new Vector3(-361.081f, -190.02f, -271.01f);
                }
                break;
        }
    }

    // 手の先の角度を初期に戻す関数
    private void SetHandRot()
    {
        lhObj.transform.localRotation = lhInitialRot;
        rhObj.transform.localRotation = rhInitialRot;
        lThumbObj.transform.localRotation = lThumbInitialRot;
        rThumbObj.transform.localRotation = rThumbInitialRot;
        lIndexObj.transform.localRotation = lIndexInitialRot;
        rIndexObj.transform.localRotation = rIndexInitialRot;
        lMiddleObj.transform.localRotation = lMiddleInitialRot;
        rMiddleObj.transform.localRotation = rMiddleInitialRot;
        lRingObj.transform.localRotation = lRingInitialRot;
        rRingObj.transform.localRotation = rRingInitialRot;
        lLittleObj.transform.localRotation = lLittleInitialRot;
        rLittleObj.transform.localRotation = rLittleInitialRot;
    }
    #endregion

    #region 抵抗に関するスクリプト
    // プレイヤーの角度の最大値を計算する関数
    private float SetMaxAngle()
    {
        float xAngle =  Mathf.Abs(Mathf.Repeat(playerObj.transform.localEulerAngles.x + 180, 360) - 180);
        float zAngle =  Mathf.Abs(Mathf.Repeat(playerObj.transform.localEulerAngles.z + 180, 360) - 180);
        float max = Mathf.Max(xAngle, zAngle);
        return max;
    }

    // 抵抗値の計算と代入をする関数
    private void SetDragNum(float _pushNum, float _maxAngle)
    {
        minusPerSecond = 1 + (_maxAngle / angleMagNum);
        dragNum += _pushNum - Time.deltaTime * minusPerSecond;
        if(dragNum <= 0f)
        {
            dragNum = 0f;
        }
        rb.drag = dragNum;
    }
    #endregion

    #region 勝敗に関するスクリプト
    // 土俵外に出ているかの判定
    public void SetInDohyo()
    {
        Vector2 lfPlace =  new Vector2(lfObj.transform.position.x, lfObj.transform.position.z);
        Vector2 rfPlace =  new Vector2(rfObj.transform.position.x, rfObj.transform.position.z);
        Vector2 dohyoPlace =  new Vector2(dohyoObj.transform.position.x, dohyoObj.transform.position.z);

        dohyoLDis = Vector2.Distance(lfPlace, dohyoPlace);
        dohyoRDis = Vector2.Distance(rfPlace, dohyoPlace);
        float maxDis = Mathf.Max(dohyoLDis, dohyoRDis);
        if(maxDis > dohyoRadius)
        {
            SetResult(true, false, false, true);
            enemy.SetResult(true, true, false, false);
        }
    }

    // 勝敗の決着時に呼ばれる関数
    public void SetResult(bool _isEnd, bool _isResult, bool _isfallDown, bool _isOutDohyo)
    {
        if(!isEnd)
        {
            isEnd =  _isEnd;
            isResult = _isResult;
            isFallDown = _isfallDown;
            isOutDohyo = _isOutDohyo;
            Game2Manager.Instance.SetGameResult(playerNum, isResult, graFBNum, graLRNum, isFallDown, isOutDohyo, (int)playStyle, angularDif, isInColl, isOutColl);
        }
    }
    #endregion

    #region 自動移動に関するスクリプト
    // 相手と衝突の有無時に呼ばれる関数
    public void SetCollision(bool _isCollision)
    {
        isCollision = _isCollision;
    }

    // 相手の胸元方向と自身の視線方向のベクトルを計算する関数
    private void SetEnemyTransform()
    {
        nowPos = this.transform.position;
        enemyPos = enemy.gameObject.transform.position;
        viewPos = viewObj.transform.position;

        target = enemyPos - nowPos;
        viewDir = viewPos - nowPos;
    }

    // 非接触時相手の胸元方向の単位移動ベクトルを計算する関数
    private Vector2 FindTransform()
    {
        Vector2 _move = new Vector2();

        if(target.x > 0)
        {
            _move.x = 1f;
        }
        else if(target.x < 0)
        {
            _move.x = -1f;
        }

        if(target.z > 0)
        {
            _move.y = 1f;
        }
        else if(target.z < 0)
        {
            _move.y = -1f;
        }

        return _move * moveDirSpeed;
    }

    // 非接触時相手の胸元方向に移動する関数
    private void SetCollisionMove(float rightDir, float frontDir)
    {
        this.transform.Translate(
            Time.deltaTime * rightDir * moveSpeedMagNum * moveLRDisMagNum * speedMagNum,
            0f, 
            Time.deltaTime * frontDir * moveSpeedMagNum * moveFBDisMagNum * speedMagNum,
            Space.World
        );
    }
    #endregion

    #region Resetに関するスクリプト
    // Resetを可能にする関数
    public void SetResetOK()
    {
        isReplay = true;
    }

    // 再度遊ぶ際に状態をResetする関数
    public void SetReset()
    {
        gameStart = false;
        playerModeDecide = false;
        levelModeDecide = false;
        weightInput = false;
        isStartPush = false;
        isTachiaiMove = false;
        isTachiaiEnd = false;
        isEnd = false;
        isFallDown = false;
        isOutDohyo = false;
        isReplay = false;
        this.transform.position = thisInitialPos;
        this.transform.rotation = thisInitialRot;
        playerObj.transform.localPosition = playerInitialPos;
        playerObj.transform.localRotation = playerInitialRot;
        playerObj.transform.localScale = playerInitialScale;
        lsObj.transform.localRotation = lsInitialRot;
        rsObj.transform.localRotation = rsInitialRot;
        SetHandRot();
        startPushTime = 0;
        penaltyNum = 0;
        pushTimeLag = 0;
        lFFBNum = 0;
        lFLRNum = 0;
        rFFBNum = 0;
        rFLRNum = 0;
        graFBNum = 0;
        graLRNum = 0;
        rikishiUI.SetGravityUI(graLRNum, graFBNum);
        dragNum = 0;
        maxAngle = 0;
        SetFootPlace();
        SetGravityPlace();
        SetSpineAngle();
        SetPlayStyle(PlayStyle.Yothu);
    }
    #endregion

    // 重心値の表示を行う関数
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere (playerObj.transform.position + playerObj.transform.rotation * gravityPlace, 0.1f);
        // Gizmos.DrawSphere (playerObj.transform.position + playerObj.transform.rotation * (gravityPlace / playerScale), 0.1f);
    }
}