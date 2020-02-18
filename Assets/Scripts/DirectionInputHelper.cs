using UnityEngine;

namespace DungeonCrawlers 
{
    public enum Direction
    {
        Left, Up, Right, Down,
        UpperLeft, UpperRight, LowerRight, LowerLeft,
        Neutral
    }

    public static class DirectionInputHelper
    {
        public static Direction GetFourDirection(Vector2 dir) {
            if (dir.x == dir.y) {
                return Direction.Neutral;
            } else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) {
                if (dir.x > 0) return Direction.Right;
                return Direction.Left;
            } else {
                if (dir.y > 0) return Direction.Up;
                return Direction.Down;
            }
        }

        public static Direction GetEightDirection(Vector2 dir) {
            float inputAngle = Mathf.Atan2(Mathf.Abs(dir.x), Mathf.Abs(dir.y)) * Mathf.Rad2Deg;
            if (inputAngle <= 23 || inputAngle >= 67) return GetFourDirection(dir);
            if (dir.x > 0) {
                if (dir.y > 0) return Direction.UpperRight;
                return Direction.LowerRight;
            } else {
                if (dir.y > 0) return Direction.UpperLeft;
                return Direction.LowerLeft;
            }
        }
    }
}