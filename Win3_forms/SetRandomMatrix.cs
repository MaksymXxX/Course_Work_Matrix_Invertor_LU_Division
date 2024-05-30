using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Win3_forms
{
    internal class SetRandomMatrix
    {
        private Random random = new Random(); // Ініціалізація генератора випадкових чисел

        // Метод для генерації випадкової матриці 
        public void GenerateRandomMatrix(Form form, int matrix_size)
        {
            for (int row = 0; row < matrix_size; row++) 
            {
                for (int column = 0; column < matrix_size; column++) 
                {
                    int textBoxIndex = (row * 10) + (column + 1); // Розрахунок індексу текстового поля
                    string textBoxName = "textBox" + textBoxIndex; 
                    var textBox = form.Controls.Find(textBoxName, true).FirstOrDefault() as TextBox; // Пошук текстового поля

                    if (textBox != null) // Якщо текстове поле знайдено
                    {
                        double randomValue = Math.Round(random.NextDouble() * 200 - 100, 2); // Генерація випадкового значення
                        textBox.Text = randomValue.ToString(); // Запис випадкового значення
                        textBox.BackColor = SystemColors.Window; // Встановлення кольору фону
                    }
                }
            }
        }
    }
}
