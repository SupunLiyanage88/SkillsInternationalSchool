using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SkillsInternationalSchool
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["SchoolDb"].ConnectionString;
        }

        public DataTable GetStudents()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    string query = "SELECT regNo, firstName, lastName, email, mobilePhone FROM Registration ORDER BY regNo";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving students: {ex.Message}");
                }
            }
        }

        public DataTable SearchStudentsByName(string searchTerm)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    string query = @"SELECT regNo, firstName, lastName, email, mobilePhone FROM Registration 
                                   WHERE firstName LIKE @search OR lastName LIKE @search 
                                   ORDER BY firstName, lastName";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                    
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error searching students: {ex.Message}");
                }
            }
        }

        public DataTable GetStudentDetails(int regNo)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    string query = "SELECT * FROM Registration WHERE regNo = @regNo";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@regNo", regNo);
                    
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving student details: {ex.Message}");
                }
            }
        }

        public int GetTotalStudents()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    string query = "SELECT COUNT(*) FROM Registration";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error getting total students: {ex.Message}");
                }
            }
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
