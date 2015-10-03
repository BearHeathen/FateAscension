using RogueSharp;
using RogueSharp.Random;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FateAscension
{
    public class MapCreationStrategy : IMapCreationStrategy<Map>
    {
        private static IRandom _random;
        private readonly int _height;
        private readonly int _maxRooms;
        private readonly int _roomMaxSize;
        private readonly int _roomMinSize;
        private readonly int _width;

        public MapCreationStrategy(int width, int height, int maxRooms, int roomMaxSize, int roomMinSize, IRandom random = null)
        {
            _width = width;
            _height = height;
            _maxRooms = maxRooms;
            _roomMaxSize = roomMaxSize;
            _roomMinSize = roomMinSize;
            _random = random ?? new DotNetRandom();
        }

        public Map CreateMap()
        {
            var rooms = new List<Microsoft.Xna.Framework.Rectangle>();
            var map = new Map();
            map.Initialize(_width, _height);

            for (int r = 0; r < _maxRooms; r++)
            {
                int roomWidth = _random.Next(_roomMinSize, _roomMaxSize);
                int roomHeight = _random.Next(_roomMinSize, _roomMaxSize);
                int roomXPosition = _random.Next(0, _width - roomWidth - 1);
                int roomYPosition = _random.Next(0, _height - roomHeight - 1);

                var newRoom = new Microsoft.Xna.Framework.Rectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);
                bool newRoomIntersects = false;
                foreach (Microsoft.Xna.Framework.Rectangle room in rooms)
                {
                    if (newRoom.Intersects(room))
                    {
                        newRoomIntersects = true;
                        break;
                    }
                }
                if (!newRoomIntersects)
                {
                    rooms.Add(newRoom);
                }
            }

            foreach (Microsoft.Xna.Framework.Rectangle room in rooms)
            {
                MakeRoom(map, room);
            }

            for (int r = 0; r < rooms.Count; r++)
            {
                if (r == 0)
                {
                    continue;
                }

                int previousRoomCenterX = rooms[r - 1].Center.X;
                int previousRoomCenterY = rooms[r - 1].Center.Y;
                int currentRoomCenterX = rooms[r].Center.X;
                int currentRoomCenterY = rooms[r].Center.Y;

                if (_random.Next(0, 2) == 0)
                {
                    MakeHorizontalTunnel(map, previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    MakeVerticalTunnel(map, previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
                }
                else
                {
                    MakeVerticalTunnel(map, previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    MakeHorizontalTunnel(map, previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
                }
            }

            return map;
        }
        private static void MakeRoom(Map map, Microsoft.Xna.Framework.Rectangle room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    map.SetCellProperties(x, y, true, true);
                }
            }
        }
        private static void MakeHorizontalTunnel(Map map, int xStart, int xEnd, int yPosition)
        {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                map.SetCellProperties(x, yPosition, true, true);
            }
        }
        private static void MakeVerticalTunnel(Map map, int yStart, int yEnd, int xPosition)
        {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                map.SetCellProperties(xPosition, y, true, true);
            }
        }
    }
}

    

