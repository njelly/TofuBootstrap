using UnityEngine;

namespace Tofunaut.Bootstrap
{
    public static class Vector2IntExtensions
    {
        public static Vector2Int RoundToVector2Int(this Vector2 v) => new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
        public static Vector2Int XZ(this Vector3Int v) => new Vector2Int(v.x, v.z);
    }
}