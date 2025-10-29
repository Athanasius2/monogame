using Microsoft.Xna.Framework;
using MonoGames1.Objects;

namespace MonoGames1.Components
{
    public class BodyComponent
    {
        public Body Body = default!;

        public Vector2 Position
        {
            get => Body.Bounds.Position;
            set => Body.Bounds.Position = value;
        }

        public Vector2 Direction
        {
            get => Body.Direction;
            set => Body.Direction = value;
        }

        public int Speed
        {
            get => Body.Speed;
            set => Body.Speed = value;
        }
    }
}
