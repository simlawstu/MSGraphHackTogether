using Azure;
using Azure.Data.Tables;
using System;

namespace SummarizeEmailsAndSend.TableStorageEntities
{
    public class EmailToProcess : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        public string UserId { get; set; }
        public string TeamId { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
