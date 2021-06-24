using Assets.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Receivers
{
    public interface IGazeReceiver
    {
        /// <summary>
        /// Should be called when the object is being looked at
        /// </summary>
        void GazingUpon(GazingUponMessage message);

        /// <summary>
        /// Should be called when the object is no longer being looked at
        /// </summary>
        void NotGazingUpon();
    }
}
