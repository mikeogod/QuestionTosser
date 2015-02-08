﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using System.Data.Odbc;
using System.Configuration;

namespace SignalRChat
{
    public class ClassRoomHub : Hub
    {
        public override Task OnDisconnected(bool stopCalled)
        {
            using (OdbcConnection conn = new OdbcConnection(ConfigurationManager.ConnectionStrings["QuestionTosserMySQLDBConnection"].ConnectionString))
            {
                conn.Open();
                string sqlStr = "DELETE FROM `class` WHERE prof_connection_id=?";
                OdbcCommand comm = new OdbcCommand(sqlStr, conn);
                comm.Parameters.AddWithValue("profConnID", Context.ConnectionId);
                int rowsDeleted = comm.ExecuteNonQuery();

            }
            return base.OnDisconnected(stopCalled);
        }
        [HubMethodName("StartClass")]
        public Task StartClass(string classID)
        {
            try{
                using(OdbcConnection conn = new OdbcConnection(ConfigurationManager.ConnectionStrings["QuestionTosserMySQLDBConnection"].ConnectionString))
                {
                    conn.Open();
                    string sqlStr="Update `class` SET prof_connection_id=? WHERE id=?";
                    OdbcCommand comm=new OdbcCommand(sqlStr, conn);
                    comm.Parameters.AddWithValue("connectionId", Context.ConnectionId);
                    comm.Parameters.AddWithValue("classname", classID);
                    int rowsUpdated=comm.ExecuteNonQuery();
                    if(rowsUpdated==1)
                    {
                        return Clients.Client(Context.ConnectionId).getResponse("Class room started!");
                    }
                    else
                    {
                        return Clients.Client(Context.ConnectionId).getResponse("Class doesn't exist!");
                    }
                }
            }
            catch (OdbcException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return Clients.Client(Context.ConnectionId).getResponse("StartClass has encountered an exception!");
            }
        }

        [HubMethodName("Toss")]
        public void Toss(string name, string question, string profConnID)
        {
            Clients.Client(profConnID).getResponse("A student asked: "+question);
        }

    }
}