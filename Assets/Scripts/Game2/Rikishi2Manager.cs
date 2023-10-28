using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikishi2Manager : MonoBehaviour
{
    #region 変数宣言
    #region プレイヤーの攻撃状態の変数
    private enum PlayStyle
    {
        Yothu = 1,
        Mawashi = 2,
        Oshi = 3,
        Hataki = 4
    };
    private PlayStyle playStyle;  // 現在の攻撃状態
    #endregion

    #region 参照に関する変数
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
    [SerializeField] private GameObject leObj;  // 左肘のオブジェクト
    [SerializeField] private GameObject reObj;  // 右肘のオブジェクト
    [SerializeField] private GameObject lhPosObj;  // 左手の座標オブジェクト（localScale用）
    [SerializeField] private GameObject rhPosObj;  // 右手の座標オブジェクト（localScale用）
    [SerializeField] private GameObject lhObj;  // 左手のオブジェクト
    [SerializeField] private GameObject rhObj;  // 右手のオブジェクト
    [SerializeField] private GameObject lhColliderObj;  // 左手の当たり判定のオブジェクト
    [SerializeField] private GameObject rhColliderObj;  // 右手の当たり判定のオブジェクト
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
    [SerializeField] private GameObject lWholeLObj;  // 左足全体(当たり判定込み)のオブジェクト
    [SerializeField] private GameObject rWholeLObj;  // 右足全体(当たり判定込み)のオブジェクト
    [SerializeField] private GameObject lLObj;  // 左足全体のオブジェクト
    [SerializeField] private GameObject rLObj;  // 右足全体のオブジェクト
    [SerializeField] private GameObject lKObj;  // 左膝のオブジェクト
    [SerializeField] private GameObject rKObj;  // 右膝のオブジェクト
    [SerializeField] private GameObject lFObj;  // 左足のオブジェクト
    [SerializeField] private GameObject rFObj;  // 右足のオブジェクト
    [SerializeField] private GameObject viewObj;  // 視線ベクトルオブジェクト
    [SerializeField] private GameObject lOFObj;  // 左足外前座標オブジェクト
    [SerializeField] private GameObject lOBObj;  // 左足外後座標オブジェクト
    [SerializeField] private GameObject lIFObj;  // 左足内前座標オブジェクト
    [SerializeField] private GameObject lIBObj;  // 左足内後座標オブジェクト
    [SerializeField] private GameObject rOFObj;  // 右足外前座標オブジェクト
    [SerializeField] private GameObject rOBObj;  // 右足外後座標オブジェクト
    [SerializeField] private GameObject rIFObj;  // 右足内前座標オブジェクト
    [SerializeField] private GameObject rIBObj;  // 右足内後座標オブジェクト
    #endregion
    #region 基本情報に関する変数
    [Header("基本情報")]
    public int playerNum;  // プレイヤーナンバー
    [SerializeField] private float lossyScaleNum;  // プレイヤーオブジェクトの全体の大きさ
    [SerializeField] private float localScaleNum;  // プレイヤーオブジェクトのローカルの大きさ
    [SerializeField] private float scaleYNum;  // プレイヤーオブジェクトの身長の大きさ
    [SerializeField] private Vector3 scaleVector;  // プレイヤーオブジェクトの大きさVector
    [SerializeField] private float footLengNum;  // 足の長さの値
    private Rigidbody rb;
    #endregion
    #region 座標に関する変数
    [Header("足や座標の情報")]
    [SerializeField] private Vector3 lf;  // 左足のワールド座標
    [SerializeField] private Vector3 rf;  // 右足のワールド座標
    [SerializeField] private Vector2 footInidis;  // 左右の足の初期距離(xが横方向、yが縦方向)
    [SerializeField] private Vector2 footDis;  // 左右の足の距離(xが横方向、yが縦方向)
    [SerializeField] private float enemyDis;  // 敵プレイヤーとの距離
    private float hatakiNotMoveMax = 1.8f;  // はたきで動かずに重心移動する最大距離
    private float hatakiMax = 1.9f;  // はたきができる最大距離
    private float attackMax = 1.1f;  // はたき以外の攻撃ができる最大距離
    [SerializeField] private float dohyoLDis;  // 土俵の中心とプレイヤーの左足との距離
    [SerializeField] private float dohyoRDis;  // 土俵の中心とプレイヤーの左足との距離
    private float dohyoRadius = 4.85f;  // 土俵の半径
    #endregion
    #region 立会いに関する変数
    [Header("立会い")]
    [SerializeField] private float startPushTime = 0f;  // 立会いのボタンを押す時間
    [SerializeField] private int penaltyNum = 0;  // 立会いの反則回数
    [SerializeField] private float pushTimeLag = 0f;  // 立会いのボタンを押した時間差
    private float pushMaxLag = 6f;  // 立会いの最大時間差
    [SerializeField] private Vector3 startPos;  // プレイヤーの立会いによる開始座標
    private float startPosXSlope = 0.55f;  // 立会い開始X座標の傾き
    private float startPosXIntercept = 0.45f;  // 立会い開始X座標の切片
    private float tachiaiSpeedMag = 3f;  // 立会いのスピード倍率
    #endregion
    #region 抵抗に関する変数
    [Header("抵抗")]
    [SerializeField] private float dragNum = 0f;  // 倒れる際の抵抗値
    [SerializeField] private float maxAngle;  // 倒れている時の最大角度
    private float angleMagNum = 20f;  // 角度による抵抗値減速係数
    [SerializeField] private float minusPerSecond;  // 抵抗値の減る秒速値
    #endregion
    #region 体重に関する変数
    [Header("体重")]
    [SerializeField] private float weightNum;  // 体重の数値
    [SerializeField] private float weightMin = 80f;  // 体重の最小値
    [SerializeField] private float weightMax = 220f;  // 体重の最大値
    [SerializeField] private float powerMagNum;  // 体重パワーの倍率
    [SerializeField] private float speedMagNum;  // 移動スピード倍率
    #endregion
    #region 入力値に関する変数
    [Header("足入力値")]
    [SerializeField] private float lFFBNum = 0f;  // 左足前後方の値（前方と後方の最大値:5）
    [SerializeField] private float lFLRNum = 0f;  // 左足左右の値（左と右の最大値:5）
    [SerializeField] private float rFFBNum = 0f;  // 右足前後方の値（前方と後方の最大値:5）
    [SerializeField] private float rFLRNum = 0f;  // 右足左右の値（左と右の最大値:5）
    private float notMoveMagNum = 0.95f;  // 重心移動のみの入力倍率
    private float footMax = 5f;  // 足の値の最大値
    private float clossMax = 2f;  // クロスの時の最大値
    #endregion
    #region 移動に関する変数
    [Header("移動倍率数値")]
    [SerializeField] private float moveLRDisMagNum;  // 左右移動距離の倍率数値
    [SerializeField] private float moveFBDisMagNum;  // 前後移動距離の倍率数値
    float moveFBGraMagNum = 0.009f;  // 重心前後移動の倍率数値
    private float moveLRGraMagNum = 0.02f;  // 重心左右移動の倍率数値
    // private float graFBMagSlope;  // 重心前後移動の倍率の傾き
    // private float graFBMagIntercept;  // 重心前後移動の倍率の切片
    // private float graLRMagSlope;  // 重心左右移動の倍率の傾き
    // private float graLRMagIntercept = 0f;  // 重心左右移動の倍率の切片
    private float moveSpeedMagNum = 5f;  // 移動スピードの倍率数値
    #endregion
    #region 重心に関する変数
    [Header("重心")]
    public float graFBNum = 0f;  // 前後方重心の値（重心耐久の最大値:15）
    public float graLRNum = 0f;  // 左右方重心の値（重心耐久の最大値:15）
    public float graMax = 15f;  // 重心耐久の最大値
    [SerializeField] private float playStyleMagNum;  // 攻撃状態に応じた重心移動倍率
    private float wholeY;  // 全身のワールドY座標
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
    #endregion
    #region 角度に関する変数
    [Header("角度計算")]
    [SerializeField] private float angleY;  // 全身のY方向角度
    [SerializeField] private float enemyAngleY;  // 相手の全身のY方向角度
    [SerializeField] private Vector3 viewPos;  // 視線の空オブジェクトの座標
    [SerializeField] private Vector3 viewDir;  // 視線方向のベクトル
    [SerializeField] private float angularDif = 0;  // 相手の方向と自身の向きの角度差
    [SerializeField] private float angDifAbs = 0;  // 相手方向と自身の向きの角度差の絶対値
    private float spineFBSlope = 3f;  // 上半身の前後の角度の傾き
    private float spineFBIntercept = 10f;  // 上半身の前後の角度の切片
    private float spineLRSlope = -3f;  // 上半身の左右の角度の傾き
    private float spineLRIntercept = 0f;  // 上半身の左右の角度の切片
    private float sXSlope;  // 肩のX軸の角度の傾き
    private float sXIntercept;  // 肩のX軸の角度の切片
    private float sYSlope;  // 肩のY軸の角度の傾き
    private float sYIntercept;  // 肩のY軸の角度の切片
    private float sZSlope;  // 肩のZ軸の角度の傾き
    private float sZIntercept;  // 肩のZ軸の角度の切片
    private float eXScaleSlope;  // 肘のX軸のScaleの傾き
    private float eXScaleIntercept;  // 肘のX軸のScaleの切片
    private float hXSlope;  // 手のX軸の角度の傾き
    private float hXIntercept;  // 手のX軸の角度の切片
    private float hYSlope;  // 手のY軸の角度の傾き
    private float hYIntercept;  // 手のY軸の角度の切片
    private float hZSlope;  // 手のZ軸の角度の傾き
    private float hZIntercept;  // 手のZ軸の角度の切片
    #endregion
    #region bool変数
    [Header("bool判定")]
    [SerializeField] private bool gameStart = false;  // ゲーム開始ボタンを押したか否か
    [SerializeField] private bool playStart = false;  // 操作方法の決定ボタンを押したか否か
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
    #endregion
    #region コンピュータ対戦の際の変数
    [Header("コンピュータ対戦")]
    [SerializeField] float myGraFBPer;  // 自身の前後重心の状態割合
    [SerializeField] float myGraLRPer;  // 自身の左右重心の状態割合
    [SerializeField] float eneGraFBPer;  // 相手の前後重心の状態割合
    [SerializeField] float eneGraLRPer;  // 相手の左右重心の状態割合
    [SerializeField] float angDifPer;  // 相手の方向と自身の向きの角度差の状態割合
    [SerializeField] float dohyoLDisPer;  // 土俵の中心と左足との距離の状態割合
    [SerializeField] float dohyoRDisPer;  // 土俵の中心と右足との距離の状態割合
    private float cpuPushTime = 1f;  // コンピュータの立会いのボタンを押す時間
    private float stateWaitTime = 0.1f;  // コンピュータのStateの変更待機時間
    private enum CpuState
    {
        State,  // 初期状態：何も入力がない状態
        MyGraMove,  // 自身の重心移動の入力
        EneGraMove,  // 相手の重心移動の入力
        LFMove,  // 左足の移動の入力
        RFMove,  // 右足の移動の入力
        MyRotate,  // 自転の入力
        EneRotate  // 公転の入力
    };
    private CpuState cpuState;  // コンピュータの現在の入力状態
    private CpuState cpuNextState;  // コンピュータの次の入力状態
    #endregion
    #region 自動移動に関する変数
    [Header("自動移動計算")]
    [SerializeField] private Vector3 target;  // 相手の胸元方向へのベクトル
    [SerializeField] private Vector2 moveDir;  // 相手の胸元方向への単位ベクトル
    private float moveDirSpeed = 0.375f;  // 相手の胸元方向への単位ベクトルの倍率
    [SerializeField] private Vector3 nowPos;  // 自身の座標
    [SerializeField] private Vector3 enemyPos;  // 相手の座標
    #endregion
    #region 初期値を保存する変数
    [Header("初期情報")]
    [SerializeField] private Vector3 thisInitialPos;  // プレイヤー全体の初期座標
    [SerializeField] private Quaternion thisInitialRot;  // プレイヤー全体の初期角度
    [SerializeField] private Vector3 playerInitialPos;  // プレイヤーオブジェクトの初期座標
    [SerializeField] private Quaternion playerInitialRot;  // プレイヤーオブジェクトの初期角度
    [SerializeField] private Quaternion lsInitialRot;  // 左肩オブジェクトの初期角度
    [SerializeField] private Quaternion rsInitialRot;  // 右肩オブジェクトの初期角度
    [SerializeField] private Quaternion leInitialRot;  // 左肘オブジェクトの初期角度
    [SerializeField] private Quaternion reInitialRot;  // 右肘オブジェクトの初期角度
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
    [SerializeField] private Vector3 lLInitialPos;  // 左足全体の初期ローカル座標
    [SerializeField] private Vector3 rLInitialPos;  // 右足全体の初期ローカル座標
    [SerializeField] private Quaternion lLInitialRot;  // 左足全体のオブジェクトの初期角度
    [SerializeField] private Quaternion rLInitialRot;  // 右足全体のオブジェクトの初期角度
    [SerializeField] private Quaternion lKInitialRot;  // 左膝オブジェクトの初期角度
    [SerializeField] private Quaternion rKInitialRot;  // 右膝オブジェクトの初期角度
    [SerializeField] private Quaternion lFInitialRot;  // 左足オブジェクトの初期角度
    [SerializeField] private Quaternion rFInitialRot;  // 右足オブジェクトの初期角度
    [SerializeField] private Vector3 playerInitialScale;  // プレイヤーオブジェクトの初期スケール
    [SerializeField] private Vector3 leInitialScale;  // 左肘の初期スケール
    [SerializeField] private Vector3 reInitialScale;  // 右肘の初期スケール
    [SerializeField] private Vector3 lhInitialScale;  // 左手の初期スケール
    [SerializeField] private Vector3 rhInitialScale;  // 右手の初期スケール
    #endregion
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
                #region タイトルスタート
                if(!gameStart)
                {
                    if(Input.GetButtonDown("Decide1"))
                    {
                        gameStart = true;
                        Game2Manager.Instance.PushGameStart();
                    }
                }
                #endregion
                #region 操作方法
                else
                {
                    if(!playStart)
                    {
                        if(Input.GetButtonDown("Decide1"))
                        {
                            playStart = true;
                            Game2Manager.Instance.PushPlayStart();
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
                                        switch(Game2Manager.Instance.gamePlayer)
                                        {
                                            case Game2Manager.GamePlayer.One:
                                                rikishiUI.SetWeightInput();
                                                break;
                                            case Game2Manager.GamePlayer.Two:
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
                                        break;
                                }
                            }
                #endregion
                #region 立会いペナルティ入力
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
                            switch(Game2Manager.Instance.gamePlayer)
                            {
                                case Game2Manager.GamePlayer.One:
                                    SetCpuTachiaiInput();
                                    break;
                                case Game2Manager.GamePlayer.Two:
                                    if(Input.GetButtonDown("Decide2"))
                                    {
                                        TachiaiInput();
                                        rikishiUI.SetTachiaiInput();
                                    }
                                    break;
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
                    SetEnemyRot();
                    SetFootInput();
                    SetShoulderRot();
                    SetEnemyDis();
                    SetGravityNum(right1 + right2, front1 + front2);
                    SetGravityPlace();
                    rikishiUI.SetGravityUI(graLRNum, graFBNum);
                    rikishiUI.SetArrowSprite((int)playStyle, angDifAbs, isAttack, isHataki);
                    SetGraPanelNum();
                    SetSpineRot();
                    SetHandCollider();
                    SetInDohyo();
                    SetRayCast();
                    maxAngle = SetMaxAngle();
                    SetDragNum(0, maxAngle);

                    switch(playerNum)
                    {
                        #region プレイヤー1の入力
                        case 1:
                            if(Input.GetAxis("LeftHorizontal1") != 0f || Input.GetAxis("LeftVertical1") != 0f)
                            {
                                SetEnemyGraInput(Input.GetAxis("LeftHorizontal1") * notMoveMagNum, Input.GetAxis("LeftVertical1") * notMoveMagNum);
                            }

                            if((Input.GetAxis("RightHorizontal1") != 0f || Input.GetAxis("RightVertical1") != 0f) && 
                                Input.GetAxis("MoveFoot1") == 0f
                                )
                            {
                                SetOwnGravity(2, Input.GetAxis("RightHorizontal1") * notMoveMagNum, Input.GetAxis("RightVertical1") * notMoveMagNum);
                            }

                            if(Input.GetAxis("MoveFoot1") < 0f &&
                                (Input.GetAxis("RightHorizontal1") != 0f || Input.GetAxis("RightVertical1") != 0f)
                                )
                            {
                                SetLeftFootNum(Input.GetAxis("RightHorizontal1") * speedMagNum, Input.GetAxis("RightVertical1") * speedMagNum);
                            }

                            if(Input.GetAxis("MoveFoot1") > 0f &&
                                (Input.GetAxis("RightHorizontal1") != 0f || Input.GetAxis("RightVertical1") != 0f)
                                )
                            {
                                SetRightFootNum(Input.GetAxis("RightHorizontal1") * speedMagNum, Input.GetAxis("RightVertical1") * speedMagNum);
                            }

                            if(Input.GetAxis("RightHorizontal1") == 0f && Input.GetAxis("RightVertical1") == 0f)
                            {
                                SetLeftFootNum(0, 0);
                                SetRightFootNum(0, 0);
                            }

                            if(Input.GetAxis("Rotation1") != 0f)
                            {
                                SetWholeRot(enemy.gameObject, -Input.GetAxis("Rotation1") * speedMagNum);
                            }

                            if(Input.GetAxis("MyRotation1") != 0f)
                            {
                                SetWholeRot(this.gameObject, Input.GetAxis("MyRotation1") * speedMagNum);
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
                                // SetCollisionMove(moveDir.x * speedMagNum, moveDir.y * speedMagNum);
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
                            switch(Game2Manager.Instance.gamePlayer)
                            {
                                case Game2Manager.GamePlayer.One:
                                    SetStatePercent();
                                    switch(cpuState)
                                    {
                                        case CpuState.State:
                                            StateUpdate();
                                            break;
                                        case CpuState.MyGraMove:
                                            MyGraMoveUpdate();
                                            break;
                                        case CpuState.EneGraMove:
                                            EneGraMoveUpdate();
                                            break;
                                        case CpuState.LFMove:
                                            LFMoveUpdate();
                                            break;
                                        case CpuState.RFMove:
                                            RFMoveUpdate();
                                            break;
                                        case CpuState.MyRotate:
                                            MyRotateUpdate();
                                            break;
                                        case CpuState.EneRotate:
                                            EneRotateUpdate();
                                            break;
                                    }

                                    if(cpuState != cpuNextState)
                                    {
                                        switch(cpuState)
                                        {
                                            case CpuState.State:
                                                StateEnd();
                                                break;
                                            case CpuState.MyGraMove:
                                                MyGraMoveEnd();
                                                break;
                                            case CpuState.EneGraMove:
                                                EneGraMoveEnd();
                                                break;
                                            case CpuState.LFMove:
                                                LFMoveEnd();
                                                break;
                                            case CpuState.RFMove:
                                                RFMoveEnd();
                                                break;
                                            case CpuState.MyRotate:
                                                MyRotateEnd();
                                                break;
                                            case CpuState.EneRotate:
                                                EneRotateEnd();
                                                break;
                                        }
                                        cpuState = cpuNextState;
                                    }
                                    break;
                                case Game2Manager.GamePlayer.Two:
                                    if(Input.GetAxis("LeftHorizontal2") != 0f || Input.GetAxis("LeftVertical2") != 0f)
                                    {
                                        SetEnemyGraInput(Input.GetAxis("LeftHorizontal2") * notMoveMagNum, Input.GetAxis("LeftVertical2") * notMoveMagNum);
                                    }

                                    if((Input.GetAxis("RightHorizontal2") != 0f || Input.GetAxis("RightVertical2") != 0f) && 
                                        Input.GetAxis("MoveFoot2") == 0f
                                        )
                                    {
                                        SetOwnGravity(2, Input.GetAxis("RightHorizontal2") * notMoveMagNum, Input.GetAxis("RightVertical2") * notMoveMagNum);
                                    }

                                    if(Input.GetAxis("MoveFoot2") < 0f &&
                                        (Input.GetAxis("RightHorizontal2") != 0f || Input.GetAxis("RightVertical2") != 0f)
                                        )
                                    {
                                        SetLeftFootNum(Input.GetAxis("RightHorizontal2") * speedMagNum, Input.GetAxis("RightVertical2") * speedMagNum);
                                    }

                                    if(Input.GetAxis("MoveFoot2") > 0f &&
                                        (Input.GetAxis("RightHorizontal2") != 0f || Input.GetAxis("RightVertical2") != 0f)
                                        )
                                    {
                                        SetRightFootNum(Input.GetAxis("RightHorizontal2") * speedMagNum, Input.GetAxis("RightVertical2") * speedMagNum);
                                    }

                                    if(Input.GetAxis("RightHorizontal2") == 0f && Input.GetAxis("RightVertical2") == 0f)
                                    {
                                        SetLeftFootNum(0, 0);
                                        SetRightFootNum(0, 0);
                                    }

                                    if(Input.GetAxis("Rotation2") != 0f)
                                    {
                                        SetWholeRot(enemy.gameObject, -Input.GetAxis("Rotation2") * speedMagNum);
                                    }

                                    if(Input.GetAxis("MyRotation2") != 0f)
                                    {
                                        SetWholeRot(this.gameObject, Input.GetAxis("MyRotation2") * speedMagNum);
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
                                        // SetCollisionMove(moveDir.x * speedMagNum, moveDir.y * speedMagNum);
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

        lf = lWholeLObj.transform.position;
        rf = rWholeLObj.transform.position;
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
        // scaleVector = playerObj.transform.localScale;
        // scaleVector.y = scaleYNum;
        // scaleVector.x = localScaleNum;
        // scaleVector.z = localScaleNum;
        // playerObj.transform.localScale = scaleVector;
        lossyScaleNum = playerObj.transform.lossyScale.x;
        SetMoveDisMagNum();
        // SetMoveGraMagNum();
    }

    // 移動距離倍率の数値の計算を行う関数
    private void SetMoveDisMagNum()
    {
        footInidis = new Vector2(Mathf.Abs(lLInitialPos.x - rLInitialPos.x), Mathf.Abs(lLInitialPos.z - rLInitialPos.z)) * lossyScaleNum;
        // 足の長さに応じて移動距離を変更（例:長さ90⇒左右120,前後75：自分）
        moveLRDisMagNum = (((120f / 90f) * footLengNum) - footInidis.x) / (footMax * lossyScaleNum * 2f);
        moveFBDisMagNum = (((75f / 90f) * footLengNum) - footInidis.y) / (footMax * lossyScaleNum * 2f);
    }

    // 重心移動倍率の数値の計算を行う関数
    // private void SetMoveGraMagNum()
    // {
    //     switch(Game2Manager.Instance.gameMode)
    //     {
    //         case Game2Manager.GameMode.Easy:
    //             break;
    //         case Game2Manager.GameMode.Normal:
    //             graLRMagSlope = 0.2f / graMax;
    //             if(localScaleNum < 1.5f)
    //             {
    //                 if(graFBNum < 0f)
    //                 {
    //                     graFBMagSlope = 0.22f / graMax;
    //                     graFBMagIntercept = -0.15f / graMax;
    //                 }
    //                 else
    //                 {
    //                     graFBMagSlope = 0.19f / graMax;
    //                     graFBMagIntercept = -0.11f / graMax;
    //                 }
    //             }
    //             else
    //             {
    //                 if(graFBNum < 0f)
    //                 {
    //                     graFBMagSlope = 0.14f / graMax;
    //                     graFBMagIntercept = -0.03f / graMax;
    //                 }
    //                 else
    //                 {
    //                     graFBMagSlope = 0.1f / graMax;
    //                     graFBMagIntercept = 0.025f / graMax;
    //                 }
    //             }
    //             break;
    //     }
        
    //     moveFBGraMagNum = graFBMagSlope * localScaleNum + graFBMagIntercept;
    //     moveLRGraMagNum = graLRMagSlope * localScaleNum + graLRMagIntercept;
    // }

    // 初期状態の記録
    private void SetInitialNum()
    {
        playerInitialScale = playerObj.transform.localScale;
        thisInitialPos = this.transform.position;
        thisInitialRot = this.transform.rotation;
        playerInitialPos = playerObj.transform.localPosition;
        playerInitialRot = playerObj.transform.localRotation;
        lLInitialPos = lWholeLObj.transform.localPosition;
        rLInitialPos = rWholeLObj.transform.localPosition;
        lsInitialRot = lsObj.transform.localRotation;
        rsInitialRot = rsObj.transform.localRotation;
        leInitialRot = leObj.transform.localRotation;
        reInitialRot = reObj.transform.localRotation;
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
        lLInitialRot = lLObj.transform.localRotation;
        rLInitialRot = rLObj.transform.localRotation;
        lKInitialRot = lKObj.transform.localRotation;
        rKInitialRot = rKObj.transform.localRotation;
        lFInitialRot = lFObj.transform.localRotation;
        rFInitialRot = rFObj.transform.localRotation;
        leInitialScale = leObj.transform.localScale;
        reInitialScale = reObj.transform.localScale;
        lhInitialScale = lhPosObj.transform.localScale;
        rhInitialScale = rhPosObj.transform.localScale;
        scaleYNum = playerObj.transform.localScale.y;
        footLengNum = lWholeLObj.transform.position.y;
        wholeY = wholeObj.transform.position.y;
        center = new Vector3(0f, wholeY, -0.03f);
    }
    #endregion

    #region コンピュータ対戦に関するスクリプト
    // コンピュータの立会いの入力に関するスクリプト
    private void SetCpuTachiaiInput()
    {
        if(startPushTime > cpuPushTime)
        {
            TachiaiInput();
        }
    }

    // コンピュータの状態遷移を行うスクリプト
    private void SetCpuState(CpuState nextState)
    {
        cpuNextState = nextState;
    }

    // 現在の状態割合の計算を行う関数
    private void SetStatePercent()
    {
        myGraFBPer = Mathf.Abs(graFBNum) / graMax;
        myGraLRPer = Mathf.Abs(graLRNum) / graMax;
        eneGraFBPer = Mathf.Abs(graMax - enemy.graFBNum) / graMax;
        eneGraLRPer = Mathf.Abs(graMax - enemy.graLRNum) / graMax;
        angDifPer = angDifAbs / 90f;
        dohyoLDisPer = dohyoLDis / dohyoRadius;
        dohyoRDisPer = dohyoRDis / dohyoRadius;
    }

    #region State状態の処理
    // CpuState.StateのUpdate処理
    private void StateUpdate()
    {
        
    }

    // CpuState.Stateの終了処理
    private void StateEnd()
    {
        
    }
    #endregion
    #region MyGraMove状態の処理
    // CpuState.MyGraMoveのUpdate処理
    private void MyGraMoveUpdate()
    {
        
    }

    // CpuState.MyGraMoveの終了処理
    private void MyGraMoveEnd()
    {
        
    }
    #endregion
    #region EneGraMove状態の処理
    // CpuState.EneGraMoveのUpdate処理
    private void EneGraMoveUpdate()
    {
        
    }

    // CpuState.EneGraMoveの終了処理
    private void EneGraMoveEnd()
    {
        
    }
    #endregion
    #region LFMove状態の処理
    // CpuState.LFMoveのUpdate処理
    private void LFMoveUpdate()
    {
        
    }

    // CpuState.LFMoveの終了処理
    private void LFMoveEnd()
    {
        
    }
    #endregion
    #region RFMove状態の処理
    // CpuState.RFMoveのUpdate処理
    private void RFMoveUpdate()
    {
        
    }

    // CpuState.RFMoveの終了処理
    private void RFMoveEnd()
    {
        
    }
    #endregion
    #region MyRotate状態の処理
    // CpuState.MyRotateのUpdate処理
    private void MyRotateUpdate()
    {
        
    }

    // CpuState.MyRotateの終了処理
    private void MyRotateEnd()
    {
        
    }
    #endregion
    #region EneRotate状態の処理
    // CpuState.EneRotateのUpdate処理
    private void EneRotateUpdate()
    {
        
    }

    // CpuState.EneRotateの終了処理
    private void EneRotateEnd()
    {
        
    }
    #endregion
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
        SetPlayStyleMagNum();
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
            rFLRNum -= Time.deltaTime * rightPosi * moveSpeedMagNum;
            lFLRNum += Time.deltaTime * rightPosi * moveSpeedMagNum;
            myLFLRPos += rightPosi;
            if((graLRNum < 0 && rightPosi < 0) || (graLRNum > 0 && rightPosi > 0))
            {
                myLFLRGra += -rightPosi;
            }
            if(angDifAbs <= 120f)
            {
                if((playStyle == PlayStyle.Hataki && enemyDis < hatakiNotMoveMax) || (playStyle != PlayStyle.Hataki && isAttack && !isCollision))
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
            rFFBNum -= Time.deltaTime * frontPosi * moveSpeedMagNum;
            lFFBNum += Time.deltaTime * frontPosi * moveSpeedMagNum;
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
                if((playStyle == PlayStyle.Hataki && enemyDis < hatakiNotMoveMax) || (playStyle != PlayStyle.Hataki && isAttack && !isCollision))
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
            SetLeftLegRot(0);
        }
        else
        {
            lFOpeInput = true;
            SetOwnGravity(3, myLFLRGra, myLFFBGra);
            SetPlayerPos(myLFLRPos, myLFFBPos);
            SetFootPlace();
            SetLeftLegRot(1);
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
            rFLRNum += Time.deltaTime * rightPosi * moveSpeedMagNum;
            lFLRNum -= Time.deltaTime * rightPosi * moveSpeedMagNum;
            rFLRInput = true;
            myRFLRPos += rightPosi;
            if((graLRNum < 0 && rightPosi < 0) || (graLRNum > 0 && rightPosi > 0))
            {
                myRFLRGra += -rightPosi;
            }
            if(angDifAbs <= 120f)
            {
                if((playStyle == PlayStyle.Hataki && enemyDis < hatakiNotMoveMax) || (playStyle != PlayStyle.Hataki && isAttack && !isCollision))
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
            rFFBNum += Time.deltaTime * frontPosi * moveSpeedMagNum;
            lFFBNum -= Time.deltaTime * frontPosi * moveSpeedMagNum;
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
                if((playStyle == PlayStyle.Hataki && enemyDis < hatakiNotMoveMax) || (playStyle != PlayStyle.Hataki && isAttack && !isCollision))
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
            SetRightLegRot(0);
        }
        else
        {
            rFOpeInput = true;
            SetOwnGravity(4, myRFLRGra, myRFFBGra);
            SetPlayerPos(myRFLRPos, myRFFBPos);
            SetFootPlace();
            SetRightLegRot(1);
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
            (attackLLR + attackMoveLLR + attackMoveRLR) * powerMagNum * playStyleMagNum,
            (attackLFB + attackMoveLFB + attackMoveRFB) * powerMagNum * playStyleMagNum
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

    // プレイ状態に応じて重心変化倍率を変更する関数
    private void SetPlayStyleMagNum()
    {
        switch(playStyle)
        {
            case PlayStyle.Yothu:
                playStyleMagNum = 1f;
                break;
            case PlayStyle.Mawashi:
                playStyleMagNum = 1f;
                break;
            case PlayStyle.Oshi:
                playStyleMagNum = 0.2f * localScaleNum + 0.7f;
                break;
            case PlayStyle.Hataki:
                playStyleMagNum = -0.2f * localScaleNum + 1.3f;
                break;
        }
    }

    // 重心値の変化を行う関数
    private void SetGravityNum(float rightPosi, float frontPosi)
    {
        graFBNum += Time.deltaTime * frontPosi * moveSpeedMagNum;
        graLRNum += Time.deltaTime * rightPosi * moveSpeedMagNum;
        // SetMoveGraMagNum();
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
    #endregion

    #region オブジェクトの座標に関するスクリプト
    // 敵との距離を測る関数
    public void SetEnemyDis()
    {
        Vector2 playerPlace =  new Vector2(this.transform.position.x, this.transform.position.z);
        Vector2 enemyPlace =  new Vector2(enemy.gameObject.transform.position.x, enemy.gameObject.transform.position.z);
        enemyDis = Vector2.Distance(playerPlace, enemyPlace);
        if(enemyDis <= attackMax)
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
        if(enemyDis <= hatakiMax)
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
            Time.deltaTime * rightPosi * moveSpeedMagNum * moveLRDisMagNum * lossyScaleNum,
            0f,
            Time.deltaTime * frontPosi * moveSpeedMagNum * moveFBDisMagNum * lossyScaleNum
        );
    }

    // 足の位置の変更を行う関数
    private void SetFootPlace()
    {
        lWholeLObj.transform.localPosition = 
            new Vector3
            (
                lFLRNum * moveLRDisMagNum + lLInitialPos.x,
                lLInitialPos.y,
                lFFBNum * moveFBDisMagNum + lLInitialPos.z
            );

        rWholeLObj.transform.localPosition = 
            new Vector3
            (
                rFLRNum * moveLRDisMagNum + rLInitialPos.x,
                rLInitialPos.y,
                rFFBNum * moveFBDisMagNum + rLInitialPos.z
            );

        rikishiUI.SetFootOpeUIPlace(lWholeLObj.transform.position, rWholeLObj.transform.position);
    }
    #endregion

    #region オブジェクトの角度に関するスクリプト  
    // 相手の向きとの角度計算を行う関数
    private void SetEnemyRot()
    {
        angularDif = Vector3.SignedAngle(target, viewDir, Vector3.up);
        angDifAbs = Mathf.Abs(angularDif);
    }

    // 上半身の角度の変更を行う関数
    private void SetSpineRot()
    {
        spineObj.transform.localEulerAngles = new Vector3(
            spineFBSlope * graFBNum + spineFBIntercept,
            0,
            spineLRSlope * graLRNum + spineLRIntercept
        );
    }

    // 全身の角度の回転を行う関数
    private void SetWholeRot(GameObject target, float rotateSpeed)
    {
        this.transform.RotateAround
        (
            target.transform.position,
            Vector3.up,
            Time.deltaTime * rotateSpeed * moveSpeedMagNum * moveSpeedMagNum
        );
    }

    // プレイスタイルに応じて肩の角度や大きさの係数を求める関数
    private void SetShoulderRot()
    {
        switch(playStyle)
        {
            case PlayStyle.Yothu:
                if(angDifAbs <= 60)
                {
                    if(-graMax < graFBNum && graFBNum < 0f)
                    {
                        sXSlope = -2.3222f;
                        sXIntercept = -261.569f;
                        sYSlope = -0.1938f;
                        sYIntercept = 24.242f;
                        sZSlope = 0.627f;
                        sZIntercept = -78.114f;
                        eXScaleSlope = -0.04f;
                        eXScaleIntercept = 0.8f;
                    }
                    else if(graFBNum < 7.5f)
                    {
                        sXSlope = 0.8016f;
                        sXIntercept = -261.569f;
                        sYSlope = 18.6952f;
                        sYIntercept = 24.242f;
                        sZSlope = 19.8164f;
                        sZIntercept = -78.114f;
                        eXScaleSlope = -0.068f;
                        eXScaleIntercept = 0.8f;
                    }
                    else if(graFBNum < graMax)
                    {
                        sXSlope = 3.9496f;
                        sXIntercept = -285.179f;
                        sYSlope = 3.0304f;
                        sYIntercept = 141.728f;
                        sZSlope = 3.9244f;
                        sZIntercept = 41.076f;
                        eXScaleSlope = -0.036f;
                        eXScaleIntercept = 0.56f;
                    }
                }
                if(enemyDis <= attackMax)
                {
                    SetHandRot(1);
                    SetFingerRot(1);
                    if(angDifAbs <= 60)
                    {
                        SetShoulderRot(1);
                        SetElbowHandScale(1);
                        SetElbowRot(1);
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
                }
                else
                {
                    SetShoulderRot(0);
                    SetElbowRot(0);
                    SetElbowHandScale(0);
                    SetHandRot(0);
                    SetFingerRot(0);
                }
                break;
            case PlayStyle.Mawashi: 
                if(angDifAbs <= 60)
                {
                    if(-graMax < graFBNum && graFBNum < 0f)
                    {
                        sXSlope = -1.5444f;
                        sXIntercept = -228.113f;
                        sYSlope = 0.6172f;
                        sYIntercept = 54.203f;
                        sZSlope = 1.8372f;
                        sZIntercept = -57.373f;
                        eXScaleSlope = -0.032f;
                        eXScaleIntercept = 1.26f;
                    }
                    else if(graFBNum < 7.5f)
                    {
                        sXSlope = -0.7928f;
                        sXIntercept = -228.113f;
                        sYSlope = 2.3316f;
                        sYIntercept = 54.203f;
                        sZSlope = 3.518f;
                        sZIntercept = -57.373f;
                        eXScaleSlope = -0.048f;
                        eXScaleIntercept = 1.26f;
                    }
                    else if(graFBNum < graMax)
                    {
                        sXSlope = -1.3932f;
                        sXIntercept = -223.61f;
                        sYSlope = 4.0684f;
                        sYIntercept = 41.178f;
                        sZSlope = 5.5964f;
                        sZIntercept = -72.961f;
                        eXScaleSlope = -0.052f;
                        eXScaleIntercept = 1.29f;
                    }
                }
                if(enemyDis <= attackMax)
                {
                    SetElbowRot(1);
                    SetHandRot(2);
                    SetFingerRot(2);
                    if(angDifAbs <= 60)
                    {
                        SetShoulderRot(1);
                        SetElbowHandScale(1);
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
                }
                else
                {
                    SetShoulderRot(0);
                    SetElbowRot(0);
                    SetElbowHandScale(0);
                    SetHandRot(0);
                    SetFingerRot(0);
                }
                break;
            case PlayStyle.Oshi:
                if(angDifAbs <= 60)
                {
                    if(-graMax < graFBNum && graFBNum < 0f)
                    {
                        if(enemy.graFBNum < 0f)
                        {
                            sXSlope = -0.075f - 0.0148f * enemy.graFBNum;
                            sXIntercept = -200.532f - 0.28193333f * enemy.graFBNum;
                            sYSlope = 0.8936f + 0.00796f * enemy.graFBNum;
                            sYIntercept = 112.44f + 0.1766f * enemy.graFBNum;
                            sZSlope = 2.074f - 0.05250667f * enemy.graFBNum;
                            sZIntercept = -23.042f - 1.31346667f * enemy.graFBNum;
                            eXScaleSlope = -0.036f + 0.00026667f * enemy.graFBNum;
                            eXScaleIntercept = 0.46f - 0.016f * enemy.graFBNum;
                            hXSlope = -0.0058f - 0.02673333f * enemy.graFBNum;
                            hXIntercept = 21.582f - 0.86f * enemy.graFBNum;
                            hYSlope = 0.5728f + 0.03402667f * enemy.graFBNum;
                            hYIntercept = 36.409f + 0.592f * enemy.graFBNum;
                            hZSlope = 1.2104f + 0.11476f * enemy.graFBNum;
                            hZIntercept = 104.666f + 4.18853333f * enemy.graFBNum;
                        }
                        else
                        {
                            sXSlope = -0.075f - 0.02312f * enemy.graFBNum;
                            sXIntercept = -200.532f - 0.2002f * enemy.graFBNum;
                            sYSlope = 0.8936f + 0.01896f * enemy.graFBNum;
                            sYIntercept = 112.44f + 0.1068f * enemy.graFBNum;
                            sZSlope = 2.074f + 0.07172f * enemy.graFBNum;
                            sZIntercept = -23.042f + 4.0128f * enemy.graFBNum;
                            eXScaleSlope = -0.036f - 0.0008f * enemy.graFBNum;
                            eXScaleIntercept = 0.46f - 0.032f * enemy.graFBNum;
                            hXSlope = -0.0058f + 0.05464f * enemy.graFBNum;
                            hXIntercept = 21.582f + 0.7742f * enemy.graFBNum;
                            hYSlope = 0.5728f + 0.06316f * enemy.graFBNum;
                            hYIntercept = 36.409f + 0.889f * enemy.graFBNum;
                            hZSlope = 1.2104f + 0.09584f * enemy.graFBNum;
                            hZIntercept = 104.666f - 2.9786f * enemy.graFBNum;
                        }
                    }
                    else if(graFBNum < graMax)
                    {
                        if(enemy.graFBNum < 0f)
                        {
                            sXSlope = 2.907f + 0.12428f * enemy.graFBNum;
                            sXIntercept = -200.532f - 0.28193333f * enemy.graFBNum;
                            sYSlope = 1.1368f + 0.02878667f * enemy.graFBNum;
                            sYIntercept = 112.44f + 0.1766f * enemy.graFBNum;
                            sZSlope = 2.5182f - 0.03188f * enemy.graFBNum;
                            sZIntercept = -23.042f - 1.31346667f * enemy.graFBNum;
                            eXScaleSlope = -0.042f + 0.0012f * enemy.graFBNum;
                            eXScaleIntercept = 0.46f - 0.016f * enemy.graFBNum;
                            hXSlope = -0.6956f - 0.07994667f * enemy.graFBNum;
                            hXIntercept = 21.582f - 0.86f * enemy.graFBNum;
                            hYSlope = 2.2692f + 0.14942667f * enemy.graFBNum;
                            hYIntercept = 36.409f + 0.592f * enemy.graFBNum;
                            hZSlope = 1.2534f + 0.06286667f * enemy.graFBNum;
                            hZIntercept = 104.666f + 4.18853333f * enemy.graFBNum;
                        }
                        else
                        {
                            sXSlope = 2.907f + 0.06728f * enemy.graFBNum;
                            sXIntercept = -200.532f - 0.2002f * enemy.graFBNum;
                            sYSlope = 1.1368f - 0.18148f * enemy.graFBNum;
                            sYIntercept = 112.44f + 0.1068f * enemy.graFBNum;
                            sZSlope = 2.5182f + 0.2074f * enemy.graFBNum;
                            sZIntercept = -23.042f + 4.0128f * enemy.graFBNum;
                            eXScaleSlope = -0.042f + 0.004f * enemy.graFBNum;
                            eXScaleIntercept = 0.46f - 0.032f * enemy.graFBNum;
                            hXSlope = -0.6956f - 0.139f * enemy.graFBNum;
                            hXIntercept = 21.582f + 0.7742f * enemy.graFBNum;
                            hYSlope = 2.2692f - 0.6876f * enemy.graFBNum;
                            hYIntercept = 36.409f + 0.889f * enemy.graFBNum;
                            hZSlope = 1.2534f - 0.14652f * enemy.graFBNum;
                            hZIntercept = 104.666f - 2.9786f * enemy.graFBNum;
                        }
                    }
                }
                if(enemyDis <= attackMax)
                {
                    if(angDifAbs <= 60)
                    {
                        SetShoulderRot(1);
                        SetElbowHandScale(1);
                        SetElbowRot(1);
                        SetHandRot(3);
                        SetFingerRot(3);
                    }
                    else
                    {
                        SetShoulderRot(0);
                        SetElbowRot(0);
                        SetElbowHandScale(0);
                        SetHandRot(0);
                        SetFingerRot(0);
                    }
                }
                else
                {
                    SetShoulderRot(0);
                    SetElbowRot(0);
                    SetElbowHandScale(0);
                    SetHandRot(0);
                    SetFingerRot(0);
                }
                break;
            case PlayStyle.Hataki:
                if(angDifAbs <= 60)
                {
                    if(-graMax < graFBNum && graFBNum < 0f)
                    {   
                        if(enemy.graFBNum < 0f)
                        {
                            sXSlope = 1.69324f - 0.9776f * enemyDis + (0.05083327f - 0.0443333f * enemyDis) * enemy.graFBNum;
                            sXIntercept = -198.6599f - 8.619f * enemyDis + (1.743939997f - 0.76793333f * enemyDis) * enemy.graFBNum;
                            sYSlope = 3.05488f - 0.7272f * enemyDis + (-0.035222673f - 0.00529333f * enemyDis) * enemy.graFBNum;
                            sYIntercept = 163.5251f - 26.229f * enemyDis + (1.886440003f - 0.76226667f * enemyDis) * enemy.graFBNum;
                            sZSlope = 4.251f - 0.406f * enemyDis + (-0.050290673f + 0.00642667f * enemyDis) * enemy.graFBNum;
                            sZIntercept = 75.3616f - 29.194f * enemyDis + (2.704533327f - 1.03733333f * enemyDis) * enemy.graFBNum;
                            eXScaleSlope = -0.0536f + 0.004f * enemyDis + (0.000026676f + 0.00026667f * enemyDis) * enemy.graFBNum;
                            eXScaleIntercept = -1.105f + 1.95f * enemyDis + (-0.07906667f - 0.004f * enemyDis) * enemy.graFBNum;
                        }
                        else
                        {
                            sXSlope = 1.69324f - 0.9776f * enemyDis + (0.26656f - 0.0908f * enemyDis) * enemy.graFBNum;
                            sXIntercept = -198.6599f - 8.619f * enemyDis + (0.73628f - 0.3032f * enemyDis) * enemy.graFBNum;
                            sYSlope = 3.05488f - 0.7272f * enemyDis + (0.035368f + 0.00288f * enemyDis) * enemy.graFBNum;
                            sYIntercept = 163.5251f - 26.229f * enemyDis + (2.49302f - 1.1278f * enemyDis) * enemy.graFBNum;
                            sZSlope = 4.251f - 0.406f * enemyDis + (-0.0173f + 0.0366f * enemyDis) * enemy.graFBNum;
                            sZIntercept = 75.3616f - 29.194f * enemyDis + (1.08334f - 0.4346f * enemyDis) * enemy.graFBNum;
                            eXScaleSlope = -0.0536f + 0.004f * enemyDis + (0.00144f - 0.0016f * enemyDis) * enemy.graFBNum;
                            eXScaleIntercept = -1.105f + 1.95f * enemyDis + (-0.0602f - 0.022f * enemyDis) * enemy.graFBNum;
                        }
                    }
                    else if(graFBNum < graMax)
                    {
                        if(enemy.graFBNum < 0f)
                        {
                            sXSlope = 5.21838f - 2.2122f * enemyDis + (0.195477327f - 0.07909333f * enemyDis) * enemy.graFBNum;
                            sXIntercept = -198.6599f - 8.619f * enemyDis + (1.743939997f - 0.76793333f * enemyDis) * enemy.graFBNum;
                            sYSlope = -0.45932f + 1.1748f * enemyDis + (-0.187052f + 0.07228f * enemyDis) * enemy.graFBNum;
                            sYIntercept = 163.5251f - 26.229f * enemyDis + (1.886440003f - 0.76226667f * enemyDis) * enemy.graFBNum;
                            sZSlope = 2.65462f + 0.6102f * enemyDis + (-0.085372f + 0.03348f * enemyDis) * enemy.graFBNum;
                            sZIntercept = 75.3616f - 29.194f * enemyDis + (2.704533327f - 1.03733333f * enemyDis) * enemy.graFBNum;
                            eXScaleSlope = -0.022f - 0.02f * enemyDis + (0.002213324f - 0.00186667f * enemyDis) * enemy.graFBNum;
                            eXScaleIntercept = -1.105f + 1.95f * enemyDis + (-0.07906667f - 0.004f * enemyDis) * enemy.graFBNum;
                        }
                        else
                        {
                            sXSlope = 5.21838f - 2.2122f * enemyDis + (0.03398f + 0.007f * enemyDis) * enemy.graFBNum;
                            sXIntercept = -198.6599f - 8.619f * enemyDis + (0.73628f - 0.3032f * enemyDis) * enemy.graFBNum;
                            sYSlope = -0.45932f + 1.1748f * enemyDis + (0.802132f - 0.45308f * enemyDis) * enemy.graFBNum;
                            sYIntercept = 163.5251f - 26.229f * enemyDis + (2.49302f - 1.1278f * enemyDis) * enemy.graFBNum;
                            sZSlope = 2.65462f + 0.6102f * enemyDis + (0.045908f - 0.06692f * enemyDis) * enemy.graFBNum;
                            sZIntercept = 75.3616f - 29.194f * enemyDis + (1.08334f - 0.4346f * enemyDis) * enemy.graFBNum;
                            eXScaleSlope = -0.022f - 0.02f * enemyDis + (-0.00016f + 0.0024f * enemyDis) * enemy.graFBNum;
                            eXScaleIntercept = -1.105f + 1.95f * enemyDis + (-0.0602f - 0.022f * enemyDis) * enemy.graFBNum;
                        }
                    }
                }
                if(enemyDis <= hatakiMax)
                {
                    if(angDifAbs <= 60)
                    {
                        SetShoulderRot(1);
                        SetElbowRot(1);
                        SetElbowHandScale(1);
                        SetHandRot(4);
                        SetFingerRot(4);
                    }
                    else
                    {
                        SetShoulderRot(0);
                        SetElbowRot(0);
                        SetElbowHandScale(0);
                        SetHandRot(0);
                        SetFingerRot(0);
                    }
                }
                else
                {
                    SetShoulderRot(0);
                    SetElbowRot(0);
                    SetElbowHandScale(0);
                    SetHandRot(0);
                    SetFingerRot(0);
                }
                break;
        }
    }

    // 肩の角度を変更する関数
    private void SetShoulderRot(int _attacknum)
    {
        switch(_attacknum)
        {
            case 0:
                lsObj.transform.localRotation = lsInitialRot;
                rsObj.transform.localRotation = rsInitialRot;
                break;
            case 1:
                lsObj.transform.localEulerAngles = new Vector3(
                    sXSlope * graFBNum + sXIntercept,
                    sYSlope * graFBNum + sYIntercept,
                    sZSlope * graFBNum + sZIntercept
                );
                rsObj.transform.localEulerAngles = new Vector3(
                    sXSlope * graFBNum + sXIntercept,
                    sYSlope * graFBNum + sYIntercept,
                    sZSlope * graFBNum + sZIntercept
                );
                break;
        }
    }

    // 肘の角度を変更する関数
    private void SetElbowRot(int _attacknum)
    {
        switch(_attacknum)
        {
            case 0:
                leObj.transform.localRotation = leInitialRot;
                reObj.transform.localRotation = reInitialRot;
                break;
            case 1:
                leObj.transform.localEulerAngles = new Vector3(-12.081f, -15.295f, 18.526f);
                reObj.transform.localEulerAngles = new Vector3(-12.081f, -15.295f, 18.526f);
                break;
        }
    }

    // 肘や手の大きさを変更する関数
    private void SetElbowHandScale(int _attacknum)
    {
        switch(_attacknum)
        {
            case 0:
                leObj.transform.localScale = leInitialScale;
                reObj.transform.localScale = reInitialScale;
                lhPosObj.transform.localScale = lhInitialScale;
                rhPosObj.transform.localScale = rhInitialScale;
                break;
            case 1:
                leObj.transform.localScale = new Vector3(
                    eXScaleSlope * graFBNum + eXScaleIntercept,
                    1,
                    1
                );
                reObj.transform.localScale = new Vector3(
                    eXScaleSlope * graFBNum + eXScaleIntercept,
                    1,
                    1
                );
                lhPosObj.transform.localScale = new Vector3(
                    1 / leObj.transform.localScale.x,
                    1,
                    1
                );
                rhPosObj.transform.localScale = new Vector3(
                    1 / reObj.transform.localScale.x,
                    1,
                    1
                );
                break;
        }
    }

    // 手の先の角度を変更する関数
    private void SetHandRot(int _attacknum)
    {
        switch(_attacknum)
        {
            case 0:
                lhObj.transform.localRotation = lhInitialRot;
                rhObj.transform.localRotation = rhInitialRot;
                break;
            case 1:
                lhObj.transform.localEulerAngles = new Vector3(30.655f, -5.938f, -20.974f);
                rhObj.transform.localEulerAngles = new Vector3(30.655f, -5.938f, -20.974f);
                break;
            case 2:
                lhObj.transform.localEulerAngles = new Vector3(-6.23f, -1.585f, -1.984f);
                rhObj.transform.localEulerAngles = new Vector3(-6.23f, -1.585f, -1.984f);
                break;
            case 3:
                lhObj.transform.localEulerAngles = new Vector3(
                    hXSlope * graFBNum + hXIntercept,
                    hYSlope * graFBNum + hYIntercept,
                    hZSlope * graFBNum + hZIntercept
                );
                rhObj.transform.localEulerAngles = new Vector3(
                    hXSlope * graFBNum + hXIntercept,
                    hYSlope * graFBNum + hYIntercept,
                    hZSlope * graFBNum + hZIntercept
                );
                break;
            case 4:
                lhObj.transform.localEulerAngles = new Vector3(34.54f, -3.507f, -14.147f);
                rhObj.transform.localEulerAngles = new Vector3(34.54f, -3.507f, -14.147f);
                break;   
        }
    }

    // 指の角度を変更する関数
    private void SetFingerRot(int _attacknum)
    {
        switch(_attacknum)
        {
            case 0:
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
                break;
            case 1:
                lThumbObj.transform.localEulerAngles = new Vector3(-38.814f, 144.835f, 4.534f);
                rThumbObj.transform.localEulerAngles = new Vector3(-38.814f, 144.835f, 4.534f);
                lIndexObj.transform.localEulerAngles = new Vector3(14.481f, 176.018f, 2.948f);
                rIndexObj.transform.localEulerAngles = new Vector3(14.481f, 176.018f, 2.948f);
                lMiddleObj.transform.localEulerAngles = new Vector3(23.765f, 177.59f, 2.105f);
                rMiddleObj.transform.localEulerAngles = new Vector3(23.765f, 177.59f, 2.105f);
                lRingObj.transform.localEulerAngles = new Vector3(28.935f, 176.837f, -3.297f);
                rRingObj.transform.localEulerAngles = new Vector3(28.935f, 176.837f, -3.297f);
                lLittleObj.transform.localEulerAngles = new Vector3(29.761f, 177.865f, -7.876f);
                rLittleObj.transform.localEulerAngles = new Vector3(29.761f, 177.865f, -7.876f);
                break;
            case 2:
                lThumbObj.transform.localEulerAngles = new Vector3(-35.904f, 117.309f, 29.463f);
                rThumbObj.transform.localEulerAngles = new Vector3(-35.904f, 117.309f, 29.463f);
                lIndexObj.transform.localEulerAngles = new Vector3(12.581f, -177.547f, 22.851f);
                rIndexObj.transform.localEulerAngles = new Vector3(12.581f, -177.547f, 22.851f);
                lMiddleObj.transform.localEulerAngles = new Vector3(22.963f, -175.203f, 17.187f);
                rMiddleObj.transform.localEulerAngles = new Vector3(22.963f, -175.203f, 17.187f);
                lRingObj.transform.localEulerAngles = new Vector3(29.185f, -177.123f, 11.515f);
                rRingObj.transform.localEulerAngles = new Vector3(29.185f, -177.123f, 11.515f);
                lLittleObj.transform.localEulerAngles = new Vector3(31.231f, -176.602f, 8.399f);
                rLittleObj.transform.localEulerAngles = new Vector3(31.231f, -176.602f, 8.399f);
                break;
            case 3:
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
                break;
            case 4:
                lThumbObj.transform.localEulerAngles = new Vector3(-5.032f, 160.233f, -32.896f);
                rThumbObj.transform.localEulerAngles = new Vector3(-5.032f, 160.233f, -32.896f);
                lIndexObj.transform.localEulerAngles = new Vector3(14.787f, 175.928f, 18.425f);
                rIndexObj.transform.localEulerAngles = new Vector3(14.787f, 175.928f, 18.425f);
                lMiddleObj.transform.localEulerAngles = new Vector3(24.012f, -178.938f, 16.393f);
                rMiddleObj.transform.localEulerAngles = new Vector3(24.012f, -178.938f, 16.393f);
                lRingObj.transform.localEulerAngles = new Vector3(28.81f, -179.58f, 12.123f);
                rRingObj.transform.localEulerAngles = new Vector3(28.81f, -179.58f, 12.123f);
                lLittleObj.transform.localEulerAngles = new Vector3(29.423f, -177f, 20.866f);
                rLittleObj.transform.localEulerAngles = new Vector3(29.423f, -177f, 20.866f);
                break;   
        }
    }

    // 左足の角度を変更する関数
    private void SetLeftLegRot(int _inputNum)
    {
        switch(_inputNum)
        {
            case 0:
                lLObj.transform.localRotation = lLInitialRot;
                lKObj.transform.localRotation = lKInitialRot;
                lFObj.transform.localRotation = lFInitialRot;
                break;
            case 1:
                lLObj.transform.localEulerAngles = new Vector3(-38.392f, -4.941f, 22.577f);
                lKObj.transform.localEulerAngles = new Vector3(72.595f, -148.66f, -166.321f);
                lFObj.transform.localEulerAngles = new Vector3(-141.3f, 0.437f, -12.036f);
                break;
        }
    }

    // 右足の角度を変更する関数
    private void SetRightLegRot(int _inputNum)
    {
        switch(_inputNum)
        {
            case 0:
                rLObj.transform.localRotation = rLInitialRot;
                rKObj.transform.localRotation = rKInitialRot;
                rFObj.transform.localRotation = rFInitialRot;
                break;
            case 1:
                rLObj.transform.localEulerAngles = new Vector3(-38.392f, -4.941f, 22.577f);
                rKObj.transform.localEulerAngles = new Vector3(72.595f, -148.66f, -166.321f);
                rFObj.transform.localEulerAngles = new Vector3(-141.3f, 0.437f, -12.036f);
                break;
        }
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
    // 手の当たり判定の出現に関する関数
    private void SetHandCollider()
    {
        if(maxAngle > 15f && (Mathf.Abs(graFBNum) > graMax || Mathf.Abs(graLRNum) > graMax))
        {
            lhColliderObj.SetActive(true);
            rhColliderObj.SetActive(true);
        }
        else
        {
            lhColliderObj.SetActive(false);
            rhColliderObj.SetActive(false);
        }
    }

    // 土俵外に出ているかの判定
    private void SetInDohyo()
    {
        Vector2 lfPlace =  new Vector2(lWholeLObj.transform.position.x, lWholeLObj.transform.position.z);
        Vector2 rfPlace =  new Vector2(rWholeLObj.transform.position.x, rWholeLObj.transform.position.z);
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
            Time.deltaTime * rightDir * moveSpeedMagNum * moveLRDisMagNum,
            0f, 
            Time.deltaTime * frontDir * moveSpeedMagNum * moveFBDisMagNum,
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
        playStart = false;
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
        SetShoulderRot(0);
        SetElbowRot(0);
        SetElbowHandScale(0);
        SetHandRot(0);
        SetFingerRot(0);
        lhColliderObj.SetActive(false);
        rhColliderObj.SetActive(false);
        SetLeftLegRot(0);
        SetRightLegRot(0);
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
        SetSpineRot();
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