using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineJudgeCtrl : MonoBehaviour
{
    [SerializeField] private RikishiManager me;  // 自身の全体スクリプト
    [SerializeField] private RikishiManager enemy;  // 相手の全体スクリプト

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Dohyo")
        {
            me.SetResult(true, false);
            enemy.SetResult(true, true);
        }
    }
}
