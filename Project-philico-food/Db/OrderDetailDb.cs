using Project_philico_food.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_philico_food.Db
{
    internal class OrderDetailDb
    {
        private readonly SQLiteConnection _con;
        public string Err {  get; set; }

        public OrderDetailDb()
        {
            _con = DbConnect.SQLiteConnection;
        }

        public bool Add(OrderDetailModel m)
        {
            try
            {
                const string sql = @"INSERT INTO OrderDetail(OrderNumber, Datez, Timez, Weight, WeightType)
                                     VALUES(@OrderNumber, @Datez, @Timez, @Weight, @WeightType)";
                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@OrderNumber", m.OrderNumber));
                    cmd.Parameters.Add(new SQLiteParameter("@Datez", m.Datez));
                    cmd.Parameters.Add(new SQLiteParameter("@Timez", m.Timez));
                    cmd.Parameters.Add(new SQLiteParameter("@Weight", m.Weight));
                    cmd.Parameters.Add(new SQLiteParameter("@WeightType", m.WeightType));


                    //cmd.Parameters.AddWithValue("@OrderNumber", m.OrderNumber);
                    //cmd.Parameters.AddWithValue("@Datez", m.Datez);
                    //cmd.Parameters.AddWithValue("@Timez", m.Timez);
                    //cmd.Parameters.AddWithValue("@Weight", m.Weight);
                    //cmd.Parameters.AddWithValue("@WeightType", m.WeightType);
                    //cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { Err = ex.Message; return false; }
        }

        public List<OrderDetailModel> GetByOrderNumber(string orderNumber)
        {
            var list = new List<OrderDetailModel>();
            try
            {
                const string sql = @"SELECT * FROM OrderDetail WHERE OrderNumber=@no ORDER BY Id ASC";
                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    //cmd.Parameters.AddWithValue("@no", orderNumber);
                    cmd.Parameters.Add(new SQLiteParameter("@no", orderNumber));
                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        foreach (DataRow r in dt.Rows)
                        {
                            list.Add(new OrderDetailModel
                            {
                                Id = Convert.ToInt32(r["Id"]),
                                OrderNumber = r["OrderNumber"]?.ToString(),
                                Datez = r["Datez"]?.ToString(),
                                Timez = r["Timez"]?.ToString(),
                                Weight = Convert.ToInt32(r["Weight"]),
                                WeightType = r["WeightType"]?.ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex) { Err = ex.Message; return null; }
            return list;
        }

        public DataTable GetOpenFirstWeighTable()
        {
            var dt = new DataTable();
            try
            {
                const string sql = @"
            SELECT  od.OrderNumber,
                    od.Datez,
                    od.Timez,
                    od.Weight,
                    o.LicensePlate
            FROM OrderDetail od
            INNER JOIN Orders o ON o.OrderNumber = od.OrderNumber
            WHERE od.WeightType = 'IN' AND o.Status = 'Process'
            ORDER BY od.Id DESC";

                using (var da = new SQLiteDataAdapter(sql, _con))
                {
                    da.Fill(dt);
                }
            }
            catch (Exception ex) { Err = ex.Message; return null; }
            return dt;
        }
        public bool DeleteByOrderNumber(string orderNumber)
        {
            try
            {
                const string sql = "DELETE FROM OrderDetail WHERE OrderNumber=@no";
                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    //cmd.Parameters.AddWithValue("@no", orderNumber);
                    cmd.Parameters.Add(new SQLiteParameter("@no", orderNumber));
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex) { Err = ex.Message; return false; }
        }


    }
}
