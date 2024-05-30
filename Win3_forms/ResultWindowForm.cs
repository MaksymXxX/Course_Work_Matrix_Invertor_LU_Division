using System;
using System.IO;
using System.Windows.Forms;

namespace Win3_forms
{
    public partial class ResultWindowForm : Form
    {
        // Змінна для зберігання вхідної матриці
        private double[,] inputMatrix;
        // Змінна для зберігання оберненої матриці
        private double[,] inverseMatrix;

        private int iterationCount;
        public ResultWindowForm(double[,] inputMatrix, double[,] inverseMatrix, int iterationCount)
        {
            InitializeComponent();
            this.inputMatrix = inputMatrix;
            this.inverseMatrix = inverseMatrix;
            this.iterationCount = iterationCount;

            // Отримання розміру матриці
            int matrixSize = inverseMatrix.GetLength(0);

            // Приховування всіх клітинок за замовчуванням
            for (int i = 1; i <= 100; i++)
            {
                string labelName = "label" + i;
                var labels = Controls.Find(labelName, true);
                if (labels != null && labels.Length > 0)
                {
                    var label = labels[0] as Label;
                    if (label != null)
                    {
                        label.Visible = false;
                    }
                }
            }

            // Відображення клітинок з елементами оберненої матриці
            for (int row = 0; row < matrixSize; row++)
            {
                for (int col = 0; col < matrixSize; col++)
                {
                    int labelIndex = (row * 10) + col + 1;
                    string labelName = "label" + labelIndex;
                    var labels = Controls.Find(labelName, true);
                    if (labels != null && labels.Length > 0)
                    {
                        var label = labels[0] as Label;
                        if (label != null)
                        {
                            // Встановлення тексту клітинки  елементом оберненої матриці
                            label.Text = inverseMatrix[row, col].ToString("F4");
                            label.Visible = true;
                        }
                    }
                }

                var label220 = Controls.Find("label220", true);
                if (label220 != null && label220.Length > 0)
                {
                    var label = label220[0] as Label;
                    if (label != null)
                    {
                        label.Text = iterationCount.ToString();
                        label.Visible = true;
                    }
                }
            }
        }

        // Обробник події натискання кнопки "Зберегти матрицю"
        private void button1_Click(object sender, EventArgs e)
        {
            // Створення об'єкту класу SaveInverseMatrix для збереження матриць
            SaveInverseMatrix saveInverseMatrix = new SaveInverseMatrix();
            saveInverseMatrix.SaveMatrix(inputMatrix, inverseMatrix, this);
        }
    }
}