using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlagManager : MonoBehaviour
{
    // 獲得フラッグカウント
    private int[] _flagCount;

    // 勝利チーム
    public static Team _winTeam;

    // フラッグ本数
    public int _flagNum;

    // カード画像
    private Sprite[] _flagImages;

    // セーブ
    private Save _save;

    // 自分のフィールド
    public Transform _playerFieldTransform;

    // 敵のフィールド
    public Transform _opponentFieldTransform;

    // フィールドプレハブ
    [SerializeField] private GameObject _fieldPrefab;

    // フラッグプレハブ
    [SerializeField] private GameObject _flagPrefab;


    // コンストラクタ
    private void Start()
    {
        _flagCount = new int[2]{ 0, 0 };
        _flagNum   = ( SceneManager._battleMode == BattleMode.NormalBattle ? 9 : 5 );
        _flagImages = Resources.LoadAll<Sprite>( "flag" );

        _save = GameObject.FindGameObjectWithTag( "Save" ).GetComponent<Save>();

        _playerFieldTransform   = GameObject.Find( "PlayerField"   ).transform;
        _opponentFieldTransform = GameObject.Find( "OpponentField" ).transform;

        FieldInstantiate();
        FlagInstantiate();
    }


    // _flagCountのGet
    public int GetFlagCount( Team team )
    {
        return _flagCount[(int)team];
    }


    // _flagCountのSet
    public void SetFlagCount( Team team )
    {
        _flagCount[(int)team]++;
    }


    // アップデート
    private void Update()
    {
        for( int num = 0; num < _flagCount.Length; num++ )
        {
            if( _flagCount[num] == _flagNum/2+1 )
            {
                _winTeam = (Team)num;
                //_save.SaveBattleData();
                //_save.LoadBattleData();
                UnityEngine.SceneManagement.SceneManager.LoadScene( "ResultScene" );
            }
        }
    }


    // フィールド生成
    private void FieldInstantiate()
    {
        if( _flagNum == 5 )
        {
            _playerFieldTransform.position   -= new Vector3( 50, 0, 0 );
            _opponentFieldTransform.position -= new Vector3( 50, 0, 0 );
        }

        for( int num = 0; num < _flagNum; num++ )
        {
            GameObject playerField      = Instantiate( _fieldPrefab, _playerFieldTransform   );
            GameObject opponentField    = Instantiate( _fieldPrefab, _opponentFieldTransform );

            playerField.name    = "PlayerField"+(num+1);
            opponentField.name  = "OpponentField"+(num+1);

            playerField.tag   = "Field";
            opponentField.tag = "Field";
        }
    }


    // フラッグ生成
    private void FlagInstantiate()
    {
        Transform flagTransform  = GameObject.Find( "Flag" ).transform;

        for( int num = 0; num < _flagNum; num++ )
        {
            GameObject flag  = Instantiate( _flagPrefab,  flagTransform  );
            flag.name  = "Flag"+(num+1);
            flag.transform.localPosition = new Vector3( 100*num-400+( _flagNum == 5 ? 150 : 0 ), 0, 0 );
            flag.GetComponent<Image>().sprite = _flagImages[num];
        }
    }


    // 勝利陣営
    public static Team GetWinTeam()
    {
        return _winTeam;
    }
}