using UnityEngine;

public class Tile : MonoBehaviour
{
    public Point pos;
    public int height;
    public Vector3 center { get { return new Vector3(pos.x, height, pos.y); } }
    
    public TileTypes type;


    #region Generation
    void Match()
    {
        transform.localPosition = new Vector3( pos.x, height / 2f, pos.y );
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void Load(Point p)
    {
        pos = p;
        Match();
    }

    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z));
    }
    #endregion
}
