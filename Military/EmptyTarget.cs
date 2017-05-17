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
        private object threadLock = new object();

        public new int HealthPoints
        {
            get
            {
                return _healthPoints;
            }
            set
            {
                lock (threadLock)
                {
                    if ((_healthPoints - value) <= 0)
                    {
                        _healthPoints = 0;
                    }
                    else
                    {
                        _healthPoints = value;
                    }
                }
            }
        }

        public EmptyTarget(int x, int y, int code) : base(x, y, code)
        {
            _healthPoints = 100;
            X = x;
            Y = y;
            Name = code;
        }
    }
}
