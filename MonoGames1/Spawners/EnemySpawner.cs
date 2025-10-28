using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Shapes;
using MonoGames1.Components;
using MonoGames1.EventArgs;
using System;
using System.Collections.Generic;

namespace MonoGames1.Spawners;
public class EnemySpawner
{
    private World _world;

    private int _maxEnemies;
    private int _enemyCount;

    private int _worldWidth;
    private int _worldHeight;

    public EnemySpawner(World world, int initMaxEnemies, int worldWidth, int worldHeight)
    {
        _world = world;
        _maxEnemies = initMaxEnemies;

        _worldWidth = worldWidth;
        _worldHeight = worldHeight;
    }

    /// <summary>
    /// Event handler for when an enemy dies
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void OnEnemyDeath(object? sender, PositionEventArgs e)
    {
        _enemyCount--;
    }

    public void Update()
    {
        if (_enemyCount == 0)
        {
            _enemyCount = ++_maxEnemies;
            for (int i = 0; i < _maxEnemies; i++)
            {
                CreateEnemy();
            }
        }
    }

    private void CreateEnemy()
    {
        Entity enemy = _world.CreateEntity();

        Random r = new();
        Vector2 enemyPosition = new(r.Next(0, _worldWidth), r.Next(0, _worldHeight));

        List<Vector2> enemyVertices = new()
        {
            new Vector2(0, 0),
            new Vector2(32, 0),
            new Vector2(32, 32),
            new Vector2(0, 32),
        };

        SizeF enemySize = new SizeF(32, 32);

        enemy.Attach(new FighterComponent
        {
            Speed = 100,
            Damage = 10,
            Health = 100,
            Color = Color.Red,
            Polygon = new Polygon(enemyVertices)
        });

        enemy.Attach(new BodyComponent(new MonoGame.Extended.RectangleF(enemyPosition, enemySize)));
    }
}
