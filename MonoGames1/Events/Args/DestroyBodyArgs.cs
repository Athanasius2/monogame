using MonoGame.Extended.Collisions;
using System;

namespace MonoGames1.Events.Args;

public class DestroyBodyArgs : EventArgs
{
    public ICollisionActor Body = default!;
}
