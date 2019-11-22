using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO.Ports;
using System.Management;
using System.Runtime.InteropServices;

namespace AutoEmailCharger
{
    class Program
    {
        public static MailMessage mail = new MailMessage();
        public static SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        public static SerialPort _serialPort = new SerialPort();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            try
            {
                //Hide Window
                IntPtr hWnd = GetConsoleWindow();
                if (hWnd != IntPtr.Zero)
                    ShowWindow(hWnd, 0);


                System.Management.ObjectQuery query = new ObjectQuery("Select * FROM Win32_Battery");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject mo in collection)
                {
                    foreach (PropertyData property in mo.Properties)
                    {
                        if (property.Name == "EstimatedChargeRemaining" && Convert.ToInt32(property.Value) < 20)
                        {
                            InitialPort();
                            if (!(_serialPort.IsOpen))
                                _serialPort.Open();
                            _serialPort.Write("1");
                            InitialMail("Charge Notification--IN", "Auto E-mail");

                            if ((_serialPort.IsOpen))
                                _serialPort.Close();

                            SmtpServer.Send(mail);

                            break;
                        }
                        else if (property.Name == "EstimatedChargeRemaining" && Convert.ToInt32(property.Value) > 90)
                        {
                            InitialPort();
                            if (!(_serialPort.IsOpen))
                                _serialPort.Open();
                            _serialPort.Write("0");
                            InitialMail("Charge Notification--Not Charge", "Auto E-mail");

                            if ((_serialPort.IsOpen))
                                _serialPort.Close();

                            SmtpServer.Send(mail);

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                InitialMail(ex.Message, "Auto E-mail");
                Console.ReadLine();
            }
        }
        public static void InitialMail(string subject,string body)
        {
            mail.From = new MailAddress("systemautoemails@gmail.com");
            mail.To.Add("danielandreson6@gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("systemautoemails@gmail.com", "");
            SmtpServer.EnableSsl = true;
            mail.Subject = subject;
            mail.Body = body;
        }

        public static void InitialPort()
        {
            _serialPort.PortName = "COM5";
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
        }
       
    }
}
