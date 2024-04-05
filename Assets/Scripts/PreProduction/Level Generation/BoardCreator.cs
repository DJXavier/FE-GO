using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] GameObject[] tileViewPrefabs;
    [SerializeField] GameObject tileSelectionIndicatorPrefab;

    Transform marker
    {
        get
        {
            if (_marker == null)
            {
                GameObject instance = Instantiate(tileSelectionIndicatorPrefab);
                _marker = instance.transform;
            }
            return _marker;
        }
    }
    Transform _marker;

    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    [SerializeField] int width = 15;
    [SerializeField] int depth = 20;

    [SerializeField] Point pos;

    [SerializeField] LevelData levelData;


    #region Generation
    public void CreateBoard()
    {
        for (int y = 0; y < depth; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Point p = new Point(x, y);
                GetOrCreate(p);
            }
        }
    }

    Tile Create(TileTypes type)
    {
        GameObject instance;

        switch(type)
        {
            case TileTypes.Forest:
                instance = Instantiate(tileViewPrefabs[1]);
                break;
            case TileTypes.Water:
                instance = Instantiate(tileViewPrefabs[2]);
                break;
            default:
                instance = Instantiate(tileViewPrefabs[0]);
                break;
        }

        instance.transform.parent = transform;
        Tile t = instance.GetComponent<Tile>();
        t.height = 1;
        t.type = type;
        return t;
    }

    bool IsSpecialTilePlaceable(Point p, TileTypes type, int range, int maxContained)
    {
        int containedSpecial = SurroudingTileTypes(p, type, range);

        if (containedSpecial > 0 && containedSpecial < maxContained)
        {
            List<Point> adjacentPoints = new List<Point>
            {
                new Point(p.x - 1, p.y),
                new Point(p.x, p.y - 1)
            };

            Tile toCheck;
            foreach (Point adjacentPoint in adjacentPoints)
            {
                if (tiles.ContainsKey(adjacentPoint))
                {
                    toCheck = tiles[adjacentPoint];
                    if (toCheck.type == TileTypes.Water)
                        return true;
                }
            }
        }
        else if (containedSpecial == 0)
        {
            return true;
        }

        return false;
    }

    int SurroudingTileTypes(Point p, TileTypes type, int range)
    {
        int similarTiles = 0;

        for (int y = p.y - range; y <= p.y; y++)
        {
            for (int x = p.x - range; x <= p.x + range; x++)
            {
                Point checkPoint = new Point(x, y);
                if (tiles.ContainsKey(checkPoint))
                {
                    Tile toCheck = tiles[checkPoint];
                    similarTiles += (toCheck.type == type ? 1 : 0);
                }
            }
        }

        return similarTiles;
    }

    Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
            return tiles[p];
        
        List<GameObject> availableTiles = new List<GameObject>
        {
            tileViewPrefabs[0]
        };
        if (IsSpecialTilePlaceable(p, TileTypes.Forest, 2, 7))
            availableTiles.Add(tileViewPrefabs[1]);
        if (IsSpecialTilePlaceable(p, TileTypes.Water, 3, 16))
            availableTiles.Add(tileViewPrefabs[2]);
        
        int tileIndex = System.Array.IndexOf(tileViewPrefabs, availableTiles[Random.Range(0, availableTiles.Count)]);
        TileTypes type = (TileTypes) tileIndex;
        
        Tile t = Create(type);
        t.Load(p);
        tiles.Add(p, t);

        return t;
    }

    public void UpdateMarker()
    {
        Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = t != null ? t.center : new Vector3(pos.x, 0, pos.y);
    }

    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);
        tiles.Clear();
    }

    public void Save()
    {
        string filePath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory();
        
        LevelData board = ScriptableObject.CreateInstance<LevelData>();
        board.tiles = new Dictionary<Vector3, TileTypes>();
        foreach (Tile t in tiles.Values)
            board.tiles.Add(new Vector3(t.pos.x, 0, t.pos.y), t.type);
        
        string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, name);
        AssetDatabase.CreateAsset(board, fileName);
    }

    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");
        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        AssetDatabase.Refresh();
    }

    public void Load ()
    {
        Clear();
        if (levelData == null)
            return;

        foreach (Vector3 v in levelData.tiles.Keys)
        {
            Tile t = Create(levelData.tiles[v]);
            t.Load(v);
            tiles.Add(t.pos, t);
        }
    }
    #endregion
}
