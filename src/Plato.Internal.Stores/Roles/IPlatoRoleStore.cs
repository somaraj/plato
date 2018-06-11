﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Plato.Abstractions.Stores;
using Plato.Internal.Models.Roles;

namespace Plato.Internal.Stores.Roles
{
    public interface IPlatoRoleStore : IStore<Role>
    {
        Task<Role> GetByName(string name);

        Task<IList<Role>> GetRolesByUserId(int userId);

        Task<Role> GetByNormalizedName(string nameNormalized);
    }
}