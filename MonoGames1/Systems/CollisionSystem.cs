using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGames1.Components;
using MonoGames1.Events;
using MonoGames1.Events.Args;
using System;

namespace MonoGames1.Systems
{
    public class CollisionSystem : EntityUpdateSystem
    {
        private CollisionComponent _collisionComponent;
        private EventBus _eventBus;

        public CollisionSystem(EventBus eventBus, RectangleF worldSize) : base(Aspect.All(typeof(BodyComponent)))
        {
            _collisionComponent = new CollisionComponent(worldSize);
            _eventBus = eventBus;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            _collisionComponent.Update(gameTime);
        }

        public void OnCreateBody(CreateBodyArgs e)
        {
            _collisionComponent.Insert(e.Body);
        }
    }
}
