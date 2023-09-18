using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spine2JudgeCtrll : MonoBehaviour
{
    [SerializeField] private Rikishi2Manager me;  // 自身の全体スクリプト
    [SerializeField] private Rikishi2Manager enemy;  // 相手の全体スクリプト

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Dohyo")
        {
            me.SetResult(true, false);
            enemy.SetResult(true, true);
        }
    }
}