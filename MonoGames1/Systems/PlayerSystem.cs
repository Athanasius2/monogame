using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGames1.Components;

namespace MonoGames1.Systems
{
    internal class PlayerSystem : EntityProcessingSystem
    {
        private ComponentMapper<Fighter> _fighterMapper;

        public PlayerSystem() : base(Aspect.All(typeof(Player), typeof(Fighter))) { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _fighterMapper = mapperService.GetMapper<Fighter>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            Fighter fighter = _fighterMapper.Get(entityId);

            if(Keyboard.GetState().IsKeyDown(Keys.W))
                fighter.Position.Y -= fighter.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                fighter.Position.Y += fighter.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                fighter.Position.X -= fighter.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                fighter.Position.X += fighter.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
