using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalMinimum.Import
{
        
    public interface ITileAction
    {
        string ActionAsString { get; }

        int GetInt(int index);
        bool HasValue(int index);

        int targetX { get; }
        int targetY { get; }
    }

    public interface ITile
    {
        int Col { get; set; }
        int Row { get; set; }
        int Level { get; set; }
        GameObject GameObject { get; set; }

        void AddAction(string instruction);
        void SetRoomType(int tileType);

        void SetNeighbour(Direction direction, ITile tile);
    }

    [System.Serializable]
    public abstract class ImportTile<TileType, TileAction> : ITile where TileAction : ITileAction
    {
        private ImportTile<TileType, TileAction> east;
        private ImportTile<TileType, TileAction> west;
        private ImportTile<TileType, TileAction> north;
        private ImportTile<TileType, TileAction> south;

        private int col;
        private int row;
        private int level;

        private GameObject gameObject;

        [SerializeField]
        protected List<TileAction> actions = new List<TileAction>();

        public IEnumerable<TileAction> Actions()
        {
            for (int i = 0, l = actions.Count; i < l; i++)
            {
                yield return actions[i];
            }
        }

        public abstract void AddAction(string actionKey);

        public bool HasAction(TileAction action)
        {
            for (int i = 0, l = actions.Count; i < l; i++)
            {
                if (actions[i].Equals(action))
                {
                    return true;
                }
            }
            return false;
        }

        TileType _roomType;

        public virtual void SetRoomType(int typeIndex)
        {
            int index = 0;
            foreach (var value in Enum.GetValues(typeof(TileType)))
            {
                if (index == typeIndex)
                {
                    _roomType = (TileType)value;
                    return;
                }
                index++;
            }
            throw new System.ArgumentException(string.Format("Either index {0} is out of range for enum or {1} is not an enum", typeIndex, typeof(TileType)));
        }

        public void SetNeighbour(Direction direction, ITile tile)
        {
            switch (direction)
            {
                case Direction.East:
                    east = (ImportTile < TileType, TileAction >) tile;
                    break;
                case Direction.North:
                    north = (ImportTile<TileType, TileAction>)tile;
                    break;
                case Direction.South:
                    south = (ImportTile<TileType, TileAction>)tile;
                    break;
                case Direction.West:
                    west = (ImportTile<TileType, TileAction>)tile;
                    break;
            }
        }

        public TileType RoomType { get { return _roomType; } }

        public int Col
        {
            get
            {
                return col;
            }

            set
            {
                col = value;
            }
        }

        public int Row
        {
            get
            {
                return row;
            }

            set
            {
                row = value;
            }
        }

        public int Level
        {
            get
            {
                return level;
            }

            set
            {
                level = value;
            }
        }

        public GameObject GameObject
        {
            get
            {
                return gameObject;
            }

            set
            {
                gameObject = value;
            }
        }

        public ImportTile<TileType, TileAction> East
        {
            get
            {
                return east;
            }

            set
            {
                east = value;
            }
        }

        public ImportTile<TileType, TileAction> West
        {
            get
            {
                return west;
            }

            set
            {
                west = value;
            }
        }

        public ImportTile<TileType, TileAction> North
        {
            get
            {
                return north;
            }

            set
            {
                north = value;
            }
        }

        public ImportTile<TileType, TileAction> South
        {
            get
            {
                return south;
            }

            set
            {
                south = value;
            }
        }
    }

    public class ImportGrid<Tile>: MonoBehaviour where Tile : ITile, new()
    {

        [SerializeField]
        string dataPrefix = "level";

        [SerializeField]
        string startTileModifier = "*";

        [SerializeField]
        string endTileModifier = "#";

        [SerializeField]
        char csvColumnSep = ';';

        [SerializeField, Tooltip("Separates the information pieces about a single tile.")]
        char csvDataSep = ',';

        protected Tile[][] matrix = null;
        protected Tile startTile = default(Tile);
        protected Tile endTile = default(Tile);
        protected List<Tile> tileList = new List<Tile>();

        int nRows;
        List<int> nColumns = new List<int>();

        public void ImportLevel(int level)
        {
            ClearPrevious();
            LoadData(level);
            ConnectTiles();
        }

        public void PlaceAssets(
            System.Action<Tile> PlaceStartTile,
            System.Action<Tile> PlaceEndTile,
            System.Action<Tile> PlaceOtherTile)
        {
            if (PlaceStartTile != null)
            {
                PlaceStartTile(startTile);
            }
            if (PlaceEndTile != null)
            {
                PlaceEndTile(endTile);
            }

            foreach (var tile in tileList)
            {
                if (tile.Equals(startTile) && PlaceStartTile != null || tile.Equals(endTile) && PlaceEndTile != null)
                {
                    continue;
                }

                PlaceOtherTile(tile);
            }
        }

        public void ConfigureAssets(System.Action<Tile> ConfigurationFunc)
        {
            foreach (var tile in tileList)
            {

                ConfigurationFunc(tile);
            }
        }

        void ClearPrevious()
        {
            if (tileList != null)
            {
                foreach (var tile in tileList)
                {
                    if (tile.GameObject)
                    {
                        Destroy(tile.GameObject);
                    }
                }
            }
            matrix = null;
            startTile = default(Tile);
            endTile = default(Tile);
            tileList.Clear();
            nRows = 0;
            nColumns.Clear();
        }

        void LoadData(int level)
        {
            TextAsset asset = Resources.Load(dataPrefix + level) as TextAsset;
            string dataString = asset.text.Replace("\r", "\n");
            string[] rows = dataString.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            matrix = new Tile[rows.Length][];
            nRows = rows.Length;
            for (int row = 0; row < rows.Length; ++row)
            {

                string rowValue = rows[row];
                string[] columns = rowValue.Split(csvColumnSep);

                matrix[row] = new Tile[columns.Length];
                nColumns.Add(columns.Length);

                for (int col = 0; col < columns.Length; ++col)
                {
                    string[] tileInstructions = columns[col].Split(csvDataSep);
                    string tileType = tileInstructions[0];
                    if (tileType != "")
                    {
                        Tile room = new Tile();

                        matrix[row][col] = room;
                        room.Col = col;
                        room.Row = row;
                        room.Level = level;
                        if (tileType.StartsWith(startTileModifier))
                        {
                            startTile = room;
                            tileType = tileType.Replace(startTileModifier, "");
                        }
                        if (tileType.StartsWith(endTileModifier))
                        {
                            endTile = room;
                            tileType = tileType.Replace(endTileModifier, "");
                        }

                        int tileTypeIndex = 0;
                        try
                        {
                            tileTypeIndex = int.Parse(tileType[0].ToString());
                        }
                        catch (System.FormatException)
                        {
                            Debug.LogError(string.Format("Position {0} {1}, bad type '{2}'", row, col, tileType));
                            throw;
                        }

                        if (tileInstructions.Length > 1)
                        {
                            for (int i = 1; i < tileInstructions.Length; ++i)
                            {
                                room.AddAction(tileInstructions[i]);
                            }
                        }
                        room.SetRoomType(tileTypeIndex);

                        tileList.Add(room);
                    }
                }
            }

        }

        void ConnectTiles()
        {

            for (int i = 0, l = tileList.Count; i < l; i++)
            {
                Tile tile = tileList[i];
                int col = tile.Col;
                int row = tile.Row;

                if (row > 0)
                {
                    tile.SetNeighbour(Direction.North, matrix[row - 1][col]);
                }

                if (row < nRows - 1)
                {
                    tile.SetNeighbour(Direction.South, matrix[row + 1][col]);
                }
                
                if (col > 0)
                {
                    tile.SetNeighbour(Direction.West, matrix[row][col - 1]);
                }
                
                if (col < nColumns[row] - 1)
                {
                    tile.SetNeighbour(Direction.East, matrix[row][col + 1]);
                }
            }
        }

    }

}