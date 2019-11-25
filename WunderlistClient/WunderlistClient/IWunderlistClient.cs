namespace WunderlistClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Wunderlist.DataModels;

    /// <summary>
    /// Interface for the Wunderlist Client
    /// </summary>
    public interface IWunderlistClient
    {
#region List operations
        /// <summary>
        /// Get all Lists a user has permission to
        /// </summary>
        /// <returns></returns>
        IEnumerable<List> GetLists();

        /// <summary>
        /// Get details of a specific list
        /// </summary>
        /// <returns></returns>
        List GetList(long listId);

        /// <summary>
        /// Create a new Wunderlist list
        /// </summary>
        /// <returns></returns>
        List CreateList(string title);

        /// <summary>
        /// Update the title of a list
        /// </summary>
        /// <returns></returns>
        List UpdateListTitle(long listId, string newTitle);

        /// <summary>
        /// Make the specified list public
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        List MakeListPublic(long listId);

        /// <summary>
        /// Delete the list
        /// </summary>
        /// <returns></returns>
        bool DeleteList(long listId);
#endregion

#region Task operations
        /// <summary>
        /// Get all tasks for a list
        /// </summary>
        /// <returns></returns>
        IEnumerable<Wunderlist.DataModels.Task> GetTasks(long listId, Wunderlist.DataModels.Task.TaskState taskState = Wunderlist.DataModels.Task.TaskState.Nothing);

        /// <summary>
        /// Get details of a specific task
        /// </summary>
        /// <returns></returns>
        Wunderlist.DataModels.Task GetTask(long taskId);

        /// <summary>
        /// Create a new Wunderlist task
        /// </summary>
        /// <returns></returns>
        Wunderlist.DataModels.Task CreateTask(long listId, string title);

        /// <summary>
        /// Update the details of a task
        /// </summary>
        /// <returns></returns>
        Wunderlist.DataModels.Task UpdateTask(Wunderlist.DataModels.Task task);

        /// <summary>
        /// Delete the task
        /// </summary>
        /// <returns></returns>
        bool DeleteTask(long taskId);
#endregion

    }
}
