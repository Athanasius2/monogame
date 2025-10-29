using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Shapes;
using MonoGames1.Components;
using MonoGames1.Events;
using MonoGames1.Events.Args;
using System;
using System.Collections.Generic;

namespace MonoGames1.Systems
{
    internal class PlayerSystem : EntityProcessingSystem
    {
        private ComponentMapper<FighterComponent> _fighterMapper = default!;
        private ComponentMapper<BodyComponent> _bodyMapper = default!;

        private EventBus _eventBus;
        private SizeF _worldSize;

        public PlayerSystem(EventBus eventBus, SizeF worldSize) : base(Aspect.All(typeof(PlayerComponent), typeof(FighterComponent), typeof(BodyComponent))) 
        {
            _worldSize = worldSize;
            _eventBus = eventBus;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _fighterMapper = mapperService.GetMapper<FighterComponent>();
            _bodyMapper = mapperService.GetMapper<BodyComponent>();
            CreatePlayer();
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
            _eventBus.Push(new PositionEventArgs { Position = body.Position });
        }

        private void CreatePlayer()
        {
            Entity _player = CreateEntity();
            _player.Attach(new PlayerComponent());

            List<Vector2> playerVertices = new()
            {
                new Vector2(0, 0),
                new Vector2(32, 0),
                new Vector2(32, 32),
                new Vector2(0, 32),
            };

            SizeF playerSize = new SizeF(32, 32);

            Vector2 playerPosition = new()
            {
                X = (_worldSize.Width - playerSize.Width) / 2,
                Y = (_worldSize.Height - playerSize.Height) / 2
            };

            _player.Attach(new FighterComponent
            {
                Speed = 200,
                Damage = 10,
                Health = 100,
                Color = Color.Blue,
                Polygon = new Polygon(playerVertices)
            });

            _player.Attach(new BodyComponent(new RectangleF(playerPosition, playerSize)));

        }
    }
}
