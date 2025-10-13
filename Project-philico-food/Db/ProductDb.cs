using Project_philico_food.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_philico_food.Db
{
    internal class ProductDb
    {

        public ProductDb()
        {
            this._con = DbConnect.SQLiteConnection;
        }

        public string Err { get; set; }

        private readonly SQLiteConnection _con;
        public bool addNew(ProductModel model)
        {
            try
            {
                string sql = "INSERT INTO Product (ProductName,ProductCode) VALUES(@ProductName,@ProductCode)";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@ProductName", model.ProductName));
                    cmd.Parameters.Add(new SQLiteParameter("@ProductCode", model.ProductCode));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return false;


            }
            return true;
        }

        public List<ProductModel> getProductByNamesAndCode(string prdName, string prdCode)
        {
            List<ProductModel> lists = new List<ProductModel>();
            try
            {
                string sql = $"SELECT * FROM Product WHERE ProductCode = '{prdCode}' and ProductName = '{prdName}'";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, _con))
                {
                    DataTable tb = new DataTable();
                    da.Fill(tb);
                    if (tb.Rows.Count == 0)
                        return null;
                    foreach (DataRow rw in tb.Rows)
                    {
                        ProductModel model = new ProductModel
                        {
                            Id = int.Parse(rw["Id"].ToString()),
                            ProductCode = rw["ProductCode"].ToString(),
                            ProductName = rw["ProductName"].ToString()
                        };
                        lists.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return lists;
        }

        public List<ProductModel> getProductByNames(string prdName)
        {
            List<ProductModel> lists = new List<ProductModel>();
            try
            {
                string sql = $"SELECT * FROM Product WHERE ProductName = '{prdName}'";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, _con))
                {
                    DataTable tb = new DataTable();
                    da.Fill(tb);
                    if (tb.Rows.Count == 0)
                        return null;
                    foreach (DataRow rw in tb.Rows)
                    {
                        ProductModel model = new ProductModel
                        {
                            Id = int.Parse(rw["Id"].ToString()),
                            ProductCode = rw["ProductCode"].ToString(),
                            ProductName = rw["ProductName"].ToString()
                        };
                        lists.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return lists;
        }

        public List<ProductModel> getProductByCode(string prdCode)
        {
            List<ProductModel> lists = new List<ProductModel>();
            try
            {
                string sql = $"SELECT * FROM Product WHERE ProductCode = '{prdCode}'";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, _con))
                {
                    DataTable tb = new DataTable();
                    da.Fill(tb);
                    if (tb.Rows.Count == 0)
                        return null;
                    foreach (DataRow rw in tb.Rows)
                    {
                        ProductModel model = new ProductModel
                        {
                            Id = int.Parse(rw["Id"].ToString()),
                            ProductCode = rw["ProductCode"].ToString(),
                            ProductName = rw["ProductName"].ToString()
                        };
                        lists.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return lists;
        }

        public List<ProductModel> getAllProduct()
        {
            List<ProductModel> lists = new List<ProductModel>();
            try
            {
                string sql = "SELECT * FROM Product";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, _con))
                {
                    DataTable tb = new DataTable();
                    da.Fill(tb);

                    if (tb.Rows.Count == 0)
                    {
                        return null;
                    }

                    foreach (DataRow dr in tb.Rows)
                    {
                        ProductModel model = new ProductModel
                        {
                            Id = int.Parse(dr["Id"].ToString()),
                            ProductCode = dr["ProductCode"].ToString(),
                            ProductName = dr["ProductName"].ToString(),
                        };
                        lists.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;

            }
            return lists;
        }

        public bool updateProduct(ProductModel model)
        {
            try
            {
                string sql = "UPDATE Product " +
                    "SET ProductName = @ProductName, " +
                    "ProductCode = @ProductCode " +
                    "WHERE Id = @Id";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@ProductName", model.ProductName));
                    cmd.Parameters.Add(new SQLiteParameter("@ProductCode", model.ProductCode));
                    cmd.Parameters.Add(new SQLiteParameter("@Id", model.Id));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return false;

            }
            return true;
        }

        public bool deleteProductById(int id)
        {

            try
            {
                string sql = $"DELETE FROM Product WHERE Id = {id}";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return false;

            }
            return true;
        }

    }
}
