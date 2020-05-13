using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMove : MonoBehaviour
{
    private Vector3 _move;
    private bool _beRay = false;
    private GameObject _card;
    [SerializeField] Camera _camera;
    [SerializeField] GameObject _setPos;

    private void Start()
    {
        _camera = Camera.main;
    }


    // アップデート
    private void Update()
    {
        if( Input.GetMouseButtonDown( 0 ) )
        {
            RayCheck();
        }

        if( _beRay )
        {
            MovePos();
        }
        else
        {
            SetCard();
        }

        if( Input.GetMouseButtonUp( 0 ) )
        {
            _beRay = false;
        }
    }


    // rayチェック
    private void RayCheck()
    {
        Ray ray = new Ray();
        RaycastHit hit = new RaycastHit();

        ray = Camera.main.ScreenPointToRay( Input.mousePosition );

        if( Physics.Raycast( ray, out hit ) )
        {
            _card = hit.collider.gameObject;
            _beRay = true;
        }

        if( _card == null )
        {
            _beRay = false;
        }
    }


    // 座標移動
    private void MovePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _camera.transform.position.y;

        _move = Camera.main.ScreenToWorldPoint( mousePos );
        _card.transform.position = _move;
    }


    // カード固定
    private void SetCard()
    {
        
    }
}