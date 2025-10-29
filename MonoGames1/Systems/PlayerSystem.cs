using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Shapes;
using MonoGames1.Components;
using MonoGames1.Events;
using MonoGames1.Events.Args;
using MonoGames1.Objects;
using System.Collections.Generic;

namespace MonoGames1.Systems
{
    internal class PlayerSystem : EntityUpdateSystem
    {
        private FighterComponent _fighterComponent = default!;
        private BodyComponent _bodyComponent = default!;

        private EventBus _eventBus;
        private SizeF _worldSize;

        public PlayerSystem(EventBus eventBus, SizeF worldSize) : base(Aspect.All(typeof(PlayerComponent), typeof(FighterComponent), typeof(BodyComponent))) 
        {
            _worldSize = worldSize;
            _eventBus = eventBus;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            CreatePlayer();
        }

        public override void Update(GameTime gameTime)
        {
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
                _bodyComponent.Position += Utils.GetPositionChange(direction, _bodyComponent.Speed, gameTime);
                _eventBus.Push(new PositionEventArgs { Position = _bodyComponent.Position });
            }
        }

        public void OnDamage(DamageEventArgs damageEventArgs)
        {
            _fighterComponent.Health -= damageEventArgs.Damage;
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

            SizeF playerSize = new(32, 32);

            Vector2 playerPosition = new()
            {
                X = (_worldSize.Width - playerSize.Width) / 2,
                Y = (_worldSize.Height - playerSize.Height) / 2
            };

            _fighterComponent = new FighterComponent
            {
                Damage = 10,
                Health = 100,
                MaxHealth = 100,
                Color = Color.Blue,
                Polygon = new Polygon(playerVertices)
            };

            _player.Attach(_fighterComponent);

            _bodyComponent = new BodyComponent
            {
                Body = new PlayerBody(
                    new RectangleF(playerPosition, playerSize),
                    Vector2.Zero,
                    200
                )
            };

            _player.Attach(_bodyComponent);

            _eventBus.Push(new PositionEventArgs { Position = _bodyComponent.Position });
            _eventBus.Push(new CreateBodyArgs { Body = _bodyComponent.Body });
        }

    }
}
