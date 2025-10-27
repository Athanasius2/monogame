using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        PlayerSystem playerSystem = new();
        EnemySystem enemySystem = new();
        RenderSystem renderSystem = new(_spriteBatch);

        playerSystem.PlayerMove += enemySystem.OnPlayerMove;
        
        Console.WriteLine("Building World...");
        _world = new WorldBuilder()
            .AddSystem(playerSystem)
            .AddSystem(enemySystem)
            .AddSystem(renderSystem)
            .Build();

        _player = _world.CreateEntity();
        _player.Attach(new PlayerComponent());

        var playerVertices = new List<Vector2>
        {
            new Vector2(0, 0),
            new Vector2(32, 0),
            new Vector2(0, 32),
            new Vector2(32, 32),
        };

        _player.Attach(new FighterComponent
        {
            Position = new Vector2(0, 0),
            Speed = 100,
            Damage = 10,
            Health = 100,
            Color = Color.Blue,
            Polygon = new Polygon(playerVertices)
        });

        _enemySpawner =
            new EnemySpawner(_world, 4,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight
            );

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
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _world.Draw(gameTime);

        base.Draw(gameTime);
    }

}
