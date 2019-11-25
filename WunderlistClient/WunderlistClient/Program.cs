using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WunderlistClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var accessToken = "14d7d670baee90ecb353cfb9832d3c1b11174f10667e74642e6c270483f6";
                IWunderlistClient client = new WunderlistClient(accessToken);
                
                // client.GetLists();

                // client.GetList(165035214);

                // client.CreateList("ClientTest");

                // client.UpdateListTitle(165035214, "Client list 1");

                // client.MakeListPublic(165035214);

                // client.DeleteList(165035214);

                // client.CreateTask(165147079, "ClientTest");

                // var newTask = new Wunderlist.DataModels.Task() { Id = 1213675005, Title = "Client Task 1", Starred = true };
                // client.UpdateTask(newTask);

                // client.CreateTask(165147079, "Client Task 3");

                // client.GetTasks(165147079);

                // client.GetTasks(165147079, Wunderlist.DataModels.Task.TaskState.Pending);

                // client.GetTasks(165147079, Wunderlist.DataModels.Task.TaskState.Completed);

                client.DeleteTask(1213761951);
                
            }
            catch(Exception)
            { }
        }
    }
}
