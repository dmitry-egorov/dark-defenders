using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Mono.Client.Presenters
{
    public class PlayerFollowingOperator
    {
        private readonly Camera _camera;

        private IdentityOf<RemoteEntity> _currentPlayerId;
        private bool _playerFound;
        private Vector _currentPlayerPosition;

        public PlayerFollowingOperator(Camera camera)
        {
            _camera = camera;
        }

        public void NotifyCreated(IdentityOf<RemoteEntity> id, Vector initialPosition, RemoteEntityType type)
        {
            if (type != RemoteEntityType.Player)
            {
                return;
            }

            _currentPlayerId = id;
            _playerFound = true;
            _currentPlayerPosition = initialPosition;
        }

        public void Update()
        {
            if (PlayerNotFound())
            {
                return;
            }

            var current = _camera.GetPosition();

            var delta = (_currentPlayerPosition - current) * 0.1f;

            var newPosition = current + delta;

            _camera.SetPosition(newPosition);
        }

        private bool PlayerNotFound()
        {
            return !_playerFound;
        }

        public void NotifyMoved(IdentityOf<RemoteEntity> id, Vector newPosition)
        {
            if (id != _currentPlayerId)
            {
                return;
            }

            _currentPlayerPosition = newPosition;
        }
    }
}