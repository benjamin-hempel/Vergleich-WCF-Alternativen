using System;
using System.Collections.Generic;
using System.Text;
using IPCChatApplication.Shared;

namespace IPCServer
{
    class ChatService : IChatService
    {
        static List<Message> history = new List<Message>();
        static List<string> userList = new List<string>();

        public void SendChat(string name, string text)
        {
            Message message = new Message(name, text, DateTime.Now);
            history.Add(message);

            Console.WriteLine($"{name} {DateTime.Now}: {text}");
        }

        public void Join(string name)
        {
            Console.WriteLine($"User {name} has joined the chat!");
            userList.Add(name);
        }

        public void LogOut(string name)
        {
            Console.WriteLine($"User {name} has left the chat!");
            userList.Remove(name);
        }

        public List<Message> GetChats(int index)
        {
            if (index == 0 && history.Count == 0) return null;

            List<Message> newChats = new List<Message>();
            for(int i = index; i < history.Count; i++)
            {
                newChats.Add(history[i]);
            }

            return newChats;
        }
        public List<string> RefreshUserList()
        {
            return userList;
        }

    }
}
