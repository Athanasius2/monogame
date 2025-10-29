using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System.Collections.Generic;

namespace MonoGames1.Objects
{
    public class Body : ICollisionActor
    {
        public IShapeF Bounds { get; }
        public Vector2 Direction;
        public int Speed;

        public Queue<ICollisionActor> Others = new();

        public Body(RectangleF bounds, Vector2 direction, int speed)
        {
            Bounds = bounds;
            Direction = direction;
            Speed = speed;
        }

        public virtual void OnCollision(CollisionEventArgs collisionInfo)
        {
            Others.Enqueue(collisionInfo.Other);
        }
    }
}
