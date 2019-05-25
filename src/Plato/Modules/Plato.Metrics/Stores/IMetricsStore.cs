﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Plato.Internal.Stores.Abstractions;

namespace Plato.Metrics.Stores
{
    public interface IMetricsStore<TModel> : IStore2<TModel> where TModel : class
    {
  
    }

}
