using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System.Collections.Generic;

namespace MonoGames1.Objects
{
    public class Body : ICollisionActor
    {
        public IShapeF Bounds { get; }

        public Queue<ICollisionActor> Others = new();

        public Body(RectangleF bounds)
        {
            Bounds = bounds;
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            Others.Enqueue(collisionInfo.Other);
        }
    }
}
