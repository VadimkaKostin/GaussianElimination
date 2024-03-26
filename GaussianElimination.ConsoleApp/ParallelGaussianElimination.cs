using System.Collections.Concurrent;

namespace GaussianElimination.ConsoleApp;

public static class ParallelGaussianElimination
{
    public static double[] SolveSystemOfLinearEquations(double[,] matrix, int variablesAmount)
    {
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

            Parallel.For(fromInclusive: i + 1, toExclusive: variablesAmount, j =>
            {
                double ratio;

                ratio = matrix[j, i] / matrix[i, i];

                for (int k = 0; k < variablesAmount + 1; k++)
                {
                    matrix[j, k] = matrix[j, k] - ratio * matrix[i, k];
                }
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
