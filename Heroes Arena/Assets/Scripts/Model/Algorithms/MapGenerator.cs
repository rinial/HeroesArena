using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HeroesArena
{
    // Class that contains methods for generation of maps of arbitrary sizes.
    public static class MapGenerator
    {
        // The width of the border inside which objects cannot be placed (objects are closer to the center).
        public const int ObjectsMargin = 3;

        // Returns a newly generated map based on given width and height
        public static MapParameters Generate(int unitNum, int width, int height, int minWallNum, int maxWallNum, int minPotionNum, int maxPotionNum)
        {
            List<Cell> cells = new List<Cell>();

            // ground and outer walls
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    var tile = (i == 0 || j == 0 || i == height - 1 || j == width - 1) ? TileType.WallLow : GetRandomGroundTile();
                    Cell c = new Cell(new Coordinates(i, j), new BasicTile(tile));
                    cells.Add(c);
                }
            }

            // set random walls
            int objectCount = Random.Range(minWallNum, maxWallNum + 1);
            for (int i = 0; i < objectCount; i++)
            {
                var pos = GetRandomPosition(width, height, 2);
                var tile = TileType.WallLow;
                cells[(int)(pos.y * width + pos.x)].Tile = new BasicTile(tile);
            }
            
            // set random health potions
            objectCount = Random.Range(minPotionNum, maxPotionNum + 1);
            while (objectCount > 0)
            {
                var pos = GetRandomPosition(width, height, 1);
                ObjectType obj = GetRandomObject();
                Cell cell = cells[(int)(pos.y * width + pos.x)];
                if (!cell.IsOccupied)
                {
                    new BasicObject(cell, obj);
                    --objectCount;
                }
            }

            // set random units
            while (unitNum > 0)
            {
                var pos = GetRandomPosition(width, height, 1);
                ClassTag clas = GetRandomClass();
                Direction facing = GetRandomDirection();
                Cell cell = cells[(int)(pos.y * width + pos.x)];
                if (!cell.IsOccupied)
                {
                    new BasicUnit(cell, facing, clas);
                    --unitNum;
                }
            }

            return new MapParameters(cells);
        }

        private static Vector2 GetRandomPosition(int width, int height, int margin = 0)
        {
            int x = Random.Range(margin, width - margin);
            int y = Random.Range(margin, height - margin);
            return new Vector2(x, y);
        }

        // TODO should depend on the array of ground tiles...
        private static TileType GetRandomGroundTile()
        {
            // 25% tile1, 75% tile2
            return Random.Range(0, 4) == 0 ? TileType.RedGround2 : TileType.RedGround1;
        }

        // returns a random object type
        private static ObjectType GetRandomObject()
        {
            var objects = (ObjectType[])Enum.GetValues(typeof(ObjectType));
            ObjectType obj = objects[Random.Range(0, objects.Length)];
            return obj != ObjectType.None ? obj : GetRandomObject();
        }

        // returns a random class tag
        private static ClassTag GetRandomClass()
        {
            var classes = (ClassTag[])Enum.GetValues(typeof(ClassTag));
            ClassTag clas = classes[Random.Range(0, classes.Length)];
            return clas != ClassTag.None ? clas : GetRandomClass();
        }

        // returns a random class tag
        private static Direction GetRandomDirection()
        {
            var directions = (Direction[])Enum.GetValues(typeof(Direction));
            return directions[Random.Range(0, directions.Length)];
        }

        // Returns a newly generated squared map based on given size
        public static MapParameters Generate(int unitNum, int size, int numWalls, int numPotions)
        {
            return Generate(unitNum, size, size, numWalls, numWalls, numPotions, numPotions);
        }
    }
}