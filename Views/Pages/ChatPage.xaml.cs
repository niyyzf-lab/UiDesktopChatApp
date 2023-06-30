using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UiDesktopChatApp.Tools;
using UiDesktopChatApp.UserControls;
using Wpf.Ui.Common;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
using Newtonsoft.Json;
using MessageBox = Wpf.Ui.Controls.MessageBox;
using System.Security.Cryptography;
using System.Threading;
using UiDesktopChatApp.Views.Windows;
using System.Windows.Controls;
using TextBox = Wpf.Ui.Controls.TextBox;
using System.Text.Json.Serialization;
using System.Net.Http;

namespace UiDesktopChatApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class ChatPage:INavigableView<ViewModels.DashboardViewModel>
    {
        public ObservableCollection<Person> People { get; set; }
        public ObservableCollection<Person> PeopleGroup { get; set; }
        public ObservableCollection<MessageItem> Messages { get; set; }

        public ViewModels.DashboardViewModel ViewModel
        {
            get;
        }

        public ChatPage(ViewModels.DashboardViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
            People = new ObservableCollection<Person>
            {
                new Person { Title ="Para Habib1",UserId="1"},
                new Person { Title ="Para Habib2",UserId="2"},
                new Person { Title ="Para Habib3",UserId="3"},
            };
            PeopleGroup = new ObservableCollection<Person>
            {
                new Person { Title ="群组1",UserId="G1"},
                new Person { Title ="群组2",UserId="G2"},
                new Person { Title ="群组3",UserId="G3"},
            };
            UserList.ItemsSource = People;
            Messages = new();
            ChatMessagePage.ItemsSource = Messages;
            UserList.SelectedIndex = 0;
        }


        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            People.Add(new Person { Title = "ssssds", UserId = "4"});
        }



        private void UserList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (UserList.SelectedItem != null)
            {
                if (UserList.SelectedItem is Person selectedItem)
                {
                    UserName.Text= selectedItem.Title;
                    var selectedItem1 = UserList.SelectedItem as Person;
                    if (Messages !=null)
                    {
                        Messages.Clear();
                        string json = selectedItem1.MessageList;
                        if (json != null)
                        {
                            ObservableCollection<MessageItem> messageItems = JsonConvert.DeserializeObject<ObservableCollection<MessageItem>>(json);
                            if(messageItems != null)
                            {
                                foreach (var item in messageItems)
                                {
                                    Messages.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UserInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Height < 100)
            {
                double lineHeight = textBox.FontSize * 1.2; // 估算每行的高度，可根据实际情况调整
                double minHeight = 33.78; // TextBox 的最小高度，可根据实际情况调整
                double desiredHeight = textBox.LineCount * lineHeight;
                textBox.Height = Math.Max(minHeight, desiredHeight);
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserInput.Text != null)
            {
                if (UserInput.Text.Length <= 500)
                {
                    ComboBoxItem selectedItem = (ComboBoxItem)ChatSelect.SelectedItem;
                    var selectedItem1 = UserList.SelectedItem as Person;
                    if ((bool)UserSwitch.IsChecked)
                    {
                        MainWindow mainWindow = new();
                        
                        Messages.Add(new MessageItem { Message = UserInput.Text, Title = "周文博", IsMirrored = UserSwitch.IsChecked });
                    }
                    else if(selectedItem.Content.ToString()=="群组")
                    {
                        Messages.Add(new MessageItem { Message = UserInput.Text, Title = popName.Text, IsMirrored = UserSwitch.IsChecked });
                    }
                    else
                    {
                        Messages.Add(new MessageItem { Message = UserInput.Text, Title = UserName.Text, IsMirrored = UserSwitch.IsChecked });
                    }
                    if (selectedItem1 != null)
                    {
                        // 获取UserId属性
                        string userId = selectedItem1.UserId;
                        SerializeMessagesToJson(Messages, userId);
                        selectedItem1.MessageList = JsonConvert.SerializeObject(Messages);
                    }
                }
                else
                {
                    OpenMessagebox();
                }
            };
        }
        public void SerializeMessagesToJson(ObservableCollection<MessageItem> messages, string uid)
        {
            string json = JsonConvert.SerializeObject(messages);

            string fileName = uid + ".json";
            string appFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            string userFolderPath = Path.Combine(appFolderPath, "User");
            string filePath = Path.Combine(userFolderPath, fileName);

            Directory.CreateDirectory(userFolderPath);
            File.WriteAllText(filePath, json);
        }
        private void OpenMessagebox()
        {
            var MessageBox = new MessageBox();
            MessageBox.Width = 50;
            MessageBox.MicaEnabled = true;
            MessageBox.ButtonLeftName = "确认";
            MessageBox.ButtonRightClick += MessageBox_ButtonRightClick;
            MessageBox.ButtonLeftClick += MessageBox_ButtonLeftClick;
            MessageBox.Show( "错误","字数太多了");
        }

        private void MessageBox_ButtonRightClick(object sender, RoutedEventArgs e)
        {
            (sender as Wpf.Ui.Controls.MessageBox)?.Close();
        }

        private void MessageBox_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            (sender as Wpf.Ui.Controls.MessageBox)?.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            ListBox listBox = (ListBox)FindName("UserList");
            if (selectedItem != null & listBox != null)
            {
                string selectedContent = selectedItem.Content.ToString();
                switch (selectedContent)
                {
                    case "联系人":
                        listBox.ItemsSource = People;
                        break;
                    case "群组":
                        listBox.ItemsSource = PeopleGroup;
                        break;
                }
            }
        }
    }
    public class Person
    {
        public string? Title { get; set; }
        public string UserId { get; set; }
        public string? MessageList { get; set; }
    }
    public class MessageItem
    {
        public string? Message { get; set; }
        public string Title { get; set; }
        public bool? IsMirrored { get; set; }
        public int Uid { get; set; }
    }
}