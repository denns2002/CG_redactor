using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace _6
{
    public partial class Form1 : Form
    {

        private float rotation = 1.0f;
        List<string> figures = new List<string>();
        List<string> figuresNames = new List<string>();
        List<List<float>> figuresAttrs = new List<List<float>>();
        List<float[]> figuresColors = new List<float[]>();
        double angle = 0;
        double dangle = 0;
        bool loaded = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loaded = true; //установкавида
            comboBox1.SelectedIndex = 0;
            SetupViewport(glControl1); //выбор оси для вращения (X)
        }

        private void SetupViewport(GLControl glControl)
        {

            float aspectRatio = (float)glControl.Width / (float)glControl.Height;
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1f, 5000000000);
            GL.MultMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void glControl1_Resize_1(object sender, EventArgs e)
        {
            if (!loaded) return;
            SetupViewport(glControl1);
        }//resize

        private void glControl1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f); //цветфона
            GL.Enable(EnableCap.DepthTest);
            this.Paint += new PaintEventHandler(glControl1_Paint_1);
        }

        [Obsolete]
        private void glControl1_Paint_1(object sender, PaintEventArgs e)
        {
            if (!loaded) return;
            //onload

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //выборвида

            //Vid(comboBox1.SelectedIndex.ToString());
            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 modelview = Matrix4.LookAt(new Vector3(-300, 300, 200), new Vector3(0, 0, 0), new Vector3(0, 0, 1));
            GL.LoadMatrix(ref modelview);//выборосивращения
            switch (comboBox1.Text)
            {
                case "X": GL.Rotate(angle, 1, 0, 0); break;
                case "Y": GL.Rotate(angle, 0, 1, 0); break;
                case "Z": GL.Rotate(angle, 0, 0, 1); break;
            }
            GL.Scale(scale, scale, scale);

            Vector3 clr1 = new Vector3(1.0f, 1.0f, 0.0f);
            Vector3 clr2 = new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 clr3 = new Vector3(0.2f, 0.9f, 1.0f);
            Vector3 clr4 = new Vector3(1.0f, 0.9f, 1.0f);

            //Рисуем оси
            GL.Begin(BeginMode.Lines);
            GL.Color3(clr1); //X-желтая
            GL.Vertex3(-3000.0f, 0.0f, 0.0f);
            GL.Vertex3(3000.0f, 0.0f, 0.0f);
            GL.End();
            GL.Begin(BeginMode.Lines);
            GL.Color3(clr2); //Y-красная
            GL.Vertex3(0.0f, -3000.0f, 0.0f);
            GL.Vertex3(0.0f, 3000.0f, 0.0f);
            GL.End();
            GL.Begin(BeginMode.Lines);
            GL.Color3(clr3); //Z-голубая
            GL.Vertex3(0.0f, 0.0f, -3000f);
            GL.Vertex3(0.0f, 0.0f, 3000.0f);
            GL.End();

            for (int i = 0; i < figures.Count(); i++)
            {
                switch (figures[i])
                {
                    case "Parallelepiped":
                        Parallelepiped(figuresAttrs[i][0], figuresAttrs[i][1], figuresAttrs[i][2], figuresAttrs[i][3], figuresAttrs[i][4], figuresAttrs[i][5], figuresAttrs[i][6], figuresAttrs[i][7], figuresAttrs[i][8], figuresAttrs[i][9], figuresAttrs[i][10], figuresAttrs[i][11], figuresAttrs[i][12], figuresColors[i]);
                        break;

                    case "Trapezoid":
                        Trapezoid(figuresAttrs[i][0], figuresAttrs[i][1], figuresAttrs[i][2], figuresAttrs[i][3], figuresAttrs[i][4], figuresAttrs[i][5], figuresAttrs[i][6], figuresAttrs[i][7], figuresAttrs[i][8], figuresAttrs[i][9], figuresAttrs[i][10], figuresAttrs[i][11], figuresAttrs[i][12], figuresColors[i]);
                        break;

                    case "Octahedron":
                        Octahedron(figuresAttrs[i][0], figuresAttrs[i][1], figuresAttrs[i][2], figuresAttrs[i][3], figuresAttrs[i][4], figuresAttrs[i][5], figuresAttrs[i][6], figuresAttrs[i][7], figuresAttrs[i][8], figuresAttrs[i][9], figuresAttrs[i][10], figuresAttrs[i][11], figuresAttrs[i][12], figuresColors[i]);
                        break;

                    case "PyramidReg":
                        PyramidReg(figuresAttrs[i][0], figuresAttrs[i][1], figuresAttrs[i][2], figuresAttrs[i][3], figuresAttrs[i][4], figuresAttrs[i][5], figuresAttrs[i][6], figuresAttrs[i][7], figuresAttrs[i][8], figuresAttrs[i][9], figuresAttrs[i][10], figuresAttrs[i][11], figuresAttrs[i][12], figuresColors[i]);
                        break;

                    case "Cylinder":
                        Cylinder(figuresAttrs[i][0], figuresAttrs[i][1], figuresAttrs[i][2], figuresAttrs[i][3], figuresAttrs[i][4], figuresAttrs[i][5], figuresAttrs[i][6], figuresAttrs[i][7], figuresAttrs[i][8], figuresAttrs[i][9], figuresAttrs[i][10], figuresAttrs[i][11], figuresAttrs[i][12], figuresColors[i]);
                        break;

                    case "Sphere":
                        Sphere(figuresAttrs[i][0], figuresAttrs[i][1], figuresAttrs[i][2], figuresAttrs[i][3], figuresAttrs[i][4], figuresAttrs[i][5], figuresAttrs[i][6], figuresAttrs[i][7], figuresAttrs[i][8], figuresAttrs[i][9], figuresAttrs[i][10], figuresAttrs[i][11], figuresAttrs[i][12], figuresAttrs[i][13], figuresAttrs[i][14], figuresColors[i]);
                        break;

                    case "Torus":
                        Torus(figuresAttrs[i][0], figuresAttrs[i][1], figuresAttrs[i][2], figuresColors[i], figuresAttrs[i][3]);
                        break;

                    case "Helix":
                        Helix(figuresAttrs[i][0], figuresAttrs[i][1], figuresAttrs[i][2], figuresColors[i], figuresAttrs[i][3]);
                        break;
                }
            }

            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(figuresNames.ToArray());

            glControl1.SwapBuffers();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            angle = 0;
            glControl1.Invalidate();

        }

        public void Rotate(int dangle)
        {
            while (_run)
            {
                angle += dangle;
                glControl1.Invalidate();
                Thread.Sleep(20);
            }
        }

        private bool _run = false;
        private void button1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            Task.Factory.StartNew(() => Rotate(1));
        }

        private void button1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = false;
        }

        private void button3_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            Task.Factory.StartNew(() => Rotate(-1));
        }

        private void button3_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = false;
        }

        int figureCount = 1;
        [Obsolete]
        private void button4_Click(object sender, EventArgs e)
        {
            var X = float.Parse(textBox1.Text);
            var Y = float.Parse(textBox2.Text);
            var Z = float.Parse(textBox6.Text);
            var r = float.Parse(textBox8.Text);
            var r2 = float.Parse(textBox10.Text);
            var count = float.Parse(textBox9.Text);
            var scale = float.Parse(textBox7.Text, CultureInfo.InvariantCulture.NumberFormat);
            var width = float.Parse(textBox3.Text) * scale;
            var lenght = float.Parse(textBox4.Text) * scale;
            var height = float.Parse(textBox5.Text) * scale;
            float rX = float.Parse(textBox13.Text);
            float rY = float.Parse(textBox12.Text);
            float rZ = float.Parse(textBox11.Text);

            float[] colors = new float[3];
            var color = comboBox4.Text;

            switch (color)
            {
                case "Red": colors = new float[] { 1, 0, 0 }; break;
                case "Blue": colors = new float[] { 0, 0, 1 }; break;
                case "Green": colors = new float[] { 0, 1, 0 }; break;
                case "Yellow": colors = new float[] { 1, 1, 0 }; break;
                case "Purple": colors = new float[] { 1, 0, 1 }; break;
                case "Aqua": colors = new float[] { 0, 1, 1 }; break;
            }

            switch (comboBox2.Text)
            {
                case "Pyramid": //
                    figures.Add("PyramidReg");
                    figuresAttrs.Add(new List<float> { X, Y, Z, rX, rY, rZ, r, r2, width, lenght, height, count, scale / 2 });
                    figuresColors.Add(colors);
                    figuresNames.Add(figureCount.ToString() + ". PyramidReg " + color);
                    break;

                case "Parallelepiped": //
                    figures.Add("Parallelepiped");
                    figuresAttrs.Add(new List<float> { X, Y, Z, rX, rY, rZ, 0, 0, width, lenght, height, 0, 0 });
                    figuresColors.Add(colors);
                    figuresNames.Add(figureCount.ToString() + ". Parallelepiped " + color);
                    break;

                case "Trapezoid": //
                    figures.Add("Trapezoid");
                    figuresAttrs.Add(new List<float> { X, Y, Z, rX, rY, rZ, 0, 0, width, lenght, height, 0, 0 });
                    figuresColors.Add(colors);
                    figuresNames.Add(figureCount.ToString() + ". Trapezoid " + color);
                    break;

                case "Octahedron": //
                    figures.Add("Octahedron");
                    figuresAttrs.Add(new List<float> { X, Y, Z, rX, rY, rZ, 0, 0, width / 2, lenght / 2, height / 2, 0, 0 });
                    figuresColors.Add(colors);
                    figuresNames.Add(figureCount.ToString() + ". Octahedron " + color);
                    break;

                case "Cylinder": //
                    figures.Add("Cylinder");
                    figuresAttrs.Add(new List<float> { X, Y, Z, rX, rY, rZ, r, r2, 0, 0, height, count, scale / 2 });
                    figuresColors.Add(colors);
                    figuresNames.Add(figureCount.ToString() + ". Cylinder " + color);
                    break;

                case "Sphere": //
                    figures.Add("Sphere");
                    figuresAttrs.Add(new List<float> { X, Y, Z, rX, rY, rZ, r, 0, 0, 0, 0, 0, 0, 25, 25 });
                    figuresColors.Add(colors);
                    figuresNames.Add(figureCount.ToString() + ". Sphere " + color);
                    break;

                case "Torus":
                    figures.Add("Torus");
                    figuresAttrs.Add(new List<float> { X, Y, Z, rX, rY, rZ, scale / 2 });
                    figuresColors.Add(colors);
                    figuresNames.Add(figureCount.ToString() + ". Torus " + color);
                    break;

                case "Helix":
                    figures.Add("Helix");
                    figuresAttrs.Add(new List<float> { X, Y, Z, scale / 3 });
                    figuresColors.Add(colors);
                    figuresNames.Add(figureCount.ToString() + ". Helix " + color);
                    break;
            }

            figureCount++;
            glControl1.Invalidate();
        }

        private List<Vector3> CreateColor(int count, float[] colors)
        {
            var color = new List<Vector3>();
            float number = 0.6f / count;

            for (int i = 1; i < count + 1; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (colors[j] == 1) { continue; }
                    colors[j] = number * i;
                }
                color.Add(new Vector3(colors[0], colors[1], colors[2]));
            }

            return color;
        }

        //ok
        private void Parallelepiped(float X, float Y, float Z, float rX, float rY, float rZ, float r, float r2, float width, float lenght, float height, float count, float scale, float[] colors)
        {
            var parallelepiped = new float[][][] {
                new float[][] {
                    new float[] {X, Y, Z},
                    new float[] { X + width, Y, Z },
                    new float[] { X + width, Y, Z + height },
                    new float[] { X, Y, Z + height } },
                new float[][] {
                    new float[] { X + width, Y, Z },
                    new float[] { X + width, Y + lenght, Z },
                    new float[] { X + width, Y + lenght, Z + height },
                    new float[] { X + width, Y, Z + height } },
                new float[][] {
                    new float[] { X, Y, Z },
                    new float[] { X, Y + lenght, Z },
                    new float[] { X, Y + lenght, Z + height },
                    new float[] { X, Y, Z + height } },
                new float[][] {
                    new float[] { X, Y+lenght, Z },
                    new float[] { X+width, Y+lenght, Z } ,
                    new float[] { X + width, Y + lenght, Z + height },
                    new float[] { X, Y+lenght, Z+height } },
                new float[][] {
                    new float[] { X, Y, Z },
                    new float[] { X + width, Y, Z },
                    new float[] { X + width, Y + lenght, Z },
                    new float[] { X, Y + lenght, Z } },
                new float[][] {
                    new float[] { X, Y, Z + height }, 
                    new float[] { X + width, Y, Z + height }, 
                    new float[] { X + width, Y + lenght, Z + height }, 
                    new float[] { X, Y + lenght, Z + height } }
            };

            double angle;
            double cos;
            double sin;
            float newX;
            float newY;
            float newZ;

            foreach (var first in parallelepiped)
            {
                foreach (var second in first)
                {
                    if (rX != 0)
                    {
                        angle = rX / 180.0 * Math.PI;
                        cos = Math.Cos(angle);
                        sin = Math.Sin(angle);

                        for (int i = 0; i < first.Count(); i++)
                        {
                            newY = second[1];
                            newZ = second[2];
                            second[1] = newY * (float)cos - newZ * (float)sin;
                            second[2] = newZ * (float)cos + newY * (float)sin;
                        }
                    }

                    if (rY != 0)
                    {
                        angle = rY / 180.0 * Math.PI;
                        cos = Math.Cos(angle);
                        sin = Math.Sin(angle);

                        for (int i = 0; i < first.Count(); i++)
                        {
                            newX = second[0];
                            newZ = second[2];
                            second[0] = newX * (float)cos - newZ * (float)sin;
                            second[2] = newZ * (float)cos + newX * (float)sin;
                        }
                    }

                    if (rZ != 0)
                    {
                        angle = rZ / 180.0 * Math.PI;
                        cos = Math.Cos(angle);
                        sin = Math.Sin(angle);

                        for (int i = 0; i < first.Count(); i++)
                        {
                            newX = second[0];
                            newY = second[1];
                            second[0] = newX * (float)cos - newY * (float)sin;
                            second[1] = newY * (float)cos + newX * (float)sin;
                        }
                    }
                }
            }

            var parallelepipedColors = CreateColor(6, colors);

            foreach (var figure in parallelepiped.Zip(parallelepipedColors, Tuple.Create))
            {
                GL.Begin(BeginMode.Polygon);

                foreach (var primitive in figure.Item1)
                {
                    GL.Color3(figure.Item2);
                    GL.Vertex3(primitive[0], primitive[1], primitive[2]);
                }

                GL.End();
            }
        }

        //ok
        private void Trapezoid(float X, float Y, float Z, float rX, float rY, float rZ, float r, float r2, float width, float lenght, float height, float count, float scale, float[] colors)
        {
            var trapezoid = new float[][][] {
                new float[][] {
                    new float[] { X, Y, Z },
                    new float[] { X+width, Y, Z },
                    new float[] {X+width*3/4, Y+lenght/4, Z+height/2 },
                    new float[] { X+width/4, Y+lenght/4, Z+height/2 } },
                new float[][] {
                    new float[] { X, Y, Z }, 
                    new float[] { X, Y+lenght, Z }, 
                    new float[] { X+width/4, Y+lenght*3/4, Z+height/2 }, 
                    new float[] { X+width/4, Y+lenght/4, Z+height/2 } },
                new float[][] {
                    new float[] { X, Y+lenght, Z }, 
                    new float[] { X+width, Y+lenght, Z },
                    new float[] { X+width*3/4, Y+lenght*3/4, Z+height/2 }, 
                    new float[] { X+width/4, Y+lenght*3/4, Z+height/2 } },
                new float[][] {
                    new float[] { X+width, Y+lenght, Z }, 
                    new float[] { X+width, Y, Z }, 
                    new float[] { X+width*3/4, Y+lenght/4, Z+height/2 }, 
                    new float[] { X+width*3/4, Y+lenght*3 /4, Z+height/2 } } };

            var reqt = new float[][] {
                new float[] { X, Y, Z },
                new float[] { X + width, Y, Z },
                new float[] { X + width, Y + lenght, Z },
                new float[] { X, Y + lenght, Z }  };
            var reqt2 = new float[][] { 
                new float[] { X + width / 4, Y + lenght / 4, Z + height / 2 },
                new float[] { X + width * 3 / 4, Y + lenght / 4, Z + height / 2 }, 
                new float[] { X + width * 3 / 4, Y + lenght * 3 / 4, Z + height / 2 }, 
                new float[] { X + width / 4, Y + lenght * 3 / 4, Z + height / 2 } };

            var trapezoidColors = CreateColor(6, colors);

            double angle;
            double cos;
            double sin;
            float newX;
            float newY;
            float newZ;

            if (rX != 0)
            {
                angle = rX / 180.0 * Math.PI;
                cos = Math.Cos(angle);
                sin = Math.Sin(angle);

                foreach (var first in reqt)
                {
                    for (int i = 0; i < reqt.Count(); i++)
                    {
                        newY = first[1];
                        newZ = first[2];
                        first[1] = newY * (float)cos - newZ * (float)sin;
                        first[2] = newZ * (float)cos + newY * (float)sin;
                    }
                }

                foreach (var first in reqt2)
                {
                    for (int i = 0; i < reqt2.Count(); i++)
                    {
                        newY = first[1];
                        newZ = first[2];
                        first[1] = newY * (float)cos - newZ * (float)sin;
                        first[2] = newZ * (float)cos + newY * (float)sin;
                    }
                }

                foreach (var first in trapezoid)
                {
                    foreach (var second in first)
                    {
                        for (int i = 0; i < first.Count(); i++)
                        {
                            newY = second[1];
                            newZ = second[2];
                            second[1] = newY * (float)cos - newZ * (float)sin;
                            second[2] = newZ * (float)cos + newY * (float)sin;
                        }
                    }
                }
            }

            if (rY != 0)
            {
                angle = rY / 180.0 * Math.PI;
                cos = Math.Cos(angle);
                sin = Math.Sin(angle);

                foreach (var first in trapezoid)
                {
                    foreach (var second in first)
                    {
                        for (int i = 0; i < first.Count(); i++)
                        {
                            newX = second[0];
                            newZ = second[2];
                            second[0] = newX * (float)cos - newZ * (float)sin;
                            second[2] = newZ * (float)cos + newX * (float)sin;
                        }
                    }
                }
            }

            if (rZ != 0)
            {
                angle = rZ / 180.0 * Math.PI;
                cos = Math.Cos(angle);
                sin = Math.Sin(angle);

                foreach (var first in trapezoid)
                {
                    foreach (var second in first)
                    {
                        for (int i = 0; i < first.Count(); i++)
                        {
                            newX = second[0];
                            newY = second[1];
                            second[0] = newX * (float)cos - newY * (float)sin;
                            second[1] = newY * (float)cos + newX * (float)sin;
                        }
                    }
                }
            }


            foreach (var figure in trapezoid.Zip(trapezoidColors, Tuple.Create))
            {
                GL.Begin(BeginMode.Polygon);
                foreach (var primitive in figure.Item1)
                {
                    GL.Color3(figure.Item2);
                    GL.Vertex3(primitive);
                }
                GL.End();
            }

            GL.Begin(BeginMode.Polygon);
            GL.Color3(trapezoidColors.First());
            foreach (var prim in reqt)
            {
                GL.Vertex3(prim[0], prim[1], prim[2]);
            }
            GL.End();

            GL.Begin(BeginMode.Polygon);
            GL.Color3(trapezoidColors.Last());
            foreach (var prim in reqt2)
            {
                GL.Vertex3(prim[0], prim[1], prim[2]);
            }
            GL.End();
        }

        //ok
        private void Octahedron(float X, float Y, float Z, float rX, float rY, float rZ, float r, float r2, float width, float lenght, float height, float count, float scale, float[] colors)
        {
            var octaedr = new float[][][] {
                new float[][] {
                    new float[] { X, Y, Z } , 
                    new float[] { X+width, Y, Z } , 
                    new float[] { X+width/2, Y+lenght/2, Z+height } },
                new float[][] {
                    new float[] { X, Y, Z }, 
                    new float[] { X+width, Y, Z }, 
                    new float[] { X+width/2, Y+lenght/2, Z-height } },
                new float[][] {
                    new float[] { X, Y, Z },
                    new float[] { X, Y+lenght, Z }, 
                    new float[] { X+width/2, Y+lenght/2, Z+height } },
                new float[][] {
                    new float[] { X, Y, Z }, 
                    new float[] { X, Y+lenght, Z },
                    new float[] { X+width/2, Y+lenght/2, Z-height } },
                new float[][] {
                    new float[] { X, Y+lenght, Z },
                    new float[] { X+width, Y+lenght, Z }, 
                    new float[] { X+width/2, Y+lenght/2, Z+height } },
                new float[][] {
                    new float[] { X, Y+lenght, Z }, 
                    new float[] { X+width, Y+lenght, Z },
                    new float[] { X+width/2, Y+lenght/2, Z-height } },
                new float[][] {
                    new float[] { X+width, Y+lenght, Z }, 
                    new float[] { X+width, Y, Z }, 
                    new float[] { X+width/2, Y+lenght/2, Z+height } },
                new float[][] {
                    new float[] { X+width, Y+lenght, Z }, 
                    new float[] { X+width, Y, Z }, 
                    new float[] { X+width/2, Y+lenght/2, Z-height } },
            };

            var octaedrColors = CreateColor(8, colors);

            double angle;
            double cos;
            double sin;
            float newX;
            float newY;
            float newZ;

            foreach (var first in octaedr)
            {
                foreach (var second in first)
                {
                    if (rX != 0)
                    {
                        angle = rX / 180.0 * Math.PI;
                        cos = Math.Cos(angle);
                        sin = Math.Sin(angle);

                        for (int i = 0; i < first.Count(); i++)
                        {
                            newY = second[1];
                            newZ = second[2];
                            second[1] = newY * (float)cos - newZ * (float)sin;
                            second[2] = newZ * (float)cos + newY * (float)sin;
                        }
                    }

                    if (rY != 0)
                    {
                        angle = rY / 180.0 * Math.PI;
                        cos = Math.Cos(angle);
                        sin = Math.Sin(angle);

                        for (int i = 0; i < first.Count(); i++)
                        {
                            newX = second[0];
                            newZ = second[2];
                            second[0] = newX * (float)cos - newZ * (float)sin;
                            second[2] = newZ * (float)cos + newX * (float)sin;
                        }
                    }

                    if (rZ != 0)
                    {
                        angle = rZ / 180.0 * Math.PI;
                        cos = Math.Cos(angle);
                        sin = Math.Sin(angle);

                        for (int i = 0; i < first.Count(); i++)
                        {
                            newX = second[0];
                            newY = second[1];
                            second[0] = newX * (float)cos - newY * (float)sin;
                            second[1] = newY * (float)cos + newX * (float)sin;
                        }
                    }
                }
            }

            foreach (var figure in octaedr.Zip(octaedrColors, Tuple.Create))
            {
                GL.Begin(BeginMode.Triangles);

                foreach (var primitive in figure.Item1)
                {
                    GL.Color3(figure.Item2);
                    GL.Vertex3(primitive);
                }

                GL.End();
            }
        }

        //ok
        private void PyramidReg(float X, float Y, float Z, float rX, float rY, float rZ, float r, float r2, float width, float lenght, float height, float count, float scale, float[] colors)
        {
            // X, Y, Z, rX, rY, rZ, r, r2, width, lenght, height, count, scale / 2
            var pyramid = new List<Vector3[]>();
            List<double> x = new List<double>(); 
            List<double> y = new List<double>();
            List<double> z = new List<double>();


            var pyramidColors = CreateColor((int)count + 1, colors);
            for (int i = 1; i <= count; i++)
            {
                x.Add(1+(r * Math.Cos(2 * Math.PI * i / count) * (double)scale));
                y.Add(1+(r * Math.Sin(2 * Math.PI * i / count) * (double)scale));
                z.Add(1);
            }
            var point = new double[] { 1, 1, 1 + height };
            double angle;
            double cos;
            double sin;

            if (rX != 0)
            {
                angle = rX / 180.0 * Math.PI;
                cos = Math.Cos(angle);
                sin = Math.Sin(angle);
                var newY = point[1];
                var newZ = point[2];
                point[1] = newY * cos - newZ * sin;
                point[2] = newZ * cos + newY * sin;

                for (int i = 0; i < x.Count(); i++)
                {
                    newY = y[i];
                    newZ = z[i];
                    y[i] = newY * cos - newZ * sin;
                    z[i] = newZ * cos + newY * sin;
                }
            }

            if (rY != 0)
            {
                angle = rY / 180.0 * Math.PI;
                cos = Math.Cos(angle);
                sin = Math.Sin(angle);
                var newX = point[0];
                var newZ = point[2];
                point[0] = newX * cos - newZ * sin;
                point[2] = newZ * cos + newX * sin;

                for (int i = 0; i < x.Count(); i++)
                {
                    newX = x[i];
                    newZ = z[i];
                    x[i] = newX * cos - newZ * sin;
                    z[i] = newZ * cos + newX * sin;
                }
            }

            if (rZ != 0)
            {
                angle = rZ / 180.0 * Math.PI;
                cos = Math.Cos(angle);
                sin = Math.Sin(angle);
                var newX = point[0];
                var newY = point[1];
                point[0] = newX * cos - newY * sin;
                point[1] = newY * cos + newX * sin;

                for (int i = 0; i < x.Count(); i++)
                {
                    newX = x[i];
                    newY = y[i];
                    x[i] = newX * cos - newY * sin;
                    y[i] = newY * cos + newX * sin;
                }
            }

            GL.Color3(pyramidColors.Last());
            GL.Begin(BeginMode.Polygon);
            for (int i = 0; i < count; i++)
            {
                GL.Vertex3(new Vector3(X + (float)x[i], Y + (float)y[i], Z + (float)z[i]));
            }
            GL.End();

            for (int i = 0; i < count; i++)
            {
                GL.Color3(pyramidColors[i]);
                GL.Begin(BeginMode.Triangles);

                if (i == count - 1)
                {
                    GL.Vertex3(new Vector3(X + (float)x[i], Y + (float)y[i], Z + (float)z[i]));
                    GL.Vertex3(new Vector3(X + (float)x.First(), Y + (float)y.First(), Z + (float)z.First()));
                }
                else
                {
                    GL.Vertex3(new Vector3(X + (float)x[i], Y + (float)y[i], Z + (float)z[i]));
                    GL.Vertex3(new Vector3(X + (float)x[i + 1], Y + (float)y[i + 1], Z + (float)z[i+1]));
                }
                GL.Vertex3(X + point[0], Y + point[1], Z + point[2] );

                GL.End();
            }
        }
        
        //ok
        private void Sphere(float X, float Y, float Z, float rX, float rY, float rZ, float r, float r2, float width, float lenght, float height, float count, float scale, float nx, float ny, float[] colors)
        {
            var color = CreateColor((int)ny, colors);
            var colorParam = true;
            int colorCount = 0;

            int ix, iy;
            double x, y, z, sy, cy, sy1, cy1, sx, cx, piy, pix, ay, ay1, ax, dnx, dny, diy;
            dnx = 1.0 / (double)nx;
            dny = 1.0 / (double)ny;
            GL.Begin(PrimitiveType.QuadStrip);
            piy = Math.PI * dny;
            pix = Math.PI * dnx;

            for (iy = 0; iy < ny; iy++)
            {
                if (colorCount >= color.Count - 1) { colorParam = false; }
                if (colorCount == 0) { colorParam = true; }
                GL.Color3(color[colorCount]);
                if (colorParam) { colorCount++; }
                else { colorCount = colorCount - 1; }

                diy = (double)iy;
                ay = diy * piy;
                sy = Math.Sin(ay);
                cy = Math.Cos(ay);
                ay1 = ay + piy;
                sy1 = Math.Sin(ay1);
                cy1 = Math.Cos(ay1);
                for (ix = 0; ix <= nx; ix++)
                {
                    ax = 2.0 * ix * pix;
                    sx = Math.Sin(ax);
                    cx = Math.Cos(ax);
                    x = r * sy * cx + X;
                    y = r * sy * sx + Y;
                    z = r * cy + Z;
                    GL.Vertex3(x, y, z);
                    x = r * sy1 * cx + X;
                    y = r * sy1 * sx + Y;
                    z = r * cy1 + Z;
                    GL.Vertex3(x, y, z);
                }
            }
            GL.End();
        }


        private void Torus(float X, float Y, float Z, float[] colors, float scale)
        {
            double numc = 38, numt = 40;
            double TWOPI = 2 * Math.PI;
            var color = CreateColor(20, colors);
            var colorParam = true;
            int colorCount = 0;

            for (int i = 0; i < numc; i++)
            {
                if (colorCount >= color.Count - 1) { colorParam = false; }
                if (colorCount == 0) { colorParam = true; }
                GL.Color3(color[colorCount]);
                if (colorParam) { colorCount++; }
                else { colorCount = colorCount - 1; }

                GL.Begin(BeginMode.QuadStrip);
                for (int j = 0; j <= numt; j++)
                {
                    for (int k = 1; k >= 0; k--)
                    {
                        double s = ((i + k) % numc + 20);
                        double t = j % numt;

                        double x = (1 + 0.5 * Math.Cos(s * TWOPI / numc)) * Math.Cos(t * TWOPI / numt);
                        double y = (1 + 0.5 * Math.Cos(s * TWOPI / numc)) * Math.Sin(t * TWOPI / numt);
                        double z = 0.5 * Math.Sin(s * TWOPI / numc);

                        GL.Vertex3((15 * x) * scale + X, (15 * y) * scale + Y, (15 * z) * scale + Z);
                    }
                }

                GL.End();
            }
        }


        private void Helix(float X, float Y, float Z, float[] colors, float scale)
        {
            double numc = 50, numt = 50;
            double TWOPI = 2 * Math.PI;
            var color = CreateColor(50, colors);
            var colorParam = true;
            int colorCount = 0;

            double add = 0;

            for (int i = 0; i  < numc * 3; i++)
            {
                add = add + 0.5;
                GL.Color3(color[colorCount]);
                if (colorCount >= color.Count - 1) { colorParam = false; }
                if (colorCount == 0) { colorParam = true; }
                if (colorParam) { colorCount++; }
                else {  colorCount = colorCount - 1; }

                GL.Begin(BeginMode.QuadStrip);
                for (int j = 0; j <= numt; j++)
                {
                    for (int k = 1; k >= 0; k--)
                    {
                        double t = ((i + k) % numc + 20);
                        double s = j % numt;

                        double x = (1 + 0.5 * Math.Cos(s * TWOPI / numc)) * Math.Cos(t * TWOPI / numt);
                        double y = (1 + 0.5 * Math.Cos(s * TWOPI / numc)) * Math.Sin(t * TWOPI / numt);
                        double z = 0.5 * Math.Sin(s * TWOPI / numc);

                        x = (15 * x) + X;
                        y = (15 * y) + Y;
                        

                        if (k==0) { z = (15 * z+ add - 0.5) + Z; }
                        else { z = (15 * z + add) + Z; }

                        GL.Vertex3(x * scale, y * scale, z * scale);
                    }
                }

                GL.End();
            }
        }

        //ok
        private void Cylinder(float X, float Y, float Z, float rX, float rY, float rZ, float r, float r2, float width, float lenght, float height, float count, float scale, float[] colors)
        {
            var pyramid = new List<Vector3[]>();
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            List<double> z = new List<double>();
            List<double> x2 = new List<double>();
            List<double> y2 = new List<double>();
            List<double> z2 = new List<double>();


            var pyramidColors = CreateColor((int)count + 2, colors);
            for (int i = 1; i <= count; i++)
            {
                x.Add(1+r * Math.Cos(2 * Math.PI * i / count) * (double)scale);
                y.Add(1+r * Math.Sin(2 * Math.PI * i / count) * (double)scale);
                z.Add(1);
                x2.Add(1+r2 * Math.Cos(2 * Math.PI * i / count) * (double)scale);
                y2.Add(1+r2 * Math.Sin(2 * Math.PI * i / count) * (double)scale);
                z2.Add(1+height);
            }

            double angle;
            double cos;
            double sin;
            double newX;
            double newY;
            double newZ;

            if (rX != 0)
            {
                angle = rX / 180.0 * Math.PI;
                cos = Math.Cos(angle);
                sin = Math.Sin(angle);

                for (int i = 0; i < x.Count(); i++)
                {
                    newY = y[i];
                    newZ = z[i];
                    y[i] = newY * cos - newZ * sin;
                    z[i] = newZ * cos + newY * sin;
                }

                for (int i = 0; i < x.Count(); i++)
                {
                    newY = y2[i];
                    newZ = z2[i];
                    y2[i] = newY * cos - newZ * sin;
                    z2[i] = newZ * cos + newY * sin;
                }
            }

            if (rY != 0)
            {
                angle = rY / 180.0 * Math.PI;
                cos = Math.Cos(angle);
                sin = Math.Sin(angle);

                for (int i = 0; i < x.Count(); i++)
                {
                    newX = x[i];
                    newZ = z[i];
                    x[i] = newX * cos - newZ * sin;
                    z[i] = newZ * cos + newX * sin;
                }

                for (int i = 0; i < x.Count(); i++)
                {
                    newX = x2[i];
                    newZ = z2[i];
                    x2[i] = newX * cos - newZ * sin;
                    z2[i] = newZ * cos + newX * sin;
                }
            }

            if (rZ != 0)
            {
                angle = rZ / 180.0 * Math.PI;
                cos = Math.Cos(angle);
                sin = Math.Sin(angle);

                for (int i = 0; i < x.Count(); i++)
                {
                    newX = x[i];
                    newY = y[i];
                    x[i] = newX * cos - newY * sin;
                    y[i] = newY * cos + newX * sin;
                }

                for (int i = 0; i < x.Count(); i++)
                {
                    newX = x2[i];
                    newY = y2[i];
                    x2[i] = newX * cos - newY * sin;
                    y2[i] = newY * cos + newX * sin;
                }
            }

            float[] pol;

            Debug.Print(x.ToArray().ToString());

            GL.Color3(pyramidColors.Last());
            GL.Begin(BeginMode.Polygon);
            for (int i = 0; i < count; i++)
            {
                pol = new float[] { X + (float)x[i], Y + (float)y[i], Z + (float)z[i] };
                GL.Vertex3(pol);
            }
            GL.End();

            GL.Begin(BeginMode.Polygon);
            for (int i = 0; i < count; i++)
            {
                pol = new float[] { X + (float)x2[i], Y + (float)y2[i], Z + (float)z2[i] };
                GL.Vertex3(pol);
            }
            GL.End();

            for (int i = 0; i < count; i++)
            {
                GL.Color3(pyramidColors[i]);
                GL.Begin(BeginMode.Polygon);

                if (i == count - 1)
                {
                    GL.Vertex3(new Vector3(X + (float)x[i], Y + (float)y[i], Z + (float)z[i]));
                    GL.Vertex3(new Vector3(X + (float)x.First(), Y + (float)y.First(), Z + (float)z.First()));
                }

                else
                {
                    GL.Vertex3(new Vector3(X + (float)x[i], Y + (float)y[i], Z + (float)z[i]));
                    GL.Vertex3(new Vector3(X + (float)x[i + 1], Y + (float)y[i + 1], Z + (float)z[i+1]));
                }

                if (i == count - 1)
                {
                    GL.Vertex3(new Vector3(X + (float)x2.First(), Y + (float)y2.First(), Z + (float)z2.First()));
                    GL.Vertex3(new Vector3(X + (float)x2[i], Y + (float)y2[i], Z +(float)z2[i]));
                }

                else
                {
                    GL.Vertex3(new Vector3(X + (float)x2[i + 1], Y + (float)y2[i + 1], Z + (float)z2[i+1]));
                    GL.Vertex3(new Vector3(X + (float)x2[i], Y + (float)y2[i], Z +(float)z2[i]));
                }

                GL.End();
            }
        }

        double scale = 1;
        public void ScaleScene(double power)
        {
            while (_run)
            {
                scale += power;
                glControl1.Invalidate();
                Thread.Sleep(20);
            }
        }

        public void Transform(string type, int index, float power, int sleep = 20)
        {
            if (index != -1)
            {
                int typeID = 0;
                switch (type)
                {
                    // X, Y, Z, rX, rY, rZ, r, r2, width, lenght, height, count, scale / 2

                    case "X":
                        typeID = 0;
                        break;

                    case "Y":
                        typeID = 1;
                        break;

                    case "Z":
                        typeID = 2;
                        break;

                    case "rX":
                        typeID = 3;
                        break;

                    case "rY":
                        typeID = 4;
                        break;

                    case "rZ":
                        typeID = 5;
                        break;

                    case "r":
                        typeID = 6;
                        break;

                    case "r2":
                        typeID = 7;
                        break;

                    case "width":
                        typeID = 8;
                        break;

                    case "lenght":
                        typeID = 9;
                        break;

                    case "height":
                        typeID = 10;
                        break;

                    case "count":
                        typeID = 11;
                        break;
                }

                while (_run)
                {
                    figuresAttrs[index][typeID] = figuresAttrs[index][typeID] + power;
                    glControl1.Invalidate();
                    Thread.Sleep(sleep);
                }
            }
        }

        // remove figure
        private void button5_Click(object sender, EventArgs e)
        {
            int index = figuresNames.IndexOf(comboBox3.Text);
            figures.RemoveAt(index);
            figuresColors.RemoveAt(index);
            figuresNames.RemoveAt(index);
            figuresAttrs.RemoveAt(index);

            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(figuresNames.ToArray());

            glControl1.Invalidate();
        }

        // +X 
        private void button7_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("X", index, 1));
        }

        // -X
        private void button6_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("X", index, -1));
        }

        // +Y 
        private void button9_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("Y", index, 1));
        }

        // -Y
        private void button8_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("Y", index, -1));
        }

        // +Z
        private void button11_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("Z", index, 1));
        }

        // -Z
        private void button10_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("Z", index, -1));
        }

        //error
        private void button13_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("scale", index , 1.1f));
        }

        //error
        private void button12_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("scale", index, 0.9f));
        }

        // Scene Scale +
        private void button15_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            Task.Factory.StartNew(() => ScaleScene(0.03));
        }

        // Scene Scale -
        private void button14_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            Task.Factory.StartNew(() => ScaleScene(-0.03));
        }

        // + rotate X 
        private void button21_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("rX", index, 1));
        }

        // - rotate X 
        private void button20_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("rX", index, -1));
        }

        // + rotate Y
        private void button19_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("rY", index, 1));
        }

        // - rotate Y
        private void button18_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("rY", index, -1));
        }

        // + rotate Z 
        private void button17_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("rZ", index, 1));
        }

        // - rotate Z 
        private void button16_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("rZ", index, -1));
        }

        // + first radius
        private void button25_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("r", index, 1));
        }

        // - first radius
        private void button24_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("r", index, -1));
        }

        // + second radius
        private void button23_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("r2", index, 1));
        }

        // - second radius
        private void button22_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("r2", index, -1));
        }

        // + count
        private void button27_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("count", index, 1, 100));
        }

        // - count
        private void button26_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("count", index, -1, 100));
        }

        // + width
        private void button33_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("width", index, 1));
        }

        // - width
        private void button32_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("width", index, -1));
        }

        // + lenght
        private void button31_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("lenght", index, 1));
        }

        // - lenght
        private void button30_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("lenght", index, -1));
        }

        // + height
        private void button29_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("height", index, 1));
        }

        // - height
        private void button28_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = true;
            int index = figuresNames.IndexOf(comboBox3.Text);
            Task.Factory.StartNew(() => Transform("height", index, -1));
        }

        private void btn_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _run = false;
        }
    }
}
