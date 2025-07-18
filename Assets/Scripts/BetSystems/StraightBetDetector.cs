public class StraightBetDetector : IBetDetector
{
    private readonly float threshold;
    public StraightBetDetector(float threshold = 0.22f) { this.threshold = threshold; }

    public bool TryDetect(float xPercent, float zPercent, int row, int col, int rows, int cols, out int[] numbers)
    {
        numbers = null;
        if (xPercent > threshold && xPercent < 1 - threshold &&
            zPercent > threshold && zPercent < 1 - threshold)
        {
            numbers = new int[] { row * cols + col + 1 };
            return true;
        }
        return false;
    }
}