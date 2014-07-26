using Microsoft.Practices.Unity;

namespace Infrastructure.DDDES.Implementations.Internals
{
    internal class UnityFactory<TEntity> : IFactory<TEntity>
    {
        private readonly IUnityContainer _unityContainer;

        public UnityFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public TEntity Create()
        {
            return _unityContainer.Resolve<TEntity>();
        }
    }
}