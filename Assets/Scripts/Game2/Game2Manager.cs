using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game2Manager : MonoBehaviour
{
    #region 変数宣言
    #region ゲーム状態の変数
    [SerializeField] private int kimariteNum;  // 決まり手ナンバー
    private float waitTime = 0.3f;  // 各音声間の休止タイム
    public enum GameState
    {
        BeforePlay,
        Play,
        End
    };
    public GameState gameState;  // 現在のゲーム状態

    public enum GamePlayer
    {
        One,
        Two
    };
    public GamePlayer gamePlayer;  // 現在のプレイヤー人数
    #endregion
    #region UIオブジェクトの変数
    [Header("UI")]
    [SerializeField] private GameObject titleUI;  // タイトルのパネルオブジェクト
    [SerializeField] private Text startBText;  // スタートボタンのテキスト
    private float blinkingSpeed = 0.2f;  // 点滅スピード
    private Color32 startColor = new Color32(50, 50, 50, 255);  // ループ開始時の色
    private Color32 endColor = new Color32(50, 50, 50, 128);  // ループ終了時の色
    [SerializeField] private GameObject operatorUI;  // 操作方法のパネルオブジェクト
    [SerializeField] private GameObject gameModeUI;  // ゲームモードのパネルオブジェクト
    [SerializeField] private Button onePlayerButton;  // OnePlayerModeのボタンUI
    [SerializeField] private Button twoPlayerButton;  // TwoPlayerModeのボタンUI
    [SerializeField] private Text gyojiText;  // 始まりの掛け声のテキスト
    [SerializeField] private GameObject gameResultUI;  // ゲーム結果のパネルオブジェクト
    [SerializeField] private Text resultText;  // 決まり手のテキスト
    [SerializeField] private Image resultImage; // 決まり手のUI画像
    [SerializeField] private Sprite[] resultSprite; // 決まり手の画像配列
    [SerializeField] private Button replayButton;  // ReplayButtonのボタンUI
    #endregion
    #region カメラの参照を取る変数
    [Header("カメラ")]
    [SerializeField] private Camera cameraObj;  // メインカメラのオブジェクト
    [SerializeField] private Vector3 cameraInitialPos;  // メインカメラの初期座標
    [SerializeField] private Quaternion cameraInitialRot;  // メインカメラの初期角度

    [Header("中心座標")]
    [SerializeField] private Vector3 centerGravity;  // 二人のプレイヤーの中心重心座標
    [SerializeField] private Vector2 centerPlace;  // 二人のプレイヤーの中心座標
    #endregion
    #region プレイヤー1に対する変数
    [Header("プレイヤー1")]
    [SerializeField] private Rikishi2Manager p1Ctrl;  // プレイヤー1のスクリプト
    [SerializeField] private Rikishi2UIManager p1UICtrl;  // プレイヤー1のUIスクリプト
    [SerializeField] private GameObject p1Obj;  // プレイヤー1のオブジェクト
    [SerializeField] private Vector3 p1Gravity;  // プレイヤー1の重心座標
    [SerializeField] private Vector2 p1Place;  // プレイヤー1の位置座標
    [SerializeField] private bool p1WeightInput = false;  // プレイヤー1の体重入力
    [SerializeField] private bool p1TachiaiInput = false;  // プレイヤー1の立会い入力
    [SerializeField] private float p1TachiaiTime = 0;  // プレイヤー1の立会い入力時間
    [SerializeField] private bool p1MoveEnd = false;  // プレイヤー1の立会い移動
    [SerializeField] private bool p1ResultInput = false;  // プレイヤー1の勝敗結果データの入力
    [SerializeField] private bool p1Result = false;  // プレイヤー1の勝敗結果
    [SerializeField] private float p1GraFBNum = 0;  // プレイヤー1の最終前後重心値
    [SerializeField] private float p1GraLRNum = 0;  // プレイヤー1の最終左右重心値
    [SerializeField] private float p1AttackFBNum = 0;  // プレイヤー1の前後攻撃値
    [SerializeField] private float p1AttackLRNum = 0;  // プレイヤー1の左右攻撃値
    [SerializeField] private bool p1FallDown = false;  // プレイヤー1は倒れたか否か
    [SerializeField] private bool p1OutDohyo = false;  // プレイヤー1は土俵から出たか否か
    [SerializeField] private int p1AttackNum = 0;  // プレイヤー1の攻撃状態
    [SerializeField] private float p1AngDif = 0;  // プレイヤー1の相手方向との角度差
    [SerializeField] private bool p1InColl = false;  // プレイヤー1は内側の足が当たっているか
    [SerializeField] private bool p1OutColl = false;  // プレイヤー1は外側の足が当たっているか
    #endregion
    #region プレイヤー2に対する変数
    [Header("プレイヤー2")]
    [SerializeField] private Rikishi2Manager p2Ctrl;  // プレイヤー2のスクリプト
    [SerializeField] private Rikishi2UIManager p2UICtrl;  // プレイヤー2のUIスクリプト
    [SerializeField] private GameObject p2Obj;  // プレイヤー2のオブジェクト
    [SerializeField] private Vector3 p2Gravity;  // プレイヤー2の重心座標
    [SerializeField] private Vector2 p2Place;  // プレイヤー2の位置座標
    [SerializeField] private bool p2WeightInput = false;  // プレイヤー2の体重入力
    [SerializeField] private bool p2TachiaiInput = false;  // プレイヤー2の立会い入力
    [SerializeField] private float p2TachiaiTime = 0;  // プレイヤー2の立会い入力時間
    [SerializeField] private bool p2MoveEnd = false;  // プレイヤー2の立会い移動
    [SerializeField] private bool p2ResultInput = false;  // プレイヤー2の勝敗結果データの入力
    [SerializeField] private bool p2Result = false;  // プレイヤー2の勝敗結果
    [SerializeField] private float p2GraFBNum = 0;  // プレイヤー2の最終前後重心値
    [SerializeField] private float p2GraLRNum = 0;  // プレイヤー2の最終左右重心値
    [SerializeField] private float p2AttackFBNum = 0;  // プレイヤー2の前後攻撃値
    [SerializeField] private float p2AttackLRNum = 0;  // プレイヤー2の左右攻撃値
    [SerializeField] private bool p2FallDown = false;  // プレイヤー2は倒れたか否か
    [SerializeField] private bool p2OutDohyo = false;  // プレイヤー2は土俵から出たか否か
    [SerializeField] private int p2AttackNum = 0;  // プレイヤー2の攻撃状態
    [SerializeField] private float p2AngDif = 0;  // プレイヤー2の相手方向との角度差
    [SerializeField] private bool p2InColl = false;  // プレイヤー2は内側の足が当たっているか
    [SerializeField] private bool p2OutColl = false;  // プレイヤー2は外側の足が当たっているか
    #endregion
    #region サウンドの参照を取る変数
    [Header("サウンド")]
    [SerializeField] private AudioClip broadcastSound;  // 放送と待ったなしのサウンド
    [SerializeField] private AudioClip shoutSound;  // はっけよいのサウンド
    [SerializeField] private AudioClip startSound;  // のこったのサウンド
    [SerializeField] private AudioClip resultSound;  // 勝負ありのサウンド
    [SerializeField] private AudioClip announceSound;  // ただいまの決まり手はのサウンド
    [SerializeField] private AudioClip[] kimariteSound;  // 決まり手のサウンド
    [SerializeField] private AudioClip[] winnerSound;  // 勝者のサウンド
    [SerializeField] private AudioSource playAudioSource;  // プレイ中のAudioSource
    [SerializeField] private AudioSource seAudioSource;  // 効果音のAudioSource
    #endregion
    
    private static Game2Manager instance;
    public static Game2Manager Instance {get => instance;}
    #endregion

    private void Awake()
    {
        instance = this.GetComponent<Game2Manager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGameState(GameState.BeforePlay);
        SelectTwoPlayer();
        cameraInitialPos = cameraObj.gameObject.transform.position;
        cameraInitialRot = cameraObj.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        SetCenterGravityPlace();
        SetCenterPlace();
        SetCameraPlace();
        if(titleUI.gameObject.activeSelf)
        {
            SetBlinkButtonText();
        }
    }

    // ゲーム状態の保存
    public void SetGameState(GameState _gameState)
    {
        gameState = _gameState;
    }

    #region 操作方法までに関するスクリプト
    // PushBtoStartボタンを押した
    public void PushGameStart()
    {
        titleUI.SetActive(false);
        operatorUI.SetActive(true);
    }

    // ゲームスタートのボタンテキストの点滅を行う関数
    private void SetBlinkButtonText()
    {
        startBText.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / blinkingSpeed, 1.0f));
    }

    // PlayStartボタンを押した
    public void PushPlayStart()
    {
        operatorUI.SetActive(false);
        gameModeUI.SetActive(true);
    }
    #endregion

    #region プレイヤー人数選択に関するスクリプト
    // プレイヤー1人への変更
    public void SelectOnePlayer()
    {
        onePlayerButton.interactable = true;
        twoPlayerButton.interactable = false;
        gamePlayer = GamePlayer.One;
    }

    // プレイヤー2人への変更
    public void SelectTwoPlayer()
    {
        onePlayerButton.interactable = false;
        twoPlayerButton.interactable = true;
        gamePlayer = GamePlayer.Two;
    }

    // プレイヤー人数選択を行った
    public void SetPlayerMode()
    {
        if(onePlayerButton.interactable)
        {
            onePlayerButton.image.color = new Color32(255, 150, 150, 255);
        }
        if(twoPlayerButton.interactable)
        {
            twoPlayerButton.image.color = new Color32(255, 150, 150, 255);
        }
        StartCoroutine("DeleteModeScene");
    }

    // モード選択画面を遅らせて消すコルーチン関数
    private IEnumerator DeleteModeScene()
    {
        yield return new WaitForSeconds(1f);
        gameModeUI.SetActive(false);
        p1UICtrl.SetWeightPanel(true);
        p2UICtrl.SetWeightPanel(true);
    }
    #endregion

    #region ゲーム開始に関するスクリプト
    // プレイヤー人数におけるメインカメラの設定
    public void SetMainCamera()
    {
        switch(gamePlayer)
        {
            case GamePlayer.One:
                cameraObj.rect = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
                break;
            case GamePlayer.Two:
                cameraObj.rect = new Rect(0.375f, 0.75f, 0.25f, 0.25f);
                break;
        }
    }

    // プレイヤーの体重入力の確認を行う関数
    public void GameStart(int _playerNum)
    {
        switch(_playerNum)
        {
            case 1:
                p1WeightInput = true;
                break;
            case 2:
                p2WeightInput = true;
                break;
        }

        if(p1WeightInput && p2WeightInput)
        {
            p1UICtrl.SetWeightPanel(false);
            p2UICtrl.SetWeightPanel(false);
            p1Ctrl.TellWinLosses();
            p2Ctrl.TellWinLosses();
            p1UICtrl.SetPlayerPanel(true);
            p2UICtrl.SetPlayerPanel(true);
            StartCoroutine("SoundStart");
        }
    }

    // スタート前のアナウンスを行うコルーチン関数
    private IEnumerator SoundStart()
    {
        yield return new WaitForSeconds(waitTime);
        seAudioSource.PlayOneShot(broadcastSound);
        float broadcastTime = broadcastSound.length + waitTime;
        yield return new WaitForSeconds(broadcastTime);
        p1UICtrl.SetPlayerPanel(false);
        p2UICtrl.SetPlayerPanel(false);
        p1UICtrl.SetInGamePanel(true);
        p2UICtrl.SetInGamePanel(true);
        p1Ctrl.SetTachiaiInput(true);
        p2Ctrl.SetTachiaiInput(true);
        gyojiText.gameObject.SetActive(true);
        gyojiText.text = "はっけよい";
        seAudioSource.PlayOneShot(shoutSound);
        float shoutTime = shoutSound.length + waitTime * 2;
        yield return new WaitForSeconds(shoutTime);
        gyojiText.text = "のこった";
        seAudioSource.PlayOneShot(startSound);
        SetGameState(GameState.Play);
        p1UICtrl.SetTachiaiBActive(true);
        p2UICtrl.SetTachiaiBActive(true);
        float startWaitTime = startSound.length + waitTime;
        yield return new WaitForSeconds(startWaitTime);
        playAudioSource.Play();
        gyojiText.gameObject.SetActive(false);
    }
    #endregion

    #region ゲーム中に関するスクリプト
    // プレイヤーの立会い入力の確認を行う関数
    public void TachiaiStart(int _playerNum, float _pushTime)
    {
        switch(_playerNum)
        {
            case 1:
                p1TachiaiInput = true;
                p1TachiaiTime = _pushTime;
                break;
            case 2:
                p2TachiaiInput = true;
                p2TachiaiTime = _pushTime;
                break;
        }

        if(p1TachiaiInput && p2TachiaiInput)
        {
            p1Ctrl.SetLagStartPos(-1, p1TachiaiTime - p2TachiaiTime);
            p2Ctrl.SetLagStartPos(1, p2TachiaiTime - p1TachiaiTime);
        }
    }

    // プレイヤーの立会い移動完了の確認を行う関数
    public void TachiaiEnd(int _playerNum)
    {
        switch(_playerNum)
        {
            case 1:
                p1MoveEnd = true;
                break;
            case 2:
                p2MoveEnd = true;
                break;
        }

        if(p1MoveEnd && p2MoveEnd)
        {
            p1Ctrl.SetTachiaiEnd();
            p2Ctrl.SetTachiaiEnd();
        }
    }

    // 中心重心座標の計算を行う関数
    private void SetCenterGravityPlace()
    {
        p1Gravity = p1Ctrl.gravityWorldPos;
        p2Gravity = p2Ctrl.gravityWorldPos;
        centerGravity = (p1Gravity + p2Gravity) / 2;
        Debug.DrawLine (transform.position , transform.position + transform.rotation * centerGravity);
    }

    // 中心座標の計算を行う関数
    private void SetCenterPlace()
    {
        p1Place =  new Vector2(p1Obj.transform.position.x, p1Obj.transform.position.z);
        p2Place =  new Vector2(p2Obj.transform.position.x, p2Obj.transform.position.z);
        centerPlace = (p1Place + p2Place) / 2;
    }

    // カメラの座標の計算を行う関数
    private void SetCameraPlace()
    {
        cameraObj.gameObject.transform.position =
            new Vector3
            (
                cameraInitialPos.x + centerPlace.x,
                cameraInitialPos.y,
                cameraInitialPos.z + centerPlace.y
            );
    }
    #endregion

    #region ゲーム結果に関するスクリプト
    // ゲーム結果の決定
    public void SetGameResult(int _playerNum, bool _isResult, float _graFBNum, float _graLRNum, bool _isFallDown, bool _isOutDohyo, int _attackNum, float _angularDif, bool _isInColl, bool _isOutColl)
    {
        switch(_playerNum)
        {
            case 1:
                p1ResultInput = true;
                p1Result = _isResult;
                p1GraFBNum = _graFBNum;
                p1GraLRNum = _graLRNum;
                p1FallDown = _isFallDown;
                p1OutDohyo = _isOutDohyo;
                p1AttackNum = _attackNum;
                p1AngDif = _angularDif;
                p1InColl = _isInColl;
                p1OutColl = _isOutColl;
                break;
            case 2:
                p2ResultInput = true;
                p2Result = _isResult;
                p2GraFBNum = _graFBNum;
                p2GraLRNum = _graLRNum;
                p2FallDown = _isFallDown;
                p2OutDohyo = _isOutDohyo;
                p2AttackNum = _attackNum;
                p2AngDif = _angularDif;
                p2InColl = _isInColl;
                p2OutColl = _isOutColl;
                break;
        }

        if(p1ResultInput && p2ResultInput)
        {
            p1AttackFBNum = p2GraFBNum * Mathf.Cos(p2AngDif * Mathf.Deg2Rad) + p2GraLRNum * Mathf.Sin(-p2AngDif * Mathf.Deg2Rad);
            p1AttackLRNum = p2GraFBNum * Mathf.Sin(p2AngDif * Mathf.Deg2Rad) + p2GraLRNum * Mathf.Cos(p2AngDif * Mathf.Deg2Rad);
            p2AttackFBNum = p1GraFBNum * Mathf.Cos(p1AngDif * Mathf.Deg2Rad) + p1GraLRNum * Mathf.Sin(-p1AngDif * Mathf.Deg2Rad);
            p2AttackLRNum = p1GraFBNum * Mathf.Sin(p1AngDif * Mathf.Deg2Rad) + p1GraLRNum * Mathf.Cos(p1AngDif * Mathf.Deg2Rad);
            SetResultAction();
        }
    }

    // ゲーム結果の演出を行う関数
    private void SetResultAction()
    {
        int winnerNum = 0;
        playAudioSource.Stop();
        SetGameState(GameState.End);
        SetResultText();
        if(p1Result)
        {
            winnerNum = 1;
        }
        if(p2Result)
        {
            winnerNum = 2;
        }
        seAudioSource.PlayOneShot(resultSound);
        kimariteNum = SetWinReason();
        StartCoroutine(SetKimarite(kimariteNum, winnerNum-1));
    }

    // 勝敗結果のテキストの表示
    private void SetResultText()
    {
        if(p1Result)
        {
            p1UICtrl.GameResult("W i n !", new Color32(255, 0, 0, 255));
            p2UICtrl.GameResult("L o s e !", new Color32(0, 0, 255, 255));
        }
        if(p2Result)
        {
            p1UICtrl.GameResult("L o s e !", new Color32(0, 0, 255, 255));
            p2UICtrl.GameResult("W i n !", new Color32(255, 0, 0, 255));
        }
    }

    // 決まり手の決定
    private int SetWinReason()
    {
        int reasonNum = 0;
        if(p1Result)
        {
            if(p2InColl || p2OutColl)
            {
                if(p1AttackLRNum > p1AttackFBNum)
                {
                    reasonNum = 11;
                }
                else
                {
                    if(p2OutColl)
                    {
                        reasonNum = 9;
                    }
                    if(p2InColl)
                    {
                        reasonNum = 10;
                    }
                }
            }
            else
            {
                if(p2OutDohyo)
                {
                    if(p1AttackNum == 3)
                    {
                        reasonNum = 2;
                    }
                    else if(p1AttackNum == 1 || p1AttackNum == 2)
                    {
                        reasonNum = 0;
                    }
                }
                if(p2FallDown)
                {
                    if(p1Ctrl.graMax < Mathf.Abs(p1GraFBNum) || p1Ctrl.graMax < Mathf.Abs(p1GraLRNum))
                    {
                        reasonNum = 4;
                    }
                    else
                    {
                        switch(p1AttackNum)
                        {
                            case 1:
                            case 2:
                                if(Mathf.Abs(p1AngDif) >= 120f)
                                {
                                    reasonNum = 12;
                                }
                                else
                                {
                                    if(Mathf.Abs(p1AttackFBNum) >=  Mathf.Abs(p1AttackLRNum))
                                    {
                                        if(p1AttackFBNum < 0)
                                        {
                                            reasonNum = 1;
                                        }
                                        else
                                        {
                                            reasonNum = 7;
                                        }
                                    }
                                    else
                                    {
                                        if(p1AttackNum == 1)
                                        {
                                            reasonNum = 5;
                                        }
                                        if(p1AttackNum == 2)
                                        {
                                            reasonNum = 6;
                                        }
                                    }
                                }
                                break;
                            case 3:
                                reasonNum = 3;
                                break;
                            case 4:
                                if(Mathf.Abs(p1AttackFBNum) >=  Mathf.Abs(p1AttackLRNum))
                                {
                                    reasonNum = 7;
                                }
                                else
                                {
                                    reasonNum = 8;
                                }
                                break;
                        }
                    }
                }
            }
        }

        if(p2Result)
        {
            if(p1InColl || p1OutColl)
            {
                if(p2AttackLRNum > p2AttackFBNum)
                {
                    reasonNum = 11;
                }
                else
                {
                    if(p1OutColl)
                    {
                        reasonNum = 9;
                    }
                    if(p1InColl)
                    {
                        reasonNum = 10;
                    }
                }
            }
            else
            {
                if(p1OutDohyo)
                {
                    if(p2AttackNum == 3)
                    {
                        reasonNum = 2;
                    }
                    else if(p2AttackNum == 1 || p2AttackNum == 2)
                    {
                        reasonNum = 0;
                    }
                }
                if(p1FallDown)
                {
                    if(p2Ctrl.graMax < Mathf.Abs(p2GraFBNum) || p2Ctrl.graMax < Mathf.Abs(p2GraLRNum))
                    {
                        reasonNum = 4;
                    }
                    else
                    {
                        switch(p2AttackNum)
                        {
                            case 1:
                            case 2:
                                if(Mathf.Abs(p2AngDif) >= 120f)
                                {
                                    reasonNum = 12;
                                }
                                else
                                {
                                    if(Mathf.Abs(p2AttackFBNum) >=  Mathf.Abs(p2AttackLRNum))
                                    {
                                        if(p2AttackFBNum < 0)
                                        {
                                            reasonNum = 1;
                                        }
                                        else
                                        {
                                            reasonNum = 7;
                                        }
                                    }
                                    else
                                    {
                                        if(p2AttackNum == 1)
                                        {
                                            reasonNum = 5;
                                        }
                                        if(p2AttackNum == 2)
                                        {
                                            reasonNum = 6;
                                        }
                                    }
                                }
                                break;
                            case 3:
                                reasonNum = 3;
                                break;
                            case 4:
                                if(Mathf.Abs(p2AttackFBNum) >=  Mathf.Abs(p2AttackLRNum))
                                {
                                    reasonNum = 7;
                                }
                                else
                                {
                                    reasonNum = 8;
                                }
                                break;
                        }
                    }
                }
            }
        }
        return reasonNum;
    }

    // 決まり手の発表とアナウンスをするコルーチン関数
    private IEnumerator SetKimarite(int _kimarite, int _winner)
    {
        float resWaitTime = resultSound.length + 0.9f;
        yield return new WaitForSeconds(resWaitTime);
        seAudioSource.PlayOneShot(announceSound);
        float annWaitTime = announceSound.length + waitTime;
        yield return new WaitForSeconds(annWaitTime);
        switch(_kimarite)
        {
            case 0:
                resultText.text = "寄り切り";
                break;
            case 1:
                resultText.text = "寄り倒し";
                break;
            case 2:
                resultText.text = "押し出し";
                break;
            case 3:
                resultText.text = "押し倒し";
                break;
            case 4:
                resultText.text = "浴びせ倒し";
                break;
            case 5:
                resultText.text = "すくい投げ";
                break;
            case 6:
                resultText.text = "上手投げ";
                break;
            case 7:
                resultText.text = "引き落とし";
                break;
            case 8:
                resultText.text = "はたき込み";
                break;
            case 9:
                resultText.text = "外掛け";
                break;
            case 10:
                resultText.text = "内掛け";
                break;
            case 11:
                resultText.text = "掛け投げ";
                break;
            case 12:
                resultText.text = "後ろもたれ";
                break;
        }
        gameResultUI.SetActive(true);
        seAudioSource.PlayOneShot(kimariteSound[_kimarite]);
        resultImage.sprite = resultSprite[_kimarite];
        float winWaitTime = kimariteSound[_kimarite].length + waitTime;
        yield return new WaitForSeconds(winWaitTime);
        seAudioSource.PlayOneShot(winnerSound[_winner]);
        float replayWaitTime = winnerSound[_winner].length + waitTime;
        StartCoroutine("SetReplayButton", replayWaitTime);
    }

    // replayボタンを遅れて登場させるコルーチン関数
    private IEnumerator SetReplayButton(float _replayWaitTime)
    {
        yield return new WaitForSeconds(_replayWaitTime);
        replayButton.gameObject.SetActive(true);
        p1Ctrl.SetResetOK();
        p2Ctrl.SetResetOK();
    }
    #endregion

    #region Replayに関するスクリプト
    // ReplayButtonを押した(値のリセット)
    public void PushReplayDown()
    {
        gameResultUI.SetActive(false);
        resultText.text = "";
        replayButton.gameObject.SetActive(false);
        p1WeightInput = false;
        p2WeightInput = false;
        p1TachiaiInput = false;
        p2TachiaiInput = false;
        p1TachiaiTime = 0;
        p2TachiaiTime = 0;
        p1MoveEnd = false;
        p2MoveEnd = false;
        p1ResultInput = false;
        p2ResultInput = false;
        p1Result = false;
        p2Result = false;
        p1GraFBNum = 0;
        p2GraFBNum = 0;
        p1GraLRNum = 0;
        p2GraLRNum = 0;
        p1AttackFBNum = 0;
        p1AttackLRNum = 0;
        p2AttackFBNum = 0;
        p2AttackLRNum = 0;
        p1FallDown = false;
        p2FallDown = false;
        p1OutDohyo = false;
        p2OutDohyo = false;
        p1AttackNum = 0;
        p2AttackNum = 0;
        p1AngDif = 0;
        p2AngDif = 0;
        p1InColl = false;
        p2InColl = false;
        p1OutColl = false;
        p2OutColl = false;
        onePlayerButton.image.color = new Color32(255, 255, 255, 255);
        twoPlayerButton.image.color = new Color32(255, 255, 255, 255);
        StartCoroutine("Reload");
    }

    // ゲーム状態を初期状態に戻すコルーチン関数
    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(1f);
        operatorUI.SetActive(true);
        p1Ctrl.SetReset();
        p2Ctrl.SetReset();
        p1UICtrl.SetResetUI();
        p2UICtrl.SetResetUI();
        yield return new WaitForSeconds(0.2f);
        SetGameState(GameState.BeforePlay);
    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere (transform.position + transform.rotation * centerGravity, 0.1f);
    }
}