using MonoGame.Extended.ECS;
using MonoGames1.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public void OnEnemyDeath(object? sender, PositionEventArgs e)
    {
        _enemyCount--;
    }

    public void Update()
    {
        if (_enemyCount == 0)
        {

        }
    }

    private void Spawn(int count)
    {
        _enemyCount = ++_maxEnemies;
        for (int i = 0; i < _maxEnemies; i++)
        {
            CreateEnemy();
        }
    }

    private void CreateEnemy()
    {
        Entity enemy = _world.CreateEntity();
    }

}
