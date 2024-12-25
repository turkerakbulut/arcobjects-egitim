using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ArcObjectsEgitim;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace EngineUygulamasi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (axMapControl1.Map.get_Layer(0)!=null)
                {
                    IMap map = axMapControl1.Map;
                    ILayer layer = Util.LayerAl(map, 0);
                    string[] fields = Util.FieldlariAl((layer as IFeatureLayer).FeatureClass);
                    for (int i = 0; i < fields.Length; i++)
                    {
                        listBox1.Items.Add(fields[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.MessageBoxGoster(ex.ToString(), "Hata");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            if (listBox1.SelectedIndex!=-1)
            {
                textBox1.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Util.Selectfeature(axMapControl1.Map, 
                    (axMapControl1.Map.get_Layer(0) as IFeatureLayer), textBox1.Text);
                
                (axMapControl1.Map as IActiveView).Refresh();
                
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text += "=";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text += ">";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text += "<";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            if (listBox1.SelectedIndex!=-1)
            {
                ITable table = (axMapControl1.Map.get_Layer(0) as IFeatureLayer).FeatureClass as ITable;

                int index = table.FindField(listBox1.Items[listBox1.SelectedIndex].ToString());

                IField field = table.Fields.get_Field(index);

                string[] dt = Util.GetFieldValues2(table, field);

                for (int i = 0; i < dt.Length; i++)
                {
                    listBox2.Items.Add(dt[i]);
                }
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text += listBox2.Items[listBox2.SelectedIndex].ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
           
        }

        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            double buffer = Convert.ToDouble(textBox2.Text);
            try
            {
                IPoint point = Util.EkranKoordinatlariniMapKoordinatlarinaCevir
                    ((axMapControl1.Map as IActiveView), e.x, e.y);
                IFeatureCursor fcursor = Util.SpatialFilterPerformToLayer
                    ((axMapControl1.Map.get_Layer(0) as IFeatureLayer), point);
                IFeature feature = fcursor.NextFeature();
                Util.SecilenDetayaBufferUygula(axMapControl1.Map,buffer, feature, point);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            try
            {
                //ClipGPTool kes = new ClipGPTool();
                //kes.InputFeature = textBox3.Text;
                //kes.ClipFeature = textBox4.Text;
                //kes.OutputFeature = textBox5.Text;

                //kes.ToolCalistir();

                IFeatureClass fc = Util.AccessWorkspacedenFeatureClassAl
                    (@"C:\New Personal Geodatabase (2).mdb", "sonuc2"); //disaridan alinacak
                IFeatureLayer fl = new FeatureLayerClass();
                fl.FeatureClass = fc;
                axMapControl1.Map.AddLayer(fl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }       
    }
}