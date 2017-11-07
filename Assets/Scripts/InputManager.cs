using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : EventSystem, IPointerClickHandler
{
    public Directions direction;
    private Hunter hunter;
    
    protected override void Start()
    {
        if (direction == Directions.Left)
        {
            hunter = GameObject.Find("Blue Hunter").GetComponent<Hunter>() as Hunter;
        }
        else
        {
            hunter = GameObject.Find("Red Hunter").GetComponent<Hunter>() as Hunter;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (direction == Directions.Left)
        {
            hunter.BlueSide();
        }
        else
        {
            hunter.RedSide();
        }
    }
}
