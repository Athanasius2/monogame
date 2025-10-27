using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGames1.Components;
using MonoGames1.EventArgs;
using System;

namespace MonoGames1.Systems
{
    internal class PlayerSystem : EntityProcessingSystem
    {
        public event EventHandler<PositionEventArgs> PlayerMove;

        private ComponentMapper<FighterComponent> _fighterMapper;

        public PlayerSystem() : base(Aspect.All(typeof(PlayerComponent), typeof(FighterComponent))) { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _fighterMapper = mapperService.GetMapper<FighterComponent>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            FighterComponent fighter = _fighterMapper.Get(entityId);

            Vector2 newPosition = fighter.Position;

            if(Keyboard.GetState().IsKeyDown(Keys.W))
                newPosition.Y -= fighter.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                newPosition.Y += fighter.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                newPosition.X -= fighter.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                newPosition.X += fighter.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(fighter.Position != newPosition)
            {
                fighter.Position = newPosition;
                OnPlayerMove(new PositionEventArgs { Position = newPosition });
            }
        }

        protected virtual void OnPlayerMove(PositionEventArgs e)
        {
            PlayerMove?.Invoke(this, e);
        }
    }
}
