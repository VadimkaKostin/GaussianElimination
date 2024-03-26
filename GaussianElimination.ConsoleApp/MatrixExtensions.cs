namespace GaussianElimination.ConsoleApp;

public static class MatrixExtensions
{
    public static T[] GetRow<T>(this T[,] matrix, int index) where T : struct
    {
        int length = matrix.GetLength(1);
        T[] rowArray = new T[length];
        for (int i = 0; i < length; i++)
        {
            rowArray[i] = matrix[index, i];
        }
        return rowArray;
    }

    public static void SetRow<T>(this T[,] matrix, int index, T[] row) where T : struct
    {
        for (int i = 0; i < row.Length; i++)
        {
            matrix[index, i] = row[i];
        }
    }
}
