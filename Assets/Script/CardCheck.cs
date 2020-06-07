using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardCheck : MonoBehaviour
{
    // 隊列成立フラグ
    public bool _platoonFlag;

    // カードリスト
    private List<CardData> _cardList = new List<CardData>();

    // 隊列
    public Platoon _platoon;

    // 合計点数
    public int _totalNumber;

    // 現在時間
    public DateTime _timeNow;

    // コンストラクタ
    private void Start()
    {
        _platoonFlag = false;

        _platoon = Platoon.Unfinished;

        _totalNumber = 0;
    }


    // 隊列チェック
    public void CheckPlatoon( CardData[] cardDatas )
    {
        _timeNow = DateTime.Now;

        for( int num = 0; num < cardDatas.Length; num++ )
        {
            _cardList.Add( cardDatas[num] );
            _totalNumber += cardDatas[num]._number;
        }

        SortNum();

        if( Wedge() )
        {
            _platoon = Platoon.Wedge;
        }
        else if( Phalanx() )
        {
            _platoon = Platoon.Phalanx;
        }
        else if( Battalion() )
        {
            _platoon = Platoon.Battalion;
        }
        else if( Skirmisher() )
        {
            _platoon = Platoon.Skirmisher;
        }
        else
        {
            _platoon = Platoon.Host;
        }
    }


    // カードを数字順にソート
    private void SortNum()
    {
        CardData tmpData = new CardData();

        if( _cardList[0]._number > _cardList[1]._number )
        {
            tmpData = _cardList[0];
            _cardList[0] = _cardList[1];
            _cardList[1] = tmpData;
        }
        
        if( _cardList[2]._number < _cardList[0]._number )
        {
            tmpData = _cardList[2];
            _cardList[2] = _cardList[0];
            _cardList[0] = tmpData;
        }

        if( _cardList[1]._number > _cardList[2]._number )
        {
            tmpData = _cardList[1];
            _cardList[1] = _cardList[2];
            _cardList[2] = tmpData;
        }
    }


    // くさび形
    private bool Wedge()
    {
        bool flag = true;

        CardColor color = _cardList[0]._color;

        for( int num = 1; num <= 2; num++ )
        {
            if( color != _cardList[num]._color )
            {
                flag = false;
            }
        }

        int number = _cardList[0]._number;

        for( int num = 1; num <= 2; num++ )
        {
            if( number+num != _cardList[num]._number )
            {
                flag = false;
            }
        }

        return flag;
    }


    // 方陣
    private bool Phalanx()
    {
        bool flag = false;
        
        if( ( _cardList[0]._number == _cardList[1]._number )
        &&  ( _cardList[0]._number == _cardList[2]._number )
        ){
            flag = true;
        }

        return flag;
    }


    // 大隊
    private bool Battalion()
    {
        bool flag = false;
        
        if( ( _cardList[0]._color == _cardList[1]._color )
        &&  ( _cardList[0]._color == _cardList[2]._color )
        ){
            flag = true;
        }

        return flag;
    }


    // 散兵
    private bool Skirmisher()
    {
        bool flag = true;

        int number = _cardList[0]._number;

        for( int num = 1; num <= 2; num++ )
        {
            if( number+num != _cardList[num]._number )
            {
                flag = false;
            }
        }

        return flag;
    }
}