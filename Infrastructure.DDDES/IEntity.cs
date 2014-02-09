﻿using Infrastructure.Data;

namespace Infrastructure.DDDES
{
    public interface IEntity<TEntity>
    {
        IdentityOf<TEntity> Id { get; }
    }
}