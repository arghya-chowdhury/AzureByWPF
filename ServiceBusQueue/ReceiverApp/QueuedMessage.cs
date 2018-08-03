using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus.Messaging;

namespace ReceiverApp
{
    public class QueuedMessage
    {
        public QueuedMessage(BrokeredMessage msg)
        {
            Body = msg.GetBody<string>();
            ContentType = msg.ContentType;
            CorrelationId = msg.CorrelationId;
            DeadLetterSource = msg.DeadLetterSource;
            DeliveryCount = msg.DeliveryCount;
            EnqueuedSequenceNumber = msg.EnqueuedSequenceNumber;
            EnqueuedTimeUtc = msg.EnqueuedTimeUtc;
            ExpiresAtUtc = msg.ExpiresAtUtc;
            ForcePersistence = msg.ForcePersistence;
            IsBodyConsumed = msg.IsBodyConsumed;
            Label = msg.Label;
            LockedUntilUtc = msg.LockedUntilUtc;
            LockToken = msg.LockToken;
            MessageId = msg.MessageId;
            PartitionKey = msg.PartitionKey;
            Properties = msg.Properties.Flatten();
            ReplyTo = msg.ReplyTo;
            ReplyToSessionId = msg.ReplyToSessionId;
            ScheduledEnqueueTimeUtc = msg.ScheduledEnqueueTimeUtc;
            SequenceNumber = msg.SequenceNumber;
            SessionId = msg.SessionId;
            Size = msg.Size;
            State = msg.State;
            TimeToLive = msg.TimeToLive;
            To = msg.To;
            ViaPartitionKey = msg.ViaPartitionKey;
        }

        public string Body { get; set; }

        public string ContentType { get; set; }

        public string CorrelationId { get; set; }

        public string DeadLetterSource { get; set; }

        public int DeliveryCount { get; set; }

        public long EnqueuedSequenceNumber { get; set; }

        public DateTime EnqueuedTimeUtc { get; set; }

        public DateTime ExpiresAtUtc { get; set; }

        public bool ForcePersistence { get; set; }

        public bool IsBodyConsumed { get; set; }

        public string Label { get; set; }

        public DateTime LockedUntilUtc { get; set; }

        public Guid LockToken { get; set; }

        public string MessageId { get; set; }

        public string PartitionKey { get; set; }

        public string Properties { get; set; }

        public string ReplyTo { get; set; }

        public string ReplyToSessionId { get; set; }

        public DateTime ScheduledEnqueueTimeUtc { get; set; }

        public long SequenceNumber { get; set; }

        public string SessionId { get; set; }

        public long Size { get; set; }

        public MessageState State { get; set; }

        public TimeSpan TimeToLive { get; set; }

        public string To { get; set; }

        public string ViaPartitionKey { get; set; }
    }

    public static class ExtendedKeyValuePairCollection
    {
        public static string Flatten(this IDictionary<string, object> properties)
        {
            var strBldr = new StringBuilder();
            properties.ToList().ForEach(p => strBldr.AppendFormat("{0}:{1}\n", p.Key, p.Value.ToString()));
            return strBldr.ToString();
        }
    }

}
