using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGames1.Components;
using MonoGames1.Events;
using MonoGames1.Events.Args;
using MonoGames1.Objects;

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

            Vector2 direction = _playerPosition - body.Position;

            body.Position += Utils.GetPositionChange(direction, body.Speed, gameTime);

            // process collisions for enemy
            while(body.Body.Others.TryDequeue(out ICollisionActor? result))
            {
                if (result is PlayerBody other)
                {
                    DestroyEntity(entityId);
                    _eventBus.Push(new DamageEventArgs() { Damage = fighter.Damage });
                    _eventBus.Push(new DestroyBodyArgs() { Body = body.Body });
                }
                if (result is ProjectileBody)
                {
                    DestroyEntity(entityId);
                    _eventBus.Push(new DestroyBodyArgs() { Body = body.Body });
                }
            }
        }

        public void OnPlayerMove(PositionEventArgs e)
        {
            _playerPosition = e.Position;
        }
    }
}
