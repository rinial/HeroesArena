using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesArena
{
    // Class that contains methods for generation of maps of arbitrary sizes.
    public static class MapGenerator
    {
            // The width of the border inside which objects cannot be placed (objects are closer to the center).
            public const int ObjectsMargin = 3;

            // Returns a newly generated map based on given width and height
            public static Map Generate(int width, int height, int minWallNum, int maxWallNum) {
                List<Cell> cells = new List<Cell>();

                // ground and outer walls
                for (int i = 0; i < height; ++i)
                {
                    for (int j = 0; j < width; ++j)
                    {
                        var tile = (i == 0 || j == 0 || i == height - 1 || j == width - 1) ? TileType.WallLow : GetRandomGroundTile();
                        Cell c = new Cell(new Coordinates(i, j), new BasicTile(tile, tile != TileType.Wall && tile != TileType.WallLow));
                        cells.Add(c);
                    }
                }

                // set random walls
                int objectCount = Random.Range (minWallNum, maxWallNum + 1);
                for (int i = 0; i < objectCount; i++) {
                    var pos = GetRandomPosition(width, height, 2);
                    var tile = TileType.WallLow;
                    cells[(int)(pos.y * width + pos.x)].Tile = new BasicTile(tile, false);
                }

                return new Map(cells);
            }

            private static Vector2 GetRandomPosition(int width, int height, int margin = 0) {
                int x = Random.Range(margin, width - margin);
                int y = Random.Range(margin, height - margin);
                return new Vector2(x, y);
            }

            // TODO should depend on the array of ground tiles...
            private static TileType GetRandomGroundTile() {
                // 25% tile1, 75% tile2
                return Random.Range(0, 4) == 0 ? TileType.RedGround2 : TileType.RedGround1;
            }

            // Returns a newly generated squared map based on given size
            public static Map Generate(int size, int numWalls) {
                return Generate(size, size, numWalls, numWalls);
            }
    }
}