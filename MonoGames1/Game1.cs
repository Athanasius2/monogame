using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGames1.Events;
using MonoGames1.Events.Args;
using MonoGames1.Spawners;
using MonoGames1.Systems;

namespace MonoGames1;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics = default!;
    private SpriteBatch _spriteBatch = default!;
    private World _world = default!;
    private readonly EventBus _eventBus = new();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        SizeF worldSize = new(
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        );

        int startingEnemies = 4;

        // Initializing systems
        PlayerSystem playerSystem = new(_eventBus, worldSize);
        EnemySystem enemySystem = new(_eventBus);
        ProjectileSystem projectileSystem = new(_eventBus);
        RenderSystem renderSystem = new(_spriteBatch);
        HealthBarSystem healthBarSystem = new(_spriteBatch);
        EnemySpawner enemySpawner =  new(_eventBus, startingEnemies, worldSize);
        CollisionSystem collisionSystem = new(_eventBus, new RectangleF(Vector2.Zero, worldSize));

        // Register event handlers
        _eventBus.Subscribe<PositionEventArgs>(enemySystem.OnPlayerMove);
        _eventBus.Subscribe<CreateBodyArgs>(collisionSystem.OnCreateBody);
        _eventBus.Subscribe<DestroyBodyArgs>(collisionSystem.OnDestroyBody);
        _eventBus.Subscribe<DamageEventArgs>(playerSystem.OnDamage);
        
        // Initialize systems
        _world = new WorldBuilder()
            .AddSystem(playerSystem)
            .AddSystem(enemySystem)
            .AddSystem(projectileSystem)
            .AddSystem(enemySpawner)
            .AddSystem(collisionSystem)
            .AddSystem(renderSystem)
            .AddSystem(healthBarSystem)
            .Build();

        // Dispatch any events raised durring the entity systems initialization
        _eventBus.DispatchAll();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        _world.Update(gameTime);
        _eventBus.DispatchAll();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _world.Draw(gameTime);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
