using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.ServiceModel;
using IPCChatApplication.Shared;
using System.Net;
using JKang.IpcServiceFramework;
using System.Threading.Tasks;

namespace WPFClient.ViewModel
{
    public class MainViewModel : ViewModelBase, IDataErrorInfo
    {
        IpcServiceClient<IChatService> client;

        public string Username { get; set; }
        public string ChatText { get; set; }
        public string Userlist { get; set; } = "Teilnehmer";
        public ObservableCollection<Message> Messages { get; set; }
        public string WindowTitle { get; set; }
        public Visibility LoginVisibility { get; set; }
        public Visibility ChatViewVisibility { get; set; }
        public ICommand LoginCommand { get; private set; }
        public ICommand SendCommand { get; private set; }
        public ICommand LogOutCommand { get; private set; }

        // Error handler
        public string Error => string.Empty;

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case nameof(Username):
                        if (Username == "")
                            return "Username is required!";
                        break;
                }
                return string.Empty;
            }
        }

        public MainViewModel()
        {
            LoginVisibility = Visibility.Visible;
            ChatViewVisibility = Visibility.Collapsed;

            LoginCommand = new RelayCommand(LoginMethod);
            SendCommand = new RelayCommand(SendMethod);

            if (IsInDesignMode)
            {
                WindowTitle = "Chat Application (Design)";
            }
            else
            {
                WindowTitle = "Chat Application";
            }

            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);
        }

        public async void LoginMethod()
        {
            if (!string.IsNullOrEmpty(Username))
            {
                LoginVisibility = Visibility.Collapsed;
                ChatViewVisibility = Visibility.Visible;

                client = new IpcServiceClientBuilder<IChatService>()
                    .UseTcp(IPAddress.Loopback, 8000)
                    .Build();

                await client.InvokeAsync(x => x.Join(Username));

                var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(RefreshMethod);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
            }
        }

        public void SendMethod()
        {
            if (!string.IsNullOrEmpty(ChatText))
                client.InvokeAsync(x => x.SendChat(Username, ChatText));
            ChatText = string.Empty;
        }

        public async void RefreshMethod(object sender, EventArgs e)
        {
            List<string> userList = await client.InvokeAsync(x => x.RefreshUserList());

            string users = userList.Count + " Teilnehmer:";
            foreach (string user in userList)
                users += " " + user;
            Userlist = users;

            int index;
            if (Messages == null) index = 0;
            else index = Messages.Count;

            var history = await client.InvokeAsync(x => x.GetChats(index));

            if (history == null) return;
            if (Messages == null)
            {
                Messages = new ObservableCollection<Message>(history);
                return;
            }
            foreach (var message in history) Messages.Add(message);
        }

        public void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close the application?", "Chat Application", MessageBoxButton.YesNo) == MessageBoxResult.No)
                e.Cancel = true;
            else
            {
                if (!string.IsNullOrEmpty(Username))
                    client.InvokeAsync(x => x.LogOut(Username));
            }
        }

    }
}
