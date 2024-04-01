using System.Diagnostics;

namespace GaussianElimination.ConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Input values
            Console.Write("Enter number of variables: ");
            int n = int.Parse(Console.ReadLine()!);

            Console.WriteLine();

            double[,] initialMatrix = ReadSystemEquationMatrix(n);

            double[,] matrix = (double[,])initialMatrix.Clone();

            Console.WriteLine("Matrix has been read successfully!");

            Stopwatch consistentStopwatch = Stopwatch.StartNew();

            double[] consistentSolution = ConsistentGaussianElimination.SolveSystemOfLinearEquations(matrix, n);

            consistentStopwatch.Stop();

            bool consistentSolvedSuccessfully = CheckSolution(initialMatrix, consistentSolution, n);

            Console.WriteLine($"\nConsistent result: ({string.Join(", ", consistentSolution)})");
            Console.WriteLine($"Successfull: {consistentSolvedSuccessfully}");
            Console.WriteLine($"Time involved: {consistentStopwatch.Elapsed} ({consistentStopwatch.Elapsed.TotalMicroseconds} microseconds)");

            Stopwatch parallelStopwatch = Stopwatch.StartNew();

            double[] parallelSolution = ParallelGaussianElimination.SolveSystemOfLinearEquations(matrix, n);

            parallelStopwatch.Stop();

            bool parallelSolvedSuccessfully = CheckSolution(initialMatrix, parallelSolution, n);

            Console.WriteLine($"\nParallel result: ({string.Join(", ", parallelSolution)})");
            Console.WriteLine($"Successfull: {parallelSolvedSuccessfully}");
            Console.WriteLine($"Time involved: {parallelStopwatch.Elapsed} ({parallelStopwatch.Elapsed.TotalMicroseconds} microseconds)");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nError occured: " + ex.Message);
        }
    }

    private static double[,] ReadSystemEquationMatrix(int n)
    {
        double[,] matrix = new double[n, n + 1];

        string[] mitrixLines = File.ReadAllLines($"matrix_{n}.txt");

        for (int i = 0; i < n; i++)
        {
            int[] coefficients = mitrixLines[i].Split(' ').Select(str => int.Parse(str)).ToArray();

            for (int j = 0; j < n + 1; j++)
            {
                matrix[i, j] = coefficients[j];
            }
        }

        return matrix;
    }

    private static bool CheckSolution(double[,] matrix, double[] solution, int n)
    {
        double epsilon = 1e-2;

        for (int i = 0; i < n; i++)
        {
            double sum = 0;

            for (int j = 0; j < n; j++)
            {
                sum += matrix[i, j] * solution[j];
            }

            sum -= matrix[i, n];

            if (Math.Abs(sum) > epsilon)
            {
                return false;
            }
        }

        return true;
    }

    private static void GenerateSystemOfLinearEquations(int variablesAmount)
    {
        Random random = new();

        using StreamWriter writer = new($"matrix_{variablesAmount}.txt");

        for (int i = 0; i < variablesAmount; i++)
        {
            int[] row = new int[variablesAmount + 1];

            for (int j = 0; j < variablesAmount + 1; j++)
            {
                row[j] = random.Next(10000, 100000);
            }

            writer.WriteLine(string.Join(' ', row));
        }
    }
}
