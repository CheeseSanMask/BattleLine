using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// カードの色
public enum CardColor
{
    Red,        // 赤
    Orange,     // 橙
    Purple,     // 紫
    Blue,       // 青
    Green,      // 緑
    Yellow,     // 黄
    Max         // MAX
}


public class CardData : MonoBehaviour
{
    // 色
    public CardColor _color;

    // 数字
    public int _number;

    // カード持ち主
    public Team _team;


    // デフォルトコンストラクタ
    public CardData()
    {
        _color  = CardColor.Max;
        _number = CardManager.CardMaxNum;
        _team   = Team.Non;
    }


    // 引数付きコンストラクタ
    public CardData( CardColor color, int number, Team team )
    {
        _color  = color;
        _number = number;
        _team   = team;
    }
}