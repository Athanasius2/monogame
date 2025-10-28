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
        public event EventHandler<PositionEventArgs> PlayerMove = default!;

        private ComponentMapper<FighterComponent> _fighterMapper = default!;
        private ComponentMapper<BodyComponent> _bodyMapper = default!;

        public PlayerSystem() : base(Aspect.All(typeof(PlayerComponent), typeof(FighterComponent), typeof(BodyComponent))) { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _fighterMapper = mapperService.GetMapper<FighterComponent>();
            _bodyMapper = mapperService.GetMapper<BodyComponent>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            FighterComponent fighter = _fighterMapper.Get(entityId);
            BodyComponent body = _bodyMapper.Get(entityId);

            Vector2 direction = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                direction.Y = -1;

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                direction.Y = 1;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                direction.X = -1;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                direction.X = 1;


            if(direction != Vector2.Zero)
            {
                direction.Normalize();

                Vector2 delta;
                delta.X = (float)(direction.X * fighter.Speed * gameTime.ElapsedGameTime.TotalSeconds);
                delta.Y = (float)(direction.Y * fighter.Speed * gameTime.ElapsedGameTime.TotalSeconds);
                body.Position += delta;
            }
            OnPlayerMove(new PositionEventArgs { Position = body.Position });
        }

        protected virtual void OnPlayerMove(PositionEventArgs e)
        {
            PlayerMove?.Invoke(this, e);
        }
    }
}
