using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMove : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // 親
    public Transform _parent;

    // ゲームマネージャ
    private GameObject _card;

    // カードデータ
    private CardData _cardData;

    // 座標
    private Vector2 _posOrizin;

    // 移動フラグ
    private bool _moveFlag;

    // ゲームマネージャ
    private CardManager _cardManager;

    
    // コンストラクタ
    private void Start()
    {
        GameObject gameObject = GameObject.FindWithTag( "CardManager" );

        _cardManager = gameObject.GetComponent<CardManager>();
        _cardData = this.GetComponent<CardData>();
        _posOrizin = this.transform.localPosition;
        _moveFlag = false;
    }


    // ドラッグし始めた瞬間
    public void OnBeginDrag( PointerEventData eventData )
    {
        if( this.transform.parent.tag == "Field" )
        {
            return;
        }

        _moveFlag = ( _cardData._team == _cardManager._nowTurn );

        _parent = this.transform.parent;
        transform.SetParent( _parent.parent, false );

        GetComponent<CanvasGroup>().blocksRaycasts = !_moveFlag;
    }


    // ドラッグ中
    public void OnDrag( PointerEventData eventData )
    {
        this.transform.position = eventData.position;
    }


    // ドラッグ終了した瞬間
    public void OnEndDrag( PointerEventData eventData )
    {
        transform.SetParent( _parent, false );
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
