using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Shapes;
using MonoGames1.Components;
using MonoGames1.Events;
using MonoGames1.Events.Args;
using MonoGames1.Objects;
using System;
using System.Collections.Generic;

namespace MonoGames1.Spawners;
public class EnemySpawner : EntityUpdateSystem
{
    private int _maxEnemies;

    private EventBus _eventBus;
    private SizeF _worldSize;

    public EnemySpawner(EventBus eventBus, int initMaxEnemies, SizeF worldSize) :
        base(Aspect.All(typeof(FighterComponent), typeof(BodyComponent)).Exclude(typeof(PlayerComponent)))
    {
        _eventBus = eventBus;
        _maxEnemies = initMaxEnemies;
        _worldSize = worldSize;
    }

    public override void Initialize(IComponentMapperService mapperService) 
    { 
        CreateEnemies(_maxEnemies);
    }

    public override void Update(GameTime gameTime)
    {
        if (ActiveEntities.Count == 0)
        {
            CreateEnemies(++_maxEnemies);
        }
    }

    private void CreateEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateEnemy();
        }
    }

    private void CreateEnemy()
    {
        Entity enemy = CreateEntity();

        Random r = new();
        Vector2 enemyPosition = new(r.Next(0,(int)_worldSize.Width), r.Next(0, (int) _worldSize.Height));

        List<Vector2> enemyVertices = new()
        {
            new Vector2(0, 0),
            new Vector2(32, 0),
            new Vector2(32, 32),
            new Vector2(0, 32),
        };

        SizeF enemySize = new(32, 32);

        enemy.Attach(new FighterComponent
        {
            Damage = 10,
            Health = 100,
            Color = Color.Red,
            Polygon = new Polygon(enemyVertices)
        });

        BodyComponent body = new()
        {
            Body = new Body(
                new RectangleF(enemyPosition, enemySize),
                Vector2.Zero,
                100
            ),
        };

        enemy.Attach(body);
        _eventBus.Push(new CreateBodyArgs{ Body = body.Body });
    }
}
