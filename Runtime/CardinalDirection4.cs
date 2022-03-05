using System;
using UnityEngine;

namespace Tofunaut.Bootstrap
{
    public enum CardinalDirection4
    {
        North,
        East,
        South,
        West,
    }

    public static class CardinalDirection4Ext
    {
        public static CardinalDirection4 ToCardinalDirection(this Vector2 v)
        {
            var highestDot = float.MinValue;
            var index = 0;
            for (var i = 0; i < Enum.GetValues(typeof(CardinalDirection4)).Length; i++)
            {
                var dot = Vector2.Dot(v, ((CardinalDirection4) i).ToVector2());
                if (dot < highestDot)
                    continue;

                highestDot = dot;
                index = i;
            }

            return (CardinalDirection4) index;
        }
        
        public static Vector2 ToVector2(this CardinalDirection4 dir) => dir switch
        {
            CardinalDirection4.North => Vector2.up,
            CardinalDirection4.East => Vector2.right,
            CardinalDirection4.South => Vector2.down,
            CardinalDirection4.West => Vector2.left,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }
}