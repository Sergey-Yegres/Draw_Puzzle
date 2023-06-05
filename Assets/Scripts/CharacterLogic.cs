using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLogic : MonoBehaviour
{
    private LineRenderer _line;
    [SerializeField] private Transform _destinationPoint;
    private GameManager GM;
    private void Start()
    {
        _line = GetComponent<LineRenderer>();

        _line.startColor = GetComponent<SpriteRenderer>().color;
        _line.endColor = GetComponent<SpriteRenderer>().color;
        GM = GameManager.Instance;
        GM.AddNewCharacterInList(this);
    }

    public bool LineAlreadyDrawn { get; private set; }
    public bool CharacterWin { get; private set; }
    private void OnMouseDrag()
    {
        if (LineAlreadyDrawn)
        {
            DeleteLine();
            LineAlreadyDrawn = false;
        }
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));

        if (_line.positionCount > 0)
        {
            if(_line.GetPosition(_line.positionCount - 1) != currentPosition)
                CreateNewPoint(currentPosition);
        }
        else
            CreateNewPoint(currentPosition);
        

    }
    private void CreateNewPoint(Vector3 currentPosition)
    {
        _line.positionCount++;
        _line.SetPosition(_line.positionCount - 1, currentPosition);
    }
    private void OnMouseUp()
    {
        Vector3 lastPoint = _line.GetPosition(_line.positionCount - 1);
        if (CompareCoordinates(lastPoint.x, _destinationPoint.position.x) && CompareCoordinates(lastPoint.y, _destinationPoint.position.y))
        {
            LineAlreadyDrawn = true;
            GM.CheckAllCharactersForPossibilityMove();
        }
        else
            DeleteLine();
    }
    private Coroutine _move;
    public void StartMove()
    {
        _move = StartCoroutine(Move());

        IEnumerator Move()
        {
            for (int i = 1; i < _line.positionCount; i++)
            {
                yield return new WaitForSeconds(10 * Vector3.Distance(_line.GetPosition(i - 1), _line.GetPosition(i)) * Time.fixedDeltaTime / GM.MoveSpeed);
                transform.position = _line.GetPosition(i);
            }
            CharacterWin = true;
            GM.CheckAllCharactersForWin();
        }
    }

    private void DeleteLine()
    {
        _line.positionCount = 0;
    }
    private bool CompareCoordinates(float lastPointCoordinate,float destinationPointCoordinate)
    {
        float DPSize = 1; //Destination point size
        if(lastPointCoordinate >= destinationPointCoordinate -  DPSize && lastPointCoordinate <= destinationPointCoordinate + DPSize)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CharacterLogic>())
        {
            StopCoroutine(_move);
            GM.OpenLoseScreen();
        }
    }
}
