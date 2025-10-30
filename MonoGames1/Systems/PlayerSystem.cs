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
        private ShapeComponent _shapeComponent = default!;

        private EventBus _eventBus;
        private SizeF _worldSize;

        // Direction the player is facing. Init to facing the screen.
        private Vector2 _direction = new Vector2(0, -1);

        // how long after shooting before you can shoot again in seconds
        private float _projectileDelay = 0.2f;
        private float _projectileElapsed = 1f;

        public PlayerSystem(EventBus eventBus, SizeF worldSize) : base(Aspect.All(typeof(PlayerComponent))) 
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
            _projectileElapsed += gameTime.GetElapsedSeconds();
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
                _direction = direction;
                _bodyComponent.Direction = direction;
                _bodyComponent.Position += Utils.GetPositionChange(direction, _bodyComponent.Speed, gameTime);
                _eventBus.Push(new PositionEventArgs { Position = _bodyComponent.Position });
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _projectileElapsed >= _projectileDelay)
            {
                _projectileElapsed = 0;
                SpawnProjectile();
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

            _fighterComponent = new()
            {
                Damage = 10,
                Health = 100,
                MaxHealth = 100,
            };

            _player.Attach(_fighterComponent);

            _shapeComponent = new()
            {
                Color = Color.Blue,
                Polygon = new Polygon(playerVertices)
            };

            _player.Attach(_shapeComponent);

            _bodyComponent = new()
            {
                Body = new PlayerBody(
                    new RectangleF(playerPosition, playerSize),
                    new Vector2(),
                    200
                )
            };

            _player.Attach(_bodyComponent);

            _eventBus.Push(new PositionEventArgs { Position = _bodyComponent.Position });
            _eventBus.Push(new CreateBodyArgs { Body = _bodyComponent.Body });
        }

        private void SpawnProjectile()
        {
            // direction ([x, y] + [1, 1]) * 16 gives the starting position of the projectile relative to
            // the player position 
            Vector2 position = _bodyComponent.Position + (_direction + Vector2.One) * 16;


            Entity projectile = CreateEntity();
            projectile.Attach(new ProjectileComponent
            {
                Direction = _direction,
                Speed = 500,
                Damage = 10,
                MaxAge = 1,
            });

            ProjectileBody projectileBody = new ProjectileBody(new RectangleF(position, new Vector2(4, 4)), _direction, 500); 

            projectile.Attach(new BodyComponent { 
                Body = projectileBody
            });

            List<Vector2> projectileVertices = new()
            {
                new Vector2(0, 0),
                new Vector2(4, 0),
                new Vector2(4, 4),
                new Vector2(0, 4),
            };

            projectile.Attach(new ShapeComponent { Color = Color.Black, Polygon = new(projectileVertices) });

            _eventBus.Push(new CreateBodyArgs { Body = projectileBody});
        }
    }
}
