using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiScriptLib;
using UtilsLib.ExtensionMethods;

namespace MultiScript.Forms
{
    public partial class RegisterServer : Form
    {
        public event EventHandler Canceled;

        public event EventHandler Saved;

        private RegisteredServer server;

        private RegisteredConnectionString SelectedConnectionString
        {
            get
            {
                return ltConnectionStrings.SelectedItem as RegisteredConnectionString;
            }
        }

        public RegisteredServer Server
        {
            get
            {
                return server;
            }
            set
            {
                server = value;

                LoadFormFromServer(server);
            }
        }

        public void FocusOnServerName()
        {
            txtServerName.Focus();
        }

        public RegisterServer()
        {
            server = new RegisteredServer {ServerId = Guid.NewGuid()};

            StartPosition = FormStartPosition.CenterScreen;

            InitializeComponent();
        }

        private void LoadFormFromServer(RegisteredServer server)
        {
            txtServerName.Text = server.ServerName;

            ltConnectionStrings.Items.Clear();

            foreach (RegisteredConnectionString connectionString in server.ConnectionStrings)
            {
                ltConnectionStrings.Items.Add(connectionString);
            }
        }

        private void ltConnectionStrings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedConnectionString != null)
            {
                txtConnectionString.Text = SelectedConnectionString.ConnectionString;

                txtDisplayName.Text = SelectedConnectionString.DisplayName;
            }
        }

        private void btnSaveConnectionString_Click(object sender, EventArgs e)
        {
            if (IsConnectionStringValid())
            {
                Server.ServerName = txtServerName.Text;

                ConnectionStringErrorProvider.SetError(txtConnectionString, string.Empty);

                RegisteredConnectionString connectionString = new RegisteredConnectionString();

                if (SelectedConnectionString != null)
                {
                    connectionString = SelectedConnectionString;

                    connectionString.DisplayName = txtDisplayName.Text;

                    connectionString.ConnectionString = txtConnectionString.Text;

                    Server.ConnectionStrings.RemoveAll(
                        dd => dd.ConnectionStringId.Equals(connectionString.ConnectionStringId));

                    Server.ConnectionStrings.Add(connectionString);
                }
                else
                {
                    connectionString = new RegisteredConnectionString
                    {
                        DisplayName = txtDisplayName.Text,
                        ConnectionString = txtConnectionString.Text,
                        ConnectionStringId = Guid.NewGuid()
                    };

                    Server.ConnectionStrings.Add(connectionString);
                }

                LoadFormFromServer(Server);

                txtConnectionString.Text = string.Empty;

                txtDisplayName.Text = string.Empty;

                txtDisplayName.Focus();
            }
            else
            {
                ConnectionStringErrorProvider.SetError(txtConnectionString, "Connection String and Display Name are required");
            }
        }

        private bool IsConnectionStringValid()
        {
            return !txtConnectionString.Text.IsNullOrWhiteSpace() &&
                   !txtDisplayName.Text.IsNullOrWhiteSpace();
        }

        private bool IsServerValid()
        {
            return !txtServerName.Text.IsNullOrWhiteSpace() &&
                   Server.ConnectionStrings.SafeAny();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (Canceled != null)
            {
                Canceled(this, EventArgs.Empty);
            }

            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsServerValid())
            {
                ServerErrorProvider.SetError(txtServerName, string.Empty);

                Server.ServerName = txtServerName.Text;

                if (Saved != null)
                {
                    Saved(this, EventArgs.Empty);
                }

                Close();
            }
            else
            {
                ServerErrorProvider.SetError(txtServerName, "Server name and at least one connection string is required");
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedConnectionString != null)
            {
                Server.ConnectionStrings.RemoveAll(
                    dd => dd.ConnectionStringId.Equals(SelectedConnectionString.ConnectionStringId));

                txtConnectionString.Text = string.Empty;

                txtDisplayName.Text = string.Empty;

                LoadFormFromServer(Server);
            }
        }

        private void connectionStringsMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (SelectedConnectionString == null)
            {
                e.Cancel = true;
            }
        }
    }
}
