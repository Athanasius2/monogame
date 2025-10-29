using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace MonoGames1.Objects;

public class ProjectileBody : Body
{
    public ProjectileBody(RectangleF bounds, Vector2 direction, int speed) : base(bounds, direction, speed)
    {
    }
}
