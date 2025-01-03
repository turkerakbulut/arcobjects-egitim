using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ArcObjectsEgitim;
using ESRI.ArcGIS.Geodatabase;

namespace DesktopUygulamasi
{    
    [Guid("8eaa90b9-defd-4df3-95de-bc45f19b1c99")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("DesktopUygulamasi.MapveLayerlaraErisimBtn")]
    public sealed class MapveLayerlaraErisimBtn : BaseCommand
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

        public MapveLayerlaraErisimBtn()
        {            
            base.m_category = "TKGMEgitimGrubu"; //localizable text
            base.m_caption = "Map ve Layerlara erisim";  //localizable text
            base.m_message = "Butona basildi";  //localizable text 
            base.m_toolTip = "Map ve Layer bilgilerini alir";  //localizable text 
            base.m_name = "DesktopUygulamasi_MapveLayerlaraErisimBtn";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
}

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            m_application = hook as IApplication;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            try
            {
                IMaps maps = Util.MapleriGetir(m_application);
                IMap map = Util.MapAl(maps, 0);
                ILayer layer = Util.LayerAl(map, 0);
                IFeatureClass fc = Util.FeatureClassAl(layer);
                string[] fields = Util.FieldlariAl(fc);

                frmInfo frm = new frmInfo(fields);
                frm.ShowDialog();                
            }
            catch (Exception ex)
            {
                ArcObjectsEgitim.Util.MessageBoxGoster(ex.ToString(), "Hata");
            }           
        }

        #endregion
    }
}
