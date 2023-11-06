using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rikishi2UIManager : MonoBehaviour
{
    #region 変数宣言
    [SerializeField] private Rikishi2Manager rikishiManager;  // RikishiManagerプログラム
    [SerializeField] private Text resultText;  // 結果テキスト
    [SerializeField] private GameObject playerPanel;  // プレイヤーパネル
    [SerializeField] private GameObject inGamePanel;  // ゲーム中のUIパネル
    [SerializeField] private Text matchiResultText;  // 勝敗結果のテキスト
    #region 体重関連
    [SerializeField] private Image weightPanel; // 体重パネルのImage
    [SerializeField] private Text weightText; // 体重テキスト
    [SerializeField] private Slider weightSlider;  // 体重スライダー
    [SerializeField] private Button decideButton;  // 体重決定ボタン
    [SerializeField] private float weightInitialNum;  // 体重の初期値
    #endregion
    #region 立会いパネルや立会いの入力UI
    [SerializeField] private Image tachiaiPanel; // 立合いパネルのImage
    [SerializeField] private Image tachiaiInputImage; // 立合いの入力に応じた画像
    [SerializeField] private Sprite[] tachiaiInputSprite; // 立合いの入力に応じた画像配列（0が開始前、1が立会い時）
    [SerializeField] private Image playImage; // プレイ状態のUI画像
    [SerializeField] private Sprite[] playSprite; // プレイ状態の画像配列（0が立合い、1が四つ、2がまわし、3が押し、4がはたき）
    [SerializeField] private Text penaltyText; // 立会いのペナルティテキスト
    #endregion
    #region 重心関連
    [SerializeField] private Image gravityPanel; // 重心座標パネルのImage
    [SerializeField] private Image gravityImage; // 重心座標のUI画像
    private float graUIMoveMagNum;  // 重心UIの移動倍率数値
    [SerializeField] private Image graMoveUpArrow; // 上方向重心移動可能画像
    [SerializeField] private Image graMoveDownArrow; // 下方向重心移動可能画像
    [SerializeField] private Image graMoveLeftArrow; // 左方向重心移動可能画像
    [SerializeField] private Image graMoveRightArrow; // 右方向重心移動可能画像
    [SerializeField] private Sprite[] graMoveSprite; // 重心移動画像
    #endregion
    #region 操作関連
    [SerializeField] private Image ArrowInputImage; // 方向パッドの入力状態のUI画像
    [SerializeField] private Text ArrowInputText;
    [SerializeField] private Sprite[] ArrowInputSprite; // 方向パッドの入力状態の画像配列（0が未入力、1が上入力、2が下入力、3が左入力、4が右入力）
    [SerializeField] private Image lFCircleImage; // 左足の操作中のUI画像
    [SerializeField] private Image rFCircleImage; // 右足の操作中のUI画像
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
        lfCircley = lFCircleImage.gameObject.transform.position.y;
        rfCircley = rFCircleImage.gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        rikishiManager.SetWeightNum(weightSlider.value);
        if(tachiaiInputImage.gameObject.activeSelf && Game2Manager.Instance.gameState == Game2Manager.GameState.Play)
        {
            SetBlinkTachiai();
        }
    }

    // 重心値のUIの移動値を計算する関数
    public void SetGraUIMoveMagNum(float _graMax)
    {
        graUIMoveMagNum = 140f / _graMax;
    }

    // プレイヤー人数に対するUIの配置
    public void SetUIPlace(int _playerNum)
    {
        switch(Game2Manager.Instance.gamePlayer)
        {
            case Game2Manager.GamePlayer.One:
                switch(_playerNum)
                {
                    case 1:
                        tachiaiPanel.rectTransform.localPosition = new Vector3(-735f, 450f, 0);
                        gravityPanel.rectTransform.localPosition = new Vector3(-835f, -415f, 0);
                        ArrowInputImage.rectTransform.localPosition = new Vector3(885f, -390f, 0);
                        break;
                }
                break;
            case Game2Manager.GamePlayer.Two:
                switch(_playerNum)
                {
                    case 1:
                        tachiaiPanel.rectTransform.localPosition = new Vector3(-255f, 450f, 0);
                        gravityPanel.rectTransform.localPosition = new Vector3(-355f, -415f, 0);
                        ArrowInputImage.rectTransform.localPosition = new Vector3(405f, -390f, 0);
                        break;
                    case 2:
                        tachiaiPanel.rectTransform.localPosition = new Vector3(255f, 450f, 0);
                        gravityPanel.rectTransform.localPosition = new Vector3(355f, -415f, 0);
                        ArrowInputImage.rectTransform.localPosition = new Vector3(-405f, -390f, 0);
                        break;
                }
                break;
        }
    }

    #region 体重に関するスクリプト
    // 体重パネルの表示状態を管理する関数
    public void SetWeightPanel(bool _isActive)
    {
        weightPanel.gameObject.SetActive(_isActive);
    }

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
        weightText.text = "Weight：" + _weightNum.ToString("f0") + "Kg";
    }

    // 決定ボタンを押して体重の入力を行った
    public void SetWeightInput()
    {   
        decideButton.interactable = false;
        weightSlider.interactable = false;
        weightPanel.color = new Color32(0, 0, 0, 200);
        rikishiManager.WeightInput();
    }
    #endregion

    #region プレイ中のUI
    // プレイヤーパネルの表示状態を管理する関数
    public void SetPlayerPanel(bool _isActive)
    {
        playerPanel.SetActive(_isActive);
    }

    // ゲーム中パネルの表示状態を管理する関数
    public void SetInGamePanel(bool _isActive)
    {
        inGamePanel.SetActive(_isActive);
    }

    // 勝敗結果のテキストを管理する関数
    public void SetMatchResultText(int _winsNum, int _lossesNum)
    {
        matchiResultText.text = _winsNum + "勝 " + _lossesNum + "敗";
    }

    // 立会いのBボタンの画像の表示状態に関する関数
    public void SetTachiaiBActive(bool _isActive)
    {   
        tachiaiInputImage.gameObject.SetActive(_isActive);
        if(Game2Manager.Instance.gameState == Game2Manager.GameState.Play)
        {
            tachiaiInputImage.sprite = tachiaiInputSprite[1];
        }
        if(tachiaiInputImage.gameObject.activeSelf && Game2Manager.Instance.gameState == Game2Manager.GameState.BeforePlay)
        {
            StartCoroutine("SetTachiaiDelete");
        }
    }

    // 立会いの入力画像をすぐに消させるコルーチン関数
    private IEnumerator SetTachiaiDelete()
    {
        yield return new WaitForSeconds(0.2f);
        SetTachiaiBActive(false);
    }

    // 立会いのBボタンの点滅を行う関数
    private void SetBlinkTachiai()
    {
        tachiaiInputImage.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / blinkingSpeed, 1.0f));
    }

    // 立会いの入力を行った
    public void SetTachiaiInput()
    {   
        tachiaiPanel.color = new Color32(0, 0, 0, 200);
    }

    // ペナルティ回数の数値をテキストに表示する関数
    public void SetPenaltyText(int _penaltyNum)
    {
        penaltyText.text = "✖：" + _penaltyNum + "回";
    }

    // プレイ中の画像を変更する関数
    public void SetPlayImage(int _playNum)
    {
        playImage.sprite = playSprite[_playNum];
    }

    // プレイ中の画像の点滅を行う関数
    public void SetBlinkPlay()
    {
        playImage.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / blinkingSpeed, 1.0f));
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

        // 仮：画像が来たら消す
        if(_operateNum == 1)
        {
            ArrowInputText.text = "攻";
        }
        else if(_operateNum == 2)
        {
            ArrowInputText.text = "守";
        }
        else
        {
            ArrowInputText.text = "";
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
    
    // 重心パネルの色を変更する関数
    public void SetGraPanelColor(int _playerNum)
    {
        switch(_playerNum)
        {
            case 0:
                gravityPanel.color = new Color32(255, 255, 255, 100);
                break;
            case 1:
                gravityPanel.color = new Color32(255, 0, 0, 100);
                break;
            case 2:
                gravityPanel.color = new Color32(0, 0, 255, 100);
                break;
            case 3:
                gravityPanel.color = new Color32(255, 0, 255, 100);
                break;
        }
    }

    // 重心値のUIを表示する関数
    public void SetGravityUI(float _graLRNum, float _graFBNum)
    {
        gravityImage.rectTransform.localPosition = 
            new Vector3(
                _graLRNum * graUIMoveMagNum,
                _graFBNum * graUIMoveMagNum,
                0
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
    #endregion
    
    #region ゲーム終了後のUI
    // ゲーム結果の表示
    public void GameResult(string _result, Color32 _color)
    {
        resultText.gameObject.SetActive(true);
        resultText.text = _result;
        resultText.color = _color;
    }

    // 再度遊ぶ際にUIをResetする関数
    public void SetResetUI()
    {
        inGamePanel.SetActive(false);
        resultText.gameObject.SetActive(false);
        decideButton.interactable = true;
        weightSlider.interactable = true;
        weightPanel.color = new Color32(255, 255, 255, 100);
        weightSlider.value = weightInitialNum;
        SetWeightText(weightSlider.value);
        tachiaiPanel.color = new Color32(255, 255, 255, 100);
        tachiaiInputImage.sprite = tachiaiInputSprite[0];
        gravityPanel.color = new Color32(255, 255, 255, 100);
        SetPlayImage(0);
        SetPenaltyText(0);
        SetArrowActive(0, false);
        SetArrowActive(1, false);
        SetArrowActive(2, false);
        SetArrowActive(3, false);
        SetArrowPatImage(0);
        SetFootOpeActive(false, false);
    }
    #endregion
}