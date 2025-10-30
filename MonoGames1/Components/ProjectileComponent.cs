using Microsoft.Xna.Framework;

namespace MonoGames1.Components;

public class ProjectileComponent
{
    /// <summary>
    /// Direction the projectile travels
    /// </summary>
    public Vector2 Direction;

    /// <summary>
    /// Speed of the projectile in pixels per second
    /// </summary>
    public int Speed;

    /// <summary>
    /// how much damage the projectile inflicts
    /// </summary>
    public int Damage;

    /// <summary>
    /// How long the projectile lasts before despawning in seconds
    /// </summary>
    public float MaxAge;

    /// <summary>
    /// current age of the projectile in seconds
    /// </summary>
    public float Age;
}
