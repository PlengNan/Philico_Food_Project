using Project_philico_food.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Project_philico_food.Db
{
    internal class OrderDb
    {
        private readonly SQLiteConnection _con;
        public string Err { get; set; }

        public OrderDb()
        {
            _con = DbConnect.SQLiteConnection;
            if (_con.State != ConnectionState.Open) _con.Open();
        }

        public string GenerateOrderNumber()
        {
            try
            {
                string yy = DateTime.Now.ToString("yy", System.Globalization.CultureInfo.CreateSpecificCulture("EN-en"));
                string MM = DateTime.Now.ToString("MM", System.Globalization.CultureInfo.CreateSpecificCulture("EN-en"));
                string dd = DateTime.Now.ToString("dd", System.Globalization.CultureInfo.CreateSpecificCulture("EN-en"));
                string prefix = $"OR{yy}{MM}{dd}-";

                const string sql = @"
                    SELECT OrderNumber
                    FROM Orders
                    WHERE OrderNumber LIKE @pfx || '%'
                    ORDER BY Id DESC
                    LIMIT 1";

                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    //cmd.Parameters.AddWithValue("@pfx", $"OR{yy}{MM}{dd}");
                    cmd.Parameters.Add(new SQLiteParameter("@pfx", $"OR{yy}{MM}{dd}"));
                    var last = cmd.ExecuteScalar() as string;

                    if (string.IsNullOrEmpty(last))
                        return $"{prefix}1";

                    int seq = 1;
                    int dash = last.LastIndexOf('-');
                    if (dash > 0 && int.TryParse(last.Substring(dash + 1), out int n))
                        seq = n + 1;

                    return $"{prefix}{seq}";
                }
            }
            catch (Exception ex) { Err = ex.Message; return null; }
        }

        public bool addNew(OrderModel model)
        {
            try
            {
                const string sql = @"
                    INSERT INTO Orders
                        (OrderNumber, ProductName, CustomerName, Note, NetWeight, Status, LicensePlate)
                    VALUES
                        (@OrderNumber, @ProductName, @CustomerName, @Note, @NetWeight, @Status, @LicensePlate)";

                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@OrderNumber", model.OrderNumber));
                    cmd.Parameters.Add(new SQLiteParameter("@ProductName", model.ProductName));
                    cmd.Parameters.Add(new SQLiteParameter("@CustomerName", model.CustomerName));
                    cmd.Parameters.Add(new SQLiteParameter("@Note", model.Note ?? ""));
                    cmd.Parameters.Add(new SQLiteParameter("@NetWeight", model.NetWeight));
                    cmd.Parameters.Add(new SQLiteParameter("@Status", model.Status));
                    cmd.Parameters.Add(new SQLiteParameter("@LicensePlate", model.LicensePlate));


                    //cmd.Parameters.AddWithValue("@OrderNumber", model.OrderNumber);
                    //cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    //cmd.Parameters.AddWithValue("@CustomerName", model.CustomerName);
                    //cmd.Parameters.AddWithValue("@Note", model.Note ?? "");
                    //cmd.Parameters.AddWithValue("@NetWeight", model.NetWeight);
                    //cmd.Parameters.AddWithValue("@Status", model.Status);
                    //cmd.Parameters.AddWithValue("@LicensePlate", model.LicensePlate);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { Err = ex.Message; return false; }
        }

        public List<OrderModel> getAllOrder()
        {
            var list = new List<OrderModel>();
            try
            {
                const string sql = "SELECT * FROM Orders";
                using (var da = new SQLiteDataAdapter(sql, _con))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0) return null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new OrderModel
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            OrderNumber = dr["OrderNumber"]?.ToString(),
                            ProductName = dr["ProductName"]?.ToString(),
                            CustomerName = dr["CustomerName"]?.ToString(),
                            Note = dr["Note"]?.ToString(),
                            Status = dr["Status"]?.ToString(),
                            LicensePlate = dr["LicensePlate"]?.ToString(),
                            NetWeight = dr["NetWeight"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NetWeight"])
                        });
                    }
                }
            }
            catch (Exception ex) { Err = ex.Message; return null; }
            return list;
        }

        public List<OrderModel> getAllOrderByStatus(string status)
        {
            var list = new List<OrderModel>();
            try
            {
                const string sql = "SELECT * FROM Orders WHERE Status=@s ORDER BY Id DESC";
                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    //cmd.Parameters.AddWithValue("@s", status);
                    cmd.Parameters.Add(new SQLiteParameter("@s", status));

                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count == 0) return null;

                        foreach (DataRow dr in dt.Rows)
                        {
                            list.Add(new OrderModel
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                OrderNumber = dr["OrderNumber"]?.ToString(),
                                ProductName = dr["ProductName"]?.ToString(),
                                CustomerName = dr["CustomerName"]?.ToString(),
                                Note = dr["Note"]?.ToString(),
                                Status = dr["Status"]?.ToString(),
                                LicensePlate = dr["LicensePlate"]?.ToString(),
                                NetWeight = dr["NetWeight"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NetWeight"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex) { Err = ex.Message; return null; }
            return list;
        }

        public OrderModel getOrderByOrderNumberOrId(string orderNumber)
        {
            //OrderModel model = new OrderModel();
            //try
            //{
            //    string sql = $"SELECT * FROM Ordesrs WHERE OrderNumber = '{orderNumber}'";
            //    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, _con))
            //    {
            //        DataTable tb = new DataTable();
            //        da.Fill(tb);
            //        if (tb.Rows.Count == 0 || tb == null)
            //            return null;
            //        foreach (DataRow dr in tb.Rows)
            //        {
            //            model = new OrderModel
            //            {
            //                Id = int.Parse(dr["Id"].ToString()),
            //                CustomerName = dr["CustomerName"].ToString(),
            //                ProductName = dr["ProductName"].ToString(),
            //                Note = dr["Note"].ToString(),
            //                NetWeight = int.Parse(dr["NetWeight"].ToString()),
            //                OrderNumber = dr["OrderNumber"].ToString(),
            //                Status = dr["Status"].ToString(),
            //                LicensePlate = dr["LicensePlate"].ToString()
            //            };
            //            break;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Err = ex.Message;
            //    return null;
            //}
            //return model;
            try
            {
                const string sql = "SELECT * FROM Orders WHERE OrderNumber = @no";
                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.AddWithValue("@no", orderNumber);

                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        var tb = new DataTable();
                        da.Fill(tb);
                        if (tb == null || tb.Rows.Count == 0) return null;

                        var dr = tb.Rows[0];
                        return new OrderModel
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            CustomerName = dr["CustomerName"]?.ToString(),
                            ProductName = dr["ProductName"]?.ToString(),
                            Note = dr["Note"]?.ToString(),
                            NetWeight = dr["NetWeight"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NetWeight"]),
                            OrderNumber = dr["OrderNumber"]?.ToString(),
                            Status = dr["Status"]?.ToString(),
                            LicensePlate = dr["LicensePlate"]?.ToString()
                        };
                    }
                }
            }
            catch (Exception ex) { Err = ex.Message; return null; }
        }

        public OrderModel getOrderByOrderNumberOrId(int id)
        {
            OrderModel model = new OrderModel();
            try
            {
                string sql = $"SELECT * FROM Orders WHERE Id = {id}";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, _con))
                {
                    DataTable tb = new DataTable();
                    da.Fill(tb);
                    if (tb.Rows.Count == 0 || tb == null)
                        return null;
                    foreach (DataRow dr in tb.Rows)
                    {
                        model = new OrderModel
                        {
                            Id = int.Parse(dr["Id"].ToString()),
                            CustomerName = dr["CustomerName"].ToString(),
                            ProductName = dr["ProductName"].ToString(),
                            Note = dr["Note"].ToString(),
                            NetWeight = int.Parse(dr["NetWeight"].ToString()),
                            OrderNumber = dr["OrderNumber"].ToString(),
                            Status = dr["Status"].ToString(),
                            LicensePlate = dr["LicensePlate"].ToString()

                        };
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Err = ex.Message;
                return null;
            }
            return model;
        }


        public bool UpdateNetWeight(string orderNumber, int netWeight, string newStatus, string noteEnc)
        {
            try
            {
                const string sql = "UPDATE Orders SET NetWeight=@w, Status=@s, Note=@n WHERE OrderNumber=@no";
                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@w", netWeight));                 
                    cmd.Parameters.Add(new SQLiteParameter("@s", newStatus ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SQLiteParameter("@n", noteEnc ?? ""));            
                    cmd.Parameters.Add(new SQLiteParameter("@no", orderNumber));
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { Err = ex.Message; return false; }
        }

        public OrderModel GetActiveByPlate(string plate)
        {
            try
            {
                const string sql = @"
                    SELECT * FROM Orders
                    WHERE LicensePlate=@p AND Status='Process'
                    ORDER BY Id DESC
                    LIMIT 1";

                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    //cmd.Parameters.AddWithValue("@p", plate);
                    cmd.Parameters.Add(new SQLiteParameter("@p", plate));
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (!rd.Read()) return null;

                        return new OrderModel
                        {
                            Id = Convert.ToInt32(rd["Id"]),
                            OrderNumber = rd["OrderNumber"]?.ToString(),
                            ProductName = rd["ProductName"]?.ToString(),
                            CustomerName = rd["CustomerName"]?.ToString(),
                            Note = rd["Note"]?.ToString(),
                            Status = rd["Status"]?.ToString(),
                            LicensePlate = rd["LicensePlate"]?.ToString(),
                            NetWeight = rd["NetWeight"] == DBNull.Value ? 0 : Convert.ToInt32(rd["NetWeight"])
                        };
                    }
                }
            }
            catch (Exception ex) { Err = ex.Message; return null; }
        }

        public bool DeleteOrder(string orderNumber)
        {
            try
            {
                const string sql = "DELETE FROM Orders WHERE OrderNumber=@no";
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
