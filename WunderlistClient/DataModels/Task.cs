namespace Wunderlist.DataModels
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Definition for Wunderlist Task
    /// </summary>
    public class Task
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "assignee_id")]
        public long AssigneeId { get; set; }

        [JsonProperty(PropertyName = "assigner_id")]
        public long AssignerId { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "created_by_id")]
        public long CreatedById{ get; set; }

        [JsonProperty(PropertyName = "due_date")]
        public string DueDate { get; set; }

        [JsonProperty(PropertyName = "list_id")]
        public long ListId { get; set; }

        [JsonProperty(PropertyName = "revision")]
        public int Revision { get; set; }

        [JsonProperty(PropertyName = "starred")]
        public bool Starred { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "completed_at")]
        public string CompletedAt { get; set; }

        [JsonProperty(PropertyName = "completed_by_id")]
        public string CompletedBy { get; set; }

        public enum RecurrenceType
        {
            Nothing=0,
            Day = 1,
            Week = 2,
            Month = 3,
            Year = 4,
        };

        public enum TaskState
        {
            Nothing = 0,
            Completed = 1,
            Pending = 2,
        };
    }
}
