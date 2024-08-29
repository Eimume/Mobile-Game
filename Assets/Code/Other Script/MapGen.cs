using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGen : MonoBehaviour
{
   public GameObject[] mapPrefabsOneExit; //R L T D - 4 object
    public GameObject[] mapPrefabsTwoExit; //12 object
    public string[] mapPrefabsDirection; //RL RT RD LR LT LD TD TR TL DT DR DL
    private Dictionary<GameObject, string> _mapPrefabsList = new Dictionary<GameObject, string>();

    protected Vector2 _nextRoomGenPosition;
    protected string _nextRoomGenDirection;
    public float _addPos = 10;

    public int maxRoom;
    public int minRoom;
    private int roomCounts;
    [HideInInspector] public static int currentRoomLeft;


    private void Start()
    {
        roomCounts = UnityEngine.Random.Range(minRoom, maxRoom + 1);
        currentRoomLeft = roomCounts;
        SetUpFirstRoom();
        Debug.Log(roomCounts+1);
    }

    private void Update()
    {
        

        if (currentRoomLeft != 0)
        {
            SetUpForNextRoom(_nextRoomGenDirection);
        }
        
    }

    void SetUpFirstRoom()
    {
        int RandomStartDirection = UnityEngine.Random.Range(0, 3); //1R 2L 3T 4D
        _nextRoomGenPosition = new Vector2Int(0, 0);
        Instantiate(mapPrefabsOneExit[RandomStartDirection], new Vector2(0, 0), Quaternion.identity);
        switch (RandomStartDirection)
        {
            case 0:
                _nextRoomGenDirection = "LR";
                break;
            case 1:
                _nextRoomGenDirection = "RL";
                break;
            case 2:
                _nextRoomGenDirection = "DT";
                break;
            case 3:
                _nextRoomGenDirection = "TD";
                break;
            default:
                break;
        }
    }

    void SetUpForNextRoom(string Direction)
    {
        if (currentRoomLeft > 0)
        {
            string exitDirection = Direction[1].ToString();
            string entryDirection = "";
            switch (exitDirection)
            {
                case "R":
                    entryDirection = "L";
                    break;
                case "L":
                    entryDirection = "R";
                    break;
                case "T":
                    entryDirection = "D";
                    break;
                case "D":
                    entryDirection = "T";
                    break;
                default:
                    break;
            }
            bool roomGenerated = false;

            while (!roomGenerated)
            {
                _nextRoomGenDirection = entryDirection;
                int RandomChanceToSwitchDirection = UnityEngine.Random.Range(0, 7);

                switch (RandomChanceToSwitchDirection)
                {
                    case 0:
                        _nextRoomGenDirection += "R";
                        break;
                    case 1:
                        _nextRoomGenDirection += "L";
                        break;
                    case 2:
                        _nextRoomGenDirection += "T";
                        break;
                    case 3:
                        _nextRoomGenDirection += "D";
                        break;
                    default:
                        _nextRoomGenDirection += exitDirection;
                        break;
                }

                
                if (_nextRoomGenDirection[1] != _nextRoomGenDirection[0])
                {
                    switch (_nextRoomGenDirection[1])
                    {
                        case 'R':
                            _nextRoomGenPosition = new Vector2(_nextRoomGenPosition.x + _addPos, _nextRoomGenPosition.y);
                            break;
                        case 'L':
                            _nextRoomGenPosition = new Vector2(_nextRoomGenPosition.x - _addPos, _nextRoomGenPosition.y);
                            break;
                        case 'T':
                            _nextRoomGenPosition = new Vector2(_nextRoomGenPosition.x, _nextRoomGenPosition.y + _addPos);
                            break;
                        case 'D':
                            _nextRoomGenPosition = new Vector2(_nextRoomGenPosition.x, _nextRoomGenPosition.y - _addPos);
                            break;
                    }

                    if (!CheckOverlap(_nextRoomGenPosition))
                    {
                        int index = Array.IndexOf(mapPrefabsDirection, _nextRoomGenDirection);
                        if (index != -1)
                        {
                            GameObject newRoom = Instantiate(mapPrefabsTwoExit[index], _nextRoomGenPosition, Quaternion.identity);
                            SetGameObjectName(newRoom, _nextRoomGenDirection);
                            currentRoomLeft--;
                            roomGenerated = true;
                        }
                        else
                        {
                            Debug.LogError("Cannot find prefab for direction: " + _nextRoomGenDirection);
                        }
                    }
                }
            }
        }
    }


    bool CheckOverlap(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, new Vector2(_addPos, _addPos), 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    void SetGameObjectName(GameObject obj, string direction)
    {
        obj.name = "Room_" + direction;
    }
}
