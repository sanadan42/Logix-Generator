using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace LogixGenerator
{
    class ProgramInterface
    {
        private string inputFile;
        private Controller PLC;

        public ProgramInterface(string inputFile)
        {
            PLC = new Controller();

            this.inputFile = inputFile;

        }

        public string PLCName
        {
            get { return this.PLC.Name; }
            set { this.PLC.Name = value; }
        }

        public string ProcessorType
        {
            get { return this.PLC.ProcessorType; }
            set { this.PLC.ProcessorType = value; }
        }

        public void ReadData()
        {
            ReadExcel rExcel = new ReadExcel();
            DataSet ds = rExcel.ReadExcelFile(this.inputFile);
            PLC.PopulateData(ds);
        }
        
        public void WriteData()
        {
            string outFileName = Path.GetFullPath(this.inputFile).Replace(Path.GetFileName(this.inputFile), this.PLCName + ".L5K");
            System.IO.StreamWriter outFile = new System.IO.StreamWriter(outFileName);
           
            outFile.WriteLine(PLC.CompleteProgram);

            outFile.Close();
        }

        public void WriteHMIAlarms()
        {
            string outFileName = Path.GetFullPath(this.inputFile).Replace(Path.GetFileName(this.inputFile), this.PLCName + ".xml");
            System.IO.StreamWriter outFile = new System.IO.StreamWriter(outFileName);

            outFile.WriteLine(PLC.CreateHMIAlarmXML());

            outFile.Close();
        }

    }
}
