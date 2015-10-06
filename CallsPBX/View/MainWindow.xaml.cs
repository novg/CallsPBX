using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using CallsPBX.Models;

namespace CallsPBX
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "Текстовые документы (*.txt)|*.txt|Все файлы(*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, CallsContent());
            }
        }

        private string CallsContent()
        {
            List<string> contentList = new List<string>();
            string horLine = "---------------------------------------------------------------------";
            contentList.Add(horLine);
            contentList.Add(string.Format("{0, -17}{1, -12}{2, -15}{3, -15}{4, -17}",
                "Дата", "Время", "Длительность", "Набираемый", "Вызывающий"));
            contentList.Add(string.Format("{0, -17}{0, -12}{0, -15}{1, -15}{1, -17}", "вызова", "номер"));
            contentList.Add(horLine);

            foreach (DataRowView row in dataGrid.Items)
            {
                string date = string.Empty;
                string time = string.Empty;
                if (row.Row.ItemArray[0] is DateTime)
                {
                    DateTime dateTime = (DateTime)row.Row.ItemArray[0];
                    date = dateTime.ToString("yyyy-MM-dd HH:mm").Split()[0];
                    time = dateTime.ToString("yyyy-MM-dd HH:mm").Split()[1];
                }
                string duration = row.Row.ItemArray[1].ToString();
                string inNumber = row.Row.ItemArray[2].ToString();
                string outNumber = row.Row.ItemArray[3].ToString();
                contentList.Add(string.Format("{0, -17}{1, -12}{2, -15}{3, -15}{4, -17}",
                    date, time, duration, inNumber, outNumber));
            }
            contentList.Add(horLine);

            string content = string.Join(Environment.NewLine, contentList);
            return content;
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            if (settingsWindow.ShowDialog() == true)
            {
                //OpenConnection();
            }
        }
    }
}
