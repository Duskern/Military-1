using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Military
{
    public class EmptyTarget : Target
    {
        public override int HealthPoints
        {
            get
            {
                return _healthPoints;
            }

            set
            {
                Monitor.Enter(this);
                if (_healthPoints <= 0)
                {
                    _healthPoints = 0;
                }
                else
                {
                    _healthPoints = value;
                }
                Monitor.Exit(this);
            }
        }

        public EmptyTarget(int x, int y) : base(x, y)
        {
            _healthPoints = 0;
            X = x;
            Y = y;
        }
    }
}
