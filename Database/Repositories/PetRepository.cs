﻿using Dapper;
using Navislamia.Database.Entities;
using Navislamia.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navislamia.Database.Repositories
{
    public class PetRepository : IRepository
    {
        const string query = "select * from dbo.PetResource with (nolock)";

        IDbConnection dbConnection;

        IEnumerable<PetBase> Data;

        public string Name => "PetResource";

        public int Count => Data?.Count() ?? 0;

        public IEnumerable<T> GetData<T>() => (IEnumerable<T>)Data;

        public PetRepository(IDbConnection connection) => dbConnection = connection;

        public async Task<IRepository> Load()
        {
            Data = await dbConnection.QueryAsync<PetBase>(query);

            return this;
        }
    }
}
