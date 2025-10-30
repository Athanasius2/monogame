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
        private ComponentMapper<ShapeComponent> _shapeMapper = default!;
        private ComponentMapper<BodyComponent> _bodyComponent = default!;
        private SpriteBatch _spriteBatch;

        public RenderSystem(SpriteBatch spriteBatch) : base(Aspect.All(typeof(ShapeComponent), typeof(BodyComponent)))
        {
            _spriteBatch = spriteBatch;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _shapeMapper = mapperService.GetMapper<ShapeComponent>();
            _bodyComponent = mapperService.GetMapper<BodyComponent>();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                ShapeComponent shape = _shapeMapper.Get(entityId);
                BodyComponent body = _bodyComponent.Get(entityId);
                _spriteBatch.DrawPolygon(body.Position, shape.Polygon, shape.Color, 5);
            }
        }
    }
}
