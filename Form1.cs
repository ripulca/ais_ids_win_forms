using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ais_ids
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<string> GetHardwareInfo()
        {
            ConnectionOptions connection = new ConnectionOptions();

            List<string> result = new List<string>();

            try
            {

                ManagementObjectSearcher searcher;

                if ((bool)checkBox.Checked)
                {
                    connection.Username = username.Text;
                    connection.Password = password.Text;
                    connection.Authority = "ntlmdomain:" + domain.Text;

                    ManagementScope scope = new ManagementScope("\\\\" + ipaddr.Text + "\\root\\CIMV2", connection);
                    scope.Connect();

                    ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_SoftwareElement");

                    searcher = new ManagementObjectSearcher(scope, query);
                }
                else
                {
                    searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
                }
                string text = "";
                foreach (ManagementObject obj in searcher.Get())
                {
                    if (obj["InstallSource"] != null)
                    {
                        if (obj["Name"] != null)
                        {
                            text += "ProductName: " + obj["Name"].ToString().Trim() + "\n";
                        }
                        else
                        {
                            text += "PackageName: \n";
                        }
                        text += "Version: " + obj["Version"].ToString().Trim() + "\n";
                        if (obj["InstallLocation"] != null)
                        {
                            text += "InstallLocation: " + obj["InstallLocation"].ToString().Trim() + "\n";
                        }
                        else
                        {
                            text += "InstallLocation: \n";
                        }
                        text += "InstallSource: " + obj["InstallSource"].ToString().Trim() + "\n";
                        text += "Vendor: " + obj["Vendor"].ToString().Trim() + "\n";
                        text += "\n";
                    }
                }

                output.Text = text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetHardwareInfo();
        }
    }
}
