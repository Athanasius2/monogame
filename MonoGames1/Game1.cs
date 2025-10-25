using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Shapes;
using MonoGames1.Components;
using System;
using System.Collections.Generic;

namespace MonoGames1;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private World _world;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Console.WriteLine("Building World...");
        _world = new WorldBuilder()
            .AddSystem(new Systems.PlayerSystem())
            .AddSystem(new Systems.RenderSystem(_spriteBatch))
            .Build();

        CreatePlayer();
    }

    protected override void Update(GameTime gameTime)
    {
        _world.Update(gameTime);
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

    private void CreatePlayer()
    {

        var player = _world.CreateEntity();
        player.Attach(new Player());

        var playerEffect = new BasicEffect(GraphicsDevice)
        {
            VertexColorEnabled = true,
            Projection = Matrix.CreateOrthographicOffCenter
            (
                0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                0,
                0,
                1
            )
        };

        var playerVertices = new List<Vector2>
        {
            new Vector2(50, 0),
            new Vector2(0, 100),
            new Vector2(100, 100)
        };

        player.Attach(new Fighter
        {
            Position = new Vector2(100, 100),
            Speed = 100,
            Damage = 10,
            Health = 100,
            Polygon = new Polygon(playerVertices)
        });


    }
}
