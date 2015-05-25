namespace Gaia.WebWidgets.Samples.Combinations.WebApps.Chess.Core
{
    using System;

    [Flags]
    public enum Direction
    {
        Top = 1,
        Right = 2,
        Left = 4,
        Bottom = 8,
        TopRight = Top | Right,
        TopLeft = Top | Left,
        BottomRight = Bottom | Right,
        BottomLeft = Bottom | Left
    }
}