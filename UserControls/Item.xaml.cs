using System.Globalization;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UiDesktopChatApp.Tools;
using System.Windows.Data;
using Wpf.Ui.Appearance;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace UiDesktopChatApp.UserControls
{
    public partial class Item : UserControl
    {
        private bool isParentSelected = true;
        public Item()
        {
            InitializeComponent();
            Loaded += Item_Loaded;
            Unloaded += Item_Unloaded;
            
        }
       
        public void Item_Loaded(object sender,RoutedEventArgs e)
        {
            LoadMessageList(this.UserId);
        }

        public void Item_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        public void LoadMessageList(string uid)
        {
            string folderPath = "User";
            string fileName = $"{uid}.json";
            string filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    this.MessageList = json;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"无法读取 JSON 文件: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(folderPath); // 创建文件夹
                    File.WriteAllText(filePath, ""); // 创建空的 JSON 文件
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"无法创建 JSON 文件: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Item));

        public string TagName
        {
            get { return (string)GetValue(TagNameProperty); }
            set { SetValue(TagNameProperty, value); }
        }

        public static readonly DependencyProperty TagNameProperty = DependencyProperty.Register("TagName", typeof(string), typeof(Item));

        public string UserId
        {
            get { return (string)GetValue(UserIdProperty); }
            set { SetValue(UserIdProperty, value); }
        }

        public static readonly DependencyProperty UserIdProperty = DependencyProperty.Register("UserId", typeof(string), typeof(Item));


        public string MessageList
        {
            get { return (string)GetValue(MessageListProperty); }
            set { SetValue(MessageListProperty, value); }
        }

        public static readonly DependencyProperty MessageListProperty = DependencyProperty.Register("MessageList", typeof(string), typeof(Item));

        private void UserControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LoadMessageList(this.UserId);
        }
    }
    /// <summary>
    /// 转化器
    /// </summary>
    public class AbbreviationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string title)
            {
                return AbbreviationGenerator.GenerateAbbreviation(title);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class InverseColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                Color originalColor = brush.Color;
                Color invertedColor = Color.FromRgb((byte)(255 - originalColor.R), (byte)(255 - originalColor.G), (byte)(255 - originalColor.B));
                return new SolidColorBrush(invertedColor);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class HalfValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width)
            {
                return width / 2;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
