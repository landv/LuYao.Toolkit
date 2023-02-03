using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace LuYao.Toolkit.Events;

public class EntityUpdatedEvent<TEntity> : PubSubEvent<TEntity> where TEntity : IEntity
{
}
