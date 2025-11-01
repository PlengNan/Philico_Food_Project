using Project_philico_food.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Web.UI.WebControls;

namespace Project_philico_food.Db
{
    internal class UsersDb
    {
        private readonly SQLiteConnection _con;
        public string Err { get; private set; }

        public UsersDb()
        {
            _con = DbConnect.SQLiteConnection;
            if (_con.State != System.Data.ConnectionState.Open) _con.Open();

         
        }
        public List<UsersModel> GetAll()
        {
            List<UsersModel> lists = new List<UsersModel>();
            try
            {
                string sql = "SELECT * FROM  Users ORDER BY Id DESC";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, _con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        return null;
                    }
                    foreach (DataRow r in dt.Rows)
                    {
                        UsersModel model = new UsersModel
                        {
                            Id = Convert.ToInt32(r["Id"]),
                            Name = r["Name"]?.ToString(),
                            Email = r["Email"]?.ToString(),
                            Phone = r["Phone"]?.ToString(),
                            Username = r["Username"]?.ToString(),
                            Password = r["Password"]?.ToString()
                        };
                        lists.Add(model);
                    }
                
                }
            }
            catch (Exception ex) { Err = ex.Message; return null; }
            return lists;
        }

        public List<UsersModel> SearchExactEncrypted(string nameEnc, string usernameEnc)
        {
            List<UsersModel> lists = new List<UsersModel>();
            try
            {
                const string sql = @"
                    SELECT Id, Name, Email, Phone, Username, Password
                    FROM Users
                    WHERE (@n IS NULL OR Name = @n)
                      AND (@u IS NULL OR Username = @u)
                    ORDER BY Id DESC";
                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.AddWithValue("@n", (object)nameEnc ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@u", (object)usernameEnc ?? DBNull.Value);

                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, _con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count == 0)
                            return null;
                        foreach (DataRow r in dt.Rows)
                        {
                            UsersModel model = new UsersModel
                            {
                                Id = Convert.ToInt32(r["Id"]),
                                Name = r["Name"]?.ToString(),
                                Email = r["Email"]?.ToString(),
                                Phone = r["Phone"]?.ToString(),
                                Username = r["Username"]?.ToString(),
                                Password = r["Password"]?.ToString()
                            };
                            lists.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex) { Err = ex.Message; return null; }
            return lists;
        }
        public bool Add(UsersModel m)
        {
            try
            {
                const string sql = @"INSERT INTO Users(Name, Email, Phone, Username, Password, IsActive)
                                     VALUES(@Name,@Email,@Phone,@Username,@Password,1)";
                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.AddWithValue("@Name", m.Name);
                    cmd.Parameters.AddWithValue("@Email", m.Email);
                    cmd.Parameters.AddWithValue("@Phone", m.Phone);
                    cmd.Parameters.AddWithValue("@Username", m.Username);
                    cmd.Parameters.AddWithValue("@Password", m.Password);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { Err = ex.Message; return false; }
        }

        public bool Update(UsersModel m)
        {
            try
            {
                const string sql = @"UPDATE Users
                                     SET Name=@Name, Email=@Email, Phone=@Phone, Username=@Username
                                     WHERE Id=@Id";
                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.AddWithValue("@Name", m.Name);
                    cmd.Parameters.AddWithValue("@Email", m.Email);
                    cmd.Parameters.AddWithValue("@Phone", m.Phone);
                    cmd.Parameters.AddWithValue("@Username", m.Username);
                    cmd.Parameters.AddWithValue("@Id", m.Id);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { Err = ex.Message; return false; }
        }

        public bool UpdateWithPassword(UsersModel m, string passwordEnc)
        {
            try
            {
                const string sql = @"UPDATE Users
                                     SET Name=@Name, Email=@Email, Phone=@Phone, Username=@Username, Password=@Password
                                     WHERE Id=@Id";
                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.AddWithValue("@Name", m.Name);
                    cmd.Parameters.AddWithValue("@Email", m.Email);
                    cmd.Parameters.AddWithValue("@Phone", m.Phone);
                    cmd.Parameters.AddWithValue("@Username", m.Username);
                    cmd.Parameters.AddWithValue("@Password", passwordEnc);
                    cmd.Parameters.AddWithValue("@Id", m.Id);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { Err = ex.Message; return false; }
        }

        public bool Delete(int id)
        {
            try
            {
                using (var cmd = new SQLiteCommand("DELETE FROM Users WHERE Id=@Id", _con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { Err = ex.Message; return false; }
        }

        public bool VerifyLogin(string usernameEnc, string passwordEnc, out UsersModel user)
        {
            user = null;
            try
            {
                const string sql = @"SELECT Id, Name, Email, Phone, Username, Password, IsActive
                                     FROM Users WHERE Username=@u LIMIT 1;";
                using (var cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.Parameters.AddWithValue("@u", usernameEnc);
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (!rd.Read()) { Err = "Invalid username or password."; return false; }
                        if (Convert.ToInt32(rd["IsActive"]) == 0) { Err = "This user is inactive."; return false; }

                        if (!string.Equals(passwordEnc, rd["Password"]?.ToString()))
                        { Err = "Invalid username or password."; return false; }

                        user = new UsersModel
                        {
                            Id = Convert.ToInt32(rd["Id"]),
                            Name = rd["Name"]?.ToString(),
                            Email = rd["Email"]?.ToString(),
                            Phone = rd["Phone"]?.ToString(),
                            Username = rd["Username"]?.ToString(),
                            Password = null
                        };
                        return true;
                    }
                }
            }
            catch (Exception ex) { Err = ex.Message; return false; }
        }
    }
}
