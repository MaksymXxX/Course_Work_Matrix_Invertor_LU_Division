using System;
using System.Windows.Forms;

namespace Win3_forms
{
     internal class LUDecomposition
    {
        private int iterationCount;


        // Метод для розв'язання системи рівнянь за допомогою LU-розкладу
        public double[,] SolveUsingLU(double[,] inputMatrix, out int iterations, Form form = null)
        {

            iterationCount = 0;
            double[,] matrix = inputMatrix;
            int n = matrix.GetLength(0);
            double[,] lower = new double[n, n];
            double[,] upper = new double[n, n];
            double[,] result = new double[n, n];

            // Виконання LU-розкладу
            Decompose(matrix, out lower, out upper);

            // Обернення матриці за допомогою нижньої та верхньої трикутних матриць
            result = InvertMatrix(lower, upper);

            iterations = iterationCount;
            //Повернути обернену матрицю
            return result;
        }

        // Метод для виконання розкладу матриці
        private void Decompose(double[,] matrix, out double[,] lower, out double[,] upper, Form form = null)
        {
            int n = matrix.GetLength(0);
            lower = new double[n, n];
            upper = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                // Заповнення діагоналі верхньої матриці  одиницями
                upper[i, i] = 1.0;
            }

            double[,] a = new double[n, n];
            Array.Copy(matrix, a, matrix.Length);

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {

                    // Ініціалізація суми добутків lower[i,k] та upper[k,j]
                    double sum = 0;
                    for (int k = 0; k < i; k++)
                    {
                        iterationCount++;
                        //Обчислення суми
                        sum += (lower[i, k] * upper[k, j]);

                    }

                    // Обчислення верхньої матриці
                    upper[i, j] = a[i, j] - sum;

                }

                for (int j = i + 1; j < n; j++)
                {
                    // Ініціалізація суми добутків lower[j,k] та upper[k,i]
                    double sum = 0;
                    for (int k = 0; k < i; k++)
                    {
                        iterationCount++;
                        //Обчислення суми
                        sum += (lower[j, k] * upper[k, i]);
                    }
                    if (upper[i,i] == 0)
                    {
                        throw new Exception("Матриця не може бути обернна цим методом. Спробуйте інший метод або змініть вхідну матрицю");
                    }
                    // Evaluating lower matrix
                    lower[j, i] = (a[j, i] - sum) / upper[i, i];

                }
            }

        }

        // Метод для обертання матриці за допомогою нижньої та верхньої трикутних матриць
        private double[,] InvertMatrix(double[,] lower, double[,] upper)
        {
            int n = lower.GetLength(0); // Отримуємо розмірність матриці
            double[,] inv = new double[n, n]; // Ініціалізуємо матрицю для збереження результату

            for (int i = 0; i < n; i++)
            {
                // Створюємо одиничний вектор з 1 на позиції i
                double[] e = new double[n];
                e[i] = 1.0;


                // Розв'язуємо систему рівнянь нижньої трикутної матриці для вектора e
                double[] y = ForwardSubstitution(lower, e);

                // Розв'язуємо систему рівнянь верхньої трикутної матриці для вектора y
                double[] x = BackwardSubstitution(upper, y);

                // Копіюємо отриманий результат у відповідний стовпчик оберненої матриці
                for (int j = 0; j < n; j++)
                {
                    iterationCount++;
                    inv[j, i] = x[j];

                }
            }

            return inv;
        }

        private double[] ForwardSubstitution(double[,] lower, double[] b)
        {
            int n = lower.GetLength(0);  // Отримуємо розмірність матриці
            double[] y = new double[n];  // Ініціалізуємо вектор для збереження проміжних результатів

            // Проходимо по кожному елементу вектора y
            for (int i = 0; i < n; i++)
            {
                // Ініціалізуємо змінну для збереження суми
                // Обчислюємо суму добутків відповідних елементів нижньої трикутної матриці та вектора y
                double sum = 0.0;

                for (int j = 0; j < i; j++)
                {
                    iterationCount++;
                    sum += lower[i, j] * y[j];

                }
                // Визначаємо значення поточного елемента вектора y
                y[i] = b[i] - sum;

            }

            // Повертаємо результат
            return y;
        }

        private double[] BackwardSubstitution(double[,] upper, double[] y)
        {
            int n = upper.GetLength(0);  // Отримуємо розмірність матриці
            double[] x = new double[n];

            // Проходимо по кожному елементу вектора x в зворотному порядку
            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = y[i];// Ініціалізуємо значення поточного елемента вектора x


                for (int j = i + 1; j < n; j++)
                {
                    iterationCount++;
                    x[i] -= upper[i, j] * x[j];

                }
                if (upper[i, i] == 0)
                {
                    
                    throw new Exception("Матриця не може бути обернна цим методом. Спробуйте інший метод або змініть вхідну матрицю");
                }
                // Ділимо на діагональний елемент верхньої трикутної матриці для отримання кінцевого значення
                x[i] /= upper[i, i];

            }

            // Повертаємо результат
            return x;
        }
    }
}