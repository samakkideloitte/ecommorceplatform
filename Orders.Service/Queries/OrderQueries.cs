using Dapper;
using Microsoft.Data.SqlClient;
using Orders.Service.ReadModels;

namespace Orders.Service.Queries;

public class OrderQueries
{
    private readonly IConfiguration _config;

    public OrderQueries(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<OrderReadModel>> GetAll()
    {
        using var connection = new SqlConnection(
            _config.GetConnectionString("OrdersDb"));

        var sql = """
                  SELECT
                      Id,
                      CustomerId,
                      Amount,
                      Status,
                      CreatedAt
                  FROM Orders
                  ORDER BY CreatedAt DESC
                  """;

        return await connection.QueryAsync<OrderReadModel>(sql);
    }
}