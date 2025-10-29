using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGames1.Components;

namespace MonoGames1.Systems;
public class HealthBarSystem : EntityDrawSystem
{
    private ComponentMapper<FighterComponent> _fighterMapper = default!;
    private SpriteBatch _spriteBatch;
    public HealthBarSystem(SpriteBatch spriteBatch) : base(Aspect.All(typeof(PlayerComponent), typeof(FighterComponent)))
    {
        _spriteBatch = spriteBatch;
    }

    public override void Draw(GameTime gameTime)
    {
        if (ActiveEntities.Count > 0)
        {
            int maxHealth = _fighterMapper.Get(ActiveEntities[0]).MaxHealth;
            int health = _fighterMapper.Get(ActiveEntities[0]).Health;
            health = health > 0 ? health : 0;
            _spriteBatch.DrawLine(new Vector2(50, 50), new Vector2(50 + maxHealth, 50), Color.Gray, 10);
            _spriteBatch.DrawLine(new Vector2(50, 50), new Vector2(50 + health, 50), Color.Green, 10);
        }
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _fighterMapper = mapperService.GetMapper<FighterComponent>();
    }
}
