using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Net.NetworkInformation;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
    
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {

                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
                if (dnsServers.Count > 0)
                {
                    textBox1.Text = adapter.Description;
                    foreach (System.Net.IPAddress dns in dnsServers)
                    {
                        listBox1.Items.Add(dns.ToString());
                        textBox1.Text = dns.ToString();
                    }
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    listBox1.Items.Add(objMO["Caption"].ToString());
                    ManagementBaseObject DnsEntry = objMO.GetMethodParameters("SetDNSServerSearchOrder");

                    dnsler = textBox1.Text + "," + textBox2.Text;
                    DnsEntry["DNSServerSearchOrder"] = dnsler.Split(',');
                    ManagementBaseObject DnsMbo = objMO.InvokeMethod("SetDNSServerSearchOrder", DnsEntry, null);
                    int returnCode = int.Parse(DnsMbo["returnvalue"].ToString());
                }
            }
        }
        string dnsler = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "8.8.8.8";
            textBox2.Text = "8.8.8.4";
          
            
        }

        
    }
    }

