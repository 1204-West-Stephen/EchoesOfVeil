using UnityEngine;

public enum Direction { North = 0, East = 1, South = 2, West = 3 }

public static class DirectionHelper
{
    public static Direction RotateCW(Direction d, int stepsCW)
    {
        return (Direction)(((int)d + stepsCW) % 4);
    }
}
