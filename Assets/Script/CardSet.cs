using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSet : MonoBehaviour, IDropHandler
{
    // 隊列
    private Platoon _platoon;

    // カードマネージャ
    private CardManager _cardManager;

    // カードチェック
    private CardCheck _cardCheck;

    // セーブ
    private Save _save;


    // コンストラクタ
    private void Start()
    {
        _platoon = Platoon.Unfinished;
        _cardManager = GameObject.FindGameObjectWithTag( "CardManager" ).GetComponent<CardManager>();
        _cardCheck = this.GetComponent<CardCheck>();
        _save = GameObject.FindGameObjectWithTag( "Save" ).GetComponent<Save>();
    }


    // ドロップした時
    public void OnDrop( PointerEventData eventData )
    {
        CardMove card = eventData.pointerDrag.GetComponent<CardMove>();
        CardData cardData = card.GetComponent<CardData>();
        
        if( ( card                                    )
        &&  ( this.transform.childCount < 3           )
        &&  ( cardData._team == _cardManager._nowTurn )
        ){
            Transform hand = card._parent;
            card._parent = this.transform;

            if( _cardManager.GetStrageTopNum() < _cardManager.GetMaxNum() )
            {
                _cardManager.CardInstantiate( hand );
            }
            
            _cardManager._nowTurn = 1-_cardManager._nowTurn;
            //_save.SetCardPlayOrder( cardData );
        }
    }


    // アップデート
    private void Update()
    {
        if( ( !_cardCheck._platoonFlag       )
        &&  ( this.transform.childCount == 3 )
        ){
            CardData[] cardDatas = this.transform.GetComponentsInChildren<CardData>();
            _cardCheck.CheckPlatoon( cardDatas );
            _cardCheck._platoonFlag = true;
        }
    }
}
