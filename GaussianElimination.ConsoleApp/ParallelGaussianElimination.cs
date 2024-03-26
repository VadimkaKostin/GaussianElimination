namespace GaussianElimination.ConsoleApp;

public static class ParallelGaussianElimination
{
    private static double[,] _matrix;

    public static double[] SolveSystemOfLinearEquations(double[,] matrix, int variablesAmount)
    {
        _matrix = matrix;

        ApplyGaussEliminationOnMatrix(matrix, variablesAmount);

        double[] solution = ApplyBackSubstitution(matrix, variablesAmount);

        return solution;
    }

    private static void ApplyGaussEliminationOnMatrix(double[,] matrix, int variablesAmount)
    {
        for (int i = 0; i < variablesAmount - 1; i++)
        {
            if (matrix[i, i] == 0)
            {
                throw new ArgumentException("Mathematical error.");
            }

            double[] pivotRow = matrix.GetRow(i);

            Parallel.For(fromInclusive: i + 1, toExclusive: variablesAmount, j =>
            {
                double[] currentRow = matrix.GetRow(j);

                double ratio = currentRow[i] / pivotRow[i];

                for (int k = 0; k < variablesAmount + 1; k++)
                {
                    currentRow[k] = currentRow[k] - ratio * pivotRow[k];
                }

                matrix.SetRow(j, currentRow);
            });
        }
    }

    private static double[] ApplyBackSubstitution(double[,] matrix, int variablesAmount)
    {
        double[] solution = new double[variablesAmount];

        solution[variablesAmount - 1] =
            matrix[variablesAmount - 1, variablesAmount] / matrix[variablesAmount - 1, variablesAmount - 1];

        for (int i = variablesAmount - 2; i >= 0; i--)
        {
            solution[i] = matrix[i, variablesAmount];

            for (int j = i + 1; j <= variablesAmount - 1; j++)
            {
                solution[i] = solution[i] - matrix[i, j] * solution[j];
            }

            solution[i] = solution[i] / matrix[i, i];
        }

        return solution;
    }
}
