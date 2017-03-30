using UnityEngine;
using System.Collections;

namespace HeroesArena
{
    // Represents view of the map.
    public class MapView : MonoBehaviour
    {
        public const int CELL_X_PIXEL_SIZE = 20;
        public const int CELL_Y_PIXEL_SIZE = 15;

        // These are just links to prefabs,
        // TODO move somewhere else
        [SerializeField] private GameObject Ground;
        [SerializeField] private GameObject Rogue;
        [SerializeField] private GameObject HealthPotion;

        // Shows one cell.
        public void Show(Cell cell)
        {
            // TODO tile should depend on cell.Tile
            GameObject tile = Instantiate(Ground, transform.FindChild("Tiles"));
            SetGridPosition(tile, cell.Position);
            SetSortingOrder(tile, -cell.Position.Y);

            // TODO unit should depend on cell.Unit
            if (cell.Unit != null)
            {
                GameObject unit = Instantiate(Rogue, transform.FindChild("Units"));
                SetGridPosition(unit, cell.Position);
                SetSortingOrder(unit, -cell.Position.Y);
            }

            // TODO object should depend on cell.Object
            if (cell.Object != null)
            {
                GameObject obj = Instantiate(HealthPotion, transform.FindChild("Objects"));
                SetGridPosition(obj, cell.Position);
                SetSortingOrder(obj, -cell.Position.Y);
            }
        }

        // Shows sent map.
        public void Show(Map map)
        {
            Clear();
            foreach (Cell cell in map.Cells.Values)
                Show(cell);
        }

        // Clears the map.
        public void Clear()
        {
            for (int i = 0; i < 3; ++i)
            {
                Transform temp = transform.GetChild(i);
                while (temp.childCount > 0)
                    Destroy(temp.GetChild(0));
            }
        }

        // Sets position on grid for object.
        public void SetGridPosition(GameObject obj, Coordinates coords)
        {
            obj.transform.localPosition = new Vector2(coords.X * CELL_X_PIXEL_SIZE, coords.Y * CELL_Y_PIXEL_SIZE);
        }

        // Fixes sorting order for object.
        public void SetSortingOrder(GameObject obj, int order)
        {
            obj.GetComponent<SpriteRenderer>().sortingOrder = order;
        }

        // TODO
    }
}
