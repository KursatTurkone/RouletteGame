using UnityEngine;

[CreateAssetMenu(fileName = "RouletteConfig", menuName = "Roulette/RouletteConfig")]
public class RouletteConfig : ScriptableObject
{
    public float spinDuration = 3f;
    public int wheelSpins = 4;
    public int ballSpins = 8;
    public float slideAngularSpeed = 400f;
    public float jumpDuration = 0.35f;
    public float jumpHeight = 0.10f;
    public readonly int[] numberOrder = 
    {
        0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18,
        29, 7, 28, 12, 35, 3, 26
    };
}