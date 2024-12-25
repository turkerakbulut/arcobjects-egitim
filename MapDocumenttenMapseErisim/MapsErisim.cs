using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;

namespace MapDocumenttenMapseErisim
{    
    [Guid("445c4676-457e-4d93-bb19-bca5be413352")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("MapDocumenttenMapseErisim.MapsErisim")]
    public sealed class MapsErisim : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IApplication m_application;

        public MapsErisim()
        {          
            base.m_category = "TKGM Egitim grubu"; 
            base.m_caption = "MapsErisim";  
            base.m_message = "MapsErisim";  
            base.m_toolTip = "MapsErisim";  
            base.m_name = "MapDocumenttenMapseErisim_MapsErisim";           
        }
      
        public override void OnCreate(object hook)
        {
            if (hook!=null)
            {
                m_application = hook as IApplication; 
            }        
        }
        
        public override void OnClick()
        {
            try
            {
                IMxDocument mxDoc = m_application.Document as IMxDocument; //QI e örnek
                if (mxDoc.Maps.Count==1)
                {
                    IMap map = mxDoc.ActiveView.FocusMap;//Interface Inheritance a örnek
                    m_application.Caption = map.Name;
                }
                else
                {
                    IMaps maps = mxDoc.Maps; //Interface Inheritance a örnek
                    for (int i = 0; i < maps.Count; i++)
                    {
                        m_application.Caption += maps.get_Item(i).Name;
                    }
                }               
            }
            catch (Exception ex)
            {                
                MessageDialog err = new MessageDialogClass();
                err.DoModal("Hata", ex.ToString(), "Tamam", "Iptal", 0);
            }
        }       
    }
}
