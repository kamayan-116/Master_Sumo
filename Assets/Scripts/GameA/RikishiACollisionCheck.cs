using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RikishiACollisionCheck : MonoBehaviour
{
    [SerializeField] private RikishiAManager me;  // 自身の全体スクリプト
    
    private void OnCollisionEnter(Collision other)
    {
        switch(me.playerNum)
        {
            case 1:
                if(other.gameObject.tag == "Rikishi2")
                {
                    me.SetCollision(true);
                }
                break;
            case 2:
                if(other.gameObject.tag == "Rikishi1")
                {
                    me.SetCollision(true);
                }
                break;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        switch(me.playerNum)
        {
            case 1:
                if(other.gameObject.tag == "Rikishi2")
                {
                    me.SetCollision(false);
                }
                break;
            case 2:
                if(other.gameObject.tag == "Rikishi1")
                {
                    me.SetCollision(false);
                }
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch(me.playerNum)
        {
            case 1:
                if(other.gameObject.tag == "Rikishi2")
                {
                    me.SetCollision(true);
                }
                break;
            case 2:
                if(other.gameObject.tag == "Rikishi1")
                {
                    me.SetCollision(true);
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch(me.playerNum)
        {
            case 1:
                if(other.gameObject.tag == "Rikishi2")
                {
                    me.SetCollision(false);
                }
                break;
            case 2:
                if(other.gameObject.tag == "Rikishi1")
                {
                    me.SetCollision(false);
                }
                break;
        }
    }
}
