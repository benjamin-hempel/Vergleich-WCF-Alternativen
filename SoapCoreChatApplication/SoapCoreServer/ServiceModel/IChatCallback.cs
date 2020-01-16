using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceModel;

namespace SoapCoreChatApplication.Contract
{
    interface IChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveChat(Message message);

        [OperationContract(IsOneWay = true)]
        void RefreshUserList(List<string> userList);
    }
}
