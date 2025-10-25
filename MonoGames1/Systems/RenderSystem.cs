
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGames1.Components;

namespace MonoGames1.Systems
{
    public class RenderSystem : EntityDrawSystem
    {
        private ComponentMapper<Fighter> _fighterMapper;
        private SpriteBatch _spriteBatch;

        public RenderSystem(SpriteBatch spriteBatch) : base(Aspect.All(typeof(Fighter)))
        {
            _spriteBatch = spriteBatch;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _fighterMapper = mapperService.GetMapper<Fighter>();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                Fighter fighter = _fighterMapper.Get(entityId);
                _spriteBatch.Begin();

                _spriteBatch.DrawPolygon(fighter.Position, fighter.Polygon, fighter.Color, 5);
                _spriteBatch.End();
            }
        }
    }
}
