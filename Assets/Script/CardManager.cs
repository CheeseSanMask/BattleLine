using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 隊列
public enum Platoon
{
    Wedge       = 500,  // くさび形
    Phalanx     = 400,  // 方陣
    Battalion   = 300,  // 大隊
    Skirmisher  = 200,  // 散兵
    Host        = 100,  // 烏合の衆
    Unfinished  =   0,  // 未完成
    Max                 // MAX
}


// 陣営
public enum Team
{
    Player,     // 自陣
    Opponent,   // 敵陣
    Non         // なし
}


public class CardManager : MonoBehaviour
{
    // 手札枚数
    private static readonly int CardHandNum = 7;

    // カード総枚数
    static public readonly int CardMaxNum = 60;

    // 操作可能陣営
    public Team _nowTurn;

    // カード画像
    private Sprite[] _cardImages;

    // 山札
    private List<CardData> _cardStrage;

    // 山札の一番上の枚数
    private int _storageTopNum;

    // カード枚数
    private int _maxNum;

    // プレイヤーの手札の位置
    [SerializeField] Transform _playerHandTransform;

    // 対戦相手の手札の位置
    [SerializeField] Transform _opponentHandTransform;

    // カードのprefab
    [SerializeField] GameObject _cardPrefab;


    // コンストラクタ
    private void Start()
    {
        _nowTurn = Team.Player;

        _cardStrage = new List<CardData>();

        _storageTopNum = 0;

        CardInit();

        CardShuffle();

        for( int num = 0; num < CardHandNum; num++ )
        {
            CardInstantiate( _playerHandTransform   );
            CardInstantiate( _opponentHandTransform );
        }
    }


    // カード初期化
    private void CardInit()
    {
        _cardImages = Resources.LoadAll<Sprite>( "card" );

        _maxNum = ( SceneManager._battleMode == BattleMode.NormalBattle ? CardMaxNum : CardMaxNum/2 );

        for( int num = 0; num < _maxNum; num++ )
        {
            CardColor color = (CardColor)( num/10 );
            int number      = num%10;

            CardData card = new CardData( color, number, Team.Non );
            _cardStrage.Add( card );
        }
    }


    // カードシャッフル
    private void CardShuffle()
    {
        CardData tempCard;
        int randomNum = 0;

        for( int num = 0; num < _cardStrage.Count; num++ )
        {
            tempCard = _cardStrage[num];
            randomNum = Random.Range( 0, _cardStrage.Count );

            _cardStrage[num] = _cardStrage[randomNum];
            _cardStrage[randomNum] = tempCard;
        }
    }


    // カード生成
    public CardData CardInstantiate( Transform hand )
    {
        int cardNum = (int)(_cardStrage[_storageTopNum]._color)*10+_cardStrage[_storageTopNum]._number;
        Sprite sprite = _cardImages[cardNum];
        GameObject card = Instantiate( _cardPrefab, hand );
        card.GetComponent<Image>().sprite = sprite;
        CardData cardData = card.GetComponent<CardData>();
        cardData._color  = _cardStrage[_storageTopNum]._color;
        cardData._number = _cardStrage[_storageTopNum]._number+1;
        cardData._team   = (Team)( _storageTopNum%2 );

        _storageTopNum++;

        return cardData;
    }


    // CardHandNumのGet
    public int GetCardHandNum()
    {
        return CardHandNum;
    }


    // _strageTopNumのGet
    public int GetStrageTopNum()
    {
        return _storageTopNum;
    }


    // _maxNumのGet
    public int GetMaxNum()
    {
        return _maxNum;
    }
}