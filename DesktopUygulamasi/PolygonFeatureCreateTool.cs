using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Framework;
using ArcObjectsEgitim;

namespace DesktopUygulamasi
{   
    [Guid("1ce32733-5b50-496d-8e90-c095811687f9")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("DesktopUygulamasi.PolygonFeatureCreateTool")]
    public sealed class PolygonFeatureCreateTool : BaseTool
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

        public PolygonFeatureCreateTool()
        {           
            base.m_category = "TKGMEgitimGrubu"; //localizable text 
            base.m_caption = "Create Polygon";  //localizable text 
            base.m_message = "";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "DesktopUygulamasi_PolygonFeatureCreateTool";   //unique id, non-localizable (e.g. "MyCategory_MyTool")         
        }    
        public override void OnCreate(object hook)
        {
            m_application = hook as IApplication;            
        }      
        public override void OnClick()
        {
            // TODO: Add PolygonFeatureCreateTool.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add PolygonFeatureCreateTool.OnMouseDown implementation
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            m_application.Caption = "Ekran Koordinatları X:" + X.ToString() + " Y:" + Y.ToString();
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add PolygonFeatureCreateTool.OnMouseUp implementation
        }
        public override void OnDblClick()
        {
            Util.MessageBoxGoster("Tool Double Click", "bilgi");
        }        
    }
}
