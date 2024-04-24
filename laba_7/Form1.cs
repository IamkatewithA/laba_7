using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace laba_7
{
    public partial class Form1 : Form
    {
        System.Timers.Timer timer;
        int m, s;

        int height = 5; // size of the field
        int distance = 40; // didtance between buttons
        ButtonExtended[,] allButtons; // array of buttons
        int cellsOpened = 0; //counter of oppend cells

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            allButtons = new ButtonExtended[height, height];
            Random rng = new Random();
            for(int x = 40; (x - 40) < height * distance; x += distance)
            {
                for (int y = 40; (y - 40) < height * distance; y += distance)
                {
                    ButtonExtended button = new ButtonExtended();
                    button.Location = new Point(x, y);
                    button.Size = new Size(35, 35);
                    button.isClickable = true;

                    allButtons[(x - 40)/distance, (y - 40) / distance] = button;

                    Controls.Add(button);
                    button.MouseUp += new MouseEventHandler(FieldClick);
                }
            }


            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimeEvent;
        }

        private void OnTimeEvent(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s++;
                if (s == 60)
                {
                    s = 0;
                    m++;
                }
            }));
            label2.Text = string.Format("{0}:{1}", m.ToString().PadLeft(2, '0'), s.ToString().PadLeft(2, '0'));
        }

        void FieldClick(object sender, MouseEventArgs e)
        {
            ButtonExtended button = (ButtonExtended)sender;
            if (e.Button == MouseButtons.Left && button.isClickable)
            {
                if (button.isBomb)
                {
                    Explode(button);
                }
                else
                {
                    EmptyFieldClick(button);
                    cellsOpened++;
                    
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                
                button.isClickable = !button.isClickable;
                if (!button.isClickable)
                {
                    button.Text = "B";
                }
                else
                {
                    button.Text = "";
                }
            }
            CheckWin();
        }
        void Explode(ButtonExtended button)
        {
            timer.Stop();
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (allButtons[x, y].isBomb)
                    {
                        allButtons[x, y].Text = "*";
                    }
                }
            }
            MessageBox.Show("Вы проиграли");
            
        }
        void EmptyFieldClick(ButtonExtended button)
        {
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (allButtons[x, y] == button)
                    {
                        button.Text = "" + CountBombsAround(x, y);
                    }
                }
            }
            button.Enabled = false;
        }
        int CountBombsAround (int xB, int yB)
        {
            int bombCount = 0;
            for (int x = xB - 1; x <= xB + 1; x++)
            {
                for (int y = yB - 1; y <= yB + 1; y++)
                {
                    if(x >= 0 && x < height && y >= 0 && y < height)
                    {
                        if (allButtons[x, y].isBomb)
                        {
                            bombCount++;
                        }
                    }
                  
                }
            }
            return bombCount;
        }
        int i = 0;
        private void button1_Click(object sender, EventArgs e) // ЗАПИХНУТЬ В ОТДЕЛЬНЫЙ КЛАСС
        {
            try
            {
                string bomb = textBox1.Text;
                int bMax = int.Parse(bomb);
                Random random = new Random();
                int bCounter = 0;
                while (bCounter < bMax)
                {
                    int randx = random.Next(0, height);
                    int randy = random.Next(0, height);
                    if (!allButtons[randx, randy].isBomb)
                    {
                        allButtons[randx, randy].isBomb = true;
                        bCounter++;
                    }
                }
                MessageBox.Show("Бомбы расставлены\nПриступайте к игре");
                timer.Start();
            }
            catch
            {
                MessageBox.Show("Введите число от 1 до 10");
            }
        }
    void CheckWin()
        {
            int cells = height * height;
            int emptyCells = cells - int.Parse(textBox1.Text);
            if (cellsOpened == emptyCells)
            {
                timer.Stop();
                MessageBox.Show("Вы победили");
            }
        }


    }
    class ButtonExtended:Button
    {
        public bool isBomb;
        public bool isClickable;
    }
}
