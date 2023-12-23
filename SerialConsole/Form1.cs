using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace SerialConsole
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort;

        public Form1()
        {
            InitializeComponent();
            InitializeSerialPort();
        }

        private void InitializeSerialPort()
        {
            serialPort = new SerialPort();
            serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.PortName = cmbPorts.SelectedItem.ToString();
                serialPort.BaudRate = (int)cmbBaudRate.SelectedItem;
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии порта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при закрытии порта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
             string receivedData = serialPort.ReadExisting();
            this.Invoke((MethodInvoker)delegate { textBoxReceived.Text += receivedData; });
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            cmbPorts.DataSource = SerialPort.GetPortNames();
            cmbBaudRate.DataSource = new[] { 9600, 19200, 38400, 57600, 115200 };
            cmbPorts.SelectedIndex = 0;
            cmbBaudRate.SelectedIndex = 0;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string dataToSend = textBoxSend.Text;
            SendData(dataToSend);
        }

        private void SendData(string data)
        {
            if (serialPort.IsOpen && !string.IsNullOrEmpty(data))
            {
                try
                {
                    serialPort.Write(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при отправке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Порт не открыт или данные пусты.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
