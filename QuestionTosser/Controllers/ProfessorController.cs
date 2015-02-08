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
using System.Text;

namespace QuestionTosser.Controllers
{
    public class ProfessorController : Controller
    {
        [HttpPost]
        public JsonResult ProfessorRegister()
        {
            string pUName = Request.Form["username"];
            string pPass = Request.Form["password"];
            string pName = Request.Form["name"];
            byte[] salt;
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
                    salt = RandomHash.PasswordHash.RandomSalt(4, 8);
                    pPass = RandomHash.PasswordHash.ComputeHash(pPass, "SHA256", salt);
                    comm.Parameters.AddWithValue("password", pPass);
                    comm.Parameters.AddWithValue("salt", Convert.ToBase64String(salt));
                    int rowsAffected = comm.ExecuteNonQuery();
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
        }
        [HttpPost]
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
                        byte[] salt=Convert.FromBase64String((String)reader["salt"]);
                        pPass = RandomHash.PasswordHash.ComputeHash(pPass, "SHA256", salt);
                        if (((String)reader["password"]) == pPass)
                        {
                            Session.RemoveAll();
                            Session.Add("professor", new Dictionary<string, string>{ 
                                {"username", (string)reader["username"]},
                                {"name", (string)reader["name"]},
                                {"id", reader["id"].ToString()}
                            });
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
        [HttpPost]
        public JsonResult StartClass()
        {
            if (Session!=null && Session["professor"]!=null)
            {
                string pName = ((Dictionary<string, string>)(Session["professor"]))["name"];
                string pUName = ((Dictionary<string, string>)(Session["professor"]))["username"];
                string pID = ((Dictionary<string, string>)(Session["professor"]))["id"];
                string pCName = Request.Form["classname"];
                string cCode = Request.Form["code"];
                try
                {
                    using (OdbcConnection conn = new OdbcConnection(ConfigurationManager.ConnectionStrings["QuestionTosserMySQLDBConnection"].ConnectionString))
                    {
                        conn.Open();
                        
                        
                        //string sqlStr = "SELECT * FROM `class` WHERE prof_id=? && name=?";
                        //OdbcCommand comm = new OdbcCommand(sqlStr, conn);
                        //comm.Parameters.AddWithValue("profID", pID);
                        //comm.Parameters.AddWithValue("name", pName);
                        //OdbcDataReader reader = comm.ExecuteReader();
                        //if (reader.HasRows)
                        //{
                        //    reader.Read();
                        //    Int32 classID = (Int32)reader["class_id"];
                        //    comm.Parameters.Clear();
                        //    reader.Close();

                        //    sqlStr = "UPDATE TABLE `class` SET ongoing=1, code=? WHERE class_id=?";
                        //    comm.CommandText = sqlStr;
                        //    comm.Parameters.AddWithValue("classCode", cCode);
                        //    comm.Parameters.AddWithValue("classID", classID);

                        //    int rowsAffected = comm.ExecuteNonQuery();
                        //    if (rowsAffected == 1)
                        //    {
                        //        return Json(new
                        //        {
                        //            msg = "Success",
                        //            status = "StartClassSucceed",
                        //            classname= pCName, 
                        //            classID=classID, 
                        //            code = cCode
                        //        });
                        //    }
                        //    else
                        //    {
                        //        return Json(new { msg = "Didn't update", status = "StartClassFailUnknown" });
                        //    }
                        //}
                        //else
                        {
                            
                            //comm.Parameters.Clear();
                            //reader.Close();

                            
                            string sqlStr= "INSERT INTO `class`(name, prof_id, code) VALUES(?, ?, ?);";
                            OdbcCommand comm = new OdbcCommand(sqlStr, conn);

                            OdbcTransaction tran = conn.BeginTransaction();
                            comm.Transaction = tran;

                            comm.Parameters.AddWithValue("name", pCName);
                            comm.Parameters.AddWithValue("profID", pID);
                            comm.Parameters.AddWithValue("code", cCode);
                            OdbcDataReader reader=comm.ExecuteReader();

                            comm.Parameters.Clear();
                            reader.Close();

                            sqlStr = "SELECT LAST_INSERT_ID();";
                            comm.CommandText = sqlStr;

                            Int32 classID = Convert.ToInt32(comm.ExecuteScalar());

                            tran.Commit();

                            return Json(new 
                            { 
                                msg = "Success",
                                status = "StartClassSucceed",  
                                classname = pCName,
                                classID = classID,
                                code = cCode
                            });
                        }
                    }
                }
                catch (OdbcException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    return Json(new { msg = "Something about database went wrong", status="ClassStartFailDB"});
                }
            }
            else if (Session == null || Session["professor"] == null)
            {
                return Json(new { msg = "Professor not logged in", status = "StartClassPFailNotLoggedIn" });
            }
            else
            {
                return Json(new { msg = "Unknown state", status="Unknown" });
            }
        }


    }
}
