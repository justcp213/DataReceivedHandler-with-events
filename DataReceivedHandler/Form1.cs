using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.IO.Ports;

namespace DataReceivedHandler
{
    public partial class Form1 : Form
    {
        public static int count = 1;
        static object sender = new object();
        static  int offset = 0;
        static char[] charbytes = new char[128];
        static ArrayList buffer = new ArrayList();
        static string text = "";
        static int anzahl = 1;
        int githubtest = 0;
      ////  public static Thread doWork = new Thread(DoWork);
        public static Thread nextWork;

        private static ManualResetEvent event_dowork = new ManualResetEvent(true);
        private static ManualResetEvent event_nextwork = new ManualResetEvent(true);

        static string[] writelist = { "GS", "AM 1", "GL"};

        public Form1()
        {
            InitializeComponent();
            //serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataFired);
            serialPort1.Open();
            serialPort1.DiscardInBuffer();
            serialPort1.DiscardOutBuffer();
        }

        private static void DoWork()
        {
            while(true)
            {
                //event_dowork.WaitOne();
                if (text.Contains("\u0002") && text.Contains("\u0003"))
                {
                    Console.WriteLine(text);
                    
                    break;
                }
               
            }
            text = string.Empty;


        }


        private static void NextWork(int index, object sender) {
           // doWork.Start();
            SerialPort sp = (SerialPort)sender;
            while (index < writelist.Length) { 
            sp.Write("\u0001" + writelist[index] + "\u0003");
                index++;
                DoWork();        
                
            }
            
                index = 0;
            
            Thread.CurrentThread.Join();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //  Array.Clear(buffer, 0, buffer.Length);
            //  serialPort1.DiscardInBuffer();
            //offset = 0; count = 0;
            buffer.Clear();
            text = string.Empty;
            count = 0;
            nextWork = new Thread(() => NextWork(count, serialPort1));
            nextWork.Start();
            
            


        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort1.Close();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.IO.Ports.SerialPort sp = (System.IO.Ports.SerialPort)sender;
            int count = sp.BytesToRead;
            byte[] data = new byte[count];
            sp.Read(data, 0, data.Length);
            text += ASCIIEncoding.ASCII.GetString(data);








        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine(text);
            for(int i = 0; i < writelist.Length; i++)
            {
                richTextBox1.AppendText(writelist[i] + " \n");

            }
            richTextBox1.AppendText(text);
        }
        //// for (int i = 0; i < buffer.Length; i++) Console.Write(buffer[i]);
        //buffer.Add(charbytes);
        //  for (int i = 0; i < buffer.Count; i++) Console.Write(buffer[i]);


    }
    }

