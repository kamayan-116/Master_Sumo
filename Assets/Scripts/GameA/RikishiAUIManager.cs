using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RikishiAUIManager : MonoBehaviour
{
    [SerializeField] private RikishiAManager rikishiManager;  // RikishiManagerプログラム
    [SerializeField] private Text resultText; // 結果テキスト
    [SerializeField] private Image weightPanel; // 体重パネルのImage
    [SerializeField] private Text weightText; // 体重テキスト
    [SerializeField] private Slider weightSlider;  // 体重スライダー
    [SerializeField] private Button decideButton;  // 体重決定ボタン
    [SerializeField] private GameObject[] inputJoySticks;  // 足の入力JoyStick
    [SerializeField] private float weightInitialNum;  // プレイヤー全体の初期座標


    // Start is called before the first frame update
    void Start()
    {
        InputPushEnable(false, inputJoySticks);
        weightInitialNum = weightSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        rikishiManager.SetWeightNum(weightSlider.value);
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

    // 決定ボタンを押した
    public void DecidePushDown()
    {   
        decideButton.interactable = false;
        weightSlider.interactable = false;
        weightPanel.color = new Color32(0, 0, 0, 200);
        rikishiManager.WeightInput();
    }

    // ゲーム結果の表示
    public void GameResult(string _result, Color32 _color)
    {
        resultText.gameObject.SetActive(true);
        resultText.text = _result;
        resultText.color = _color;
    }

    // ゲーム開始や終了時に呼ばれる関数
    public void BattleStart(bool _start)
    {
        InputPushEnable(_start, inputJoySticks);
    }

    // 入力可否を行う関数
    public void InputPushEnable(bool _stickBool, GameObject[] _joySticks)
    {
        foreach(GameObject joyStick in _joySticks)
        {
            joyStick.SetActive(_stickBool);
        }
    }

    // 再度遊ぶ際にUIをResetする関数
    public void SetResetUI()
    {
        resultText.gameObject.SetActive(false);
        decideButton.interactable = true;
        weightSlider.interactable = true;
        weightPanel.color = new Color32(0, 0, 0, 0);
        weightSlider.value = weightInitialNum;
        SetWeightText(weightSlider.value);
    }
}
