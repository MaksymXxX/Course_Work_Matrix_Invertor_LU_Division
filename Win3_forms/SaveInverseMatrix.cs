using System;
using System.IO;
using System.Windows.Forms;

namespace Win3_forms
{
    internal class SaveInverseMatrix
    {
        // Метод для збереження матриць у файл
        public void SaveMatrix(double[,] inputMatrix, double[,] inverseMatrix, Form parentForm)
        {
            // Ініціалізація SaveFileDialog
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Встановлення фільтру для файлів
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                // Встановлення заголовку вікна діалогу
                saveFileDialog.Title = "Save matrices";
                // Встановлення стандартного імені файлу
                saveFileDialog.FileName = "matrices.txt";

                // Відображення діалогового вікна та перевірка, чи користувач натиснув кнопку "OK"
                if (saveFileDialog.ShowDialog(parentForm) == DialogResult.OK)
                {
                    // Виклик методу для збереження матриць у файл
                    SaveMatricesToFile(saveFileDialog.FileName, inputMatrix, inverseMatrix);
                }
            }
        }

        // Метод для запису матриць у файл
        private void SaveMatricesToFile(string filePath, double[,] inputMatrix, double[,] inverseMatrix)
        {
            try
            {
                // Відкриття потоку для запису у файл
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Запис заголовку для початкової матриці
                    writer.WriteLine("Вхідна матриця:");
                    // Запис початкової матриці у файл з форматом з 2 знаками після коми
                    WriteMatrixToFile(writer, inputMatrix, 2);

                    // Запис заголовку для оберненої матриці
                    writer.WriteLine("\nОбернена матриця:");
                    // Запис оберненої матриці у файл з форматом з 4 знаками після коми
                    WriteMatrixToFile(writer, inverseMatrix, 4);
                }

                // Відображення повідомлення про успішне збереження матриць
                MessageBox.Show("Матриці збережено успішно", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Відображення повідомлення про помилку під час збереження матриць
                MessageBox.Show($"Помилка при збереженні матриць: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для запису матриці у файл
        private void WriteMatrixToFile(StreamWriter writer, double[,] matrix, int decimalPlaces)
        {
            int matrixSize = matrix.GetLength(0); 
            string format = "F" + decimalPlaces; // Створення форматного рядка, наприклад "F2" або "F4"
            int columnWidth = 7; // Фіксована ширина для кожного стовпця

            // Запис кожного елемента матриці у файл
            for (int row = 0; row < matrixSize; row++)
            {
                for (int col = 0; col < matrixSize; col++)
                {
                    // Форматування значення елемента матриці
                    string formattedValue = matrix[row, col].ToString(format).PadLeft(columnWidth);
                    // Запис форматованого значення у файл
                    writer.Write(formattedValue);
                    // Додавання табуляції для розділення стовпців
                    if (col < matrixSize - 1)
                    {
                        writer.Write("\t");
                    }
                }
                // Перехід на новий рядок після запису всіх елементів рядка
                writer.WriteLine();
            }
        }
    }
}
