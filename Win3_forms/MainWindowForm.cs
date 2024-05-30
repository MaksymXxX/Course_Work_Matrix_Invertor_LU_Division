using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms;

namespace Win3_forms
{
    public partial class MainWindowForm : Form
    {
        // Змінна для зберігання розміру матриці
        public int matrixSize = 0;
        // Змінна для зберігання вхідної матриці
        private double[,] inputMatrix;

        public MainWindowForm()
        {
            InitializeComponent();


            // Додавання варіантів розміру матриці до комбо-боксу
            comboBox1.Items.AddRange(new string[] { "2x2", "3x3", "4x4", "5x5", "6x6", "7x7", "8x8", "9x9", "10x10" });

            // Приховування всіх текстових полів за замовчуванням
            for (int i = 1; i <= 100; i++)
            {
                string textBoxName = "textBox" + i;
                var textBox = Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;
                if (textBox != null)
                {
                    textBox.Visible = false;
                }
            }

            // Додавання варіантів методу розв'язання до комбо-боксу
            comboBox2.Items.AddRange(new string[] { "LU", "Division" });
            comboBox1.SelectedItem = null;
            // Підписка на подію зміни вибраного елемента в comboBox1
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        // Обробник події зміни вибраного елемента в comboBox1
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Отримання вибраного значення з comboBox1
            string selectedValue = comboBox1.SelectedItem?.ToString();
            int size = 0;

            // Обробка випадку, коли вибране значення містить символ "x"
            if (selectedValue != null && selectedValue.Contains("x"))
            {
                int.TryParse(selectedValue.Split('x')[0], out size);
            }

            // Присвоєння розміру матриці
            matrixSize = size;

            // Відображення/приховування текстових полів залежно від вибраного розміру матриці
            for (int i = 1; i <= 100; i++)
            {
                int row = (i - 1) / 10 + 1;
                int column = (i - 1) % 10 + 1;

                string textBoxName = "textBox" + i;
                var textBox = Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;

                if (textBox != null)
                {
                    textBox.Visible = row <= size && column <= size;
                }
            }
        }

        // Обробник події натискання кнопки "Заповнити матрицю випадковими значеннями"
        private void button1_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано розмір матриці
            if (matrixSize == 0)
            {
                MessageBox.Show("Розмірність матриці не обрано. Будь ласка, виберіть розмірність матриці та запоніть її.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Створення об'єкту класу SetRandomMatrix та генерація випадкової матриці
            SetRandomMatrix setRandomMatrix = new SetRandomMatrix();
            setRandomMatrix.GenerateRandomMatrix(this, matrixSize);
        }

        // Обробник події натискання кнопки "Очистити матрицю"
        private void button3_Click_1(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано розмір матриці
            if (matrixSize == 0)
            {
                MessageBox.Show("Розмірність матриці не обрано. Будь ласка, виберіть розмірність матриці та запоніть її.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Перевірка, чи всі текстові поля порожні
            bool isEmptyMatrix = true;
            for (int row = 0; row < matrixSize; row++)
            {
                for (int column = 0; column < matrixSize; column++)
                {
                    int textBoxIndex = (row * 10) + (column + 1);
                    string textBoxName = "textBox" + textBoxIndex;
                    var textBox = Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;

                    if (textBox != null && !string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        isEmptyMatrix = false;
                        break;
                    }
                }
                if (!isEmptyMatrix)
                {
                    break;
                }
            }

            // Якщо всі текстові поля порожні, показати повідомлення про помилку
            if (isEmptyMatrix)
            {
                MessageBox.Show("Матриця порожня. Будь ласка, заповніть матрицю.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Створення об'єкту класу ClearTheMatrix та очищення матриці
            ClearTheMatrix clearTheMatrix = new ClearTheMatrix();
            clearTheMatrix.ClearMatrix(this, matrixSize);
        }

        // Обробник події натискання кнопки "Розв'язати систему"
        private void button2_Click_1(object sender, EventArgs e)
        {
            readData();
        }

        // Функція для перевірки правильності введення даних у текстових полях
        private bool isValidDecimalPlaces(string input)
        {
            int decimalIndex = input.IndexOf(',');
            if (decimalIndex != -1 && input.Length - decimalIndex - 1 > 2)
            {
                return false;
            }
            return true;
        }

        // Функція для обробки введених даних та розрахунку оберненої матриці
        private void readData()
        {
            // Перевірка, чи вибрано розмір матриці
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Розмірність матриці не обрано. Будь ласка, виберіть розмірність матриці та запоніть її.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Перевірка, чи вибрано метод розв'язання
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Метод  розв'язання не обраний. Будь ласка, виберіть метод розв'язання.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Створення двовимірного масиву для зберігання вхідної матриці
            double[,] myArray = new double[matrixSize, matrixSize];
            List<TextBox> invalidTextBoxes = new List<TextBox>();

            // Перевірка коректності введених даних у текстових полях
            for (int row = 0; row < matrixSize; row++)
            {
                for (int column = 0; column < matrixSize; column++)
                {
                    int textBoxIndex = (row * 10) + (column + 1);
                    string textBoxName = "textBox" + textBoxIndex;
                    var textBox = Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;

                    double value;
                    if ((textBox != null && string.IsNullOrWhiteSpace(textBox.Text)) || !double.TryParse(textBox.Text, out value))
                    {
                        // Додавання некоректного текстового поля до списку
                        invalidTextBoxes.Add(textBox);
                        textBox.BackColor = Color.Red;
                    }
                    else
                    {
                        textBox.BackColor = SystemColors.Window;
                    }
                }
            }

            // Якщо є некоректні текстові поля, показати повідомлення про помилку та вийти з функції
            if (invalidTextBoxes.Count > 0)
            {
                MessageBox.Show("Не всі текстові поля заповнені або заповнені в некоректному форматі");
                return;
            }

            bool isValid = true;
            for (int row = 0; row < matrixSize; row++)
            {
                for (int column = 0; column < matrixSize; column++)
                {
                    int textBoxIndex = (row * 10) + (column + 1);
                    string textBoxName = "textBox" + textBoxIndex;
                    var textBox = Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;

                    if (textBox != null)
                    {
                        double value;
                        if (double.TryParse(textBox.Text, out value))
                        {
                            // Перевірка додаткових умов для значень у текстових полях
                            if (value < -1000 || value > 1000)
                            {
                                isValid = false;
                                textBox.BackColor = Color.Red;
                            }
                            else if (textBox.Text.IndexOf('e') != -1 || textBox.Text.IndexOf('E') != -1)
                            {
                                isValid = false;
                                textBox.BackColor = Color.Red;
                            }
                            else if (!isValidDecimalPlaces(textBox.Text))
                            {
                                isValid = false;
                                textBox.BackColor = Color.Red;
                            }
                            else if (row == column && value == 0)
                            {
                                // Перевірка, що на головній діагоналі немає нульових значень
                                isValid = false;
                                textBox.BackColor = Color.Red;
                            }
                            else
                            {
                                // Зберігання коректного значення у вхідній матриці
                                myArray[row, column] = value;
                                textBox.BackColor = SystemColors.Window;
                            }
                        }
                        else
                        {
                            isValid = false;
                            textBox.BackColor = Color.Red;
                            MessageBox.Show("Невірний формат числа в текстовому полі " + textBoxName);
                            return;
                        }
                    }
                }
            }

            // Якщо дані некоректні, показати повідомлення про помилку та вийти з функції
            if (!isValid)
            {
                MessageBox.Show("Дані в текстових полях повинні бути в межах від -1000 до 1000 та мати не більше 2 цифр після коми. Нульове значенння на головній діагоналі та експоненційна форма заборонені");
                return;
            }

            // Якщо дані коректні, зберегти вхідну матрицю та виконати розрахунок оберненої матриці
            if (isValid)
            {
                inputMatrix = myArray;

                string selectedMethod = comboBox2.SelectedItem?.ToString();

                // Перевірка, чи визначник матриці не дорівнює нулю
                if (DeterminantIsZero(inputMatrix))
                {
                    MessageBox.Show("Матриця з нульовим визначником і не може бути обернена.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Вибір методу розв'язання системи
                try
                {
                    switch (selectedMethod)
                    {
                        case "LU":
                            LUDecomposition luDecomposition = new LUDecomposition();
                            int iterations = 0;
                            double[,] inverseMatrix = luDecomposition.SolveUsingLU(inputMatrix, out iterations, this);

                            // Відкриття нового вікна для відображення результатів
                            Form form2 = new ResultWindowForm(inputMatrix, inverseMatrix, iterations);
                            form2.FormClosed += Form2_FormClosed;
                            form2.Show();
                            this.Enabled = false;

                            break;

                        case "Division":
                            CellsDivision cellsDivision = new CellsDivision();
                            iterations = 0;
                            inverseMatrix = cellsDivision.SolveUsingDivision(inputMatrix, out iterations);

                            // Відкриття нового вікна для відображення результатів
                            ResultWindowForm form2Division = new ResultWindowForm(inputMatrix, inverseMatrix, iterations);
                            form2Division.FormClosed += Form2_FormClosed;
                            form2Division.Show();
                            this.Enabled = false;
                            break;

                        default:
                            MessageBox.Show("Невірний метод обрано.");
                            break;
                    }
                }
                catch(Exception exception)
                {
                    MessageBox.Show($"{exception.Message}" , "Помилка", MessageBoxButtons.OK);
                }
                
            }
        }

        // Обробник події закриття вікна з результатами
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
        }

        // Функція для перевірки, чи визначник матриці дорівнює нулю
        private bool DeterminantIsZero(double[,] matrix)
        {
            double det = Determinant(matrix);
            return Math.Abs(det) < 1e-10;
        }

        // Рекурсивна функція для обчислення визначника матриці
        private double Determinant(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double det = 0;

            if (n == 2)
            {
                det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    double[,] subMatrix = GetSubMatrix(matrix, i, 0);
                    double subDeterminant = Determinant(subMatrix);
                    det += (i % 2 == 0 ? 1 : -1) * matrix[i, 0] * subDeterminant;
                }
            }

            return det;
        }
        
        // Допоміжна функція для отримання підматриці
        private double[,] GetSubMatrix(double[,] matrix, int exRow, int exColumn)
        {
            int n = matrix.GetLength(0);
            double[,] subMatrix = new double[n - 1, n - 1];
            int r = 0, c = 0;

            for (int i = 0; i < n; i++)
            {
                // Пропускаємо рядок, який потрібно виключити
                if (i == exRow) continue;

                for (int j = 0; j < n; j++)
                {
                    // Пропускаємо стовпець, який потрібно виключити
                    if (j == exColumn) continue;

                    // Заповнюємо підматрицю
                    subMatrix[r, c] = matrix[i, j];
                    c++;
                }

                // Переходимо до наступного рядка підматриці
                r++;
                c = 0;
            }

            return subMatrix;
        }
    }
}