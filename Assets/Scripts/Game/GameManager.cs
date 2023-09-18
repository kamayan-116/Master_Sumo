using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region 変数宣言
    public bool isSelect = false;  // GameModeの選択をしたか否か（したらtrue）
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

    public enum GameMode
    {
        Easy,
        Normal
    };
    public GameMode gameMode;  // 現在のゲームモード

    [Header("UI")]
    [SerializeField] private GameObject operatorUI;  // 操作方法のパネルオブジェクト
    [SerializeField] private GameObject gameModeUI;  // ゲームモードのパネルオブジェクト
    [SerializeField] private Button onePlayerButton;  // OnePlayerModeのボタンUI
    [SerializeField] private Button twoPlayerButton;  // TwoPlayerModeのボタンUI
    [SerializeField] private Button easyButton;  // EasyModeのボタンUI
    [SerializeField] private Button normalButton;  // NormalModeのボタンUI
    [SerializeField] private Text gyojiText;  // 始まりの掛け声のテキスト
    [SerializeField] private Button replayButton;  // ReplayButtonのボタンUI

    [Header("カメラ")]
    [SerializeField] private Camera cameraObj;  // メインカメラのオブジェクト
    [SerializeField] private Vector3 cameraInitialPos;  // メインカメラの初期座標
    [SerializeField] private Quaternion cameraInitialRot;  // メインカメラの初期角度

    [Header("中心座標")]
    [SerializeField] private Vector3 centerGravity;  // 二人のプレイヤーの中心重心座標
    [SerializeField] private Vector2 centerPlace;  // 二人のプレイヤーの中心座標

    [Header("プレイヤー1")]
    [SerializeField] private RikishiManager p1Ctrl;  // プレイヤー1のスクリプト
    [SerializeField] private RikishiUIManager p1UICtrl;  // プレイヤー1のUIスクリプト
    [SerializeField] private GameObject p1Obj;  // プレイヤー1のオブジェクト
    [SerializeField] private Vector3 p1Gravity;  // プレイヤー1の重心座標
    [SerializeField] private Vector2 p1Place;  // プレイヤー1の位置座標
    [SerializeField] private bool p1WeightInput = false;  // プレイヤー1の体重入力
    
    [Header("プレイヤー2")]
    [SerializeField] private RikishiManager p2Ctrl;  // プレイヤー2のスクリプト
    [SerializeField] private RikishiUIManager p2UICtrl;  // プレイヤー2のUIスクリプト
    [SerializeField] private GameObject p2Obj;  // プレイヤー2のオブジェクト
    [SerializeField] private Vector3 p2Gravity;  // プレイヤー2の重心座標
    [SerializeField] private Vector2 p2Place;  // プレイヤー2の位置座標
    [SerializeField] private bool p2WeightInput = false;  // プレイヤー2の体重入力
    
    private static GameManager instance;
    public static GameManager Instance {get => instance;}
    #endregion

    private void Awake()
    {
        instance = this.GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGameState(GameState.BeforePlay);
        SelectTwoPlayer();
        SelectNormalMode();
        cameraInitialPos = cameraObj.gameObject.transform.position;
        cameraInitialRot = cameraObj.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        SetCenterGravityPlace();
        SetCenterPlace();
        SetCameraPlace();
    }

    // ゲーム状態の保存
    public void SetGameState(GameState _gameState)
    {
        gameState = _gameState;
    }
    
    // GameStartボタンを押した
    public void PushGameStart()
    {
        operatorUI.SetActive(false);
        gameModeUI.SetActive(true);
    }

    #region モード選択に関するスクリプト
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
    public void DecidePlayerDown()
    {
        if(onePlayerButton.interactable)
        {
            onePlayerButton.image.color = new Color32(255, 150, 150, 255);
        }
        if(twoPlayerButton.interactable)
        {
            twoPlayerButton.image.color = new Color32(255, 150, 150, 255);
        }
    }

    // Easy Modeへの変更
    public void SelectEasyMode()
    {
        easyButton.interactable = true;
        normalButton.interactable = false;
        gameMode = GameMode.Easy;
    }

    // Normal Modeへの変更
    public void SelectNormalMode()
    {
        easyButton.interactable = false;
        normalButton.interactable = true;
        gameMode = GameMode.Normal;
    }

    // モード選択を行った
    public void DecideModeDown()
    {
        isSelect = true;
        if(easyButton.interactable)
        {
            easyButton.image.color = new Color32(255, 150, 150, 255);
        }
        if(normalButton.interactable)
        {
            normalButton.image.color = new Color32(255, 150, 150, 255);
        }
        StartCoroutine("DeleteModeScene");
    }

    // モード選択画面を遅らせて消すコルーチン関数
    private IEnumerator DeleteModeScene()
    {
        yield return new WaitForSeconds(1f);
        gameModeUI.SetActive(false);
    }
    #endregion

    #region ゲーム開始に関するスクリプト
    // プレイヤー人数におけるメインカメラの設定
    public void SetMainCamera()
    {
        switch(gamePlayer)
        {
            case GamePlayer.One:
                cameraObj.rect = new Rect(0.75f, 0f, 0.25f, 0.25f);
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
            gyojiText.gameObject.SetActive(true);
            gyojiText.text = "はっけよい";
            StartCoroutine("UIFalse");
        }
    }

    // 行司の掛け声のテキストを消すコルーチン関数
    private IEnumerator UIFalse()
    {
        yield return new WaitForSeconds(1f);
        gyojiText.text = "のこった";
        yield return new WaitForSeconds(0.5f);
        SetGameState(GameState.Play);
        gyojiText.gameObject.SetActive(false);
    }
    #endregion

    #region ゲーム中に関するスクリプト
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
        cameraObj.transform.position =
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
    public void SetGameResult(int _winnerNum)
    {
        SetGameState(GameState.End);
        StartCoroutine("SetReplayButton");
        
        switch(_winnerNum)
        {
            case 1:
                p1UICtrl.GameResult("W i n !", new Color32(255, 0, 0, 255));
                p2UICtrl.GameResult("L o s e !", new Color32(0, 0, 255, 255));
                break;
            case 2:
                p1UICtrl.GameResult("L o s e !", new Color32(0, 0, 255, 255));
                p2UICtrl.GameResult("W i n !", new Color32(255, 0, 0, 255));
                break;
        }
    }

    // replayボタンを遅れて登場させるコルーチン関数
    private IEnumerator SetReplayButton()
    {
        yield return new WaitForSeconds(3f);
        replayButton.gameObject.SetActive(true);
        p1Ctrl.isReplay = true;
        p2Ctrl.isReplay = true;
    }
    #endregion

    #region Replayに関するスクリプト
    // ReplayButtonを押した(値のリセット)
    public void PushReplayDown()
    {   
        replayButton.gameObject.SetActive(false);
        SetGameState(GameState.BeforePlay);
        isSelect = false;
        p1WeightInput = false;
        p2WeightInput = false;
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
    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere (transform.position + transform.rotation * centerGravity, 0.1f);
    }
}