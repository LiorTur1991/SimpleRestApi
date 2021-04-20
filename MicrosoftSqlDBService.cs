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
        }
        public static MicrosoftSqlDBService getInstance()
        {
            if (instance == null)
            {
                instance = new MicrosoftSqlDBService();
            }
            return instance;
        }

        public List<ProjectInfo> getUserProjects(string user)
        {
            List<ProjectInfo> projectList = new List<ProjectInfo>();
            _connection.Open();

            string command = "SELECT * FROM ProjectsTable WHERE ReferTo= '"+user+"'";
            SqlDataAdapter da = new SqlDataAdapter(command, _connection);
            DataSet ds = new DataSet();
            da.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var id = (int)row["ID"];
                var projectName = (string)row["ProjectName"];
                var ReferTo = (string)row["ReferTo"];
                Console.WriteLine("ID: " + id + "\n Project name: " + projectName + "\n ReferTo: " + ReferTo);
                projectList.Add(new ProjectInfo(id, projectName, ReferTo));
            }
            _connection.Close();
            return projectList;
        }
    }
}
