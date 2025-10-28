using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGames1.EventArgs
{
    public class CreateBodyArgs
    {
        public ICollisionActor Body;

        public CreateBodyArgs(ICollisionActor body)
        {
            Body = body;
        }
    }
}
