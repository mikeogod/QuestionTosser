using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Dynamic;

namespace QuestionTosser.Controllers
{
    public class ProfessorController : Controller
    {
        public JsonResult ProfessorRegister()
        {
            string pUName = Request.Form["username"];
            string pPass = Request.Form["password"];
            string pName = Request.Form["name"];
            string salt = "salt";
            try
            {
                using (OdbcConnection conn = new OdbcConnection(ConfigurationManager.ConnectionStrings["QuestionTosserMySQLDBConnection"].ConnectionString))
                {
                    conn.Open();
                    string sqlStr = "SELECT * FROM `professor` WHERE `username` = ?";
                    OdbcCommand comm = new OdbcCommand(sqlStr, conn);
                    comm.Parameters.AddWithValue("username", pUName);
                    OdbcDataReader reader = comm.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return Json(new { msg = "This username already exists", status = "FailUserExists" });
                    }
                    reader.Close();
                    comm.Parameters.Clear();
                    sqlStr = "INSERT INTO `professor`(username, name, password, salt) VALUES(?, ?, ?, ?)";
                    comm.CommandText = sqlStr;

                    comm.Parameters.AddWithValue("username", pUName);
                    comm.Parameters.AddWithValue("name", pName);
                    comm.Parameters.AddWithValue("password", pPass);
                    comm.Parameters.AddWithValue("salt", salt);
                    int rowsAffected = comm.ExecuteNonQuery();
                    System.Diagnostics.Debug.WriteLine(rowsAffected.ToString() + " inserted");
                    if (rowsAffected == 1)
                    {
                        return Json(new { msg = "Success", status = "RegisterPSucceed" });
                    }
                    else
                    {
                        return Json(new { msg = "Didn't insert", status = "RegisterPFailUnknown" });
                    }
                }
            }
            catch (OdbcException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return Json(new { msg = "Something about database went wrong", status = "RegisterPFailDB" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return Json(new { msg = "Something went wrong", status = "RegisterPFailGen" });
            }
            return Json(new { msg = "Not implemented" });
        }
        public JsonResult ProfessorLogin()
        {

            string pName = Request.Form["username"];
            string pPass = Request.Form["password"];
            try
            {

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["QuestionTosserMySQLDBConnection"].ConnectionString))
                {
                    connection.Open();
                    string sqlStr = "SELECT * FROM `professor` WHERE `username` = ?;";
                    OdbcCommand command = new OdbcCommand(sqlStr, connection);
                    command.Parameters.AddWithValue("username", pName);

                    //nameParam.Value = pName;
                    OdbcDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (((String)reader["password"]) == pPass)
                        {
                            Session.RemoveAll();
                            Session.Add("professor", new Dictionary<string, string>{ {"username", (string)reader["username"]}});
                            return Json(new { msg = "Success!", status = "LoginPSuceed" });
                        }
                        else
                        {
                            return Json(new { msg = "Pass no match!", status = "LoginPFailPass" });
                        }
                    }
                    else
                    {
                        return Json(new { msg = "No found!", status = "LoginPFailUserName" });
                    }
                }
            }
            catch (OdbcException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return Json(new { msg = "Something about database went wrong", status = "LoginPFailDB" });
            }
            //string connStr = ConfigurationManager.ConnectionStrings["QuestionTosserDBConnection"].ConnectionString;
            //SqlConnection conn=new SqlConnection(connStr);
            //SqlCommand command=new SqlCommand();
            //command.Connection = conn;
            //command.CommandType = CommandType.Text;
            //command.CommandText = "SELECT * FROM Class";

            //conn.Open();
            //SqlDataReader reader=command.ExecuteReader();

            //int c=reader.FieldCount;
            //System.Diagnostics.Debug.WriteLine("Column number: "+c.ToString());
        }

        public JsonResult StartClass()
        {
            if (Session!=null && Session["professor"]!=null)
            {
                string pName = ((Dictionary<string, string>)(Session["professor"]))["name"];
                string pUName = ((Dictionary<string, string>)(Session["professor"]))["username"];
                string pCName = Request.Form["classname"];
                try
                {
                    using (OdbcConnection conn = new OdbcConnection(ConfigurationManager.ConnectionStrings["QuestionTosserMySQLDBConnection"].ConnectionString))
                    {
                        string sqlStr = "SELECT * FROM `class` WHERE prof_id=? && name=?";
                        
                    }
                }
                catch (OdbcException e)
                {
                    
                
                }
                return Json(new { msg = "Success", status="StartClassPSucceed" });
            }
            else if(Session==null)
            {
                return Json(new { msg="Professor not logged in", status="StartClassPFailNotLoggedIn"});
            }
            else if (Session["student"] != null)
            {
                return Json(new { msg = "Professor not logged in", status = "StartClassPFailNotLoggedIn" });
            }

            //string profName=Session["professor"];
            
            return Json(new { msg="NotImplemented"});
        }

    }
}
