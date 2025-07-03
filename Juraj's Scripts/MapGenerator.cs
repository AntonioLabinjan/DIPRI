using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGenerator 
{
    const int GRID_SIZE = 100;
    bool[,] occupied = new bool[GRID_SIZE, GRID_SIZE];
    List<Room> rooms = new List<Room>();
    System.Random rnd = new System.Random();

    void Start()
    {
        MapGenerator gen = new MapGenerator();
        gen.Generate(25);
    }

    public void Generate(int maxRooms = 20)
    {
        TryPlaceRoom(RoomType.Hallway,
                     GRID_SIZE / 2, GRID_SIZE / 2,
                     new Vector2Int(1, 0),
                     out Room startHall);

        AddRandomHallwayDoors(startHall);

        for (int i = 0; i < maxRooms; i++)
        {
            bool placed = false;
            for (int attempt = 0; attempt < 10 && !placed; attempt++)
            {
                var parent = rooms[rnd.Next(rooms.Count)];
                if (parent.doors.Count == 0) continue;

                var door = parent.doors[rnd.Next(parent.doors.Count)];
                RoomType nextType = PickNextRoomType(parent.type);
                int nx = door.x + door.direction.x;
                int ny = door.y + door.direction.y;

                if (TryPlaceRoom(nextType, nx, ny, door.direction, out Room newRoom))
                {
                    AddDoor(newRoom, nx, ny, -door.direction,
                            locked: rnd.NextDouble() < 0.25);
                    placed = true;
                }
            }
            if (!placed)
                Debug.LogWarning($"Couldn’t place room #{i} after 10 tries.");
        }

        Debug.Log($"✅ Generated {rooms.Count} rooms:");
        foreach (var r in rooms)
            Debug.Log($" {r.type} @({r.x},{r.y}) size {r.width}x{r.height}, doors:{r.doors.Count}, items:{r.items.Count}");
    }

    RoomType PickNextRoomType(RoomType parent)
    {
        if (parent == RoomType.Hallway)
            return rnd.NextDouble() < 0.2 ? RoomType.StairUp : RoomType.GuestRoom;
        if (parent == RoomType.StairUp || parent == RoomType.StairDown)
            return RoomType.Hallway;
        return RoomType.Hallway;
    }

    bool TryPlaceRoom(RoomType type, int anchorX, int anchorY, Vector2Int dir, out Room room)
    {
        int w = 1, h = 1;
        switch (type)
        {
            case RoomType.GuestRoom:
                w = rnd.Next(2, 7);
                h = rnd.Next(2, 7);
                break;
            case RoomType.Hallway:
                w = rnd.Next(5, 16);
                h = 2; break;
            case RoomType.StairUp:
            case RoomType.StairDown:
                w = 2; h = 2; break;
        }
        int ox = anchorX - (dir.x == 1 ? 0 : w - 1);
        int oy = anchorY - (dir.y == 1 ? 0 : h - 1);

        for (int x = ox; x < ox + w; x++)
            for (int y = oy; y < oy + h; y++)
                if (x < 0 || y < 0 || x >= GRID_SIZE || y >= GRID_SIZE || occupied[x, y])
                {
                    room = null;
                    Debug.LogWarning($"❌ Failed to place {type} at anchor ({anchorX},{anchorY}) dir {dir}");

                    return false;

                }

        room = new Room { type = type, x = ox, y = oy, width = w, height = h };
        for (int x = ox; x < ox + w; x++)
            for (int y = oy; y < oy + h; y++)
                occupied[x, y] = true;

        room.doors.Add(new Door
        {
            x = anchorX,
            y = anchorY,
            direction = dir,
            isLocked = false
        });

        if (type == RoomType.Hallway)
            AddRandomHallwayDoors(room);

        if (type == RoomType.GuestRoom)
            PopulateGuestRoom(room);

        rooms.Add(room);
        return true;
    }

    void AddRandomHallwayDoors(Room hall)
    {
        int doors = Math.Max(1, hall.width / 3);
        for (int i = 0; i < doors; i++)
        {
            int dx = hall.x + rnd.Next(hall.width);
            int side = rnd.Next(2) == 0 ? 1 : -1;
            hall.doors.Add(new Door
            {
                x = dx,
                y = hall.y + (side > 0 ? hall.height - 1 : 0),
                direction = new Vector2Int(0, side),
                isLocked = false
            });
        }
    }

    void PopulateGuestRoom(Room room)
    {
        int items = rnd.Next(1, 4);
        for (int i = 0; i < items; i++)
        {
            int ix = room.x + rnd.Next(room.width);
            int iy = room.y + rnd.Next(room.height);
            string[] possible = { "Note", "Key", "Flashlight", "Notebook" };
            room.items.Add(new ItemSpawn
            {
                x = ix,
                y = iy,
                itemName = possible[rnd.Next(possible.Length)]
            });
        }
    }

    void AddDoor(Room r, int x, int y, Vector2Int dir, bool locked)
    {
        r.doors.Add(new Door { x = x, y = y, direction = dir, isLocked = locked });
    }
}
