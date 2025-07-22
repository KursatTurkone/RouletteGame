

using UnityEngine;

public enum RouletteColor
{
    Green,
    Red,
    Black
}

public static class RouletteColorHelper
{
    public static RouletteColor GetNumberColor(int number)
    {
        if (number == 0)
            return RouletteColor.Green;

        if (System.Array.Exists(RouletteBets.RedNumbers, n => n == number))
            return RouletteColor.Red;

        if (System.Array.Exists(RouletteBets.BlackNumbers, n => n == number))
            return RouletteColor.Black;

        return RouletteColor.Green; // Fallback
    }

    public static Color GetUnityColor(RouletteColor color)
    {
        switch (color)
        {
            case RouletteColor.Red: return Color.red;
            case RouletteColor.Black: return Color.gray;
            case RouletteColor.Green: return Color.green;
            default: return Color.gray;
        }
    }
}