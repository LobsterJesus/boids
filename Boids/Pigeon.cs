using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Boids
{
    internal class Pigeon : Bird
    {
        private const Single FlockSpacing = 20f;
        private List<Bird> _birds;
        
        public Pigeon(Size boundary, List<Bird> birds) : base(boundary)
        {
            _birds = birds;
        }

        public override void Move()
        {
            foreach (var bird in _birds)
            {
                if (bird == this) continue;

                var d = Vector2.Distance(Pos, bird.Pos);

                // Avoid hawks
                if (bird is Hawk)
                {
                    if (d < InterationRadius) V += RotateRadians((Pos - bird.Pos), 1) * 100;
                    continue;
                }
                
                // Keep space between birds
                if (d < FlockSpacing)
                {
                    V += (Pos - bird.Pos) * 10;
                }
                // Flock together
                else if (d < InterationRadius)
                {
                    V += (bird.Pos - Pos) * 0.05f;
                }
                // Align direction
                if (d < InterationRadius)
                {
                   V += bird.V * 0.5f;
                }
            }
            
            base.Move();
        }

        //public static Vector2 Rotate(Vector2 v, double degrees)
        //{
        //    return RotateRadians(degrees * Math.PI / 180);
        //}

        public static Vector2 RotateRadians(Vector2 v, double radians)
        {
            Single ca = (Single)Math.Cos(radians);
            Single sa = (Single)Math.Sin(radians);
            return new Vector2(ca * v.X - sa * v.Y, sa * v.X + ca * v.Y);
        }
    }
}
