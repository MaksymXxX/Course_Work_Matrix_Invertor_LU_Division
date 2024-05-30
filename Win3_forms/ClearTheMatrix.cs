using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win3_forms
{
    internal class ClearTheMatrix
    {
        // Метод для очищення матриці на формі
        public void ClearMatrix(Form form, int matrix_size)
        {
            //  Підтвердження очищення матриці
            DialogResult result = MessageBox.Show("Ви впевнені, що хочете очистити матрицю?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Якщо користувач підтвердив, що хоче очистити матрицю
            if (result == DialogResult.Yes)
            {
                // Проходження по кожному елементу матриці
                for (int row = 0; row < matrix_size; row++)
                {
                    for (int column = 0; column < matrix_size; column++)
                    {
                        // Розрахунок індексу текстового поля та його імені
                        int textBoxIndex = (row * 10) + (column + 1);
                        string textBoxName = "textBox" + textBoxIndex;

                        // Пошук текстового поля на формі та очищення його значення
                        var textBox = form.Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;
                        if (textBox != null)
                        {
                            textBox.Text = string.Empty;
                            textBox.BackColor = SystemColors.Window; // Встановлення стандартного кольору фону
                        }
                    }
                }
            }
        }
    }
}
