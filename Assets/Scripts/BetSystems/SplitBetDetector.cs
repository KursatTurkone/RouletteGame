public class SplitBetDetector : IBetDetector
{
    private readonly float _threshold;
    public SplitBetDetector(float threshold = 0.22f) { this._threshold = threshold; }

    public bool TryDetect(float xPercent, float zPercent, int row, int col, int rows, int cols, out int[] numbers)
    {
        numbers = null;
        if (xPercent <= _threshold && row > 0)
        {
            numbers = new int[]
            {
                (row - 1) * cols + col + 1,
                row * cols + col + 1
            };
            return true;
        }
        if (xPercent >= 1 - _threshold && row < rows - 1)
        {
            numbers = new int[]
            {
                row * cols + col + 1,
                (row + 1) * cols + col + 1
            };
            return true;
        }
        if (zPercent <= _threshold && col > 0)
        {
            numbers = new int[]
            {
                row * cols + (col - 1) + 1,
                row * cols + col + 1
            };
            return true;
        }
        if (zPercent >= 1 - _threshold && col < cols - 1)
        {
            numbers = new int[]
            {
                row * cols + col + 1,
                row * cols + (col + 1) + 1
            };
            return true;
        }
        return false;
    }
}