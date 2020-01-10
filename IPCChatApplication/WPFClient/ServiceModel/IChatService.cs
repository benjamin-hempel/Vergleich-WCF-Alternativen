using System;
using System.Collections.Generic;
using System.Text;

namespace IPCChatApplication.Shared
{
    public interface IChatService
    {
        void SendChat(string name, string text);

        void Join(string name);

        void LogOut(string name);

        List<Message> GetChats(int index);

        List<string> RefreshUserList();
    }
}
