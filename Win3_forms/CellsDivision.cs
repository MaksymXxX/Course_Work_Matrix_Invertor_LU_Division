using System;

namespace WinForms
{
    internal class CellsDivision
    {

        private int iterationCount = 0;
        // Метод для розв'язання системи рівнянь за допомогою розбиття на клітки 
        public double[,] SolveUsingDivision(double[,] inputMatrix, out int iterations)
        {
            iterationCount = 0;
            double[,] matrix = inputMatrix;
            int n = matrix.GetLength(0);  
            int halfSize = (n + 1) / 2;  // Визначаємо розмір половини матриці

            // Ініціалізуємо підматриці А, В, С, D
            double[,] A = new double[halfSize, halfSize];
            double[,] B = new double[halfSize, n - halfSize];
            double[,] C = new double[n - halfSize, halfSize];
            double[,] D = new double[n - halfSize, n - halfSize];

            // Заповнюємо підматрицю A
            for (int i = 0; i < halfSize; i++)
            {
                for (int j = 0; j < halfSize; j++)
                {
                    iterationCount++;
                    A[i, j] = matrix[i, j];
                }
            }

            // Заповнюємо підматрицю B
            for (int i = 0; i < halfSize; i++)
            {
                for (int j = 0; j < n - halfSize; j++)
                {
                    iterationCount++;
                    B[i, j] = matrix[i, j + halfSize];
                }
            }

            // Заповнюємо підматрицю C
            for (int i = 0; i < n - halfSize; i++)
            {
                for (int j = 0; j < halfSize; j++)
                {
                    iterationCount++;
                    C[i, j] = matrix[i + halfSize, j];
                }
            }

            // Заповнюємо підматрицю D
            for (int i = 0; i < n - halfSize; i++)
            {
                for (int j = 0; j < n - halfSize; j++)
                {
                    iterationCount++;
                    D[i, j] = matrix[i + halfSize, j + halfSize];
                }
            }

            // Обчислюємо обернену матрицю для D
            double[,] D_inv = Inverse(D);

            // Обчислюємо проміжні матриці
            double[,] X = Multiply(B, D_inv);  
            double[,] Y = Multiply(D_inv, C);  

            // Обчислюємо підматриці результуючі матриці
            double[,] A_XC = Subtract(A, Multiply(X, C));  
            double[,] K = Inverse(A_XC);  
            double[,] L = MultiplyByScalar(Multiply(K, X), -1);  
            double[,] M = MultiplyByScalar(Multiply(Y, K), -1);  
            double[,] N = Subtract(D_inv, Multiply(Y, L));  

            // Ініціалізуємо обернену матрицю
            double[,] result = new double[n, n];

            // Заповнюємо обернену матрицю значеннями з матриці K
            for (int i = 0; i < halfSize; i++)
            {
                for (int j = 0; j < halfSize; j++)
                {
                    iterationCount++;
                    result[i, j] = K[i, j];
                }
            }

            // Заповнюємо обернену матрицю значеннями з матриці L
            for (int i = 0; i < halfSize; i++)
            {
                for (int j = 0; j < n - halfSize; j++)
                {
                    iterationCount++;
                    result[i, j + halfSize] = L[i, j];
                }
            }

            // Заповнюємо обернену матрицю значеннями з матриці M
            for (int i = 0; i < n - halfSize; i++)
            {
                for (int j = 0; j < halfSize; j++)
                {
                    iterationCount++;
                    result[i + halfSize, j] = M[i, j];
                }
            }

            // Заповнюємо обернену матрицю значеннями з матриці N
            for (int i = 0; i < n - halfSize; i++)
            {
                for (int j = 0; j < n - halfSize; j++)
                {
                    iterationCount++;
                    result[i + halfSize, j + halfSize] = N[i, j];
                }
            }
            iterations = iterationCount;
            return result;  
        }

        // Метод для множення двох матриць
        private double[,] Multiply(double[,] A, double[,] B)
        {
            int rowsA = A.GetLength(0);  // Кількість рядків у матриці A
            int colsA = A.GetLength(1);  // Кількість стовпців у матриці A
            int colsB = B.GetLength(1);  // Кількість стовпців у матриці B
            double[,] result = new double[rowsA, colsB];  

            // Обчислюємо добуток матриць
            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    
                    for (int k = 0; k < colsA; k++)
                    {
                        iterationCount++;
                        result[i, j] += A[i, k] * B[k, j];
                    }
                }
            }

            return result;  // Повертаємо результат
        }

        // Метод для віднімання двох матриць
        private double[,] Subtract(double[,] A, double[,] B)
        {
            int rows = A.GetLength(0);  // Кількість рядків у матриці A
            int cols = A.GetLength(1);  // Кількість стовпців у матриці A
            double[,] result = new double[rows, cols];  

            // Обчислюємо різницю матриць
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    iterationCount++;
                    result[i, j] = A[i, j] - B[i, j];

                }
            }

            return result;  // Повертаємо результат
        }

        // Метод для множення матриці на скаляр
        private double[,] MultiplyByScalar(double[,] A, double scalar)
        {
            int rows = A.GetLength(0);  // Кількість рядків у матриці A
            int cols = A.GetLength(1);  // Кількість стовпців у матриці A
            double[,] result = new double[rows, cols]; 

            // Обчислюємо добуток матриці на скаляр
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    iterationCount++;
                    result[i, j] = A[i, j] * scalar;
                }
            }

            return result;  // Повертаємо результат
        }

        // Метод для обчислення оберненої матриці
        private double[,] Inverse(double[,] matrix)
        {
            int n = matrix.GetLength(0);  // Отримуємо розмірність матриці
            double[,] result = new double[n, n];  // Ініціалізуємо матрицю обернену
            double[,] augmentedMatrix = new double[n, 2 * n];  // Ініціалізуємо розширену матрицю

            // Заповнюємо розширену матрицю
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    iterationCount++;
                    augmentedMatrix[i, j] = matrix[i, j];
                }
                augmentedMatrix[i, i + n] = 1.0;  // Додаємо одиничну матрицю
            }

            // Обчислюємо обернену матрицю 
            for (int i = 0; i < n; i++)
            {
                double diagElement = augmentedMatrix[i, i];  // Отримуємо діагональний елемент
                if (diagElement == 0)
                {
                    // Якщо діагональний елемент дорівнює нулю, шукаємо рядок для обміну
                    for (int k = i + 1; k < n; k++)
                    {
                        if (augmentedMatrix[k, i] != 0)
                        {
                            // Міняємо місцями рядки
                            for (int j = 0; j < 2 * n; j++)
                            {
                                iterationCount++;
                                double temp = augmentedMatrix[i, j];
                                augmentedMatrix[i, j] = augmentedMatrix[k, j];
                                augmentedMatrix[k, j] = temp;
                            }
                            diagElement = augmentedMatrix[i, i];
                            break;
                        }
                    }
                }

                
                for (int j = 0; j < 2 * n; j++)
                {
                    augmentedMatrix[i, j] /= diagElement;
                }

                // Виключаємо інші елементи стовпця
                for (int k = 0; k < n; k++)
                {
                    if (k != i)
                    {
                        double factor = augmentedMatrix[k, i];
                        for (int j = 0; j < 2 * n; j++)
                        {
                            iterationCount++;
                            augmentedMatrix[k, j] -= factor * augmentedMatrix[i, j];
                        }
                    }
                }
            }

            // Заповнюємо матрицю результату значеннями з розширеної матриці
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    iterationCount++;
                    result[i, j] = augmentedMatrix[i, j + n];
                }
            }

            return result;  // Повертаємо обернену матрицю
        }
    }
}
