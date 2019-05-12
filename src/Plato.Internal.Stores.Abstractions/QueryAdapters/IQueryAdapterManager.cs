﻿using System.Collections.Generic;
using System.Text;
using Plato.Internal.Data.Abstractions;

namespace Plato.Internal.Stores.Abstractions.QueryAdapters
{

    public interface IQueryAdapterManager<TModel> where TModel : class
    {

        IEnumerable<string> BuildSelect(IQuery<TModel> query, StringBuilder builder);

        IEnumerable<string> BuildTables(IQuery<TModel> query, StringBuilder builder);

        IEnumerable<string> BuildWhere(IQuery<TModel> query, StringBuilder builder);

    }
    
}
