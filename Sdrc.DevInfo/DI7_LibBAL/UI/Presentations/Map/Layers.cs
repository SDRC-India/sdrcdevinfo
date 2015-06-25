using System;
using System.Collections;
using System.Text;
using System.Xml.Serialization;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    public class Layers : CollectionBase, ICloneable
    {

        public Layers()
            : base()
        {
        }

        public void Add(Layer p_Layer)
        {
            List.Add(p_Layer);
        }

        public Layer AddShapeFile(string SrcPath, string FileName)
        {
            Layer Lyr = new Layer();
            ShapeInfo _ShapeInfo;
            //ShapeFileReader sfr = new ShapeFileReader();
            try
            {
                _ShapeInfo = ShapeFileReader.GetShapeInfo(SrcPath, FileName);
            }
            catch (Exception ex)
            {
                //*** Invalid Shape file Duplicate Ids / invalid Id field etc
                return null;
            }
            //sfr = null;

            //*** Bugfix 21 Sep 2006 '*** Invalid Shape file Duplicate Ids
            if ((_ShapeInfo == null))
                return null;

            {
                Lyr.ID = FileName;
                Lyr.LayerName = FileName;
                Lyr.LayerPath = SrcPath;
                Lyr.SourceType = SourceType.Shapefile;
                Lyr.Extent = _ShapeInfo.Extent;
                Lyr.RecordCount = _ShapeInfo.RecordCount;
                switch (_ShapeInfo.ShapeType)
                {
                    case ShapeType.Point:
                        Lyr.LayerType = ShapeType.PointCustom;
                        break;
                    case ShapeType.Polygon:
                        Lyr.LayerType = ShapeType.PolygonCustom;
                        break;
                    case ShapeType.PolyLine:
                        Lyr.LayerType = ShapeType.PolyLineCustom;
                        break;
                }
                Lyr.Records = _ShapeInfo.Records;
            }
            List.Add(Lyr);
            return Lyr;
        }

        public Layer AddSpatialLayer(string p_SpatialMapPath, string p_LayerName)
        {
            Layer Lyr = new Layer();
            ShapeInfo _ShapeInfo;
            // ShapeFileReader sfr = new ShapeFileReader();
            //Checking if same Layer already exists in Layers Collection, if yes return the same..
            if (this[p_LayerName] == null)
            {
                
                _ShapeInfo = ShapeFileReader.GetShapeInfo(p_SpatialMapPath, p_LayerName);
                // sfr = null;
                //*** BugFix 15 May 2006 Problem: Error while reading shape file information simply ignore it.
                if ((_ShapeInfo != null))
                {
                    {
                        Lyr.ID = p_LayerName;
                        Lyr.LayerName = p_LayerName;
                        Lyr.LayerPath = p_SpatialMapPath;
                        Lyr.SourceType = SourceType.Database;
                        Lyr.Extent = _ShapeInfo.Extent;
                        Lyr.RecordCount = _ShapeInfo.RecordCount;
                        Lyr.LayerType = _ShapeInfo.ShapeType;
                        Lyr.Records = _ShapeInfo.Records;
                    }
                    List.Add(Lyr);
                }
                else
                {
                    Lyr = null;    
                }
                _ShapeInfo = null;
            }
            else
            {
                Lyr = this[p_LayerName];    //Return same Layer that was already existed in collection
            }
            return Lyr;
        }

        public void Remove(object p_Index)
        {
            List.Remove(p_Index);
        }

        public new int Count
        {
            get { return List.Count; }
        }

        public int RecordCounts()
        {
            int RetVal = 0;
            try
            {
                foreach (Layer _Layer in List)
                {
                    switch (_Layer.LayerType)
                    {
                        case ShapeType.Point:
                        case ShapeType.PolyLine:
                        case ShapeType.Polygon:
                            RetVal += _Layer.Records.Count;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return RetVal;
        }

        public int RecordCounts(string p_SourceDir)
        {
            int RetVal = 0;
            foreach (Layer _Layer in List)
            {
                switch (_Layer.LayerType)
                {
                    case ShapeType.Point:
                    case ShapeType.Polygon:
                        RetVal += _Layer.GetRecords(p_SourceDir + "\\" + _Layer.ID).Count;
                        break;
                }
            }
            return RetVal;
        }

        public Layer this[int p_Index]
        {
            get { return (Layer)List[p_Index]; }
        }

        public Layer this[string p_Id]
        {
            get
            {
                Layer RetVal = null;
                foreach (Layer _layer in List)
                {
                    if (_layer.ID == p_Id)
                    {
                        RetVal = _layer;
                        break;
                    }
                }
                return RetVal;
            }
        }


        public Layer[] this[DateTime p_Date]
        {
            get
            {
                Layer[] _Layers = null;

                foreach (Layer _layer in List)
                {
                    switch (_layer.LayerType)
                    {
                        case ShapeType.PointCustom:
                        case ShapeType.PolygonCustom:
                        case ShapeType.PolygonBuffer:
                        case ShapeType.PolyLineCustom:
                            break;
                        //Nothing to add
                        default:
                            if (_layer.StartDate <= p_Date && _layer.EndDate >= p_Date)
                            {
                                if ((_Layers == null))
                                {
                                    _Layers = new Layer[1];
                                }
                                else
                                {
                                    Layer[] _LayersTemp = new Layer[_Layers.Length + 1]; // ReDimStatement Not supported in C#, so _Layers[] are copied into Temp Array, then Temp array elements are copied back into original Array. 
                                    Array.Copy(_Layers, _LayersTemp, _Layers.Length);
                                    _Layers = _LayersTemp;     //_Temp Aarray elememts are copied back into _layers[]
                                }
                                _Layers[_Layers.Length - 1] = _layer;
                            }

                            break;
                    }
                }
                return _Layers;
            }
        }

        public void MoveToTop(int p_Index)
        {
            if (p_Index > 0)
            {
                List.Insert(0, List[p_Index]);
                List.RemoveAt(p_Index + 1);
            }
        }

        public void MoveToBottom(int p_Index)
        {
            if (p_Index < List.Count - 1)
            {
                List.Add(List[p_Index]);
                List.RemoveAt(p_Index);
            }
        }

        public void MoveUp(int p_Index)
        {
            if (p_Index > 0)
            {
                List.Insert(p_Index - 1, List[p_Index]);
                List.RemoveAt(p_Index + 1);
            }
        }

        public void MoveDown(int p_Index)
        {
            if (p_Index < List.Count - 1)
            {
                List.Insert(p_Index + 2, List[p_Index]);
                List.RemoveAt(p_Index);
            }
        }

        public void Move(int p_FromIndex, int p_ToIndex)
        {
            if (p_FromIndex < p_ToIndex)
            {
                if (p_ToIndex > 0)
                {
                    List.Insert(p_ToIndex, List[p_FromIndex]);
                    List.RemoveAt(p_FromIndex + 1);
                }
            }
            else
            {
                if (p_ToIndex < List.Count - 1)
                {
                    List.Insert(List.Count - 1, List[p_FromIndex]);
                    List.RemoveAt(p_FromIndex);

                }
            }
        }

        public int LayerIndex(string p_LayerID)
        {
            int RetVal = -1;
            for (int i = 0; i <= List.Count - 1; i++)
            {
                {
                    if (((Layer)List[i]).ID == p_LayerID)
                    {
                        RetVal = i;
                    }
                }
            }
            return RetVal;
        }

        public object Clone()
        {
            object RetVal = null;
            try
            {
                //*** Serialization is one way to do deep cloning. It works only if the objects and its references are serializable
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter oBinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                System.IO.MemoryStream oMemStream = new System.IO.MemoryStream();
                oBinaryFormatter.Serialize(oMemStream, this);
                oMemStream.Position = 0;
                RetVal = (Layers)oBinaryFormatter.Deserialize(oMemStream);
                oMemStream.Close();
                oMemStream.Dispose();
                oMemStream = null;
            }
            catch
            {

            }
            return RetVal;
        }

    }
}