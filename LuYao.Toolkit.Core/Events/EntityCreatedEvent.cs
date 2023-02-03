using Prism.Events;
using XCode;

namespace LuYao.Toolkit.Events;

public class EntityCreatedEvent<TEntity> : PubSubEvent<TEntity> where TEntity : IEntity
{
}
