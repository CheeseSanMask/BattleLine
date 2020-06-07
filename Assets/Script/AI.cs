using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    // 敵手札
    private CardData[] _opponentCards;

    // 手札優先度
    private int[] _opponentCardPriority;

    // ターゲットフィールド
    private Transform[] _targetField;

    // プレイヤーの手札の位置
    private GameObject _playerField;

    // 対戦相手の手札の位置
    private GameObject[] _opponentFields;

    // カードマネージャ
    private CardManager _cardManager;

    // フラッグマネージャ
    private FlagManager _flagManager;

    // セーブ
    private Save _save;


    // コンストラクタ
    private void Start()
    {
        _opponentCards = GameObject.Find( "OpponentHand" ).GetComponentsInChildren<CardData>();
        _playerField   = GameObject.Find( "PlayerField" );

        _cardManager = GameObject.FindGameObjectWithTag( "CardManager" ).GetComponent<CardManager>();
        _flagManager = GameObject.FindGameObjectWithTag( "FlagManager" ).GetComponent<FlagManager>();
        _save = new Save();
    }


    // 初期化
    public void Init()
    {
        if( _opponentFields == null )
        {
            _opponentFields = new GameObject[_flagManager._flagNum];
            _targetField    = new Transform[_flagManager._flagNum];
            _opponentCardPriority  = new int[_cardManager.GetCardHandNum()];
        }
    }


    // アップデート
    private void Update()
    {
        if( _cardManager._nowTurn == Team.Opponent )
        {
            Init();

            for( int num = 0; num < _flagManager._flagNum; num++ )
            {
                _opponentFields[num] = GameObject.Find( "OpponentField"+( num+1 ) );
                _targetField[num]    = this.transform;
                _opponentCardPriority[num]  = 0;
            }

            SetPriority();
            ChooseCard();
        }
    }


    // 優先度の設定
    private void SetPriority()
    {
        for( int num = 0; num < _opponentFields.Length; num++ )
        {
            CardData[] cardDatas = _opponentFields[num].GetComponentsInChildren<CardData>();

            if ( cardDatas.Length == 0 )
            {
                SetPriorityCardNon( num );
            }
            else if( cardDatas.Length == 1 )
            {
                SetPriorityCardOne( cardDatas[0] );
            }
            else
            {
                SetPriorityCardTwo( cardDatas );
            }
        }
    }


    // フィールドに0枚
    private void SetPriorityCardNon( int number )
    {
        int tmpNum = 0;

        for( int num = 0; num < _opponentCards.Length; num++ )
        {
            if( _opponentCards[tmpNum]._number <= _opponentCards[num]._number )
            {
                tmpNum = num;
            }
        }

        _opponentCardPriority[number] += 1;
    }


    // フィールドに1枚
    private void SetPriorityCardOne( CardData cardData )
    {
        string parentName = cardData.transform.parent.name.Remove( 0, 13 );
        int fieldNum = int.Parse( parentName );
        Transform fieldParent = cardData.transform.parent;

        for( int num = 0; num < _opponentCards.Length; num++ )
        {
            if( ( cardData._number == _opponentCards[num]._number-1 )
            ||  ( cardData._number == _opponentCards[num]._number+1 )
            ){
                _opponentCardPriority[num] += 25;
                _opponentFields[fieldNum]   = fieldParent.gameObject;
            }

            if( cardData._number == _opponentCards[num]._number )
            {
                _opponentCardPriority[num] += 20;
                _opponentFields[fieldNum]   = fieldParent.gameObject;
            }

            if( ( cardData._number == _opponentCards[num]._number-2 )
            ||  ( cardData._number == _opponentCards[num]._number+2 )
            ){
                _opponentCardPriority[num] += 10;
                _opponentFields[fieldNum]   = fieldParent.gameObject;
            }

            if( cardData._color == _opponentCards[num]._color )
            {
                _opponentCardPriority[num] += 15;
                _opponentFields[fieldNum]   = fieldParent.gameObject;
            }
        }
    }

    // フィールドに2枚
    private void SetPriorityCardTwo( CardData[] cardDatas )
    {
        string parentName = cardDatas[0].transform.parent.name.Remove( 0, 13 );
        int fieldNum = int.Parse( parentName );
        Transform fieldParent = cardDatas[0].transform.parent;

        if( cardDatas[1]._number < cardDatas[0]._number )
        {
            CardData tmpData = cardDatas[0];
            cardDatas[0] = cardDatas[1];
            cardDatas[1] = tmpData;
        }

        for( int num = 0; num < _opponentCards.Length; num++ )
        {
            if( ( _opponentCards[num]._number == cardDatas[0]._number-1 )
            &&  ( _opponentCards[num]._number == cardDatas[1]._number+1 )
            ){
                _opponentCardPriority[num] += 40;
                _opponentFields[fieldNum]   = fieldParent.gameObject;
            }

            if( ( _opponentCards[num]._number == cardDatas[0]._number )
            &&  ( _opponentCards[num]._number == cardDatas[1]._number )
            ){
                _opponentCardPriority[num] += 35;
                _opponentFields[fieldNum]   = fieldParent.gameObject;
            }

            if( cardDatas[0]._number == cardDatas[1]._number+1 )
            {
                if( ( _opponentCards[num]._number == cardDatas[0]._number-1 )
                ||  ( _opponentCards[num]._number == cardDatas[1]._number+1 )
                ){
                    _opponentCardPriority[num] += 30;
                    _opponentFields[fieldNum]   = fieldParent.gameObject;
                }
            }

            if( ( _opponentCards[num]._color == cardDatas[0]._color )
            &&  ( _opponentCards[num]._color == cardDatas[1]._color )
            ){
                _opponentCardPriority[num] += 25;
                _opponentFields[fieldNum]   = fieldParent.gameObject;
            }
        }
    }

    private void ChooseCard()
    {
        int priorityNum = 0;

        for( int num = 0; num < _opponentCardPriority.Length; num++ )
        {
            if( _opponentCardPriority[priorityNum] < _opponentCardPriority[num] )
            {
                priorityNum = num;
            }
        }

        _opponentCards[priorityNum].transform.parent = _opponentFields[priorityNum].transform;

        if( _cardManager.GetStrageTopNum() < _cardManager.GetMaxNum() )
        {
            CardData cardData = _cardManager.CardInstantiate( GameObject.Find( "OpponentHand" ).transform );
            _opponentCards[priorityNum] = cardData;
        }
        else
        {
            CardData cardData = _opponentCards[priorityNum];
            CardData[] cardDatas = _opponentCards;
            _opponentCards = new CardData[_opponentCards.Length-1];
            int cardsNum = 0;

            for( int num = 0; num < cardDatas.Length; num++ )
            {
                if( cardData != cardDatas[num] )
                {
                    _opponentCards[cardsNum] = cardDatas[num];
                }
            }
        }

        _cardManager._nowTurn = Team.Player;
    }
}


//    private void CardNon()
//    {
//        int tmpNum = 0;

//        for( int num = 0; num < _opponentCards.Length; num++ )
//        {
//            if( _opponentCards[tmpNum]._number < _opponentCards[num]._number )
//            {
//                tmpNum = num;
//            }
//        }

//        _opponentCards[tmpNum].transform.parent = _opponentFields[0].transform;
//        //_save.SetCardPlayOrder( _opponentCards[tmpNum] );
//    }


//    // カードを見る
//    private void CardSerch( CardData[] cardDatas )
//    {
//        if( cardDatas.Length == 1 )
//        {
//            SerchOneCard( cardDatas[0] );
//        }

//        if( cardDatas.Length == 2 )
//        {
//            SerchTwoCard( cardDatas );
//        }

//        if( cardDatas.Length == 0 )
//        {

//        }

//        int tmpNumber = 0;

//        for( int num = 0; num < _opponentCardPriority.Length; num++ )
//        {
//            if( _opponentCardPriority[tmpNumber] < _opponentCardPriority[num] )
//            {
//                tmpNumber = num;
//            }
//        }

//        _opponentCards[tmpNumber].transform.parent = _targetField[tmpNumber];
//        //_save.SetCardPlayOrder( _opponentCards[tmpNumber] );
//    }


//    private void SerchNoCard()
//    {
//        int tmpNum = 0;

//        for( int num = 0; num < _opponentCards.Length; num++ )
//        {
//            if( _opponentCards[tmpNum]._number < _opponentCards[num]._number )
//            {
//                tmpNum = num;
//            }
//        }
//    }


//    // 場に出ているのが1枚
//    private void SerchOneCard( CardData cardData )
//    {
//        string parentName = cardData.transform.parent.name.Remove( 0, 13 );
//        int fieldNum = int.Parse( parentName );

//        for( int num = 0; num < _opponentCards.Length; num++ )
//        {
//            if( ( cardData._number == _opponentCards[num]._number-1 )
//            ||  ( cardData._number == _opponentCards[num]._number+1 )
//            ){
//                _opponentCardPriority[fieldNum] += 20;
//                _targetField[fieldNum] = cardData.transform.parent;
//            }

//            if( cardData._number == _opponentCards[num]._number )
//            {
//                _opponentCardPriority[fieldNum] += 15;
//                _targetField[fieldNum] = cardData.transform.parent;
//            }

//            if( ( cardData._number == _opponentCards[num]._number-2 )
//            ||  ( cardData._number == _opponentCards[num]._number+2 )
//            ){
//                _opponentCardPriority[fieldNum] += 10;
//                _targetField[fieldNum] = cardData.transform.parent;
//            }

//            if( cardData._color == _opponentCards[num]._color )
//            {
//                _opponentCardPriority[fieldNum] += 15;
//                _targetField[fieldNum] = cardData.transform.parent;
//            }
//        }
//    }


//    // 場に出ているのが2枚
//    private void SerchTwoCard( CardData[] cardDatas )
//    {
//        string parentName = cardDatas[0].transform.parent.name.Remove( 0, 13 );
//        int fieldNum = int.Parse( parentName );

//        if( cardDatas[1]._number < cardDatas[0]._number )
//        {
//            CardData tmpData = cardDatas[0];
//            cardDatas[0] = cardDatas[1];
//            cardDatas[1] = tmpData;
//        }

//        for( int num = 0; num < _opponentCards.Length; num++ )
//        {
//            if( cardDatas[0]._number == cardDatas[1]._number-1 )
//            {
//                if( ( _opponentCards[num]._number == cardDatas[0]._number-1 )
//                ||  ( _opponentCards[num]._number == cardDatas[1]._number+1 )
//                ){
//                    _opponentCardPriority[fieldNum] += 10;
//                    _targetField[fieldNum] = cardDatas[0].transform.parent;
//                }
//            }

//            if( cardDatas[0]._number == cardDatas[1]._number+2 )
//            {
//                if( ( _opponentCards[num]._number == cardDatas[0]._number-2 )
//                ||  ( _opponentCards[num]._number == cardDatas[1]._number+2 )
//                ){
//                    _opponentCardPriority[fieldNum] += 10;
//                    _targetField[fieldNum] = cardDatas[0].transform.parent;
//                }
//            }

//            if( cardDatas[0]._number == cardDatas[1]._number )
//            {
//                if( _opponentCards[num]._number == cardDatas[0]._number )
//                {
//                    _opponentCardPriority[fieldNum] += 20;
//                    _targetField[fieldNum] = cardDatas[0].transform.parent;
//                }
//            }

//            if( cardDatas[0]._color == cardDatas[1]._color )
//            {
//                if( _opponentCards[num]._color == cardDatas[1]._color )
//                {
//                    _opponentCardPriority[fieldNum] += 15;
//                    _targetField[fieldNum] = cardDatas[0].transform.parent;
//                }
//            }
//        }
//    }
//}


/*
場に出ているカードを見る
自分の手札を見る
場に出ているとき、手札のカードを見て役を作れるカードがあるなら出す
出ていないときは空いている場に一番大きいカードを出す
*/
