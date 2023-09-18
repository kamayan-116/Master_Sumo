using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SendMessage : MonoBehaviour
{
    [SerializeField] private int playerNum;  // プレイヤーナンバー
    [SerializeField] private PlayerManager player;  // プレイヤースクリプト
    Vector2 lfAxis1;  // プレイヤー1の左足の入力値
    Vector2 rfAxis1;  // プレイヤー1の右足の入力値
    Vector2 lfAxis2;  // プレイヤー2の左足の入力値
    Vector2 rfAxis2;  // プレイヤー2の右足の入力値

    // 通知を受け取るメソッド名は「On + Action名」である必要がある
    private void OnLeftFootMove1(InputValue value)
    {
        // LeftFootMoveActionの入力値を取得
        lfAxis1 = value.Get<Vector2>();
    }

    private void OnLeftFootMove2(InputValue value)
    {
        // LeftFootMoveActionの入力値を取得
        lfAxis2 = value.Get<Vector2>();
    }

    private void OnRightFootMove1(InputValue value)
    {
        // RightFootMoveActionの入力値を取得
        rfAxis1 = value.Get<Vector2>();
    }

    private void OnRightFootMove2(InputValue value)
    {
        // RightFootMoveActionの入力値を取得
        rfAxis2 = value.Get<Vector2>();
    }

    private void Update()
    {
        switch(playerNum)
        {
            case 1:
                player.SetLeftFootNum(lfAxis1.x, lfAxis1.y);
                player.SetRightFootNum(rfAxis1.x, rfAxis1.y);
                break;
            case 2:
                player.SetLeftFootNum(lfAxis2.x, lfAxis2.y);
                player.SetRightFootNum(rfAxis2.x, rfAxis2.y);
                break;
        }
    }
}
