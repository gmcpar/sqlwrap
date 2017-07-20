using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class SQLDB
{
    private string connectionString;

    public SQLDB(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void ExecuteSP(string spName, Dictionary<string, object> paramList = null)
    {
        using (SqlConnection sqlConnection = new SqlConnection())
        {
            using (SqlCommand sqlCommand = new SqlCommand(spName, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                if (paramList != null)
                {
                    foreach (var param in paramList)
                    {
                        sqlCommand.Parameters.Add(param.Key, GetSqlDBType(param.Value)).Value = param.Value;
                    }
                }
                sqlCommand.ExecuteNonQuery();
            }
        }
    }

    public DataSet GetDataSet(string spName, Dictionary<string, object> paramList = null)
    {
        DataSet ds = new DataSet();
        DataTable dt = GetDataTable(spName, paramList);
        ds.Tables.Add(dt);
        return ds;
    }

    public DataTable GetDataTable(string spName, Dictionary<string, object> paramList = null)
    {
        DataTable dt = new DataTable();
        using (SqlConnection sqlConnection = new SqlConnection())
        {
            using (SqlCommand sqlCommand = new SqlCommand(spName, sqlConnection))
            {
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if (paramList != null)
                    {
                        foreach (var param in paramList)
                        {
                            sqlCommand.Parameters.Add(param.Key, GetSqlDBType(param.Value)).Value = param.Value;
                        }
                    }
                    sqlAdapter.Fill(dt);
                }
            }
        }
        return dt;
    }

    public IList<T> GetList<T>(string spName, Dictionary<string, object> paramList = null) where T : new()
    {
        return GetDataTable(spName, paramList).ToList<T>();
    }

    public T GetItem<T>(string spName, Dictionary<string, object> paramList = null) where T : new()
    {
        DataTable dataTable = GetDataTable(spName, paramList);
        return DataTableToListExtension.CreateItem<T>(dataTable.Rows[0], typeof(T).GetProperties());
    }
    public T GetValue<T>(string spName, Dictionary<string, object> paramList = null)
    {
        T t;

        using (SqlConnection sqlConnection = new SqlConnection())
        {
            using (SqlCommand sqlCommand = new SqlCommand(spName, sqlConnection))
            {
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if (paramList != null)
                    {
                        foreach (var param in paramList)
                        {
                            sqlCommand.Parameters.Add(param.Key, GetSqlDBType(param.Value)).Value = param.Value;
                        }
                    }
                    t = (T)sqlCommand.ExecuteScalar();
                }
            }
        }
        return t;
    }

    public SqlDbType GetSqlDBType(object o)
    {
        switch (Type.GetTypeCode(o.GetType()))
        {
            case TypeCode.Boolean:
                return SqlDbType.Bit;
            case TypeCode.Byte:
                return SqlDbType.Binary;
            case TypeCode.Char:
                return SqlDbType.Char;
            case TypeCode.DateTime:
                return SqlDbType.DateTime;
            case TypeCode.Decimal:
                return SqlDbType.Decimal;
            case TypeCode.Double:
                return SqlDbType.Float;
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
                return SqlDbType.Int;
            case TypeCode.Object:
                return SqlDbType.VarBinary;
            case TypeCode.Single:
                return SqlDbType.Float;
            case TypeCode.String:
                return SqlDbType.NVarChar;
            default:
                throw new ArgumentOutOfRangeException("Undefined Type");
        }
    }
}