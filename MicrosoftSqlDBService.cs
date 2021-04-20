using SimpleRestApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SimpleRestApi
{
    public class MicrosoftSqlDBService
    {
        SqlConnection _connection;
        private static MicrosoftSqlDBService instance = null;

        public MicrosoftSqlDBService()
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);
            CreateDataBaseSeed();
        }
        #region Seed Data Base Methods
        private void CreateDataBaseSeed()
        {
            _connection.Open();
            try
            {
                DropTables();
                CreateTables();
                CreateProjectDB();
                CreateUsersDB();
            }
            catch (SqlException e){
                Console.WriteLine(e.Message);
            }
            _connection.Close();
        }

        private void CreateUsersDB()
        {
            string cmdText = "INSERT INTO UsersTable (ID,FullName, Password,Token) VALUES('1','Lior Turgeman','123456','TGlvciBUdXJnZW1hbjoxMjM0NTY=')";
            SqlCommand cmd = new SqlCommand(cmdText, _connection);
            cmd.ExecuteNonQuery();
            cmdText = "INSERT INTO UsersTable (ID,FullName, Password,Token) VALUES('2','Jacob Levi','456789','SmFjb2IgTGV2aTo0NTY3ODk=')";
            cmd = new SqlCommand(cmdText, _connection);
            cmd.ExecuteNonQuery();
            cmdText = "INSERT INTO UsersTable (ID,FullName, Password,Token) VALUES('3','Avi Cohen','147258','QXZpIENvaGVuOjE0NzI1OA==')";
            cmd = new SqlCommand(cmdText, _connection);
            cmd.ExecuteNonQuery();
            cmdText = "INSERT INTO UsersTable (ID,FullName, Password,Token) VALUES('4','Ron Perets','963258','Um9uIFBlcmV0czo5NjMyNTg=')";
            cmd = new SqlCommand(cmdText, _connection);
            cmd.ExecuteNonQuery();

        }

        private void DropTables()
        {
            string cmdText = "DROP TABLE IF EXISTS ProjectsTable";
            SqlCommand cmd = new SqlCommand(cmdText, _connection);
            cmd.ExecuteNonQuery();

            cmdText = "DROP TABLE IF EXISTS UsersTable";
            cmd = new SqlCommand(cmdText, _connection);
            cmd.ExecuteNonQuery();
        }

        private void CreateTables()
        {
            SqlCommand cmd = new SqlCommand("CREATE TABLE ProjectsTable(ID int primary key,ProjectName char(100),ReferTo char(100));", _connection);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("CREATE TABLE UsersTable(ID int primary key,FullName char(100),Password char(100),Token char(100));", _connection);
            cmd.ExecuteNonQuery();
        }

        private void CreateProjectDB()
        {
            string cmdText = "INSERT INTO ProjectsTable (ID,ProjectName, ReferTo) VALUES('1','Microsoft','Avi Cohen')"; ;
            SqlCommand cmd = new SqlCommand(cmdText, _connection);
            cmd.ExecuteNonQuery();

            cmdText = "INSERT INTO ProjectsTable (ID,ProjectName, ReferTo) VALUES('2','Apple','Avi Cohen')"; ;
            cmd = new SqlCommand(cmdText, _connection);
            cmd.ExecuteNonQuery();

            cmdText = "INSERT INTO ProjectsTable (ID,ProjectName, ReferTo) VALUES('3','Google','Lior Turgeman')"; ;
            cmd = new SqlCommand(cmdText, _connection);
            cmd.ExecuteNonQuery();
            cmdText = "INSERT INTO ProjectsTable (ID,ProjectName, ReferTo) VALUES('4','Avatar','Jacob Levi')"; ;
            cmd = new SqlCommand(cmdText, _connection);
            cmd.ExecuteNonQuery();
        }

        #endregion

        public static MicrosoftSqlDBService getInstance()
        {
            return instance == null ? new MicrosoftSqlDBService() : instance; 
        }

        public List<ProjectInfo> getUserProjects(string user)
        {
            List<ProjectInfo> projectList = new List<ProjectInfo>();
            _connection.Open();

            string command = "SELECT * FROM ProjectsTable WHERE ReferTo= '" + user + "'";
            SqlDataAdapter da = new SqlDataAdapter(command, _connection);
            DataSet ds = new DataSet();
            da.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var id = (int)row["ID"];
                var projectName = ((string)row["ProjectName"]).Trim();
                var ReferTo = ((string)row["ReferTo"]).Trim();
                Console.WriteLine("ID: " + id + "\n Project name: " + projectName + "\n ReferTo: " + ReferTo);
                projectList.Add(new ProjectInfo(id, projectName, ReferTo));
            }
            _connection.Close();
            return projectList;
        }

        public UserInfo getUserInfo(string token)
        {
            UserInfo userInfo= null;
            _connection.Open();
            string command = "SELECT * FROM UsersTable WHERE Token= '" + token + "'";
            SqlDataAdapter da = new SqlDataAdapter(command, _connection);
            DataSet ds = new DataSet();
            da.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var FullName = ((string)row["FullName"]).Trim();
                var Password = ((string)row["Password"]).Trim();
                var serverToken = ((string)row["Token"]).Trim();
                userInfo = new UserInfo(FullName, Password, serverToken);
            }
            _connection.Close();
            return userInfo;
        }
    }
}
