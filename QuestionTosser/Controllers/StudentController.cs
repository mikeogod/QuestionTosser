﻿using System;
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
    public class StudentController : Controller
    {
        [HttpPost]
        public JsonResult StudentRegister()
        {
            
            string sUName = Request.Form["username"];
            string sPass = Request.Form["password"];
            if (sUName == String.Empty || sPass == String.Empty)
            {
                return Json(new { msg = "Invalid input", status = "RegisterSFailInvalidInput" });
            }
            byte[] salt;
            try
            {
                using (OdbcConnection conn = new OdbcConnection(ConfigurationManager.ConnectionStrings["QuestionTosserMySQLDBConnection"].ConnectionString))
                {
                    conn.Open();
                    string sqlStr = "SELECT * FROM `student` WHERE `username` = ?";
                    OdbcCommand comm = new OdbcCommand(sqlStr, conn);
                    comm.Parameters.AddWithValue("username", sUName);
                    OdbcDataReader reader = comm.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return Json(new { msg = "This username already exists", status = "RegisterSFailUserExists" });
                    }
                    reader.Close();
                    comm.Parameters.Clear();
                    sqlStr = "INSERT INTO `student`(username, password, salt) VALUES(?, ?, ?)";
                    comm.CommandText = sqlStr;

                    comm.Parameters.AddWithValue("username", sUName);
                    salt = RandomHash.PasswordHash.RandomSalt(4, 8);
                    sPass = RandomHash.PasswordHash.ComputeHash(sPass, "SHA256", salt);
                    comm.Parameters.AddWithValue("password", sPass);
                    comm.Parameters.AddWithValue("salt", Convert.ToBase64String(salt));
                    int rowsAffected = comm.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        return Json(new { msg = "Success", status = "RegisterSSucceed" });
                    }
                    else
                    {
                        return Json(new { msg = "Didn't insert", status = "RegisterSFailUnknown" });
                    }
                }
            }
            catch (OdbcException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return Json(new { msg = "Something about database went wrong", status = "RegisterSFailDB" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return Json(new { msg = "Something went wrong", status = "RegisterSFailGen" });
            }
        }

        [HttpPost]
        public JsonResult StudentLogin()
        {
            string sUName = Request.Form["username"];
            string sPass = Request.Form["password"];
            if (sUName == String.Empty || sPass == String.Empty)
            {
                return Json(new { msg = "Invalid input", status = "LoginSFailInvalidInput" });
            }
            try
            {

                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["QuestionTosserMySQLDBConnection"].ConnectionString))
                {
                    connection.Open();
                    string sqlStr = "SELECT * FROM `student` WHERE `username` = ?;";
                    OdbcCommand command = new OdbcCommand(sqlStr, connection);
                    command.Parameters.AddWithValue("username", sUName);

                    //nameParam.Value = pName;
                    OdbcDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        byte[] salt = Convert.FromBase64String((String)reader["salt"]);
                        sPass = RandomHash.PasswordHash.ComputeHash(sPass, "SHA256", salt);
                        if (((String)reader["password"]) == sPass)
                        {
                            Session.RemoveAll();
                            var student=new Dictionary<string, string>{ 
                                {"username", (string)reader["username"]},
                                {"id", reader["id"].ToString()}
                            };
                            Session.Add("student", student);
                            return Json(new { 
                                msg = "Success!", 
                                status = "LoginSSucceed",
                                user = student
                            });
                        }
                        else
                        {
                            return Json(new { msg = "Pass no match!", status = "LoginSFailPass" });
                        }
                    }
                    else
                    {
                        return Json(new { msg = "No found!", status = "LoginSFailUserName" });
                    }
                }
            }
            catch (OdbcException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return Json(new { msg = "Something about database went wrong", status = "LoginSFailDB" });
            }
        }

        [HttpPost]
        public JsonResult JoinClass()
        {

            if (Session != null && Session["student"] != null)
            {
                string code = (String)(Request.Form["code"]);
                if (code == String.Empty)
                {
                    return Json(new { msg = "Invalid input", status = "JoinClassFailInvalidInput" });
                }
                try
                {
                    using (OdbcConnection conn = new OdbcConnection(ConfigurationManager.ConnectionStrings["QuestionTosserMySQLDBConnection"].ConnectionString))
                    {
                        conn.Open();
                        string sqlStr = "SELECT prof_connection_id, name FROM `class` WHERE `code`= ?";
                        OdbcCommand comm = new OdbcCommand(sqlStr, conn);
                        comm.Parameters.AddWithValue("code", code);
                        OdbcDataReader reader = comm.ExecuteReader();
                        if (reader.HasRows)
                        {

                            reader.Read();
                            return Json(new { 
                                msg = "Join succeed", 
                                status = "JoinClassSucceed", 
                                profConnID = (String)reader["prof_connection_id"], 
                                classname= (String)reader["name"]
                            });
                        }
                        else
                        {
                            return Json(new { msg = "Wrong code", status = "JoinClassFailWrongCode" });
                        }
                    }
                }
                catch (OdbcException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    return Json(new { msg = "Something about database went wrong", status = "JoinClassFailDB" });
                }
            }
            else if (Session == null || Session["student"] == null)
            {
                return Json(new { msg = "Student not logged in", status = "JoinClassFailNotLoggedIn" });
            }
            else
            {
                return Json(new { msg = "Unknown state", status = "Unknown" });
            }
        }
    }
}
