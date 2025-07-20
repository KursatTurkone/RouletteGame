public class StraightBetDetector : IBetDetector
{
    private readonly float _threshold;
    public StraightBetDetector(float threshold = 0.22f) { this._threshold = threshold; }

    public bool TryDetect(float xPercent, float zPercent, int row, int col, int rows, int cols, out int[] numbers)
    {
        numbers = null;
        if (xPercent > _threshold && xPercent < 1 - _threshold &&
            zPercent > _threshold && zPercent < 1 - _threshold)
        {
            numbers = new int[] { row * cols + col + 1 };
            return true;
        }
        return false;
    }
}