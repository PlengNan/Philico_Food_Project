using Project_philico_food.functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_philico_food.Db
{
    public class ReportFilter
    {
        public string OrderNumber { get; set; }
        public string LicensePlate { get; set; }
        public string DatePrefix { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public List<string> DateList { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

    }

    public class StatusProcess
    {
        public int Success { get; set; }
        public int Process { get; set; }
        public int Cancle { get; set; }
    }
    public class ReportDb
    {
        private readonly SQLiteConnection _con;
        public ReportDb()
        {
            _con = DbConnect.SQLiteConnection;
            if (_con.State != System.Data.ConnectionState.Open) _con.Open();
        }

        private const string BASE_SQL = @"
                                        SELECT
                                            o.OrderNumber,
                                            o.LicensePlate,
                                            o.CustomerName,
                                            o.ProductName,
                                            o.NetWeight,
                                            COALESCE(
                                                 (SELECT dIN.Datez
                                                    FROM OrderDetail dIN
                                                   WHERE dIN.OrderNumber = o.OrderNumber AND dIN.WeightType = @wtIn
                                                   ORDER BY dIN.Id DESC
                                                   LIMIT 1),
                                                 (SELECT dOUT.Datez
                                                    FROM OrderDetail dOUT
                                                   WHERE dOUT.OrderNumber = o.OrderNumber AND dOUT.WeightType = @wtOut
                                                   ORDER BY dOUT.Id DESC
                                                   LIMIT 1),
                                                 ''
                                             ) AS Datez,
                                            
                                            COALESCE((
                                                SELECT dIN.Weight
                                                  FROM OrderDetail dIN
                                                 WHERE dIN.OrderNumber = o.OrderNumber AND dIN.WeightType = @wtIn
                                                 ORDER BY dIN.Id DESC
                                                 LIMIT 1
                                            ), 0) AS WeightIn,
                                            COALESCE((
                                                SELECT dOUT.Weight
                                                  FROM OrderDetail dOUT
                                                 WHERE dOUT.OrderNumber = o.OrderNumber AND dOUT.WeightType = @wtOut
                                                 ORDER BY dOUT.Id DESC
                                                 LIMIT 1
                                            ), 0) AS WeightOut,
                                            o.Status
                                        FROM Orders o
                                    ";
        
        public List<string> GetDistinctEncrypted(string column)
        {
            var list = new List<string>();
            string sql = $@"
                            SELECT DISTINCT {column}
                            FROM Orders
                            WHERE {column} IS NOT NULL AND {column} <> ''
                            ORDER BY {column};";
            using (var cmd = new SQLiteCommand(sql, _con))
            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    var v = rd[column]?.ToString();
                    if (!string.IsNullOrWhiteSpace(v)) list.Add(v);
                }
            }
            return list;
        }
        private void SearchWhere(ReportFilter f, out string whereSql, out SQLiteParameter[] whereParams)
        {
            var where = new List<string>();
            var ps = new List<SQLiteParameter>();

            if (!string.IsNullOrWhiteSpace(f?.LicensePlate))
            {
                where.Add("o.LicensePlate = @car");
                ps.Add(new SQLiteParameter("@car", f.LicensePlate));
            }
            if (!string.IsNullOrWhiteSpace(f?.CustomerName))
            {
                where.Add("o.CustomerName = @cus");
                ps.Add(new SQLiteParameter("@cus", f.CustomerName));
            }
            if (!string.IsNullOrWhiteSpace(f?.ProductName))
            {
                where.Add("o.ProductName = @pro");
                ps.Add(new SQLiteParameter("@pro", f.ProductName));
            }
            if (f?.DateList != null && f.DateList.Count > 0)
            {
                var names = new List<string>();
                for (int i = 0; i < f.DateList.Count; i++)
                {
                    string p = "@d" + i;
                    names.Add(p);
                    ps.Add(new SQLiteParameter(p, f.DateList[i]));
                }

                where.Add($@"
                        EXISTS (
                            SELECT 1
                            FROM OrderDetail d
                            WHERE d.OrderNumber = o.OrderNumber
                              AND d.Datez IN ({string.Join(",", names)})
                        )");
             }

            
            whereSql = where.Count > 0 ? " WHERE " + string.Join(" AND ", where) : "";
            whereParams = ps.ToArray();
        }

        public int CountOrders(ReportFilter filter)
        {
            SearchWhere(filter, out var where, out var pars);
            string sql = "SELECT COUNT(*) FROM Orders o" + where + ";";
            using (var cmd = new SQLiteCommand(sql, _con))
            {
                cmd.Parameters.AddRange(pars);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public DataTable GetReportPage(ReportFilter filter, int page, int pageSize)
        {
            if (page < 1) page = 1;
            int offset = (page - 1) * pageSize;

            SearchWhere(filter, out var where, out var pars);
            string sql = BASE_SQL + where + " ORDER BY o.Id DESC LIMIT @limit OFFSET @offset;";

            using (var da = new SQLiteDataAdapter(sql, _con))
            {
                foreach (var p in pars) da.SelectCommand.Parameters.Add(p);
                da.SelectCommand.Parameters.Add(new SQLiteParameter("@limit", pageSize));
                da.SelectCommand.Parameters.Add(new SQLiteParameter("@offset", offset));

                var aes = new AESEncryption();
                da.SelectCommand.Parameters.Add(new SQLiteParameter("@wtIn", aes.Encrypt("IN")));
                da.SelectCommand.Parameters.Add(new SQLiteParameter("@wtOut", aes.Encrypt("OUT")));

                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        string whereToday = @"
                            EXISTS (
                                SELECT 1
                                FROM OrderDetail d
                                WHERE d.OrderNumber = o.OrderNumber 
                                  AND d.Datez LIKE @today || '%'
                            )";
        string todayPrefix = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"));
        public DataTable GetTodayReportPage(ReportFilter filter, int page, int pageSize)
        {
            if (page < 1) page = 1;
            int offset = (page - 1) * pageSize;

            SearchWhere(filter, out var where, out var pars);

            where = string.IsNullOrWhiteSpace(where)
                ? " WHERE " + whereToday
                : where + " AND " + whereToday;

            string sql = BASE_SQL + where + " ORDER BY o.Id DESC LIMIT @limit OFFSET @offset;";

            using (var da = new SQLiteDataAdapter(sql, _con))
            {
                var aes = new AESEncryption();

                foreach (var p in pars) da.SelectCommand.Parameters.Add(p);


                da.SelectCommand.Parameters.Add(new SQLiteParameter("@today", aes.Encrypt(todayPrefix)));
                da.SelectCommand.Parameters.Add(new SQLiteParameter("@limit", pageSize));
                da.SelectCommand.Parameters.Add(new SQLiteParameter("@offset", offset));
                da.SelectCommand.Parameters.Add(new SQLiteParameter("@wtIn", aes.Encrypt("IN")));
                da.SelectCommand.Parameters.Add(new SQLiteParameter("@wtOut", aes.Encrypt("OUT")));

                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public StatusProcess GetStatusProcess(ReportFilter filter)
        {
            SearchWhere(filter, out var where, out var pars);

            string sql = @"
                SELECT
                    SUM(CASE WHEN o.Status IN ('Success','Completed') THEN 1 ELSE 0 END) AS SuccessCount,
                    SUM(CASE WHEN o.Status IN ('Process','Processing') THEN 1 ELSE 0 END) AS ProcessCount,
                    SUM(CASE WHEN o.Status IN ('Cancle','Cancle','Cancled','Cancled') THEN 1 ELSE 0 END) AS CancleCount
                FROM Orders o
            " + where + ";";

            using (var cmd = new SQLiteCommand(sql, _con))
            {
                cmd.Parameters.AddRange(pars);
                using (var r = cmd.ExecuteReader())
                {
                    var c = new StatusProcess();
                    if (r.Read())
                    {
                        c.Success = r.IsDBNull(0) ? 0 : r.GetInt32(0);
                        c.Process = r.IsDBNull(1) ? 0 : r.GetInt32(1);
                        c.Cancle = r.IsDBNull(2) ? 0 : r.GetInt32(2);
                    }
                    return c;
                }
            }
        }

        public int GetSumTotal(ReportFilter filter)
        {
            SearchWhere(filter, out var where, out var pars);

            where = string.IsNullOrWhiteSpace(where)
               ? " WHERE " + whereToday
               : where + " AND " + whereToday;

            string sql = "SELECT COUNT(1) FROM Orders o " + where + ";";

            using (var cmd = new SQLiteCommand(sql, _con))
            {
                cmd.Parameters.AddRange(pars);

                var aes = new AESEncryption();
                cmd.Parameters.AddWithValue("@today", aes.Encrypt(todayPrefix));

                var obj = cmd.ExecuteScalar();
                return Convert.ToInt32(obj ?? 0);
            }

        }
        public decimal GetSumNetWeightToday(ReportFilter filter)
        {
            SearchWhere(filter, out var where, out var pars);

            where = string.IsNullOrWhiteSpace(where)
                ? " WHERE " + whereToday
                : where + " AND " + whereToday;

            string sql = @"
                        SELECT COALESCE(SUM(o.NetWeight), 0)
                        FROM Orders o
                    " + where + ";";

            using (var cmd = new SQLiteCommand(sql, _con))
            {
                cmd.Parameters.AddRange(pars);

                var aes = new AESEncryption();
                cmd.Parameters.AddWithValue("@today", aes.Encrypt(todayPrefix));

                var obj = cmd.ExecuteScalar();
                return (obj == null || obj == DBNull.Value) ? 0m : Convert.ToDecimal(obj);
            }
        }

        public DataTable GetTicketTable(string orderNumber)
        {
            const string SQL = @"
                                SELECT 
                                    o.OrderNumber,
                                    o.LicensePlate,
                                    o.CustomerName,
                                    o.ProductName,
                                    'IN'  AS WeightType,
                                    'Weight In'  AS Detail,
                                    (SELECT d.Datez  FROM OrderDetail d 
                                      WHERE d.OrderNumber = o.OrderNumber AND d.WeightType = @wtIn
                                      ORDER BY d.Id DESC LIMIT 1) AS DateIn,
                                    (SELECT d.Timez  FROM OrderDetail d 
                                      WHERE d.OrderNumber = o.OrderNumber AND d.WeightType = @wtIn
                                      ORDER BY d.Id DESC LIMIT 1) AS TimeIn,
                                    (SELECT d.Weight FROM OrderDetail d 
                                      WHERE d.OrderNumber = o.OrderNumber AND d.WeightType = @wtIn
                                      ORDER BY d.Id DESC LIMIT 1) AS WeightIn,
                                    NULL AS DateOut,
                                    NULL AS TimeOut,
                                    NULL AS WeightOut,
                                    o.NetWeight
                                FROM Orders o
                                WHERE o.OrderNumber = @ord

                                UNION ALL

                                SELECT 
                                    o.OrderNumber,
                                    o.LicensePlate,
                                    o.CustomerName,
                                    o.ProductName,
                                    'OUT' AS WeightType,
                                    'Weight Out' AS Detail,
                                    NULL AS DateIn,
                                    NULL AS TimeIn,
                                    NULL AS WeightIn,
                                    (SELECT d.Datez  FROM OrderDetail d 
                                      WHERE d.OrderNumber = o.OrderNumber AND d.WeightType = @wtOut
                                      ORDER BY d.Id DESC LIMIT 1) AS DateOut,
                                    (SELECT d.Timez  FROM OrderDetail d 
                                      WHERE d.OrderNumber = o.OrderNumber AND d.WeightType = @wtOut
                                      ORDER BY d.Id DESC LIMIT 1) AS TimeOut,
                                    (SELECT d.Weight FROM OrderDetail d 
                                      WHERE d.OrderNumber = o.OrderNumber AND d.WeightType = @wtOut
                                      ORDER BY d.Id DESC LIMIT 1) AS WeightOut,
                                    o.NetWeight
                                FROM Orders o
                                WHERE o.OrderNumber = @ord

                                ORDER BY WeightType ASC;
                            ";

            using (var da = new SQLiteDataAdapter(SQL, _con))
            {
                var aes = new AESEncryption();
                da.SelectCommand.Parameters.AddWithValue("@ord", orderNumber);
                da.SelectCommand.Parameters.AddWithValue("@wtIn", aes.Encrypt("IN"));
                da.SelectCommand.Parameters.AddWithValue("@wtOut", aes.Encrypt("OUT"));

                var dt = new DataTable("dsTicket");
                da.Fill(dt);

                string Dec(object v)
                {
                    var s = v?.ToString();
                    if (string.IsNullOrEmpty(s)) return "";
                    try { return aes.Decrypt(s); } catch { return s; }
                }

                foreach (DataRow r in dt.Rows)
                {
                    r["LicensePlate"] = Dec(r["LicensePlate"]);
                    r["CustomerName"] = Dec(r["CustomerName"]);
                    r["ProductName"] = Dec(r["ProductName"]);

                    r["DateIn"] = Dec(r["DateIn"]);
                    r["TimeIn"] = Dec(r["TimeIn"]);
                    r["DateOut"] = Dec(r["DateOut"]);
                    r["TimeOut"] = Dec(r["TimeOut"]);
                }

                return dt;
            }
        }


    }
}
