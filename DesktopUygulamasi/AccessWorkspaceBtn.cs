using System;
using System.Drawing;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ArcObjectsEgitim;
using ESRI.ArcGIS.Carto;

namespace DesktopUygulamasi
{
    /// <summary>
    /// Summary description for AccessWorkspaceBtn.
    /// </summary>
    [Guid("c3cd1193-568c-4dc0-a1ab-78bfe622a930")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("DesktopUygulamasi.AccessWorkspaceBtn")]
    public sealed class AccessWorkspaceBtn : BaseCommand
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

        public AccessWorkspaceBtn()
        {
            base.m_category = "TKGMEgitimGrubu"; //localizable text
            base.m_caption = "Workspace Erisim";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "DesktopUygulamasi_AccessWorkspaceBtn";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
                        
        }       

        public override void OnCreate(object hook)
        {
            m_application = hook as IApplication;
        }

       
        public override void OnClick()
        {
            //IFeatureClass fc=
            //Util.AccessWorkspacedenFeatureClassAl(@"C:\New Personal Geodatabase.mdb", "test");
            IMaps maps = Util.MapleriGetir(m_application);
            IMap map = Util.MapAl(maps, 1);
            //Util.FeatureClassAddToMap(map, fc, fc.AliasName, true);  
            Util.Selectfeature(map, map.get_Layer(0) as IFeatureLayer, "POPULATION = 4344000");
            (map as IActiveView).Refresh();
        }    
    }
}
