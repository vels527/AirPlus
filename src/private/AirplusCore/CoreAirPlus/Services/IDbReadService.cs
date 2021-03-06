﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using CoreAirPlus.Entities;

namespace CoreAirPlus.Services
{
    public interface IDbReadService
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;
        TEntity Get<TEntity>(int id, bool includeRelatedEntities = false) where TEntity : class;
        TEntity Get<TEntity>(string userId, int id) where TEntity : class;
        IEnumerable<TEntity> GetWithIncludes<TEntity>() where TEntity : class;
        SelectList GetSelectList<TEntity>(string valueField, string textField) where TEntity : class;
        bool SaveReservation(Reservation reservation);
        bool SaveHost(Host host);
    }
}