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
        private ComponentMapper<FighterComponent> _fighterMapper = default!;
        private ComponentMapper<BodyComponent> _bodyComponent = default!;
        private SpriteBatch _spriteBatch;

        public RenderSystem(SpriteBatch spriteBatch) : base(Aspect.All(typeof(FighterComponent)))
        {
            _spriteBatch = spriteBatch;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _fighterMapper = mapperService.GetMapper<FighterComponent>();
            _bodyComponent = mapperService.GetMapper<BodyComponent>();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                FighterComponent fighter = _fighterMapper.Get(entityId);
                BodyComponent body = _bodyComponent.Get(entityId);

                _spriteBatch.Begin();

                _spriteBatch.DrawPolygon(body.Position, fighter.Polygon, fighter.Color, 5);
                _spriteBatch.End();
            }
        }
    }
}
