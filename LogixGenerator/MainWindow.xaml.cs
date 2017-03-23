using System;
using System.Collections.Generic;
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
using Microsoft.Win32;


namespace LogixGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            string fileName;

            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                fileName = dlg.FileName;
                txtFileName.Visibility = System.Windows.Visibility.Visible;
                txtFileName.Text = fileName;
            }
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // populate the combobox - this is going to be messy - i hope to find a better method in the future
            string s = LogixGenerator.Properties.Resources.ProcessorList;
            string[] processorArray = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            comboProcessorType.ItemsSource = processorArray;
            txtFileName.Text = "";
            txtFileName.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnCreatePLC_Click(object sender, RoutedEventArgs e)
        {
            string fileName, processorName, processorType;
            fileName = txtFileName.Text;
            processorName = txtProcessorName.Text;
            processorType = comboProcessorType.Text;

            bool isSafeToCreate = false;
            isSafeToCreate = !StringHelp.StringEmpty(fileName) && !StringHelp.StringEmpty(processorName) && !StringHelp.StringEmpty(processorType);

            if(isSafeToCreate)
            {
                ProgramInterface plc = new ProgramInterface(fileName);
                plc.PLCName = processorName;
                plc.ProcessorType = processorType;
                plc.ReadData();
                plc.WriteData();
                MessageBox.Show("PLC program generation complete!!!");
            }
            else
            {
                string error = "";
                if (StringHelp.StringEmpty(fileName))
                {
                    error = "Select a data source and try again.";
                }
                else if(StringHelp.StringEmpty(processorName))
                {
                    error = "Input a processor name and try again.";
                }
                else
                {
                    error = "Select a processor type and try again";
                }
                MessageBox.Show(error);
            }

            //if (!StringHelp.StringEmpty(fileName))
            //{
            //    if (!StringHelp.StringEmpty(processorName))
            //    {
            //        ProgramInterface plc = new ProgramInterface(fileName);
            //        plc.PLCName = processorName;
            //        plc.WriteData();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Input a processor name and try again.");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Select a data source and try again.");
            //}
            //MessageBox.Show("PLC program generation complete!!!");
        }

        private void btnCreateHMI_Click(object sender, RoutedEventArgs e)
        {
            string fileName, processorName, processorType;
            fileName = txtFileName.Text;
            processorName = txtProcessorName.Text;
            processorType = comboProcessorType.Text;

            bool isSafeToCreate = false;
            isSafeToCreate = !StringHelp.StringEmpty(fileName) && !StringHelp.StringEmpty(processorName) && !StringHelp.StringEmpty(processorType);

            if (isSafeToCreate)
            {
                ProgramInterface plc = new ProgramInterface(fileName);
                plc.PLCName = processorName;
                plc.ProcessorType = processorType;
                plc.ReadData();
                //plc.WriteData();
                plc.WriteHMIAlarms();
                MessageBox.Show("HMI alarm generation complete!!!");
            }
            else
            {
                string error = "";
                if (StringHelp.StringEmpty(fileName))
                {
                    error = "Select a data source and try again.";
                }
                else if (StringHelp.StringEmpty(processorName))
                {
                    error = "Input a processor name and try again.";
                }
                else
                {
                    error = "Select a processor type and try again";
                }
                MessageBox.Show(error);
            }
        }
    }
}
