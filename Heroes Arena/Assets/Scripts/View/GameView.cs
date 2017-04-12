using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HeroesArena
{
    // Represents view of the map.
    public class GameView : MonoBehaviour, IPointerClickHandler
    {
        // Notifications.
        public const string CellClickedNotification = "GameView.CellClickedNotification";
        public const string EndTurnClickedNotification = "GameView.EndTurnClickedNotification";

        // Sizes of tiles.
        public const int CellXPixelSize = 20, CellYPixelSize = 15;

        // Important references.
        public MatchController MatchController;
        public CameraController CameraController;

        // UI references.
        public GameObject Tiles;
        public GameObject Objects;
        public GameObject Units;
        public GameObject Grid;
        public GameObject Highlight;

        #region Prefabs
        // TODO maybe it should be moved somewhere else.
        // Links to some prefabs.
        [SerializeField]
        private GameObject GridTile;
        [SerializeField]
        private GameObject Ground;
        [SerializeField]
        private GameObject Rogue;
        [SerializeField]
        private GameObject HealthPotion;
        #endregion

        // Stores gameobject references and provides easy position-gameobjects access.
        private Dictionary<Coordinates, List<GameObject>> _shownCells;
        // Stores old map
        private Map _oldMap;
        // Checks if grid should be shown.
        private bool _showGrid = true;

        // Initialization.
        private void OnEnable()
        {
            _shownCells = new Dictionary<Coordinates, List<GameObject>>();
        }

        // Executed at every frame.
        private void Update()
        {
            // Shows highlight where mouse points.
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
            // New list to be stored in shownCells later.
            List<GameObject> gameObjects = new List<GameObject>();

            // TODO tile should depend on cell.Tile
            // Shows tile of the cell.
            GameObject tile = Instantiate(Ground, Tiles.transform);
            gameObjects.Add(tile);

            // TODO unit should depend on cell.Unit
            // Shows unit on the cell.
            if (cell.Unit != null)
            {
                GameObject unit = Instantiate(Rogue, Units.transform);
                gameObjects.Add(unit);
                // Updates unit animation.
                UpdateUnitAnimation(cell.Unit, unit);

                // Sets camera to follow unit if it is controlled by local player.
                if (cell.Unit == MatchController.LocalPlayer.ControlledUnit)
                    CameraController.Target = unit.transform;

                // Fill health bar.
                Parameter<int> healthPoints = cell.Unit.HealthPoints;
                unit.transform.Find("HealthBar").Find("Fill").GetComponent<Image>().fillAmount = (float)healthPoints.Current / healthPoints.Maximum;
            }

            // TODO object should depend on cell.Object
            // Shows object on the cell.
            if (cell.Object != null)
            {
                GameObject obj = Instantiate(HealthPotion, Objects.transform);
                gameObjects.Add(obj);
            }

            // Sets correct position of sorting order for new gameObjects.
            foreach (GameObject gameObj in gameObjects)
                SetOnGrid(gameObj, cell.Position);

            // Shows one grid cell if it is to be shown.
            if (_showGrid)
                ShowGridCell(cell.Position);

            // Remembers new gameObjects as shown.
            _shownCells[cell.Position] = gameObjects;
        }

        // Shows sent map.
        public void Show(Map map)
        {
            // Clears mismatches between new and old maps.
            ClearMismatch(map);
            foreach (Cell cell in map.Cells.Values)
            {
                // Shows cell if nothing is shown at the position.
                if (!_shownCells.ContainsKey(cell.Position))
                {
                    Show(cell);
                }
            }
            _oldMap = (Map)map.Clone();
        }

        // Clears everything that is not supposed to be shown.
        public void ClearMismatch(Map map)
        {
            if (_oldMap != null)
            {
                // Remembers coordinates to clear later.
                List<Coordinates> keysToDelete = new List<Coordinates>();
                foreach (Coordinates pos in _shownCells.Keys)
                {
                    // Clears gameObjects for position if it is not part of new map or if it differs from new map.
                    if (!map.Cells.ContainsKey(pos) || !_oldMap.Cells[pos].Equals(map.Cells[pos]))
                    {
                        foreach (GameObject obj in _shownCells[pos])
                            Destroy(obj);
                        keysToDelete.Add(pos);
                    }
                }
                // Clears the unneeded coordinates from shownCells.
                foreach (Coordinates coords in keysToDelete)
                {
                    _shownCells.Remove(coords);
                }
            }
        }

        // Clears the entire map.
        public void Clear()
        {
            DestroyAllChildren(Tiles.transform);
            DestroyAllChildren(Objects.transform);
            DestroyAllChildren(Units.transform);
            ClearGrid();
            Highlight.SetActive(false);
            _shownCells = new Dictionary<Coordinates, List<GameObject>>();
            _oldMap = null;
        }

        // TODO should not be here.
        // Destroys all children of transform.
        public static void DestroyAllChildren(Transform obj)
        {
            int count = obj.childCount;
            for (int j = count - 1; j >= 0; --j)
                Destroy(obj.GetChild(j).gameObject);
        }

        #region Grid
        // Shows one grid cell.
        public void ShowGridCell(Coordinates pos)
        {
            GameObject grid = Instantiate(GridTile, Grid.transform);
            SetOnGrid(grid, pos);
        }
        // Clears and then shows full grid.
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
            DestroyAllChildren(Grid.transform);
        }
        #endregion

        // TODO do here more than just direction change.
        // Updates units animation.
        public void UpdateUnitAnimation(BasicUnit unit, GameObject unitObject)
        {
            // Gets unit animator.
            Animator animator = unitObject.GetComponent<Animator>();

            if (animator == null)
                return;

            // Sets animation depending on facing.
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
            }
        }

        // Handles click on map event.
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            // Gets click position.
            Vector2 clickPos = eventData.pointerCurrentRaycast.worldPosition;

            // Notifies GameController.ActiveGameState that cell was clicked.
            this.PostNotification(CellClickedNotification, WorldToCoordinates(clickPos));
        }

        // Called when EndTurn button is clicked.
        public void OnEndTurnClick()
        {
            // Notifies GameController.ActiveGameState that EndTurn button was clicked.
            this.PostNotification(EndTurnClickedNotification);
        }

        // Gets grid coordinates from world position.
        public static Coordinates WorldToCoordinates(Vector2 pos)
        {
            int x = Mathf.RoundToInt(pos.x / CellXPixelSize);
            int y = Mathf.RoundToInt((pos.y - CellYPixelSize / 2f) / CellYPixelSize);
            return new Coordinates(x, y);
        }

        // Sets position on grid for object.
        public static void SetGridPosition(GameObject obj, Coordinates coords)
        {
            obj.transform.localPosition = new Vector2(coords.X * CellXPixelSize, coords.Y * CellYPixelSize);
        }

        // Sets sorting order for object.
        public static void SetSortingOrder(GameObject obj, int order)
        {
            obj.GetComponent<SpriteRenderer>().sortingOrder = order;
        }

        // Sets position on grid and correct sorting order for object.
        public static void SetOnGrid(GameObject gameObj, Coordinates pos)
        {
            SetGridPosition(gameObj, pos);
            SetSortingOrder(gameObj, -pos.Y);
        }
        public static void SetOnGrid(GameObject gameObj, Cell cell)
        {
            SetOnGrid(gameObj, cell.Position);
        }
    }
}
