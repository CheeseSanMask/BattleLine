using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleSelectButton : MonoBehaviour
{
    // シーンセレクト
    private SceneManager _sceneManager;


    // コンストラクタ
    private void Start()
    {
        GameObject sceneManager = GameObject.FindGameObjectWithTag( "SceneManager" );

        _sceneManager = sceneManager.GetComponent<SceneManager>();
    }


    public void OnClick()
    {
        _sceneManager.SetBattleMode( (BattleMode)Enum.Parse( typeof( BattleMode ), this.name ) );
    }
}
