using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public int[,] input = new int[3, 5];
        NeuralNetwork neuralNetwork;

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            button2.Enabled = true;
            openFileDialog1.Title = "Укажите тестируемый файл";
            openFileDialog1.ShowDialog();
            pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            Bitmap im = pictureBox1.Image as Bitmap;
            for (var i = 0; i <= 5; i++) listBox1.Items.Add(" ");

            for (var x = 0; x <= 2; x++)
            {
                for (var y = 0; y <= 4; y++)
                {
                    // listBox1.Items.Add(Convert.ToString(im.GetPixel(x, y).R));
                    int n = (im.GetPixel(x, y).R);
                    if (n >= 250) n = 0;
                    else n = 1;
                    listBox1.Items[y] = listBox1.Items[y] + "  " + Convert.ToString(n);
                    input[x, y] = n;
                    //if (n == 0) input[x, y] = 1;
                }

            }

            recognize();
        }

        public void recognize()
        {
            neuralNetwork.mul_w();
            neuralNetwork.Sum();
            if (neuralNetwork.Rez()) listBox1.Items.Add(" - True, Sum = "+Convert.ToString(neuralNetwork.sum));
            else listBox1.Items.Add( " - False, Sum = "+Convert.ToString(neuralNetwork.sum));
        }

        class NeuralNetwork
        {
            public int[,] mul;
            public int[,] weight;
            public int[,] input;
            public int limit = 15;
            public int sum ;

            public NeuralNetwork(int sizex, int sizey,int[,] inP)
            {
                weight = new int[sizex, sizey];
                mul = new int[sizex, sizey];

                input = new int[sizex, sizey];
                input = inP;
            }

            public void mul_w()
            {
                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 4; y++)
                    {
                        mul[x, y] = input[x,y]*weight[x,y];
                    }
                }
            }

            public void Sum()
            {
                sum = 0;
                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 4; y++)
                    {
                        sum += mul[x, y];
                    }
                }
            }

            public bool Rez()
            {
                if (sum >= limit)
                    return true;
                else return false;
            }
            public void incW(int[,] inP)
            {
                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 4; y++)
                    {
                        weight[x, y] += inP[x, y];
                    }
                }
            }
            public void decW(int[,] inP)
            {
                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 4; y++)
                    {
                        weight[x, y] -= inP[x, y];
                    }
                }
            }

        }



        private void Form1_Load(object sender, EventArgs e)
        {
           

            neuralNetwork = new NeuralNetwork(3, 5,input);

            openFileDialog1.Title = "Укажите файл весов";
            openFileDialog1.ShowDialog();
            string s = openFileDialog1.FileName;
            StreamReader sr = File.OpenText(s);
            string line;
            string[] s1;
            int k = 0;
            while ((line = sr.ReadLine()) != null)
            {
               
                s1 = line.Split(' ');
                for (int i = 0; i < s1.Length; i++)
                {
                    listBox1.Items.Add("");
                    if (k < 5)
                    {
                        neuralNetwork.weight[i, k] = Convert.ToInt32(s1[i]);
                        listBox1.Items[k] += Convert.ToString(neuralNetwork.weight[i, k]);
                    }

                }
                k++;

            }
            sr.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;

                if (neuralNetwork.Rez() == false)
                    neuralNetwork.incW(input);
                else neuralNetwork.decW(input);
               
                //Запись
                  string s="";
                  string[] s1 = new string[5];
                  System.IO.File.Delete("w.txt");
                  FileStream FS = new FileStream("w.txt", FileMode.OpenOrCreate);
                  StreamWriter SW = new StreamWriter(FS);

                for (int y = 0; y <= 4; y++)
                {

                    s = Convert.ToString(neuralNetwork.weight[0, y]) + " " + Convert.ToString(neuralNetwork.weight[1, y]) + " " + Convert.ToString(neuralNetwork.weight[2, y]) ;
                        

                    s1[y] = s;
                   
                    SW.WriteLine(s);

                }
                SW.Close();


            
        }
    }

}
