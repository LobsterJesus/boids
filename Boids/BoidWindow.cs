using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boids
{
    public partial class BoidWindow : Form
    {
        private const int WindowWidth = 800;
        private const int WindowHeight = 600;
        private const int NumPigeons = 2000;
        private const int NumHaws = 2;

        private List<Bird> _birds;
        private Timer _timer;
        private Stopwatch _sw = new Stopwatch();
        private long _renderTime;
        private Font _font = new Font("Arial", 12);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BoidWindow());
        }

        public BoidWindow()
        {
            InitializeComponent();
            
            // Window setup
            Text = "Boids";
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(WindowWidth, WindowHeight);
           
            // Boid setup
            var pigeons = new List<Pigeon>();
            _birds = new List<Bird>();

            for (int i = 0; i < NumPigeons; i++)
                pigeons.Add(new Pigeon(ClientSize, _birds));

            _birds.AddRange(pigeons);

            for (int i = 0; i < NumHaws; i++)
                _birds.Add(new Hawk(ClientSize, pigeons));

            _timer = new Timer();
            _timer.Tick += new EventHandler(OnTick);
            _timer.Interval = 75;
            _timer.Start();

        }

        private void OnTick(object sender, EventArgs e)
        {
            _sw.Restart();

            //foreach (var bird in _birds)
            //    bird.Move();

            Parallel.ForEach(_birds, (bird) =>
            {
                bird.Move();
            });

            //List<Task> list = new List<Task>();

            //foreach (var bird in _birds)
            //{
            //    list.Add(Task.Factory.StartNew(() =>
            //    {
            //        bird.Move();
            //    }));

            //}

            //Task.WaitAll(list.ToArray());

            _sw.Stop();
            _renderTime = _sw.ElapsedMilliseconds;
            //Console.WriteLine(_renderTime);

            Invalidate();

        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_renderTime > 0)
            {
                e.Graphics.DrawString(
                    string.Concat(1000 / _renderTime, " fps"), 
                    _font, Brushes.Black, 10, 10);
            }

            foreach (var bird in _birds)
            {
                int size = bird is Hawk ? 6 : 3;

                PointF[] p =
                {
                    new PointF(bird.Pos.X + size, bird.Pos.Y),
                    new PointF(bird.Pos.X - size, bird.Pos.Y + size),
                    new PointF(bird.Pos.X - size, bird.Pos.Y - size)
                };

                float angle;
                if (bird.V.X == 0)
                    angle = 90f;
                else
                    angle = (float)(Math.Atan(bird.V.Y / bird.V.X) * (180 / Math.PI));
                
                if (bird.V.X < 0) angle += 180f;

                var matrix = new Matrix();
                matrix.RotateAt((float)angle, new PointF(bird.Pos.X, bird.Pos.Y));

                e.Graphics.Transform = matrix;
                e.Graphics.FillPolygon(bird is Pigeon ? Brushes.Black : Brushes.Red, p);
                //e.Graphics.DrawLine(Pens.Red, bird.Pos.X, bird.Pos.Y,
                //    bird.Pos.X + bird.V.Length(), bird.Pos.Y);
                
            }
        }
    }
}
