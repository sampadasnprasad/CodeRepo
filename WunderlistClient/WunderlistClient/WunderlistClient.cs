namespace WunderlistClient
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Wunderlist.DataModels;
    using System.Configuration;

    /// <summary>
    /// Definition for the Wunderlist Client
    /// </summary>
    public class WunderlistClient : IWunderlistClient
    {
        #region Properties
        /// <summary>
        /// OAuth access token for the client
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Wunderlist Client id
        /// </summary>
        public string ClientId { get; private set; }

        /// <summary>
        /// Wunderlist Client secret
        /// </summary>
        public string ClientSecret { get; private set; }

        /// <summary>
        /// Hoste name for Wunderlist API
        /// </summary>
        public string HostName { get; private set; }

        /// <summary>
        /// Default HTTP headers to use in all the requests
        /// </summary>
        public NameValueCollection RequestHeaders { get; private set; }
        #endregion

        public WunderlistClient(string accessToken)
        {
            this.AccessToken = accessToken;
            this.InitializeClient();
        }

        public void InitializeClient()
        {
            this.ClientId = ConfigurationManager.AppSettings["ClientId"];
            this.ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
            this.HostName = ConfigurationManager.AppSettings["HostName"];

            this.RequestHeaders = new NameValueCollection();
            this.RequestHeaders.Add("X-Client-ID", this.ClientId);
            this.RequestHeaders.Add("X-Access-Token", this.AccessToken);
        }

        #region List operations
        /// <summary>
        /// Get all Lists a user has permission to
        /// </summary>
        /// <returns></returns>
        public IEnumerable<List> GetLists()
        {
            var url = this.HostName + "/lists";
            IEnumerable<List> result = null;

            try
            {
                var response = HttpHelper.DoGET(url, null, this.RequestHeaders);
                result = JsonConvert.DeserializeObject<IEnumerable<List>>(response);
            }
            catch(Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        /// Get details of a specific list
        /// </summary>
        /// <returns></returns>
        public List GetList(long listId)
        {
            if(listId == null || listId <= 0)
            {
                throw new ArgumentException("Invalid list id");
            }

            var url = this.HostName + "/lists/" + listId;
            List result = null;

            try
            {
                var response = HttpHelper.DoGET(url, null, this.RequestHeaders);
                result = JsonConvert.DeserializeObject<List>(response);
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        /// Create a new Wunderlist list
        /// </summary>
        /// <returns></returns>
        public List CreateList(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Empty title");
            }

            var url = this.HostName + "/lists";
            List result = null;

            try
            {
                var postObject = new Dictionary<string,string>();
                postObject.Add("title", title);
                var response = HttpHelper.DoPOST(url, postObject, this.RequestHeaders);
                result = JsonConvert.DeserializeObject<List>(response);
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        /// Update the title of a list
        /// </summary>
        /// <returns></returns>
        public List UpdateListTitle(long listId, string newTitle)
        {
            return this.UpdateList(listId, "title", newTitle);
        }

        /// <summary>
        /// Make the specified list public
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public List MakeListPublic(long listId)
        {
            return this.UpdateList(listId, "public", true);
        }

        /// <summary>
        /// Update the details of a list
        /// </summary>
        /// <returns></returns>
        internal List UpdateList(long listId, string propertyKey, object propertyValue)
        {
            if (listId == null || listId <= 0 || string.IsNullOrWhiteSpace(propertyKey) || propertyValue == null)
            {
                throw new ArgumentException("Invalid arguments");
            }

            var currentList = this.GetList(listId);
            if(currentList == null)
            {
                throw new Exception("Given list doesn't exist");
            }

            var url = this.HostName + "/lists/" + listId;
            List result = null;

            try
            {
                var postObject = new Dictionary<string, object>();
                postObject.Add("revision", currentList.Revision);
                postObject.Add(propertyKey, propertyValue);
                var response = HttpHelper.DoPATCH(url, postObject, this.RequestHeaders);
                result = JsonConvert.DeserializeObject<List>(response);
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        /// Delete the list
        /// </summary>
        /// <returns></returns>
        public bool DeleteList(long listId)
        {
            if (listId == null || listId <= 0)
            {
                throw new ArgumentException("Invalid arguments");
            }

            var currentList = this.GetList(listId);
            if (currentList == null)
            {
                throw new Exception("Given list doesn't exist");
            }

            var url = this.HostName + "/lists/" + listId;
            var result = false;

            try
            {
                var queryParameters = new NameValueCollection();
                queryParameters.Add("revision", currentList.Revision.ToString());
                if (!string.IsNullOrWhiteSpace(HttpHelper.DoDELETE(url, queryParameters, this.RequestHeaders)))
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }
        #endregion

        #region Task operations
        /// <summary>
        /// Get all tasks for a list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Wunderlist.DataModels.Task> GetTasks(long listId, Wunderlist.DataModels.Task.TaskState taskState = Wunderlist.DataModels.Task.TaskState.Nothing)
        {
            if(listId == null || listId <= 0)
            {
                throw new ArgumentException("Invalid list_id");
            }

            var url = this.HostName + "/tasks";
            IEnumerable<Wunderlist.DataModels.Task> result = null;

            try
            {
                var queryParameters = new NameValueCollection();
                if (taskState != Wunderlist.DataModels.Task.TaskState.Nothing)
                {
                    queryParameters.Add("completed", (taskState == Wunderlist.DataModels.Task.TaskState.Completed) ? "true" : "false");
                }
                queryParameters.Add("list_id", listId.ToString());

                var response = HttpHelper.DoGET(url, queryParameters, this.RequestHeaders);
                result = JsonConvert.DeserializeObject<IEnumerable<Wunderlist.DataModels.Task>>(response);
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        /// Get details of a specific task
        /// </summary>
        /// <returns></returns>
        public Wunderlist.DataModels.Task GetTask(long taskId)
        {
            if (taskId == null || taskId <= 0)
            {
                throw new ArgumentException("Invalid task id");
            }

            var url = this.HostName + "/tasks/" + taskId;
            Wunderlist.DataModels.Task result = null;

            try
            {
                var response = HttpHelper.DoGET(url, null, this.RequestHeaders);
                result = JsonConvert.DeserializeObject<Wunderlist.DataModels.Task>(response);
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        /// Create a new Wunderlist task
        /// </summary>
        /// <returns></returns>
        public Wunderlist.DataModels.Task CreateTask(long listId, string title)
        {
            if (string.IsNullOrWhiteSpace(title) || listId == null || listId <= 0)
            {
                throw new ArgumentException("Invalid arguments");
            }

            var url = this.HostName + "/tasks";
            Wunderlist.DataModels.Task result = null;

            try
            {
                var postObject = new Dictionary<string, object>();
                postObject.Add("list_id", listId);
                postObject.Add("title", title);
                var response = HttpHelper.DoPOST(url, postObject, this.RequestHeaders);
                result = JsonConvert.DeserializeObject<Wunderlist.DataModels.Task>(response);
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        /// Update the details of a task
        /// </summary>
        /// <returns></returns>
        public Wunderlist.DataModels.Task UpdateTask(Wunderlist.DataModels.Task task)
        {
            if (task == null || task.Id <= 0)
            {
                throw new ArgumentException("Invalid arguments");
            }

            var currentTask = this.GetTask(task.Id);
            if (currentTask == null)
            {
                throw new Exception("Given task doesn't exist");
            }

            var url = this.HostName + "/tasks/" + task.Id;
            Wunderlist.DataModels.Task result = null;

            try
            {
                var postObject = new Dictionary<string, object>();
                postObject.Add("revision", currentTask.Revision);
                
                if( task.Title != currentTask.Title) postObject.Add("title", task.Title);
                if (task.AssigneeId > 0) postObject.Add("assignee_id", task.AssigneeId);
                if (task.AssignerId > 0) postObject.Add("assigner_id", task.AssignerId);
                if (!string.IsNullOrWhiteSpace(task.DueDate)) postObject.Add("due_date", task.DueDate);
                if (task.Starred) postObject.Add("starred", task.Starred);

                var response = HttpHelper.DoPATCH(url, postObject, this.RequestHeaders);
                result = JsonConvert.DeserializeObject<Wunderlist.DataModels.Task>(response);
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        /// Delete the task
        /// </summary>
        /// <returns></returns>
        public bool DeleteTask(long taskId)
        {
            if (taskId == null || taskId <= 0)
            {
                throw new ArgumentException("Invalid arguments");
            }

            var currentTask = this.GetTask(taskId);
            if (currentTask == null)
            {
                throw new Exception("Given list doesn't exist");
            }

            var url = this.HostName + "/tasks/" + taskId;
            var result = false;

            try
            {
                var queryParameters = new NameValueCollection();
                queryParameters.Add("revision", currentTask.Revision.ToString());
                if (!string.IsNullOrWhiteSpace(HttpHelper.DoDELETE(url, queryParameters, this.RequestHeaders)))
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }
        #endregion

        internal class HttpHelper
        {
            public static string DoGET(string url, NameValueCollection queryStringParameters = null, NameValueCollection requestHeaders = null)
            {
                return DoHttp("GET", url, null, queryStringParameters, requestHeaders);
            }

            public static string DoPOST(string url, object content, NameValueCollection requestHeaders = null)
            {
                return DoHttp("POST", url, content, null, requestHeaders);
            }

            public static string DoPATCH(string url, object content, NameValueCollection requestHeaders = null)
            {
                return DoHttp("PATCH", url, content, null, requestHeaders);
            }

            public static string DoDELETE(string url, NameValueCollection queryParameters, NameValueCollection requestHeaders = null)
            {
                return DoHttp("DELETE", url, null, queryParameters, requestHeaders);
            }

            private static string DoHttp(string method, string url, object content = null, NameValueCollection queryStringParameters = null, NameValueCollection requestHeaders = null)
            {
                string result = null;
                StringBuilder queryParameters = new StringBuilder();
                if (queryStringParameters != null && queryStringParameters.Count > 0)
                {
                    foreach (var param in queryStringParameters.AllKeys)
                        queryParameters.Append(string.Format("{0}={1}&", Uri.EscapeDataString(param), Uri.EscapeDataString(queryStringParameters[param])));
                }

                if (queryParameters.Length > 0)
                    url = url + "?" + queryParameters.ToString();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.ContentType = "application/json; charset=UTF-8";
                request.Accept = "application/json";

                if (requestHeaders != null && requestHeaders.Count > 0)
                {
                    foreach (var header in requestHeaders.AllKeys)
                        request.Headers.Add(header, requestHeaders[header]);
                }

                if (content != null)
                {
                    var json = JsonConvert.SerializeObject(content);
                    byte[] postBytes = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = postBytes.Length;

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(postBytes, 0, postBytes.Length);
                }

                var response = request.GetResponse() as HttpWebResponse;
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                        }
                        break;
                    case HttpStatusCode.NoContent:
                        result = "NoContent";
                        break;
                }

                return result;
            }
        }

    }
}
