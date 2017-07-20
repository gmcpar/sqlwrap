using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

interface IDB
{
    DataSet GetDataSet(string spname, [Optional] Dictionary<string, object> paramlist);
    DataTable GetDataTable(string spname, [Optional] Dictionary<string, object> paramlist);
    IList<T> GetList<T>(string spname, [Optional] Dictionary<string, object> paramlist) where T : new();
    T GetValue<T>(string spname, [Optional] Dictionary<string, object> paramlist);
    void ExecuteSP(string spname, [Optional] Dictionary<string, object> paramlist);
}


