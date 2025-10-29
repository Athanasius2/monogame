using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGames1.Components;
using MonoGames1.Events;
using MonoGames1.Events.Args;
using MonoGames1.Objects;
using System;

namespace MonoGames1.Systems
{
    public class EnemySystem : EntityProcessingSystem
    {
        private EventBus _eventBus = new EventBus();
        private ComponentMapper<FighterComponent> _fighterMapper = default!;
        private ComponentMapper<BodyComponent> _bodyMapper = default!;

        private Vector2 _playerPosition;

        public EnemySystem(EventBus eventBus) : base(Aspect.All(typeof(FighterComponent), typeof(BodyComponent)).Exclude(typeof(PlayerComponent))) 
        {
            _eventBus = eventBus;
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _fighterMapper = mapperService.GetMapper<FighterComponent>();
            _bodyMapper = mapperService.GetMapper<BodyComponent>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            FighterComponent fighter = _fighterMapper.Get(entityId);
            BodyComponent body = _bodyMapper.Get(entityId);

            // process movement for enemy
            float angle = MathF.Atan2(_playerPosition.X - body.Position.X, 
                _playerPosition.Y - body.Position.Y);

            Vector2 delta;

            delta.X = (float)(MathF.Sin(angle) * fighter.Speed * gameTime.ElapsedGameTime.TotalSeconds);
            delta.Y = (float)(MathF.Cos(angle) * fighter.Speed * gameTime.ElapsedGameTime.TotalSeconds);

            body.Position += delta;

            // process collisions for enemy
            while(body.Body.Others.TryDequeue(out ICollisionActor? result))
            {
                if (result is PlayerBody other)
                {
                    _eventBus.Push(new DamageEventArgs() { Damage = fighter.Damage });
                    DestroyEntity(entityId);
                }
            }
        }

        public void OnPlayerMove(PositionEventArgs e)
        {
            _playerPosition = e.Position;
        }
    }
}
