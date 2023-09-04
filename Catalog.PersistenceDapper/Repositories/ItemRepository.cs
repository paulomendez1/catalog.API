using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.Entities;
using Catalog.Persistence.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.PersistenceDapper.Repositories
{
    //public class ItemRepository : IItemRepository
    //{
    //    private readonly SqlConnection _sqlConnection;

    //    public ItemRepository(string connectionString)
    //    {
    //            _sqlConnection = new SqlConnection(connectionString);
    //    }

    //    public IUnitOfWork UnitOfWork => throw new NotImplementedException();

    //    public Item Add(Item entity)
    //    {
    //        var result = _sqlConnection.ExecuteScalar<Item>
    //            ("InsertItem", entity, commandType: CommandType.StoredProcedure);
    //        return result;
    //    }

    //    public async Task<IEnumerable<Item>> GetAllAsync()
    //    {
    //        var result = await _sqlConnection.QueryAsync<Item>
    //            ("GetAllItems", commandType: CommandType.StoredProcedure);
    //        return result.AsList();
    //    }

    //    public async Task<Item> GetByIdAsync(Guid id)
    //    {
    //        return await _sqlConnection.ExecuteScalarAsync<Item> 
    //            ("GetAllItems", new { Id = id.ToString() },
    //            commandType: CommandType.StoredProcedure);
    //    }

    //    public Task<Item> GetItemWithSub(Guid id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Item Update(Item entity)
    //    {
    //        var result = _sqlConnection.ExecuteScalar<Item>
    //            ("UpdateItem", entity, commandType: CommandType.StoredProcedure);
    //        return result;
    //    }
    //}
}
