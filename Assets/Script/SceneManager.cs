using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum BattleMode
{
    NormalBattle,
    ShortBattle,
    Undefined
}


public class SceneManager : MonoBehaviour
{
    // バトルモード
    public static BattleMode _battleMode;


    // コンストラクタ
    private void Start()
    {
        _battleMode = BattleMode.Undefined;
    }


    // アップデート
    private void Update()
    {
        if( _battleMode != BattleMode.Undefined )
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene( "MainScene" );
        }
    }


    // バトルモードのSet
    public void SetBattleMode( BattleMode battleMode )
    {
        _battleMode = battleMode;
    }
}
