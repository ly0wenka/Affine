using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int h = 0; //высота изображения
        int w = 0; //ширина изображения

        int x1 ; //икс первой точки в декартовых координатах
        int y1; //игрик первой точки в декартовых координатах
        int x2; //икс второй точки в декартовых координатах
        int y2; //игрик втрой точки в декартовых координатах


        int x0 = 0; // икс точки поворота
        int y0 = 0; // игрик точки поворота
        Graphics g;
        Pen penBlack;
        Pen penRed;

        public Form1()
        {
            InitializeComponent();
            /*
            context.moveTo(@Model.X / 2, 0);
            context.lineTo(@Model.X / 2, @Model.Y);
            context.moveTo(0, @Model.Y / 2);

            context.lineTo(@Model.X, @Model.Y / 2);
            */

            g = Graphics.FromImage(pictureBox1.Image);
            //g.FillRectangle(Brushes.Red, new Rectangle(10, 10, 200, 200));
            h = pictureBox1.Height;
            w = pictureBox1.Width;
            penBlack = new Pen(Color.Black, 1);
            penRed = new Pen(Color.Red, 1);
            crest(g, penBlack, h, w);

            //Начальные значения
            x1 = 2;
            y1 = 2;
            x2 = 25;
            y2 = 25;
            x0 = 0;
            y0 = 0;

            //Точки переводим в экранные координаты
            XY(ref x1, ref y1);
            XY(ref x2, ref y2);
            XY(ref x0, ref y0);

            //Рисуем линию или более линий
            //g.DrawLine(penBlack, x1, y1, x2, y2);
            DrawLine4(x1, y1, x2, y2);
            /*
                        Change(ref x1, ref y1);
                        Change(ref x2, ref y2);


                        g.DrawLine(penRed, x1, y1, x2, y2);
                        */
            //Вывод в лейбелы
            Vivod();

            //обновление картинки
            pictureBox1.Refresh();
        }

        void DrawLine4(int x1, int y1, int x2, int y2)
        {
            richTextBox1.Text = "";
            int dx, dy, d, i1, i2, x, y, temp, ystep;
            bool swap = false;

            var bmp = pictureBox1.Image as Bitmap;
            bmp.SetPixel(x1, y1, Color.Red);
            bmp.SetPixel(x2, y2, Color.Red);
            pictureBox1.Image = bmp;
            if (Math.Abs(x1 - x2) < Math.Abs(y1 - y2))
            {
                temp = x1;
                x1 = y1;
                y1 = temp;
                temp = x2;
                x2 = y2;
                y2 = temp;
                swap = true;
            }
            if (x1 > x2)
            {
                temp = x1;
                x1 = x2;
                x2 = temp;
                temp = y1;
                y1 = y2;
                y2 = temp;
            }
            dx = x2 - x1;
            dy = Math.Abs(y2 - y1);
            d = 2 * dy - dx;
            i1 = 2 * dy;
            i2 = i1 - 2 * dx;
            x = x1;
            y = y1;
            ystep = (y1 > y2 ? -1 : 1);
            for (x++; x < x2; )
            {
                x++;
                bmp.SetPixel(swap ? y : x, swap ? x : y, Color.Blue);
                richTextBox1.Text += "(" + (swap ? ecranY(y) : ecranX(x)).ToString() +
", " + (swap ? ecranX(x) : ecranY(y)).ToString() + "); ";
                if (d < 0)
                {
                    d += i1;

                }
                else
                {
                    y += ystep;
                    
                    bmp.SetPixel(swap ? y : x, swap ? x : y, Color.Blue);
                    richTextBox1.Text += "(" + (swap ? ecranY(y) : ecranX(x)).ToString() +
", " + (swap ? ecranX(x) : ecranY(y)).ToString() + "); ";
                    d += i2;
                }

            }
            pictureBox1.Image = bmp;


        }

        void Vivod()
        {
            label7.Text = x1.ToString();
            label8.Text = y1.ToString();
            label9.Text = x2.ToString();
            label10.Text = y2.ToString();

            label19.Text = x0.ToString();
            label20.Text = y0.ToString();

            VivodEcran();
        }

        void VivodEcran()
        {
            label11.Text = ecranX(x1).ToString();
            label12.Text = ecranY(y1).ToString();
            label13.Text = ecranX(x2).ToString();
            label14.Text = ecranY(y2).ToString();
        }

        //Изменение одной точки
        void Change(ref int x, ref int y)
        {
            /*
            Matrix myMatrix = new Matrix(1, 0, -1, 0, x, y);            
            //myMatrix.RotateAt(Convert.ToSingle(textBox5.Text), new PointF(x0, y0), MatrixOrder.Append);
            myMatrix.Scale(Convert.ToSingle(textBox1.Text), Convert.ToSingle(textBox2.Text), MatrixOrder.Append);
            myMatrix.Translate(Convert.ToSingle(textBox3.Text), Convert.ToSingle(textBox4.Text), MatrixOrder.Append);

            Get(myMatrix, ref x, ref y);
             */
            Translate(ref x, ref y);
            Scale(ref x, ref y);
            Rotate(ref x, ref y);
        }
        //Смещение
        void Translate(ref int x, ref int y)
        {
            x += Convert.ToInt32(Convert.ToSingle(textBox4.Text));
            y += Convert.ToInt32(Convert.ToSingle(textBox3.Text));
        }
        //Растяжение/сжатие
        void Scale(ref int x, ref int y)
        {
            x = Convert.ToInt32(x * Convert.ToSingle(textBox2.Text));
            y = Convert.ToInt32(y * Convert.ToSingle(textBox1.Text));
        }

        //Поворот на альфа
        void Rotate(ref int x, ref int y)
        {
            double alfa = Convert.ToSingle(textBox5.Text) * Math.PI / 180;
            double c = Math.Cos(alfa);
            double s = Math.Sin(alfa);

            // XY(ref x0, ref y0);
            // x = (x - x0) * cos a - (y - y0) * sin a + x0
            // y = (x - x0) * sin a - (y - y0) * cos a + y0
            x = Convert.ToInt32((Convert.ToDouble(x - x0) * c - Convert.ToDouble(y - y0) * s + x0));
            y = Convert.ToInt32((Convert.ToDouble(x - x0) * s + Convert.ToDouble(y - y0) * c + y0));
        }
        /*
        void Get(Matrix myMatrix, ref int x1, ref int y1)
        {
            x1 = Convert.ToInt32(myMatrix.Elements[4]);
            y1 = Convert.ToInt32(myMatrix.Elements[5]);
        }*/


        //Переход от декартовой системы в экранную
        void XY(ref int x, ref int y)
        {
            x = x + w / 2;
            y = h / 2 - y;
        }
        //Перевод x в декартовую систему
        int ecranX(int x)
        {
            return x - w / 2;
        }
        //Перевод y в декартовую систему
        int ecranY(int y)
        {
            return h / 2 - y;
        }
        //Рисуем декартовую систему
        void crest(Graphics g, Pen penBlack, int h, int w)
        {
            g.DrawLine(penBlack, w / 2, 0, w / 2, h);
            g.DrawLine(penBlack, 0, h / 2, w, h / 2);
        }

        // 
        private void button1_Click(object sender, EventArgs e)
        {
            Text = "Обработка";
            pictureBox1.Image = WindowsFormsApplication1.Properties.Resources.New_Bitmap_Image;
            //  pictureBox1.Image = PictureBoxClone;
            g = Graphics.FromImage(pictureBox1.Image);
            //g.FillRectangle(Brushes.Red, new Rectangle(10, 10, 200, 200));
            //h = pictureBox1.Height;
            // w = pictureBox1.Width;
            penBlack = new Pen(Color.Black, 1);
            penRed = new Pen(Color.Red, 1);
            crest(g, penBlack, h, w);



            x0 = Convert.ToInt32(textBox6.Text);
            y0 = Convert.ToInt32(textBox7.Text);
            XY(ref x0, ref y0);

            //g.DrawLine(penBlack, x1, y1, x2, y2);


            Change(ref x1, ref y1);
            Change(ref x2, ref y2);


            //g.DrawLine(penRed, x1, y1, x2, y2);
            DrawLine4(x1, y1, x2, y2);
            Vivod();
            pictureBox1.Refresh();
            // pictureBox1.Invalidate();
            Text = "Афинные преобразования декартовой системы";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = WindowsFormsApplication1.Properties.Resources.New_Bitmap_Image;
            g = Graphics.FromImage(pictureBox1.Image);
            crest(g, penBlack, h, w);
            x1 = 2;
            y1 = 2;
            x2 = 25;
            y2 = 25;
            x0 = 0;
            y0 = 0;
            XY(ref x1, ref y1);
            XY(ref x2, ref y2);
            //g.DrawLine(penBlack, x1, y1, x2, y2);
            DrawLine4(x1, y1, x2, y2);
            Vivod();
            pictureBox1.Refresh();
        }
    }
}
