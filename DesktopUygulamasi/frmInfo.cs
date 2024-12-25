using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace DesktopUygulamasi
{
    public partial class frmInfo : Form
    {
        private string[] m_fields;
       
        public frmInfo(string [] fields)
        {
            InitializeComponent();
            this.m_fields = fields;
        }

        private void frmInfo_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < m_fields.Length; i++)
            {
                listBox1.Items.Add(m_fields[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}