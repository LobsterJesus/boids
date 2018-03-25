using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Boids
{
    internal class Hawk : Bird
    {
        private List<Pigeon> _pigeons;
        
        public Hawk(Size boundary, List<Pigeon> pigeons) : base(boundary)
        {
            _pigeons = pigeons;

            Speed /= 2f;
        }

        public override void Move()
        {
            var prey = (from p in _pigeons
                        let distance = Vector2.Distance(Pos, p.Pos)
                        where distance < InterationRadius
                        orderby distance
                        select p).FirstOrDefault();

            if (prey != null)
                V += prey.Pos - Pos;

            base.Move();
        }
    }
}
