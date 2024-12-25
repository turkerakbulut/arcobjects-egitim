using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ArcObjectsEgitim;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
/*Working with geometry*/
namespace DesktopUygulamasi
{    
    [Guid("f34e997c-6c51-4da9-9c8f-e6486eb875f3")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("DesktopUygulamasi.BufferBtn")]
    public sealed class BufferBtn : BaseCommand
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

        public BufferBtn()
        {
            base.m_category = "TKGMEgitimGrubu"; //localizable text
            base.m_caption = "Buffer Feature";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "DesktopUygulamasi_BufferBtn";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")           
        }     
        public override void OnCreate(object hook)
        {
            m_application = hook as IApplication;
        }
         
        public override void OnClick()
        {
            IMaps maps = Util.MapleriGetir(m_application);
            IMap map = Util.MapAl(maps, 0);    
            ILayer layer =Util.LayerAl(map,0); //Parsel
            IPoint point =Util.EkranKoordinatlariniMapKoordinatlarinaCevir((map as IActiveView), 50, 50);
            IFeatureCursor featureCursor = Util.SpatialFilterPerformToLayer((layer as IFeatureLayer), point);
            Util.SecilenDetayaBufferUygula(map, 100, featureCursor.NextFeature(), point);
        }        
    }
}
