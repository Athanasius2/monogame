using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGames1.Events.Args
{
    public class CreateBodyArgs : EventArgs
    {
        public ICollisionActor Body = default!;
    }
}
