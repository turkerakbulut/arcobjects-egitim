using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.AnalysisTools;

namespace EngineUygulamasi
{
    public class ClipGPTool
    {
        private Geoprocessor m_GP;
        private Clip m_Clip;

        public ClipGPTool()
        {
            m_Clip = new Clip();
            m_GP = new Geoprocessor();            
        }

        private string m_inputFeature;
        public string InputFeature
        {
            get { return m_inputFeature; }
            set { m_inputFeature = value; }
        }

        private string m_clipFeature;
        public string ClipFeature
        {
            get { return m_clipFeature; }
            set { m_clipFeature = value; }
        }

        private string m_outPut;
        public string OutputFeature
        {
            get { return m_outPut; }
            set { m_outPut = value; }
        }

        public void ToolCalistir()
        {
            m_GP.OverwriteOutput = true;
            m_GP.AddOutputsToMap = true;
            m_Clip.clip_features = ClipFeature;
            m_Clip.in_features = InputFeature;
            m_Clip.out_feature_class = OutputFeature;

            m_GP.Execute(m_Clip, null);
        }	
    }
}
