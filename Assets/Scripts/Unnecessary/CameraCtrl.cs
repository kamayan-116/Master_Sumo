using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer p1Renderer;  // プレイヤー1のマテリアル
    [SerializeField] private SkinnedMeshRenderer p2Renderer;  // プレイヤー2のマテリアル

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 透過度を下げる関数
    private void OnPreRender()
    {
        if(this.tag == "MainCamera")
        {
            p1Renderer.material.color = new Color32(255, 255, 255, 255);
            p2Renderer.material.color = new Color32(255, 255, 255, 255);
        }

        if(this.tag == "P1Camera")
        {
            p1Renderer.material.color = new Color32(255, 255, 255, 191);
            p2Renderer.material.color = new Color32(255, 255, 255, 255);
        }

        if(this.tag == "P2Camera")
        {
            p1Renderer.material.color = new Color32(255, 255, 255, 255);
            p2Renderer.material.color = new Color32(255, 255, 255, 191);
        }
    }
}
