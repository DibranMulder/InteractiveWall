using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveWall
{
    interface ISwipeablePage
    {
        void SwipeLeft();
        void SwipeRight();
        void Open();
    }
}
