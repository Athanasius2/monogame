using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;

namespace MonoGames1.Components
{
    public class FighterComponent
    {
        public Vector2 Position;
        public int Speed;
        public int Damage;
        public int Health;

        public Polygon Polygon;
        public Color Color;
    }
}
