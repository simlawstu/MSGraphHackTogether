﻿namespace SendSummarizedEmailToTeams.MailRetrieval
{
    public class RetrievedMail
    {
        public string Id { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}