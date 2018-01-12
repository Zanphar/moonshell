﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renci.SshNet;

namespace MoonShell
{
    public partial class ConnectForm : Form
    {
        public ConnectForm(string host = null, string username = null, string port = null)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            Options.ApplyTheme(this);

            this.Text += host;

            txtHost.Select();

            if (!string.IsNullOrEmpty(host))
            {
                txtHost.Text = host;
                txtPort.Select();
            }

            if (!string.IsNullOrEmpty(port))
            {
                txtPort.Text = port;
                txtUsername.Select();
            }

            if (!string.IsNullOrEmpty(username))
            {
                txtUsername.Text = username;
                txtPassword.Select();
            }
        }

        private void ConnectForm_Load(object sender, EventArgs e)
        {

        }

        private void Connect()
        {
            if (!string.IsNullOrEmpty(txtHost.Text) && !string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Text))
            {
                try
                {
                    this.Enabled = false;
                    SshClient client;

                    if (!string.IsNullOrEmpty(txtPort.Text))
                    {
                        client = new SshClient(txtHost.Text, Convert.ToInt32(txtPort.Text), txtUsername.Text, txtPassword.Text);
                    }
                    else
                    {
                        client = new SshClient(txtHost.Text, txtUsername.Text, txtPassword.Text);
                    }
                    
                    client.Connect();

                    if (client.IsConnected)
                    {
                        SshForm f = new SshForm(client);
                        f.Show();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    this.Enabled = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void ConnectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Options.CurrentOptions.SSHHost = txtHost.Text;
            Options.CurrentOptions.SSHPort = txtPort.Text;
            Options.CurrentOptions.SSHUsername = txtUsername.Text;
        }
    }
}
