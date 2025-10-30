using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGames1.Components;
using MonoGames1.Events;
using MonoGames1.Objects;

namespace MonoGames1.Systems;

public class ProjectileSystem : EntityProcessingSystem
{
    private EventBus _eventBus;

    private ComponentMapper<ProjectileComponent> _projectileMapper = default!;
    private ComponentMapper<BodyComponent> _bodyMapper = default!;

    public ProjectileSystem(EventBus eventBus) : base(Aspect.All(typeof(ProjectileComponent)))
    {
        _eventBus = eventBus;
    }
    public override void Initialize(IComponentMapperService mapperService)
    {
        _projectileMapper = mapperService.GetMapper<ProjectileComponent>();
        _bodyMapper = mapperService.GetMapper<BodyComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        ProjectileComponent projectile = _projectileMapper.Get(entityId);

        projectile.Age += gameTime.GetElapsedSeconds();
        if (projectile.Age > projectile.Duration)
        {
            DestroyEntity(entityId);
            return;
        }

        BodyComponent body = _bodyMapper.Get(entityId);
        body.Position += Utils.GetPositionChange(projectile.Direction, projectile.Speed, gameTime);

        while(body.Body.Others.TryDequeue(out ICollisionActor? other))
        {
            if (other is not ProjectileBody)
            {
                DestroyEntity(entityId);
            }
        }
    }
}
