using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace Boids
{
    internal abstract class Bird
    {
        public Vector2 V;
        public Vector2 Pos;

        protected const int Border = 100;
        protected Single Speed = 12f;
        protected Single InterationRadius = 75f;
        
        private Size _boundary;
        private static Random _rnd = new Random();

        public Bird(Size boundary)
        {
            _boundary = boundary;
            
            Pos = new Vector2(
                _rnd.Next(_boundary.Width), 
                _rnd.Next(_boundary.Height));

            //Pos = new Vector2(300, 300);

            Complex c = Complex.FromPolarCoordinates(_rnd.NextDouble() * Speed, _rnd.NextDouble() * Math.PI * 2);
            V.X = (float)c.Real;
            V.Y = (float)c.Imaginary;
            
        }

        virtual public void Move()
        {
            // Boundary check
            if (Pos.X < Border) V.X += Border - Pos.X;
            if (Pos.Y < Border) V.Y += Border - Pos.Y;
            if (Pos.X > _boundary.Width - Border)
                V.X += _boundary.Width - Border - Pos.X;
            if (Pos.Y > _boundary.Height - Border)
                V.Y += _boundary.Height - Border - Pos.Y;

            // Speed check
            var length = V.Length();
            if (length > Speed) V *= Speed / length;

            Pos += V;
        }
    }
}
