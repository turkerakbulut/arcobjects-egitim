using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;

namespace MapDocumenttenMapseErisimEngine
{
    public partial class Form1 : Form
    {
        private IMapDocument mapDoc = null;
        private int m_mapIndex = 0;
        private int m_layerIndex = 0;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();
                if (!string.IsNullOrEmpty(ofd.FileName))
                {
                    textBox1.Text = ofd.FileName;                     
                    mapDoc = new  MapDocumentClass();
                    mapDoc.Open(ofd.FileName,null);
                    for (int i = 0; i < mapDoc.MapCount; i++)
                    {                        
                        listBox1.Items.Add(mapDoc.get_Map(i).Name);
                    }                   
                }
                else
                {
                    this.Text = "Mxd Döküman seçin!!!";
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex!=-1)
                {
                   int layerCount = mapDoc.get_Map(listBox1.SelectedIndex).LayerCount;
                   int selectedMap = listBox1.SelectedIndex; 
                   string mapName = mapDoc.get_Map(listBox1.SelectedIndex).Name;
                   listBox1.Items.Clear();
                   listBox1.Items.Add(mapName);
                   for (int i = 0; i < layerCount; i++)
                   {
                       listBox1.Items.Add("--->" + mapDoc.get_Map(selectedMap).get_Layer(i).Name);
                   }
                }
                else
                {
                    this.Text = "Listeden map seçin!!!";
                }            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            m_mapIndex = listBox1.SelectedIndex;
            IMap map = mapDoc.get_Map(m_mapIndex);

            for (int i = 0; i < map.LayerCount; i++)
            {
                listBox2.Items.Add(map.get_Layer(i).Name);
            }
            //try
            //{               
            //    if (mapDoc!=null && listBox1.SelectedIndex!=-1)
            //    {
            //        axMapControl1.Map = mapDoc.get_Map(listBox1.SelectedIndex);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());                
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                m_layerIndex = listBox2.SelectedIndex;
                ILayer layer = mapDoc.get_Layer(m_mapIndex, m_layerIndex);
                axMapControl1.Map.AddLayer(layer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            axMapControl1.ClearLayers();
        }       
    }
}