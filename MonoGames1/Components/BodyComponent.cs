using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGames1.Objects;

namespace MonoGames1.Components
{
    public class BodyComponent
    {
        public Body Body;

        public Vector2 Position
        {
            get => Body.Bounds.Position;
            set => Body.Bounds.Position = value;
        }

        public BodyComponent(RectangleF bounds)
        {
            Body = new Body(bounds);
        }
    }
}
