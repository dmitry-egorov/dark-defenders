using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.Terrains;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Players
{
    public class PlayerSnapshot : IPlayerEventsReciever
    {
        public TerrainId TerrainId { get; private set; }
        public Vector Position { get; private set; }
        public Vector DesiredOrientation { get; private set; }

        public void Apply(PlayerCreated playerCreated)
        {
            TerrainId = playerCreated.TerrainId;
            Position = playerCreated.SpawnPosition;
            DesiredOrientation = new Vector(0, 0);
        }

        public void Apply(PlayersDesiredOrientationIsSet playersDesiredOrientationIsSet)
        {
            DesiredOrientation = playersDesiredOrientationIsSet.Orientation;
        }

        public void Apply(PlayerMoved playerMoved)
        {
            Position = playerMoved.NewPosition;
        }
    }
}