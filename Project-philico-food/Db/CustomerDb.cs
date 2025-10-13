using Project_philico_food.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Project_philico_food.Db
{
    internal class CustomerDb
    {
        public CustomerDb()
        {
            this._con = DbConnect.SQLiteConnection;
        }

        private readonly SQLiteConnection _con;
        public string Err { get; set; }

        public bool addNew(CustomerModel model)
        {
            try
            {
                string sql = "INSERT INTO Customer (CustomerName,CustomerCode)VALUES(@CustomerName,@CustomerCode)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@CustomerName", model.CustomerName));
                    cmd.Parameters.Add(new SQLiteParameter("@CustomerCode", model.CustomerCode));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Err = ex.Message;
                return false;
            }
            return true;
        }


        public List<CustomerModel> getAllCustomer()
        {
            List<CustomerModel> lists = new List<CustomerModel>();
            try
            {
                string sql = "SELECT * FROM Customer";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, _con))
                {
                    DataTable tb = new DataTable();
                    da.Fill(tb);
                    if (tb.Rows.Count == 0)
                    {
                        return null;
                    }

                    foreach (DataRow rw in tb.Rows)
                    {
                        CustomerModel model = new CustomerModel
                        {
                            Id = int.Parse(rw["Id"].ToString()),
                            CustomerName = rw["CustomerName"].ToString(),
                            CustomerCode = rw["CustomerCode"].ToString()
                        };
                        lists.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                Err = ex.Message;
                return null;
            }
            return lists;
        }


        public List<CustomerModel> getCustomerByCustomerName(string customerName)
        {
            List<CustomerModel> lists = new List<CustomerModel>();
            try
            {
                string sql = $"SELECT * FROM Customer WHERE CustomerName = '{customerName}'";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, _con))
                {
                    DataTable tb = new DataTable();
                    da.Fill(tb);
                    if (tb.Rows.Count == 0)
                    {
                        return null;
                    }

                    foreach (DataRow rw in tb.Rows)
                    {
                        CustomerModel model = new CustomerModel
                        {
                            Id = int.Parse(rw["Id"].ToString()),
                            CustomerName = rw["CustomerName"].ToString(),
                            CustomerCode = rw["CustomerCode"].ToString()
                        };
                        lists.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                Err = ex.Message;
                return null;
            }
            return lists;
        }


        public List<CustomerModel> getCustomerByCustomerCode(string customerCode)
        {
            List<CustomerModel> lists = new List<CustomerModel>();
            try
            {
                string sql = $"SELECT * FROM Customer WHERE CustomerCode = '{customerCode}'";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, _con))
                {
                    DataTable tb = new DataTable();
                    da.Fill(tb);
                    if (tb.Rows.Count == 0)
                    {
                        return null;
                    }

                    foreach (DataRow rw in tb.Rows)
                    {
                        CustomerModel model = new CustomerModel
                        {
                            Id = int.Parse(rw["Id"].ToString()),
                            CustomerName = rw["CustomerName"].ToString(),
                            CustomerCode = rw["CustomerCode"].ToString()
                        };
                        lists.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                Err = ex.Message;
                return null;
            }
            return lists;
        }

        public List<CustomerModel> getCustomerByCustomerNameAndCode(string customerCode, string customerName)
        {
            List<CustomerModel> lists = new List<CustomerModel>();
            try
            {
                string sql = $"SELECT * FROM Customer WHERE CustomerCode = '{customerCode}' and Customername = '{customerName}'";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, _con))
                {
                    DataTable tb = new DataTable();
                    da.Fill(tb);
                    if (tb.Rows.Count == 0)
                    {
                        return null;
                    }

                    foreach (DataRow rw in tb.Rows)
                    {
                        CustomerModel model = new CustomerModel
                        {
                            Id = int.Parse(rw["Id"].ToString()),
                            CustomerName = rw["CustomerName"].ToString(),
                            CustomerCode = rw["CustomerCode"].ToString()
                        };
                        lists.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                Err = ex.Message;
                return null;
            }
            return lists;
        }


        public bool deleteCustomerById(int id)
        {
            try
            {
                string sql = $"DELETE FROM Customer WHERE Id = {id}";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Err = ex.Message;
                return false;
            }
            return true;
        }

        public bool updateCustomer(CustomerModel model)
        {
            try
            {
                string sql = "UPDATE Customer " +
                    "SET CustomerName = @CustomerName, " +
                    "CustomerCode = @CustomerCode " +
                    "WHERE Id = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@CustomerName", model.CustomerName));
                    cmd.Parameters.Add(new SQLiteParameter("@CustomerCode", model.CustomerCode));
                    cmd.Parameters.Add(new SQLiteParameter("@Id", model.Id));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Err = ex.Message;
                return false;

            }
            return true;
        }
    }
}
