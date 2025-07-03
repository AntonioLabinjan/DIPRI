using System.Collections.Generic;
using UnityEngine;

public enum RoomType { GuestRoom, Hallway, StairUp, StairDown }

public class Room
{
    public RoomType type;
    public int x, y, width, height;        
    public List<Door> doors = new List<Door>();
    public List<ItemSpawn> items = new List<ItemSpawn>();
}

public struct Door
{
    public int x, y;                      
    public Vector2Int direction;         
    public bool isLocked;
}

public struct ItemSpawn
{
    public int x, y;                      
    public string itemName;               
}

