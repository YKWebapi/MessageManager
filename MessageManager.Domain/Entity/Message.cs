﻿/**
* author:xishuai
* address:https://www.github.com/yuezhongxin/MessageManager
**/

using MessageManager.Domain.ValueObject;
using System;

namespace MessageManager.Domain.Entity
{
    public class Message : IAggregateRoot
    {
        public Message(string title, string content, Contact sender, Contact recipient)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("title can't be null");
            }
            if (title.Length > 20)
            {
                throw new ArgumentException("标题长度不能超过20");
            }
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("content can't be null");
            }
            if (content.Length > 400)
            {
                throw new ArgumentException("内容长度不能超过400");
            }
            if (sender == null)
            {
                throw new ArgumentException("sender can't be null");
            }
            if (recipient == null)
            {
                throw new ArgumentException("recipient can't be null");
            }
            this.ID = Guid.NewGuid().ToString();
            this.Title = title;
            this.Content = content;
            this.SendTime = DateTime.Now;
            this.State = MessageState.Unread;
            this.DisplayType = MessageDisplayType.OutboxAndInbox;
            this.Sender = sender;
            this.Recipient = recipient;
        }
        public string ID { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTime SendTime { get; private set; }
        public MessageState State { get; private set; }
        public MessageDisplayType DisplayType { get; private set; }
        public virtual Contact Sender { get; private set; }
        public virtual Contact Recipient { get; private set; }

        public void SetState(Contact reader)
        {
            if (this.Recipient.Name.Equals(reader.Name) && this.State == MessageState.Unread)
            {
                this.State = MessageState.Read;
            }
        }

        public bool SetDisplayType(Contact reader)
        {
            // to do...
            switch (this.DisplayType)
            {
                case MessageDisplayType.OutboxAndInbox:
                    if (this.Sender.Name.Equals(reader.Name))
                    {
                        this.DisplayType = MessageDisplayType.Inbox;
                    }
                    else
                    {
                        this.DisplayType = MessageDisplayType.Outbox;
                    }
                    return true;
                case MessageDisplayType.Outbox:
                    break;
                case MessageDisplayType.Inbox:
                    break;
                default:
                    break;
            }
            return false;
        }
    }
}
