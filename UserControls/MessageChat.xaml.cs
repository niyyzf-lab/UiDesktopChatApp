using System.Globalization;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using UiDesktopChatApp.Tools;
using System.Collections.ObjectModel;
using UiDesktopChatApp.Views.Pages;

namespace UiDesktopChatApp.UserControls
{

    public partial class MessageChat : UserControl
    {
        public MessageChat()
        {
            InitializeComponent();
        }


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(MessageChat));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MessageChat));
    }
}