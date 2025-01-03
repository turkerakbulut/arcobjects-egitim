using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Data;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Editor;

namespace ArcObjectsEgitim
{
    public class Util
    {
        #region Query and Selection
        public static string Selectfeature(IMap map, IFeatureLayer featureLayer, string whereClause)
        {
            try
            {
                IFeatureSelection featureSelection = featureLayer as IFeatureSelection;
                IQueryFilter qf = new QueryFilterClass();
                ISelectionSet selectionSet = null;
                qf.WhereClause = whereClause;
                featureSelection.SelectFeatures(qf, esriSelectionResultEnum.esriSelectionResultNew, false);
                featureSelection.SelectionChanged();
                selectionSet = featureSelection.SelectionSet;
                return selectionSet.Count.ToString();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
                return null;
            }
        }
        public static void SelectMapFeaturesByAttributeQuery(IActiveView activeView, IFeatureLayer featureLayer, string whereClause)
        {
            if (activeView == null || featureLayer == null || whereClause == null)
                return;

            IFeatureSelection featureSelection = featureLayer as IFeatureSelection; // Dynamic Cast          
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = whereClause;
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
        }
        public static string[] FieldlariAl(IFeatureClass featureClass)
        {
            try
            {
                string[] fieldlar = new string[((featureClass as ITable).Fields.FieldCount)];

                ITable table = featureClass as ITable;
                IFields fields = table.Fields;
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    fieldlar[i] = fields.get_Field(i).AliasName;
                }
                return fieldlar;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }
        public static DataTable GetFieldValues(ITable table, IField field)
        {
            DataTable dtValues = new DataTable(field.Name + "_Values");
            dtValues.Columns.Add("Values");
            dtValues.Columns[0].DataType = System.Type.GetType("System.String");

            try
            {
                IQueryFilter queryFilter = new QueryFilterClass();
                queryFilter.SubFields = "DISTINCT(" + field.Name + ")";
                //queryFilter.WhereClause = "DISTINCT(" + field.Name + ")";

                ICursor cursor = table.Search(queryFilter, false);

                IRow row;
                int fieldIndex = table.Fields.FindField(field.Name);
                List<string> uniqueValues = new List<string>();
                while ((row = cursor.NextRow()) != null)
                {
                    object value = row.get_Value(fieldIndex);
                    if (value != null && value != DBNull.Value)
                    {
                        DataRow dtRow = dtValues.NewRow();

                        if (value is double)
                        {
                            dtRow.ItemArray[0] = value.ToString().Replace(",", ".");
                            dtValues.Rows.Add(dtRow);
                        }
                        else if (value is string || value is Guid)
                        {
                            dtRow.ItemArray[0] = "'" + value.ToString() + "'";
                            dtValues.Rows.Add(dtRow);
                        }
                        else
                        {
                            dtRow.ItemArray[0] = value.ToString();
                            dtValues.Rows.Add(dtRow);
                        }
                    }
                }

                ComReleaser.ReleaseCOMObject(cursor);
                return dtValues;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }
        public static string [] GetFieldValues2(ITable table, IField field)
        {
            string [] dizi = new string[table.RowCount(null)];
            try
            {
                IQueryFilter queryFilter = new QueryFilterClass();
                queryFilter.SubFields = "DISTINCT(" + field.Name + ")";              

                ICursor cursor = table.Search(queryFilter, false);

                IRow row;
                int fieldIndex = table.Fields.FindField(field.Name);
                List<string> uniqueValues = new List<string>();
                int index = 0;
                while ((row = cursor.NextRow()) != null)
                {
                    object value = row.get_Value(fieldIndex);
                    if (value != null && value != DBNull.Value)
                    {

                        if (value is double)
                        {
                            dizi[index] = value.ToString();
                            index += 1;
                        }
                        else if (value is string || value is Guid)
                        {
                           
                        }
                        else
                        {
                            dizi[index] = value.ToString();
                            index += 1;
                        }
                    }
                }

                ComReleaser.ReleaseCOMObject(cursor);
                return dizi;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }
        #endregion        

        #region Creating Data
        public static void DetayOlustur(IFeatureClass featureClass, IGeometry geometry)
        {
            try
            {
                IFeature feature = featureClass.CreateFeature();
                feature.Shape = geometry;
                ISubtypes subtypes = (ISubtypes)featureClass;
                IRowSubtypes rowSubtypes = (IRowSubtypes)feature;
                if (subtypes.HasSubtype)// feature Class Subtype a sahipmi
                    rowSubtypes.SubtypeCode = 1; //eger varse sıradan 1 . icin 
                rowSubtypes.InitDefaultValues();
                feature.Store();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }        
        #endregion

        #region PageLayout
        public static void AddLegend(IPageLayout pageLayout, IMap map, double posX, double posY, double legW)
        {
            if (pageLayout == null || map == null)
                return;

            IGraphicsContainer graphicsContainer = pageLayout as IGraphicsContainer; // Dynamic Cast
            IMapFrame mapFrame = graphicsContainer.FindFrame(map) as IMapFrame; // Dynamic Cast
            IUID uid = new UIDClass();
            uid.Value = "esriCarto.Legend";
            IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame((UID)uid, null); // Explicit Cast

            //Get aspect ratio
            IQuerySize querySize = mapSurroundFrame.MapSurround as IQuerySize; // Dynamic Cast
            double w = 0;
            double h = 0;
            querySize.QuerySize(ref w, ref h);
            double aspectRatio = w / h;

            IEnvelope envelope = new EnvelopeClass();
            envelope.PutCoords(posX, posY, (posX * legW), (posY * legW / aspectRatio));
            ESRI.ArcGIS.Carto.IElement element = mapSurroundFrame as IElement; // Dynamic Cast
            element.Geometry = envelope;
            graphicsContainer.AddElement(element, 0);
        }
        public static void SablonBilgiYaz(IMxDocument mxdoc, string alanAdi, string alanDegeri)
        {
            IElement element;
            try
            {
                IActiveView activeView = mxdoc.PageLayout as IActiveView;
                activeView.GraphicsContainer.Reset();
                while ((element = activeView.GraphicsContainer.Next()) != null)
                {
                    IElementProperties elementPro = element as IElementProperties;
                    if (elementPro.Name == alanAdi)
                    {
                        ITextElement textElement = element as ITextElement;
                        textElement.Text = alanDegeri;
                        activeView.GraphicsContainer.UpdateElement(textElement as IElement);
                        activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, element.Geometry.Envelope);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + ex.Source);
            }
        }
        public static void CreateTextElement(IMxDocument mxDoc, string textValue, double X, double Y)
        {

            IElement element = null;
            ITextElement textElement = null;
            IMarkerSymbol sembol = new SimpleMarkerSymbolClass();
            IRgbColor color = new RgbColorClass();
            color.Blue = 255;
            sembol.Color = color;
            sembol.Size = 6;


            if (textValue != null)
            {
                element = new MarkerElementClass();

                textElement = new TextElementClass();

                IPoint point = new PointClass();

                point.PutCoords(X, Y);

                point.SpatialReference = mxDoc.FocusMap.SpatialReference;
                (element as IMarkerElement).Symbol = sembol;
                element.Geometry = point;

                IElement txtelement = textElement as IElement;
                txtelement.Geometry = point;

                textElement.Text = textValue;

                mxDoc.ActiveView.GraphicsContainer.AddElement(element, 0);
                mxDoc.ActiveView.GraphicsContainer.AddElement(txtelement, 0);
            }

            mxDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, element, mxDoc.ActiveView.Extent);

        }
        #endregion

        #region AcessData      
        public static IMaps MapleriGetir(IApplication application)
        {
            try
            {
                IMxDocument mxdoc = application.Document as IMxDocument;
                return mxdoc.Maps;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }      
        public static IMap MapAl(IMaps maps, int index)
        {
            try
            {
                if (index < maps.Count)
                    return maps.get_Item(index);
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
                throw new ApplicationException(ex.ToString());
            }
        }
        public static ILayer LayerAl(IMap map, int index)
        {
            try
            {
                if (index < map.LayerCount)
                    return map.get_Layer(index);
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
                throw new ApplicationException(ex.ToString());
            }
        }
        public static IFeatureClass FeatureClassAl(ILayer layer)
        {
            try
            {
                if (layer is IFeatureLayer)
                {
                    IFeatureLayer featurelayer = layer as IFeatureLayer; //QI
                    return featurelayer.FeatureClass; //inheritance interface
                    //return (layer as IFeatureLayer).FeatureClass;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
                throw new ApplicationException(ex.ToString());
            }
        }       
        public static void CreateNewWorkspace(IApplication m_application, string parentFolder, string workSpcName)
        {
            try
            {
                ShapefileWorkspaceFactory shapefileworkspacefactory = new ShapefileWorkspaceFactoryClass();
                IWorkspaceName workspaceName = shapefileworkspacefactory.Create(parentFolder, workSpcName, null, 0);
                IName name = workspaceName as IName;
                IWorkspace workspace = name.Open() as IWorkspace;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }
        public IFeatureClass GetFeatureClass(IWorkspace workspace, string featClassName)
        {
            try
            {
                IFeatureWorkspace featWork = workspace as IFeatureWorkspace;
                IFeatureClass featClass = featWork.OpenFeatureClass(featClassName);
                return featClass;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static void AddShapeFileToMap(IApplication m_application, string workspacePath, string featureClassName)
        {
            try
            {
                // Open feature workspace...
                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactoryClass();
                IFeatureWorkspace featureWorkspace = workspaceFactory.OpenFromFile(workspacePath, 0) as IFeatureWorkspace;

                // Open featureClass...
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(featureClassName);

                // Create featureLayer...
                IFeatureLayer featureLayer = new FeatureLayerClass();
                featureLayer.FeatureClass = featureClass;
                featureLayer.Name = featureClass.AliasName;

                // Add featureLayer to map...
                IMxDocument mxDoc = m_application.Document as IMxDocument;
                IMap map = mxDoc.FocusMap;
                map.AddLayer(featureLayer);

                // Refresh...
                mxDoc.ActiveView.Refresh();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }
        public static IFeatureClass AccessWorkspacedenFeatureClassAl(string path, string fcName)
        {
            try
            {
                IWorkspaceFactory wsf = new AccessWorkspaceFactoryClass();
                IWorkspace ws = wsf.OpenFromFile(path, 0);

                IFeatureWorkspace fw = ws as IFeatureWorkspace; //QI
                IFeatureClass fc = fw.OpenFeatureClass(fcName);

                return fc;
            }
            catch (Exception ex)
            {
                MessageBoxGoster(ex.ToString(), "Hata");
                return null;
            }
        }
        public static void FeatureClassAddToMap
            (IMap map, IFeatureClass fc, string name, bool showtip)
        {
            try
            {
                IFeatureLayer featurelayer = new FeatureLayerClass();
                featurelayer.FeatureClass = fc;

                featurelayer.ShowTips = showtip;
                featurelayer.Name = name;

                map.AddLayer(featurelayer as ILayer);
            }
            catch (Exception ex)
            {
                MessageBoxGoster(ex.ToString(), "Hata");
            }
        }
        public static IWorkspace GetSDEWorkspace(string serverName, string instance, string user,
          string password, string database, string version)
        {
            try
            {
                IPropertySet propertySet = new PropertySetClass();
                /*Set Properties to PropertySet*/
                propertySet.SetProperty("Server", serverName);
                propertySet.SetProperty("Instance", instance);
                propertySet.SetProperty("user", user);
                propertySet.SetProperty("password", password);
                propertySet.SetProperty("Database", database);
                propertySet.SetProperty("version", version);

                IWorkspaceFactory workspaceFactory = new SdeWorkspaceFactoryClass();
                IWorkspace workspace = workspaceFactory.Open(propertySet, 0);

                return workspace;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("SDE ye Baglanilamadi=" + ex.ToString());
            }
        }
        #endregion

        #region WorkingWithGeometry
         public static void FlashGeometry(IGeometry geometry, IRgbColor color,
          IDisplay display, Int32 delay)
        {
            if (geometry == null || color == null || display == null)
            {
                return;
            }

            display.StartDrawing(display.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache); // Explicit Cast


            switch (geometry.GeometryType)
            {
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                    {
                        //Set the flash geometry's symbol.
                        ESRI.ArcGIS.Display.ISimpleFillSymbol simpleFillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
                        simpleFillSymbol.Color = color;
                        ESRI.ArcGIS.Display.ISymbol symbol = simpleFillSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                        symbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;

                        //Flash the input polygon geometry.
                        display.SetSymbol(symbol);
                        display.DrawPolygon(geometry);
                        System.Threading.Thread.Sleep(delay);
                        display.DrawPolygon(geometry);
                        break;
                    }

                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                    {
                        //Set the flash geometry's symbol.
                        ESRI.ArcGIS.Display.ISimpleLineSymbol simpleLineSymbol = new ESRI.ArcGIS.Display.SimpleLineSymbolClass();
                        simpleLineSymbol.Width = 4;
                        simpleLineSymbol.Color = color;
                        ESRI.ArcGIS.Display.ISymbol symbol = simpleLineSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                        symbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;

                        //Flash the input polyline geometry.
                        display.SetSymbol(symbol);
                        display.DrawPolyline(geometry);
                        System.Threading.Thread.Sleep(delay);
                        display.DrawPolyline(geometry);
                        break;
                    }

                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                    {
                        //Set the flash geometry's symbol.
                        ESRI.ArcGIS.Display.ISimpleMarkerSymbol simpleMarkerSymbol = new ESRI.ArcGIS.Display.SimpleMarkerSymbolClass();
                        simpleMarkerSymbol.Style = ESRI.ArcGIS.Display.esriSimpleMarkerStyle.esriSMSCircle;
                        simpleMarkerSymbol.Size = 12;
                        simpleMarkerSymbol.Color = color;
                        ESRI.ArcGIS.Display.ISymbol symbol = simpleMarkerSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                        symbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;

                        //Flash the input point geometry.
                        display.SetSymbol(symbol);
                        display.DrawPoint(geometry);
                        System.Threading.Thread.Sleep(delay);
                        display.DrawPoint(geometry);
                        break;
                    }

                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultipoint:
                    {
                        //Set the flash geometry's symbol.
                        ESRI.ArcGIS.Display.ISimpleMarkerSymbol simpleMarkerSymbol = new ESRI.ArcGIS.Display.SimpleMarkerSymbolClass();
                        simpleMarkerSymbol.Style = ESRI.ArcGIS.Display.esriSimpleMarkerStyle.esriSMSCircle;
                        simpleMarkerSymbol.Size = 12;
                        simpleMarkerSymbol.Color = color;
                        ESRI.ArcGIS.Display.ISymbol symbol = simpleMarkerSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                        symbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;

                        //Flash the input multipoint geometry.
                        display.SetSymbol(symbol);
                        display.DrawMultipoint(geometry);
                        System.Threading.Thread.Sleep(delay);
                        display.DrawMultipoint(geometry);
                        break;
                    }
            }
            display.FinishDrawing();
        }

        public static IArea GetPolygonProperties(IApplication m_application)
        {
            try
            {
                UID editorUID = new UIDClass();
                editorUID.Value = "esriEditor.Editor";
                IEditor editor = m_application.FindExtensionByCLSID(editorUID) as IEditor;

                if (editor.SelectionCount != 1)
                    return null;

                IEnumFeature selectedFeatures = editor.EditSelection;
                IFeature feature = selectedFeatures.Next();
                IArea area = null;

                while (feature != null)
                {
                    if (feature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                        area = feature.Shape as IArea;
                    else if (feature.Shape.GeometryType != esriGeometryType.esriGeometryPolygon)
                        return null;

                    feature = selectedFeatures.Next();
                }
                return area;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
                return null;
            }
        }

        /*buffer icin*/
        /*(1)Map uzerinde tıklanan noktanın map deki gercek koordinatlarına donusturu*/
        public static IPoint EkranKoordinatlariniMapKoordinatlarinaCevir(IActiveView activeView,
            int x, int y)
        {
            try
            {
                IPoint point = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);               
                return point;
            }
            catch (Exception ex) { return null; MessageBoxGoster(ex.ToString(), "Hata"); }
        }
        /*(2)*/
        public static  IFeatureCursor SpatialFilterPerformToLayer(IFeatureLayer flayer,
            IPoint apoint)
        {
            try
            {
                ISpatialFilter spatialFilter = new SpatialFilterClass();
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                spatialFilter.Geometry = apoint;                
                IFeatureCursor fCursor = flayer.Search(spatialFilter, true);                
                return fCursor;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }
        /*(3)*/
        public static string FeaturelayerPojectedMi(IFeatureLayer flayer)
        {
            IGeoDataset geodataset = flayer as IGeoDataset;
            ISpatialReference sf = geodataset.SpatialReference;
            if (sf is IProjectedCoordinateSystem)
            {
                return (sf as IProjectedCoordinateSystem).CoordinateUnit.Name;
            }
            else
            {
                return null;
            }
        }
        /*(4)*/
        public static string GetFeatureValue(IFeatureCursor fCursor, string fieldName)
        {
            IFeature feature = fCursor.NextFeature();
            return feature.get_Value(feature.Fields.FindField(fieldName)).ToString();
        }
        /*(5)*/
        public static void SecilenDetayaBufferUygula(IMap map, double bufferDistance,
            IFeature feature,IPoint apoint)
        {                  
            try
            {
                ITopologicalOperator topoOp = feature.Shape as ITopologicalOperator;
                IPolygon buffPoly = topoOp.Buffer(bufferDistance) as IPolygon;  
                
                ISelectionEnvironment selEnv = new SelectionEnvironmentClass();
                IRgbColor selColor = new RgbColor();

                selColor.Red = 0; //RGBColorUret metodu kullanılabilir.
                selColor.Green = 255;
                selColor.Blue = 255;
                selEnv.DefaultColor.RGB = selColor.RGB;

                map.SelectByShape(buffPoly, selEnv, false);
                selEnv.CombinationMethod = esriSelectionResultEnum.esriSelectionResultSubtract;

                map.SelectByShape(apoint, selEnv, true);
                selEnv.CombinationMethod = esriSelectionResultEnum.esriSelectionResultNew;

                
                IElement elem = new PolygonElementClass();
                elem.Geometry = buffPoly;
                IGraphicsContainer gCont = (map as IActiveView).FocusMap as IGraphicsContainer;               
                
                gCont.AddElement(elem, 0);            

                (map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, 
                    null, null);
            }
            catch (Exception ex)
            {
                MessageBoxGoster(ex.ToString(), "Hata");
            }
        }
        #endregion      

        #region Yardimci Metodlar
        public static void MessageBoxGoster(string mesaj, string baslik)
        {
            IMessageDialog msg = new MessageDialogClass();
            msg.DoModal(baslik, mesaj, "Tamam", "Vazgeç", 0);
        }
        public static IRgbColor RGBColorUret(int kirmizi, int yesil, int mavi)
        {
            IRgbColor rgb = new RgbColorClass();
            rgb.Red = kirmizi;
            rgb.Green = yesil;
            rgb.Blue = mavi;
            return rgb;
        }
        #endregion      

        #region "Cut Polygon"
        // ArcGIS Snippet Title: 
        // Cut Polygon
        //
        // Add the following references to the project:
        // ESRI.ArcGIS.Geometry
        // ESRI.ArcGIS.System
        // 
        // Intended ArcGIS Products for this snippet:
        // ArcGIS Desktop
        // ArcGIS Engine
        // ArcGIS Server
        //
        // Required ArcGIS Extensions:
        // (NONE)
        //
        // Notes:
        // This snippet is intended to be inserted at the base level of a Class.
        // It is not intended to be nested within an existing Method.
        //
        // GUID:
        // {B1D0DCD6-7C5D-41c3-BAEC-05D2D21A679C}
        //
        // Use the following XML documentation comments to use this snippet:
        /// <summary>Use a polyline to cut a polygon.</summary>
        ///
        /// <param name="polygon">An IPolygon interface that is the polygon to be split.</param>
        /// <param name="polyline">An IPolyline interface that is used to cut the input polygon.</param>
        /// 
        /// <returns>An IGeometryCollection with polygons resulting from the split.</returns>
        /// 
        /// <remarks></remarks>
        public ESRI.ArcGIS.Geometry.IGeometryCollection CutPolygon(ESRI.ArcGIS.Geometry.IPolygon polygon, ESRI.ArcGIS.Geometry.IPolyline polyline)
        {
            if (polygon == null || polyline == null)
            {
                return null;
            }
            ESRI.ArcGIS.Geometry.ITopologicalOperator4 topologicalOperator4 = polygon as ESRI.ArcGIS.Geometry.ITopologicalOperator4; // Dynamic Cast
            ESRI.ArcGIS.Geometry.IGeometryCollection geometryCollection = topologicalOperator4.Cut2(polyline);

            return geometryCollection;
        }
        #endregion    

        #region "Draw Polygon"
        // ArcGIS Snippet Title: 
        // Draw Polygon
        // 
        // Add the following references to the project:
        // ESRI.ArcGIS.Carto
        // ESRI.ArcGIS.Display
        // ESRI.ArcGIS.Geometry
        // ESRI.ArcGIS.System
        // 
        // Intended ArcGIS Products for this snippet:
        // ArcGIS Desktop
        // ArcGIS Engine
        // ArcGIS Server
        //
        // Required ArcGIS Extensions:
        // (NONE)
        //
        // Notes:
        // This snippet is intended to be inserted at the base level of a Class.
        // It is not intended to be nested within an existing Method.
        //
        // Use the following XML documentation comments to use this snippet:
        /// <summary>Draws a polygon on the screen in the ActiveView where the mouse is clicked. The X and Y coordinates come from a mouse down click when the user is interacting with the application.</summary>
        ///
        /// <param name="activeView">An IActiveView interface</param>
        /// 
        /// <remarks>Ideally, this function would be called from within the OnMouseDown event that was created with the ArcGIS base tool template. The X and Y values would come from the OnMouseDown e.X and e.Y values.</remarks>
        public void DrawPolygon(ESRI.ArcGIS.Carto.IActiveView activeView)
        {

            if (activeView == null)
            {
                return;
            }

            ESRI.ArcGIS.Display.IScreenDisplay screenDisplay = activeView.ScreenDisplay;

            // Constant
            screenDisplay.StartDrawing(screenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache); // Explicit Cast
            ESRI.ArcGIS.Display.IRgbColor rgbColor = new ESRI.ArcGIS.Display.RgbColorClass();
            rgbColor.Red = 255;

            ESRI.ArcGIS.Display.IColor color = rgbColor; // Implicit Cast
            ESRI.ArcGIS.Display.ISimpleFillSymbol simpleFillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
            simpleFillSymbol.Color = color;

            ESRI.ArcGIS.Display.ISymbol symbol = simpleFillSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
            ESRI.ArcGIS.Display.IRubberBand rubberBand = new ESRI.ArcGIS.Display.RubberPolygonClass();
            ESRI.ArcGIS.Geometry.IGeometry geometry = rubberBand.TrackNew(screenDisplay, symbol);
            screenDisplay.SetSymbol(symbol);
            screenDisplay.DrawPolygon(geometry);
            screenDisplay.FinishDrawing();
        }
        #endregion             

                
    } //end class
} //end name space
