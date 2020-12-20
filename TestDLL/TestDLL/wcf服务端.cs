using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDLL
{
    public partial class wcf服务端 : Form
    {
        public wcf服务端()
        {
            InitializeComponent();

            ServiceHost myServiceHost = new ServiceHost(typeof(TestDLL.FileTransferService));
            myServiceHost.Open();

            LogText("This is the SERVER console");
            LogText("Service Started!");
            foreach (Uri address in myServiceHost.BaseAddresses)
                LogText("Listening on " + address);
            LogText("Click any key to close...");
        }

        private void LogText(string text)
        {
            LogListBox.Items.Add(text);
            LogListBox.SelectedIndex = LogListBox.Items.Count - 1;
        }
    }
}
