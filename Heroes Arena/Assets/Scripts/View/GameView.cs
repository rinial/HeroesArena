using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HeroesArena
{
    // Represents view of the map.
    public class GameView : MonoBehaviour, IPointerClickHandler
    {
        public const string CellClickedNotification = "GameView.CellClickedNotification";
        public const string EndTurnClickedNotification = "GameView.EndTurnClickedNotification";

        public const int CELL_X_PIXEL_SIZE = 20;
        public const int CELL_Y_PIXEL_SIZE = 15;

        public GameObject Tiles;
        public GameObject Objects;
        public GameObject Units;
        public GameObject Grid;
        public GameObject Highlight;

        // These are just links to prefabs,
        // TODO move somewhere else
        [SerializeField]
        private GameObject GridTile;
        [SerializeField]
        private GameObject Ground;
        [SerializeField]
        private GameObject Rogue;
        [SerializeField]
        private GameObject HealthPotion;

        // Remembers cell-objects references
        private Dictionary<Coordinates, List<GameObject>> _shownCells;
        private Map _oldMap = null;
        private bool _showGrid = true;

        private void OnEnable()
        {
            _shownCells = new Dictionary<Coordinates, List<GameObject>>();
        }

        private void Update()
        {
            // Shows highlight ever cell.
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Coordinates coords = WorldToCoordinates(mousePos);
            if (_shownCells.ContainsKey(coords))
            {
                Highlight.SetActive(true);
                SetGridPosition(Highlight, coords);
            }
            else
                Highlight.SetActive(false);
        }

        // Shows one cell.
        public void Show(Cell cell)
        {
            // Debug.Log("ShowCell");
            List<GameObject> gameObjects = new List<GameObject>();

            // TODO tile should depend on cell.Tile
            GameObject tile = Instantiate(Ground, Tiles.transform);
            SetGridPosition(tile, cell.Position);
            SetSortingOrder(tile, -cell.Position.Y);
            gameObjects.Add(tile);

            // TODO unit should depend on cell.Unit
            if (cell.Unit != null)
            {
                GameObject unit = Instantiate(Rogue, Units.transform);
                SetGridPosition(unit, cell.Position);
                SetSortingOrder(unit, -cell.Position.Y);
                gameObjects.Add(unit);
                UpdateUnitAnimation(cell.Unit, unit);
            }

            // TODO object should depend on cell.Object
            if (cell.Object != null)
            {
                GameObject obj = Instantiate(HealthPotion, Objects.transform);
                SetGridPosition(obj, cell.Position);
                SetSortingOrder(obj, -cell.Position.Y);
                gameObjects.Add(obj);
            }

            if (_showGrid)
                ShowGridCell(cell.Position);

            _shownCells[cell.Position] = gameObjects;
        }

        // Shows sent map.
        public void Show(Map map)
        {
            // Debug.Log("ShowMap");
            ClearMismatch(map);
            foreach (Cell cell in map.Cells.Values)
            {
                if (!_shownCells.ContainsKey(cell.Position))
                {
                    Show(cell);
                }
            }
            _oldMap = new Map(map);
        }

        // Shows one grid cell.
        public void ShowGridCell(Coordinates pos)
        {
            GameObject grid = Instantiate(GridTile, Grid.transform);
            SetGridPosition(grid, pos);
            SetSortingOrder(grid, -pos.Y);
        }

        // Shows full grid.
        public void ShowGrid()
        {
            ClearGrid();
            _showGrid = true;
            foreach (Coordinates pos in _shownCells.Keys)
                ShowGridCell(pos);
        }

        // Hides grid.
        public void HideGrid()
        {
            _showGrid = false;
            ClearGrid();
        }

        // Clears grid.
        public void ClearGrid()
        {
            Transform grid = Grid.transform;
            int count = grid.childCount;
            for (int j = count - 1; j >= 0; --j)
                Destroy(grid.GetChild(j).gameObject);
        }

        // Updates units animation.
        // TODO do here more than just direction change.
        public void UpdateUnitAnimation(BasicUnit unit, GameObject unitObject)
        {
            // Debug.Log("UpdateUnitAnimation");
            Animator animator = unitObject.GetComponent<Animator>();

            if (animator == null)
                return;

            switch (unit.Facing)
            {
                case Direction.Up:
                    animator.SetFloat("xFacing", 0);
                    animator.SetFloat("yFacing", 1);
                    break;
                case Direction.Left:
                    animator.SetFloat("xFacing", -1);
                    animator.SetFloat("yFacing", 0);
                    break;
                case Direction.Down:
                    animator.SetFloat("xFacing", 0);
                    animator.SetFloat("yFacing", -1);
                    break;
                case Direction.Right:
                    animator.SetFloat("xFacing", 1);
                    animator.SetFloat("yFacing", 0);
                    break;
                default:
                    break;
            }
        }

        // Clears everything that is not supposed to be shown now.
        public void ClearMismatch(Map map)
        {
            // Debug.Log("ClearMismatch");
            List<Coordinates> keysToDelete = new List<Coordinates>();
            foreach (Coordinates pos in _shownCells.Keys)
            {
                if (!map.Cells.ContainsKey(pos) || !_oldMap.Cells[pos].Equals(map.Cells[pos]))
                {
                    foreach (GameObject obj in _shownCells[pos])
                        Destroy(obj);
                    keysToDelete.Add(pos);
                }
            }
            foreach (Coordinates coords in keysToDelete)
            {
                _shownCells.Remove(coords);
            }
        }

        // Clears the map.
        public void Clear()
        {
            // Debug.Log("Clear");
            for (int i = 0; i < 3; ++i)
            {
                Transform temp = transform.GetChild(i);
                int count = temp.childCount;
                for (int j = count - 1; j >= 0; --j)
                    Destroy(temp.GetChild(j).gameObject);
            }
        }

        // Handles click on map event.
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            // Debug.Log("Click Map");
            Vector2 clickPos = eventData.pointerCurrentRaycast.worldPosition;

            this.PostNotification(CellClickedNotification, WorldToCoordinates(clickPos));
        }

        public Coordinates WorldToCoordinates(Vector2 pos)
        {
            int x = Mathf.RoundToInt(pos.x / CELL_X_PIXEL_SIZE);
            int y = Mathf.RoundToInt((pos.y - CELL_Y_PIXEL_SIZE / 2f) / CELL_Y_PIXEL_SIZE);
            return new Coordinates(x, y);
        }

        public void OnEndTurnClick()
        {
            // Debug.Log("Click EndTurn");
            this.PostNotification(EndTurnClickedNotification);
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
