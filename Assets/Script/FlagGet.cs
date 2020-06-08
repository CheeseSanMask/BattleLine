using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlagGet : MonoBehaviour
{
    // 確保陣営
    Team _team;

    // カードチェック自陣
    private CardCheck _cardCheckPlayer;

    // カードチェック敵陣
    private CardCheck _cardCheckOpponent;

    // フラッグマネージャ
    private FlagManager _flagManager;


    // コンストラクタ
    private void Start()
    {
        _team = Team.Non;

        int number = int.Parse( this.name.Remove( 0, 4 ) );

        GameObject[] field = GameObject.FindGameObjectsWithTag( "Field" );
        
        for( int num = 0; num < field.Length; num++ )
        {
            if( field[num].name == "PlayerField"+number )
            {
                _cardCheckPlayer = field[num].GetComponent<CardCheck>();
            }

            if( field[num].name == "OpponentField"+number )
            {
                _cardCheckOpponent = field[num].GetComponent<CardCheck>();
            }
        }

        _flagManager = GameObject.FindGameObjectWithTag( "FlagManager" ).GetComponent<FlagManager>();
    }


    // アップデート
    private void Update()
    {
        if( ( _team == Team.Non                 )
        &&  ( _cardCheckPlayer._platoonFlag     )
        &&  ( _cardCheckOpponent._platoonFlag   )
        ){
            FlagCheck();
        }
    }


    // フラッグチェック
    private void FlagCheck()
    {
        if( _cardCheckPlayer._platoon == _cardCheckOpponent._platoon )
        {
            if( _cardCheckPlayer._totalNumber == _cardCheckOpponent._totalNumber )
            {
                _team = ( _cardCheckPlayer._timeNow < _cardCheckOpponent._timeNow ? Team.Player : Team.Opponent );
            }
            else
            {
                _team = ( _cardCheckPlayer._totalNumber > _cardCheckOpponent._totalNumber ? Team.Player : Team.Opponent );
            }
        }
        else
        {
            _team = ( _cardCheckPlayer._platoon > _cardCheckOpponent._platoon ? Team.Player : Team.Opponent );
        }

        _flagManager.SetFlagCount( _team );

        Vector3 newPos = this.transform.localPosition+new Vector3( 0, ( _team == Team.Player ? -230 : +240 ), 0 );

        this.transform.localPosition = newPos;
    }
}
