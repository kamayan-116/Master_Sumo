using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DohyoJudgeCtrl : MonoBehaviour
{
    [SerializeField] private RikishiManager me;  // 自身の全体スクリプト
    [SerializeField] private RikishiManager enemy;  // 相手の全体スクリプト

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Dohyo")
        {
            me.SetResult(true, false, true, false);
            enemy.SetResult(true, true, false, false);
        }
    }
}
