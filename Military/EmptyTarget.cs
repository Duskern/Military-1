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
        public Mutex mutex = new Mutex(); 

        public override int HealthPoints
        {
            get
            {
                return _healthPoints;
            }

            set
            {
                mutex.WaitOne();
                if (_healthPoints <= 0)
                {
                    _healthPoints = 0;
                }
                else
                {
                    _healthPoints = value;
                }
                mutex.ReleaseMutex();
            }
        }

        public EmptyTarget(int x, int y) : base(x, y, 0)
        {
            _healthPoints = 100;
            X = x;
            Y = y;
        }
    }
}
