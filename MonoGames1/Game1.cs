using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGames1.Events;
using MonoGames1.Events.Args;
using MonoGames1.Spawners;
using MonoGames1.Systems;
using System;

namespace MonoGames1;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics = default!;
    private SpriteBatch _spriteBatch = default!;
    private World _world = default!;
    private CollisionComponent _collisionComponent;
    private EventBus _eventBus = new EventBus();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

    }

    protected override void Initialize()
    {
        _collisionComponent = new CollisionComponent(new RectangleF(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        ));

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        SizeF worldSize = new SizeF(
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        );

        int startingEnemies = 4;

        // Initializing systems
        PlayerSystem playerSystem = new(_eventBus, worldSize);
        EnemySystem enemySystem = new(_eventBus);
        RenderSystem renderSystem = new(_spriteBatch);
        EnemySpawner enemySpawner =  new(_eventBus, startingEnemies, worldSize);

        // Register event handlers
        _eventBus.Subscribe<PositionEventArgs>(enemySystem.OnPlayerMove);
        
        Console.WriteLine("Building World...");
        _world = new WorldBuilder()
            .AddSystem(playerSystem)
            .AddSystem(enemySystem)
            .AddSystem(enemySpawner)
            .AddSystem(renderSystem)
            .Build();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        _world.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _world.Draw(gameTime);

        base.Draw(gameTime);
    }
}
