using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace MonoGames1.Objects
{
    public class PlayerBody : Body
    {
        public PlayerBody(RectangleF bounds, Vector2 direction, int speed) : base(bounds, direction, speed) { }
    }
}
