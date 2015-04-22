using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using System.Data.Odbc;
using System.Configuration;
using System.Net;
using System.IO;

namespace SignalRChat
{
    public class ClassRoomHub : Hub
    {
        
        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            try
            {
                using (OdbcConnection conn = new OdbcConnection(ConfigurationManager.ConnectionStrings["QuestionTosserMySQLDBConnection"].ConnectionString))
                {
                    conn.Open();
                    string sqlStr = "DELETE FROM `class` WHERE prof_connection_id=?";
                    OdbcCommand comm = new OdbcCommand(sqlStr, conn);
                    comm.Parameters.AddWithValue("profConnID", Context.ConnectionId);
                    int rowsDeleted = comm.ExecuteNonQuery();

                }

            }
            catch (OdbcException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
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
                        return Clients.Client(Context.ConnectionId).postQuestion("Class room started!");
                    }
                    else
                    {
                        return Clients.Client(Context.ConnectionId).postQuestion("Class doesn't exist!");
                    }
                }
            }
            catch (OdbcException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return Clients.Client(Context.ConnectionId).postQuestion("Class failed to start!");
            }
        }

        [HubMethodName("Toss")]
        public void Toss(string question, string profConnID, string userName, bool anonymous=false)
        {

            if (ContainsProfanity(question))
            {
                Clients.Caller.Notification("Your question contains profanity language. Please be nice.");
                return;
            }
            
            if (anonymous)
            {
                userName = "Anonymous";
            }
            if (question == "" || profConnID == "" || userName == "")
            {
                if (question == "")
                {
                    Clients.Caller.Notification("You can't toss a empty question");
                }
                return;
            }
            Clients.Client(profConnID).postQuestion(userName + " asked: " + question);
        }

        [HubMethodName("DisconnectClient")]
        public void DisconnectClient()
        {
            Clients.Caller.stopConnection();
        }

        private bool ContainsProfanity(String question)
        {
            //Use PurgoMalum web service
            bool webFailed = false;
            try
            {
                var url = "http://www.purgomalum.com/service/containsprofanity?text={0}";
                WebClient client = new WebClient();
                Stream data = client.OpenRead(String.Format(url, question));
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd().Trim();
                data.Close();
                reader.Close();
                return s=="true"; 
            }
            catch (WebException e)
            {
                //Web service call failed. Choose alternative local service instead
                webFailed = true;
            }
            if (webFailed)
            {
                //Use local filter
                List<String> badWords = new List<string>();
                badWords.Add("ass");
                badWords.Add("asshole");
                badWords.Add("ass hole");
                badWords.Add("bitch");
                badWords.Add("shit");
                badWords.Add("crap");
                badWords.Add("damn");
                badWords.Add("fuck");
                badWords.Add("fucker");
                badWords.Add("fucking");
                badWords.Add("motherfucker");
                badWords.Add("hell");
                badWords.Add("suck");
                badWords.Add("sucks");
                badWords.Add("sucker");
                badWords.Add("wtf");

                string[] words_array = question.Split(' ');

                foreach (string word in badWords)
                {
                    for (int i = 0; i < words_array.Length; i++)
                    {
                        bool if_equal = false;
                        if_equal = words_array[i].Equals(word, StringComparison.OrdinalIgnoreCase);
                        if (if_equal == true)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            return false;
        }
    }
}
