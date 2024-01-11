using System.Data.SqlClient;

namespace Todo.Provider;

public class SqlConnectionProvider
{
    private readonly IConfiguration _configuration;

    public SqlConnectionProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SqlConnection GetSqlConnection()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        return new SqlConnection(connectionString);
    }
}