using Prism.Events;
using XCode;

namespace LuYao.Toolkit.Events;

public class EntityDeletedEvent<TEntity> : PubSubEvent<TEntity> where TEntity : IEntity
{
}
