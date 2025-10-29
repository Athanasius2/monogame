using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace MonoGames1
{
    public static class Utils
    {
        public static Vector2 GetPositionChange(float direction, int speed, GameTime timeElapsed)
        {
            Vector2 directionVector = new Vector2(MathF.Cos(direction), MathF.Sin(direction));
            return GetPositionChange(directionVector, speed, timeElapsed);
        }

        public static Vector2 GetPositionChange(Vector2 direction, int speed, GameTime timeElapsed)
        {
            var normalized = direction.NormalizedCopy();
            return new Vector2
            {
                X = normalized.X * speed * timeElapsed.GetElapsedSeconds(),
                Y = normalized.Y * speed * timeElapsed.GetElapsedSeconds(),
            };
        }
    }
}
