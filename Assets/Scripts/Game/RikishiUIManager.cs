using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RikishiUIManager : MonoBehaviour
{
    #region 変数宣言
    [SerializeField] private RikishiManager rikishiManager;  // RikishiManagerプログラム
    [SerializeField] private Image resultImage;  // 結果画像
    [SerializeField] private Sprite[] resultSprite;  // 結果画像配列
    [SerializeField] private GameObject inGamePanel;  // ゲーム中のUIパネル
    [SerializeField] private Image playerNameImage;  // ゲーム中の名前画像
    [SerializeField] private Sprite[] playerNameSprite;  // ゲーム中の名前画像配列
    #region プレイヤーパネル関連
    [SerializeField] private Image playerPanel;  // プレイヤーパネル
    [SerializeField] private Text cpuLevelText;  // 対戦相手のテキスト
    [SerializeField] private Text matchiWinText;  // 勝利数のテキスト
    [SerializeField] private Text matchiLoseText;  // 敗北数のテキスト
    [SerializeField] private Slider powerSlider;  // パワースライダー
    [SerializeField] private Slider speedSlider;  // スピードスライダー
    [SerializeField] private Slider kumiSlider;  // 組みスライダー
    [SerializeField] private Slider oshiSlider;  // 押しスライダー
    [SerializeField] private Slider hatakiSlider;  // はたきスライダー
    #endregion
    #region 体重関連
    [SerializeField] private Text weightText; // 体重テキスト
    [SerializeField] private Slider weightSlider;  // 体重スライダー
    [SerializeField] private float weightInitialNum;  // 体重の初期値
    #endregion
    #region 立会いパネルや立会いの入力UI
    [SerializeField] private Image tachiaiPanel; // 立合いパネルのImage
    [SerializeField] private Text penaltyText; // 立会いのペナルティテキスト
    [SerializeField] private Image tachiaiInputImage; // 立合いの入力に応じた画像
    [SerializeField] private Image tachiaiSuccessImage; // 立合いの入力成功状況の画像
    [SerializeField] private Sprite[] tachiaiSuccessSprite; // 立合いの成功入力に応じた画像配列
    [SerializeField] private Image tachiaiFailureImage; // 立合いの入力失敗状況の画像
    [SerializeField] private Sprite[] tachiaiFailureSprite; // 立合いの失敗入力に応じた画像配列
    #endregion
    #region 重心関連
    [SerializeField] private Image gravityPanel; // 重心座標パネルのImage
    [SerializeField] private Sprite[] graInputSprite; // 重心入力に応じた画像配列
    [SerializeField] private Image gravityImage; // 重心座標のUI画像
    [SerializeField] private Vector3 graImageInitialPos;  // 重心座標のUI画像の初期座標
    private float graUIMoveMagNum;  // 重心UIの移動倍率数値
    [SerializeField] private Image graMoveUpArrow; // 上方向重心移動可能画像
    [SerializeField] private Image graMoveDownArrow; // 下方向重心移動可能画像
    [SerializeField] private Image graMoveLeftArrow; // 左方向重心移動可能画像
    [SerializeField] private Image graMoveRightArrow; // 右方向重心移動可能画像
    [SerializeField] private Sprite[] graMoveSprite; // 重心移動画像
    #endregion
    #region 操作関連
    [SerializeField] private Image statePanel;  // 力士状態の全体UIパネル
    [SerializeField] private Sprite[] stateSprite;  // 力士状態の全体画像配列
    [SerializeField] private Image playStylePanel;  // 技のUIパネル
    [SerializeField] private Image playImage; // プレイ状態のUI画像
    [SerializeField] private Sprite[] playSprite; // プレイ状態の画像配列（0が立合い、1が四つ、2がまわし、3が押し、4がはたき）
    [SerializeField] private Text playStyleText;  // 技のテキスト
    [SerializeField] private Image ArrowInputImage; // 方向パッドの入力状態のUI画像
    [SerializeField] private Sprite[] ArrowInputSprite; // 方向パッドの入力状態の画像配列（0が未入力、1が上入力、2が下入力、3が左入力、4が右入力）
    [SerializeField] private Image InputImage; // 入力状態のUI画像
    [SerializeField] private Sprite[] InputSprite; // 入力状態の画像配列（0が未入力、1が上入力、2が下入力、3が左入力、4が右入力）
    [SerializeField] private Text InputText;  // 重心攻撃状態のテキスト
    [SerializeField] private Image lFCircleImage; // 左足の操作中のUI画像
    [SerializeField] private Image rFCircleImage; // 右足の操作中のUI画像
    [SerializeField] private Image dragPanel;  // 抵抗のUIパネル
    [SerializeField] private Sprite[] dragSprite; // 抵抗パネルの画像配列
    [SerializeField] private Image dragBImage;  // 抵抗のBボタン
    [SerializeField] private Slider dragSlider;  // 抵抗値のスライダー
    private float lfCircley;  // 左足のUIのワールドY座標
    private float rfCircley;  // 右足のUIのワールドY座標
    #endregion
    #region UIの点滅
    private float blinkingSpeed = 0.2f;  // 点滅スピード
    private Color32 startColor = new Color32(255, 255, 255, 255);  // ループ開始時の色
    private Color32 endColor = new Color32(255, 255, 255, 128);  // ループ終了時の色
    #endregion
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        weightInitialNum = weightSlider.value;
        graImageInitialPos = gravityImage.rectTransform.localPosition;
        lfCircley = lFCircleImage.gameObject.transform.position.y;
        rfCircley = rFCircleImage.gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        rikishiManager.SetWeightNum(weightSlider.value);
        if(tachiaiInputImage.gameObject.activeSelf)
        {
            SetBlink(tachiaiInputImage);
        }
    }

    #region UIの基礎設定に関するスクリプト
    // 重心値のUIの移動値を計算する関数
    public void SetGraUIMoveMagNum(float _graMax)
    {
        graUIMoveMagNum = 114f / _graMax;
    }

    // プレイヤー人数に対するUIの配置
    public void SetUIPlace(int _playerNum)
    {
        switch(GameManager.Instance.gamePlayer)
        {
            case GameManager.GamePlayer.One:
                switch(_playerNum)
                {
                    case 1:
                        playerNameImage.rectTransform.localPosition = new Vector3(-600f, 450f, 0);
                        playerNameImage.sprite = playerNameSprite[0];
                        tachiaiPanel.rectTransform.localPosition = new Vector3(0f, 260f, 0);
                        statePanel.sprite = stateSprite[0];
                        statePanel.rectTransform.sizeDelta = new Vector2(1228f, 263f);
                        gravityPanel.rectTransform.localPosition = new Vector3(-135f, -10f, 0);
                        playStylePanel.rectTransform.localPosition = new Vector3(127.5f, 0f, 0);
                        dragPanel.rectTransform.localPosition = new Vector3(302.5f, 0f, 0);
                        ArrowInputImage.rectTransform.localPosition = new Vector3(475f, -70f, 0);
                        InputText.rectTransform.localPosition = new Vector3(100f, 160f, 0);
                        resultImage.rectTransform.localPosition = new Vector3(0, 370f, 0);
                        break;
                }
                break;
            case GameManager.GamePlayer.Two:
                statePanel.rectTransform.sizeDelta = new Vector2(960f, 262f);
                tachiaiPanel.rectTransform.localPosition = new Vector3(0f, 190f, 0);
                switch(_playerNum)
                {
                    case 1:
                        playerNameImage.rectTransform.localPosition = new Vector3(-320f, 450f, 0);
                        playerNameImage.sprite = playerNameSprite[1];
                        statePanel.sprite = stateSprite[1];
                        gravityPanel.rectTransform.localPosition = new Vector3(-270f, -10f, 0);
                        playStylePanel.rectTransform.localPosition = new Vector3(-7.5f, 0f, 0);
                        dragPanel.rectTransform.localPosition = new Vector3(167.5f, 0f, 0);
                        ArrowInputImage.rectTransform.localPosition = new Vector3(336f, -70f, 0);
                        InputText.rectTransform.localPosition = new Vector3(100f, 160f, 0);
                        resultImage.rectTransform.localPosition = new Vector3(5f, 100f, 0);
                        break;
                    case 2:
                        playerNameImage.rectTransform.localPosition = new Vector3(320f, 450f, 0);
                        playerNameImage.sprite = playerNameSprite[2];
                        statePanel.sprite = stateSprite[2];
                        gravityPanel.rectTransform.localPosition = new Vector3(270f, -10f, 0);
                        playStylePanel.rectTransform.localPosition = new Vector3(7.5f, 0f, 0);
                        dragPanel.rectTransform.localPosition = new Vector3(-167.5f, 0f, 0);
                        ArrowInputImage.rectTransform.localPosition = new Vector3(-336f, -70f, 0);
                        InputText.rectTransform.localPosition = new Vector3(-100f, 160f, 0);
                        resultImage.rectTransform.localPosition = new Vector3(-5f, 100f, 0);
                        break;
                }
                break;
        }
    }
    #endregion

    #region 体重に関するスクリプト
    // 体重スライダーの最大値と最小値を入力する関数
    public void SetWeightMaxMin(float _weightMax, float _weightMin)
    {
        weightSlider.maxValue = _weightMax;
        weightSlider.minValue = _weightMin;
    }

    // 体重スライダーの数値の変更を行う関数
    public void SetWeightSliderNum(float _changeValue)
    {
        weightSlider.value += _changeValue;
    }

    // 体重の数値をテキストに表示する関数
    public void SetWeightText(float _weightNum)
    {
        weightText.text = _weightNum.ToString("f0");
    }

    // 決定ボタンを押して体重の入力を行った
    public void SetWeightInput()
    {   
        weightSlider.interactable = false;
        rikishiManager.WeightInput();
    }
    #endregion

    #region プレイ中のUI
    // プレイヤーパネルの表示状態を管理する関数
    public void SetPlayerPanel(bool _isActive)
    {
        playerPanel.gameObject.SetActive(_isActive);
    }

    // ゲーム中パネルの表示状態を管理する関数
    public void SetInGamePanel(bool _isActive)
    {
        inGamePanel.SetActive(_isActive);
    }

    // 抵抗値パネルの表示を管理する関数
    public void SetDragPanel(bool _isActive)
    {
        dragSlider.gameObject.SetActive(_isActive);
        dragBImage.gameObject.SetActive(_isActive);
        if(_isActive)
        {
            SetBlink(dragBImage);
            dragPanel.sprite = dragSprite[1];
        }
        else
        {
            dragPanel.sprite = dragSprite[0];
        }
    }

    // 対戦相手レベルのテキストを表示する関数
    public void SetEnemyLevelText(int _cpulevel)
    {
        switch(GameManager.Instance.gamePlayer)
        {
            case GameManager.GamePlayer.One:
                cpuLevelText.text = "CPU " + _cpulevel + "：";
                break;
            case GameManager.GamePlayer.Two:
                cpuLevelText.text = "対人：";
                break;
        }
    }

    // 勝敗結果のテキストを管理する関数
    public void SetMatchResultText(int _winsNum, int _lossesNum)
    {
        matchiWinText.text = _winsNum.ToString();
        matchiLoseText.text = _lossesNum.ToString();
    }

    // 能力値のスライダーを管理する関数
    public void SetAbilityNumSlider(float _powerValue, float _speedValue, float _kumiValue, float _oshiValue, float _hatakiValue)
    {
        powerSlider.value = _powerValue;
        speedSlider.value = _speedValue;
        kumiSlider.value = _kumiValue;
        oshiSlider.value = _oshiValue;
        hatakiSlider.value = _hatakiValue;
    }

    // UIの点滅を行う関数
    private void SetBlink(Image _image)
    {
        _image.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / blinkingSpeed, 1.0f));
    }

    // 立会いの入力を行った
    public void SetTachiaiInput()
    {
        if(GameManager.Instance.gameState == GameManager.GameState.Play)
        {
            tachiaiSuccessImage.sprite = tachiaiSuccessSprite[1];
            SetTachiaiBActive(false);
        }   
        if(GameManager.Instance.gameState == GameManager.GameState.BeforePlay)
        {
            tachiaiFailureImage.sprite = tachiaiFailureSprite[1];
            StartCoroutine("SetFailureDelete");
        }
    }

    // 入力失敗画像をすぐに消すコルーチン関数
    private IEnumerator SetFailureDelete()
    {
        yield return new WaitForSeconds(0.2f);
        tachiaiFailureImage.sprite = tachiaiFailureSprite[0];
    }

    // 立会いのBボタンの画像の表示状態に関する関数
    public void SetTachiaiBActive(bool _isActive)
    {   
        tachiaiInputImage.gameObject.SetActive(_isActive);
    }

    // ペナルティ回数の数値をテキストに表示する関数
    public void SetPenaltyText(int _penaltyNum)
    {
        penaltyText.text = _penaltyNum.ToString();
    }

    // 立会いパネルを表示する関数
    public void SetTachiaiAppear(bool _isbool)
    {
        tachiaiPanel.gameObject.SetActive(_isbool);
    }

    // プレイ中の技のUIを変更する関数
    public void SetPlayStyleUI(int _playNum)
    {
        playImage.sprite = playSprite[_playNum];
        switch(_playNum)
        {
            case 0:
                playStyleText.text = "立会い";
                break;
            case 1:
                playStyleText.text = "四つ";
                break;
            case 2:
                playStyleText.text = "まわし";
                break;
            case 3:
                playStyleText.text = "押し";
                break;
            case 4:
                playStyleText.text = "はたき";
                break;
        }
    }

    // プレイ中の画像の点滅を行う関数
    public void SetBlinkPlay()
    {
        SetBlink(playImage);
    }

    // プレイ中の画像の透明度を戻す関数
    public void SetBlinkPlayEnd()
    {
        playImage.color = startColor;
    }

    // 操作中の方向パッドの画像に変更する関数
    public void SetArrowPatImage(int _operateNum)
    {
        ArrowInputImage.sprite = ArrowInputSprite[_operateNum];
        InputImage.sprite = InputSprite[_operateNum];

        if(_operateNum == 0)
        {
            InputImage.rectTransform.sizeDelta = new Vector2(254f, 118f);
        }
        else
        {
            InputImage.rectTransform.sizeDelta = new Vector2(230f, 130f);
        }

        if(_operateNum == 1)
        {
            InputText.text = "攻";
        }
        else if(_operateNum == 2)
        {
            InputText.text = "守";
        }
        else
        {
            InputText.text = "";
        }
    }

    // 操作中の足のUIを表示する関数
    public void SetFootOpeActive(bool _lfActive, bool _rfActive)
    {
        lFCircleImage.gameObject.SetActive(_lfActive);
        rFCircleImage.gameObject.SetActive(_rfActive);
    }

    // 足のUIを移動する関数
    public void SetFootOpeUIPlace(Vector3 _lfplace, Vector3 _rfplace)
    {
        lFCircleImage.gameObject.transform.position = new Vector3( _lfplace.x, lfCircley, _lfplace.z);
        rFCircleImage.gameObject.transform.position = new Vector3( _rfplace.x, rfCircley, _rfplace.z);
    }
    
    // 入力状況に応じた重心パネルの画像を変更する関数
    public void SetGraInputImage(int _playerNum)
    {
        gravityPanel.sprite = graInputSprite[_playerNum];
    }

    // 重心値のUIを表示する関数
    public void SetGravityUI(float _graLRNum, float _graFBNum)
    {
        gravityImage.rectTransform.localPosition = 
            new Vector3(
                _graLRNum * graUIMoveMagNum + graImageInitialPos.x,
                _graFBNum * graUIMoveMagNum + graImageInitialPos.y,
                graImageInitialPos.z
            );
    }

    // 重心移動矢印のSpriteを管理する関数
    public void SetArrowSprite(int _playState, float _angDifAbs, bool _isAttack, bool _isHataki)
    {
        switch(_playState)
        {
            case 1:
            case 2:
                if(_angDifAbs < 120f && _isAttack)
                {
                    graMoveUpArrow.sprite = graMoveSprite[0];
                    graMoveLeftArrow.sprite = graMoveSprite[0];
                    graMoveRightArrow.sprite = graMoveSprite[0];
                }
                else
                {
                    graMoveUpArrow.sprite = graMoveSprite[1];
                    graMoveLeftArrow.sprite = graMoveSprite[1];
                    graMoveRightArrow.sprite = graMoveSprite[1];
                }

                if((_angDifAbs < 60f || 120f < _angDifAbs) && _isAttack)
                {
                    graMoveDownArrow.sprite = graMoveSprite[0];
                }
                else
                {
                    graMoveDownArrow.sprite = graMoveSprite[1];
                }
                break;
            case 3:
                graMoveDownArrow.sprite = graMoveSprite[1];

                if(_angDifAbs < 60f && _isAttack)
                {
                    graMoveUpArrow.sprite = graMoveSprite[0];
                    graMoveLeftArrow.sprite = graMoveSprite[0];
                    graMoveRightArrow.sprite = graMoveSprite[0];
                }
                else
                {
                    graMoveUpArrow.sprite = graMoveSprite[1];
                    graMoveLeftArrow.sprite = graMoveSprite[1];
                    graMoveRightArrow.sprite = graMoveSprite[1];
                }
                break;
            case 4:
                graMoveUpArrow.sprite = graMoveSprite[1];
                
                if(_angDifAbs < 60f && _isHataki)
                {
                    graMoveDownArrow.sprite = graMoveSprite[0];
                    graMoveLeftArrow.sprite = graMoveSprite[0];
                    graMoveRightArrow.sprite = graMoveSprite[0];
                }
                else
                {
                    graMoveDownArrow.sprite = graMoveSprite[1];
                    graMoveLeftArrow.sprite = graMoveSprite[1];
                    graMoveRightArrow.sprite = graMoveSprite[1];
                }
                break;
        }
    }

    // 重心移動方向のUIを表示する関数
    public void SetArrowActive(int _arrowDir, bool _isActive)
    {
        switch(_arrowDir)
        {
            case 0:
                graMoveUpArrow.gameObject.SetActive(_isActive);
                break;
            case 1:
                graMoveDownArrow.gameObject.SetActive(_isActive);
                break;
            case 2:
                graMoveLeftArrow.gameObject.SetActive(_isActive);
                break;
            case 3:
                graMoveRightArrow.gameObject.SetActive(_isActive);
                break;
        }
    }

    // 抵抗値をUI表示する関数
    public void SetDragUI(float _dragNum)
    {
        dragSlider.value = _dragNum;
    }
    #endregion
    
    #region ゲーム終了後のUI
    // ゲーム結果の表示
    public void GameResult(int _result)
    {
        resultImage.gameObject.SetActive(true);
        resultImage.sprite = resultSprite[_result];
    }

    // 再度遊ぶ際にUIをResetする関数
    public void SetResetUI()
    {
        inGamePanel.SetActive(false);
        resultImage.gameObject.SetActive(false);
        weightSlider.interactable = true;
        weightSlider.value = weightInitialNum;
        SetWeightText(weightSlider.value);
        SetTachiaiAppear(true);
        SetPenaltyText(0);
        tachiaiSuccessImage.sprite = tachiaiSuccessSprite[0];
        gravityPanel.color = new Color32(255, 255, 255, 100);
        SetPlayStyleUI(0);
        SetDragPanel(false);
        SetArrowActive(0, false);
        SetArrowActive(1, false);
        SetArrowActive(2, false);
        SetArrowActive(3, false);
        SetArrowPatImage(0);
        SetFootOpeActive(false, false);
    }
    #endregion
}