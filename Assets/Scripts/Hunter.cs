using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.GooglePlayServices;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Hunter : MonoBehaviour {
    
    public static Hunter Instance { get; set; }

    public Side side;
    public float movementSpeed;
    private Directions direction;
    private bool isMoving;
    private Vector3 defaultPosition;
    private float targetXPos = 0;

    
    void Awake ()
    {
        direction = Directions.None;
        isMoving = false;
        defaultPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != transform.GetChild(0).gameObject)
        {
            Cube cube = other.GetComponent<Cube>();
            Container.Instance.RemoveCube(other.gameObject);

            if (cube != null && cube.side == this.side)
            {
                ScoreManager.Instance.IncreaseScore();
                Debug.Log(ScoreManager.Score);
            }
            else
            {
               Camera.main.GetComponent<GameManager>().isGameOver = true;
            }
        }
        
    }
    public void RedSide()
    {
        if (side == Side.Red)
        {
            direction = Directions.Right;
            isMoving = true;
            targetXPos = defaultPosition.x + transform.GetComponent<BoxCollider>().bounds.extents.x * .8f;
        }

    }
    public void BlueSide()
    {
        if (side == Side.Blue)
        {
            direction = Directions.Left;
            isMoving = true;
            targetXPos = defaultPosition.x - transform.GetComponent<BoxCollider>().bounds.extents.x * .8f;
        }
    }

    void Update ()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (side == Side.Red)
            {
                direction = Directions.Right;
                isMoving = true;
                targetXPos = defaultPosition.x + transform.GetComponent<BoxCollider>().bounds.extents.x * .8f;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (side == Side.Blue)
            {
                direction = Directions.Left;
                isMoving = true;
                targetXPos = defaultPosition.x - transform.GetComponent<BoxCollider>().bounds.extents.x * .8f; 
            }
        }
        

        switch (direction)
        {
            case Directions.Left:
                {
                    MoveLeft();
                    break;
                }

            case Directions.Right:
                {
                    MoveRight();
                    break;
                }
            case Directions.Top:
                break;
            case Directions.Bottom:
                break;
            case Directions.None:
                break;
            default:
                break;
        }

        if (side == Side.Red)
        {
            if (transform.position.x >= targetXPos && isMoving)
            {
                direction = Directions.Left;
            }

            if ((transform.position.x <= defaultPosition.x) && isMoving)
            {
                transform.position = defaultPosition;
                direction = Directions.None;
                isMoving = false;
            }
        }

        else if (side == Side.Blue)
        {
            if ((transform.position.x <= targetXPos) && isMoving)
            {
                direction = Directions.Right;
            }

            if (transform.position.x >= defaultPosition.x && isMoving)
            {
                transform.position = defaultPosition;
                direction = Directions.None;
                isMoving = false;
            }
            
           
        }


    }

    public void MoveLeft()
    {
        transform.Translate(new Vector3(-movementSpeed * Time.deltaTime, 0, 0));
    }

    public void MoveRight()
    {
        transform.Translate(new Vector3(movementSpeed * Time.deltaTime, 0, 0));
    }

}
