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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SqlClient;

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
                try
                {
                    String str = "Data Source=.\\;Initial Catalog=AirQuality_Project;Integrated Security=True;Encrypt=False;";

                    String query = "insert into data values ('" + data + "')";

                    SqlConnection con = new SqlConnection(str);

                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();

                    cmd.ExecuteNonQuery();

                    con.Close();
                }
                catch
                {


                }
            }


           

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap chartBitmap = new Bitmap(chart1.Width + 5, (chart1.Height * 7)+100);
            using (Graphics gfx = Graphics.FromImage(chartBitmap))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            {
                gfx.FillRectangle(brush, 0, 0, chart1.Width + 5, chart1.Height + 5);
            }


            StringFormat stringFormat = new StringFormat();
            stringFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;


            chart1.BackGradientStyle = GradientStyle.None;
            chart1.BackColor = Color.White;
            chart1.DrawToBitmap(chartBitmap, new Rectangle(0, 0, chart1.Width, chart1.Height));


            chart2.BackGradientStyle = GradientStyle.None;
            chart2.BackColor = Color.White;
            chart2.DrawToBitmap(chartBitmap, new Rectangle(0, 161, chart1.Width, chart1.Height ));

            chart3.BackGradientStyle = GradientStyle.None;
            chart3.BackColor = Color.White;
            chart3.DrawToBitmap(chartBitmap, new Rectangle(0, 325, chart1.Width, chart1.Height ));

            chart4.BackGradientStyle = GradientStyle.None;
            chart4.BackColor = Color.White;
            chart4.DrawToBitmap(chartBitmap, new Rectangle(0, 496, chart1.Width, chart1.Height ));

            chart5.BackGradientStyle = GradientStyle.None;
            chart5.BackColor = Color.White;
            chart5.DrawToBitmap(chartBitmap, new Rectangle(0, 667, chart1.Width, chart1.Height ));

            chart6.BackGradientStyle = GradientStyle.None;
            chart6.BackColor = Color.White;
            chart6.DrawToBitmap(chartBitmap, new Rectangle(0, 838, chart1.Width, chart1.Height ));

         


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Files (*.png)|*.png|All files (*.*)|*.*";
            saveFileDialog.Title = "Save Chart Image";
            saveFileDialog.FileName = "AirQuality.png";
            saveFileDialog.ShowDialog();
            chartBitmap.Save(saveFileDialog.FileName);
          
        }
    }
}
