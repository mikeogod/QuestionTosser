using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRChat
{
    public class ClassRoomHub : Hub
    {
        [HubMethodName("JoinAsStudent")]
        public Task JoinAsStudent()
        {
            return Groups.Add(Context.ConnectionId, "Students");
        }

        [HubMethodName("JoinAsProfessor")]
        public Task JoinAsProfessor()
        {
            return Groups.Add(Context.ConnectionId, "Professor");
        }

        [HubMethodName("Toss")]
        public void Toss(string name, string question)
        {
            Clients.Group("Professor").getResponse("A student asked: "+question);
            
        }

    }
}
