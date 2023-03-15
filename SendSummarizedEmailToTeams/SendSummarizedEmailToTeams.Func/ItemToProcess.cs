using System;

using Azure;
using Azure.Data.Tables;

namespace SendSummarizedEmailToTeams.Func
{
    public class ItemToProcess : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string UserId { get; set; }
        public string TeamId { get; set; }
        public string ChannelId { get; set; }
        public string TenantId { get; set; }
    }
}