using UnityEngine;

namespace Tofunaut.Bootstrap
{
    public static class Vector2IntExtensions
    {
        public static Vector2Int XZ(this Vector3Int v) => new Vector2Int(v.x, v.z);
    }
}