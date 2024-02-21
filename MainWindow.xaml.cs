using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FTFbyWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void 文件入口_Click(object sender, RoutedEventArgs e)
        {
            string inputFilePath = SelectTextFile();
            if (!string.IsNullOrEmpty(inputFilePath))
            {
                try
                {
                    string[] originalLines = File.ReadAllLines(inputFilePath);
                    string[] newLines = InsertEmptyLines(originalLines);

                    if (ShowSaveChangesPrompt())
                    {
                        File.WriteAllLines(inputFilePath, newLines);
                        MessageBox.Show("成功保存", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessageBox(ex.Message);
                }
            }
        }

        private string SelectTextFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "C:\\desktop",
                Filter = "文本文件 (*.txt)|*.txt",
                FilterIndex = 1,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        private string[] InsertEmptyLines(string[] originalLines)
        {

            var sb = new StringBuilder();
            int cnt = 1;
            for(int i = 0; i < originalLines.Length; i++)
            {
                sb.Append(originalLines[i]+Environment.NewLine);
                if(cnt == 4)
                {
                    sb.Append(Environment.NewLine);
                    cnt = 0;
            
                }
                cnt++;
            }
            return sb.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);;

        }

        private bool ShowSaveChangesPrompt()
        {
            string messageBoxText = "你确定要保存吗？\n请确认已有备份以免造成损失";
            string caption = "WARNING";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            return result == MessageBoxResult.Yes;
        }

        private void ShowErrorMessageBox(string message)
        {
            string caption = "似乎出了一点问题";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBox.Show(message, caption, button, icon);
        }

        
    }
}
