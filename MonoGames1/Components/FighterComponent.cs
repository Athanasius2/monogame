using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;

namespace MonoGames1.Components
{
    public class FighterComponent
    {
        public int Speed;
        public int Damage;
        public int MaxHealth;
        public int Health;

        public Polygon Polygon;
        public Color Color;
    }
}
