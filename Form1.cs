using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirQuality_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
           
            updatePorts();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();

        }

        private void updatePorts()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            updatePorts();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            serialPort1.Open();
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort1.ReadLine();
            data = data.Replace("\r", "");
            string[] values = data.Trim().Split(',');

            if (values.Length==6)
            {
                chart1.Series["Temp"].Points.AddY(Convert.ToInt32(values[1]));
                chart2.Series["Humd"].Points.AddY(Convert.ToInt32(values[0]));
                chart3.Series["Dust"].Points.AddY(Convert.ToDouble(values[4]));
                chart4.Series["Co2"].Points.AddY(Convert.ToInt32(values[3]));
                chart5.Series["O3"].Points.AddY(Convert.ToInt32(values[5]));
                chart6.Series["Tvoc"].Points.AddY(Convert.ToInt32(values[2]));

            }




        }
    }
}
