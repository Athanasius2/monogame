using Microsoft.Xna.Framework;
using MonoGame.Extended.Shapes;

namespace MonoGames1.Components
{
    public class FighterComponent
    {
        public int Damage;
        public int MaxHealth;
        public int Health;

        public Polygon Polygon = default!;
        public Color Color;
    }
}
