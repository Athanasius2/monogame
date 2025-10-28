using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Shapes;
using MonoGames1.Components;
using MonoGames1.Spawners;
using MonoGames1.Systems;
using System;
using System.Collections.Generic;

namespace MonoGames1;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private World _world;
    private Entity _player;

    private EnemySpawner _enemySpawner;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

    }

    protected override void Initialize()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Initializing systems
        PlayerSystem playerSystem = new();
        EnemySystem enemySystem = new();
        RenderSystem renderSystem = new(_spriteBatch);

        // When the player moves, send player coordinates to EnemySystem
        playerSystem.PlayerMove += enemySystem.OnPlayerMove;
        
        Console.WriteLine("Building World...");
        _world = new WorldBuilder()
            .AddSystem(playerSystem)
            .AddSystem(enemySystem)
            .AddSystem(renderSystem)
            .Build();

        _player = _world.CreateEntity();
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
            X = (GraphicsDevice.PresentationParameters.BackBufferWidth - playerSize.Width) / 2,
            Y = (GraphicsDevice.PresentationParameters.BackBufferHeight - playerSize.Height) / 2
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

        _enemySpawner =
            new EnemySpawner(_world, 4,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight
            );

        // Notify EnemySpawner when an enemy dies
        enemySystem.EnemyDeath += _enemySpawner.OnEnemyDeath;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        _world.Update(gameTime);
        _enemySpawner.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _world.Draw(gameTime);

        base.Draw(gameTime);
    }

}
