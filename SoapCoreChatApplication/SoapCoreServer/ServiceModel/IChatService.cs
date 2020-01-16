﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceModel;

namespace SoapCoreChatApplication.Contract
{
    [ServiceContract(CallbackContract = typeof(IChatCallback), SessionMode = SessionMode.Required)]
    public interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void SendChat(string name, string text);

        [OperationContract(IsOneWay = true)]
        void Join(string name);

        [OperationContract(IsOneWay = true)]
        void Refresh();

        [OperationContract(IsOneWay = true)]
        void LogOut();

        [OperationContract]
        List<Message> GetChats();
    }
}
