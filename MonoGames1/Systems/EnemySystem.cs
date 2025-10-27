using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGames1.Components;
using MonoGames1.EventArgs;
using System;

namespace MonoGames1.Systems
{
    public class EnemySystem : EntityProcessingSystem
    {
        public event EventHandler<PositionEventArgs> EnemyDeath;

        private ComponentMapper<FighterComponent> _fighterMapper;
        private ComponentMapper<PlayerComponent> _playerMapper;

        private Vector2 _playerPosition;

        public EnemySystem() : base(Aspect.All(typeof(FighterComponent)).Exclude(typeof(PlayerComponent))) { }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _fighterMapper = mapperService.GetMapper<FighterComponent>();
            _playerMapper = mapperService.GetMapper<PlayerComponent>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            if(_fighterMapper.TryGet(entityId, out FighterComponent fighter))
            {
                var angle = MathF.Atan2(_playerPosition.X - fighter.Position.X, 
                    _playerPosition.Y - fighter.Position.Y);

                Vector2 delta;

                delta.X = (float)(MathF.Cos(angle) * fighter.Speed * gameTime.ElapsedGameTime.TotalSeconds);
                delta.Y = (float)(MathF.Sin(angle) * fighter.Speed * gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public void OnPlayerMove(object? sender, PositionEventArgs e)
        {
            _playerPosition = e.Position;
        }

        protected virtual void OnEnemyDeath(PositionEventArgs e)
        {
            EnemyDeath?.Invoke(this, e);
        }
    }
}
