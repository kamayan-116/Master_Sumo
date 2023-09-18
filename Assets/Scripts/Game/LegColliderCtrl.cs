using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegColliderCtrl : MonoBehaviour
{
    private Vector3 colliderAngle;  // 当たり判定のワールド角度

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        colliderAngle = this.transform.eulerAngles;
        colliderAngle.x = 0f;
        colliderAngle.y = 0f;
        colliderAngle.z = 0f;
        this.transform.eulerAngles = colliderAngle;
    }
}
