using System.Drawing;
using System.Collections;
using System.Data;
using System.IO;
using System;
using System.Xml.Serialization;


namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    public class ShapeFileReader
    {

        # region "Structures"
        private enum FILESIZES
        {
            MainHeaderSize = 100,
            //in bytes
            IndexRecSize = 8,
            //in bytes
            ShpRecSize = 8
        }

        public enum ShapeType
        {
            NullShape = 0,
            Point = 1,
            PolyLine = 3,
            Polygon = 5,
            MultiPoint = 8,
            PointZ = 11,
            PolyLineZ = 13,
            PolygonZ = 15,
            MultiPointZ = 18,
            PointM = 21,
            PolyLineM = 23,
            PolygonM = 25,
            MultiPointM = 28,
            MultiPatch = 31
        }

        public struct FileHeader
        {
            //big Byte 0 Unused
            public int FileCode;
            //big Byte 4 Unused
            public int u1;
            //big Byte 8 Unused
            public int u2;
            //big Byte 12 Unused
            public int u3;
            //big Byte 16 Unused
            public int u4;
            //big Byte 20 Unused
            public int u5;
            //big Byte 24 Unused
            public int FileLength;
            public int Version;
            public int ShapeType;
            public double BoundingBoxXMin;
            public double BoundingBoxYMin;
            public double BoundingBoxXMax;
            public double BoundingBoxYMax;
            public double BoundingBoxZMin;
            public double BoundingBoxZMax;
            public double BoundingBoxMMin;
            public double BoundingBoxMMax;
        }

        public struct SHPRecordHeader
        {
            //*** Struture for SHP Record header
            //big
            public int RecordNumber;
            //big
            public int ContentLength;
        }

        public struct SHXRecordHeader
        {
            //*** Structure for SHX Record Header
            //big
            public int Offset;
            //big
            public int ContentLength;
        }

        //Public Structure Shapefile_Polygon
        // Public ShapeType As Integer
        // Public BoundingBox() As Double
        // Public NumParts As Integer
        // Public NumPoints As Integer
        // Public Parts() As Integer
        // Public Points() As Integer
        //End Structure


        public struct DBF_Field_Header
        {
            public string FieldName;
            public char FieldType;
            public byte FieldLength;
        }

        # endregion

        #region "Constants"
        /// <summary>
        /// ID Field length in DevInfo compatible DBF file.
        /// </summary>
        public const int ID_FIELD_LENGTH = 255;

        /// <summary>
        /// ID Field Name in DevInfo compatible DBF file.
        /// </summary>
        public const string ID_FIELD_NAME = "ID_";

        /// <summary>
        /// Name1_ Field length in DevInfo compatible DBF file, same as AreaName length as in DevInfo Database.
        /// </summary>
        public const int NAME1_FIELD_LENGTH = 60;

        /// <summary>
        /// Name1_ Field name in DevInfo compatible DBF file.
        /// </summary>
        public const string NAME1_FIELD_NAME = "NAME1_";

        #endregion

        # region "Shape File Creation"

        //*** BugFix 13 June 2006 Block File Creation
        //*** Create Shape File for selected polygon from multiple shape file - Used in Block creation process
        public static void CreateBlockFile(string sBlockId, string sBlockName, Hashtable Area_Shp, string DestFilePath, string DestFileName)
        {
            ShapeType ShapeType = ShapeType.Polygon;
            CreateBlockFile(sBlockId, sBlockName, Area_Shp, DestFilePath, DestFileName, ShapeType);

        }

        public static void CreateBlockFile(string sBlockId, string sBlockName, Hashtable Area_Shp, string DestFilePath, string DestFileName, ShapeType ShapeType)
        {
            Hashtable Shp_Area = new Hashtable();

            //- Get Encoding used in Current CultureInfo of OS.
            System.Text.Encoding FileEncoding = System.Text.Encoding.Default;

            //*** Build ShapeFile - AreaId Collection with unique Shape and comma delimited Areaids
            IDictionaryEnumerator dicEnumerator = Area_Shp.GetEnumerator();
            while (dicEnumerator.MoveNext())
            {
                if (Shp_Area.ContainsKey(dicEnumerator.Value))
                {
                    Shp_Area[dicEnumerator.Value] = Shp_Area[dicEnumerator.Value].ToString() + "," + dicEnumerator.Key.ToString();
                }
                else
                {
                    Shp_Area.Add(dicEnumerator.Value, dicEnumerator.Key);
                }
            }


            FileHeader SHPFileHeader = new FileHeader();
            FileStream oShpStream = new FileStream(DestFilePath + "\\" + DestFileName + ".shp", FileMode.Create);
            BinaryWriter oShpBinaryWriter = new BinaryWriter(oShpStream);

            FileStream oShxStream = new FileStream(DestFilePath + "\\" + DestFileName + ".shx", FileMode.Create);
            BinaryWriter oShxBinaryWriter = new BinaryWriter(oShxStream);

            FileStream oDbfStream = new FileStream(DestFilePath + "\\" + DestFileName + ".dbf", FileMode.Create);
            BinaryWriter oDbfBinaryWriter = new BinaryWriter(oDbfStream, FileEncoding);


            SHPFileHeader.FileCode = BigToLittleEndian(9994);
            SHPFileHeader.Version = 1000;
            SHPFileHeader.ShapeType = (int)ShapeType;
            ShapeFileHeaderWriter(ref oShpBinaryWriter, SHPFileHeader);
            //*** Write .Shp File Header
            ShapeFileHeaderWriter(ref oShxBinaryWriter, SHPFileHeader);
            //*** Write .Shx File Header
            DbaseFileHeaderWriter(ref oDbfBinaryWriter, 1, FileEncoding);
            //Area_Shp.Count '*** Write .dbf File Header


            int i;
            int j;
            string[] AreaId;
            string SrcFile;
            string SrcFilePath;
            string SrcFileName;
            ShapeInfo ShpInfo;
            Shape _Shape = new Shape();
            Shape _Block = new Shape();
            RectangleF _Extent = new RectangleF();

            PointF[] Pts;
            int Offset = 50;
            int RecordNumber;
            int ContentLength;
            int Points;
            int BlockPointCount = 0;
            int[] BlockPartOffset = null;

            // Iterate for all distinct shape files to
            //      set single composite multipart polygon containing all distinct shapes in refrred shapefiles
            //      set composite extent 

            dicEnumerator = Shp_Area.GetEnumerator();
            while (dicEnumerator.MoveNext())
            {
                SrcFile = dicEnumerator.Key.ToString();
                AreaId = dicEnumerator.Value.ToString().Split(","[0]);

                if (File.Exists(SrcFile))
                {
                    SrcFilePath = Path.GetDirectoryName(SrcFile);
                    SrcFileName = Path.GetFileNameWithoutExtension(SrcFile);
                    ShpInfo = GetShapeInfo(SrcFilePath, SrcFileName);
                    switch (ShapeType)
                    {
                        case ShapeType.Point:
                            //TODO
                            break;
                        case ShapeType.Polygon:
                        case ShapeType.PolyLine:

                            // Iterate for each shape
                            IDictionaryEnumerator dicShapeEnumerator = ShpInfo.Records.GetEnumerator();
                            while (dicShapeEnumerator.MoveNext())
                            {

                                _Shape = (Shape)dicShapeEnumerator.Value;
                                if (_Extent.IsEmpty)
                                {
                                    _Extent = _Shape.Extent;
                                }
                                else
                                {
                                    _Extent = RectangleF.Union(_Extent, _Shape.Extent);
                                }
                                int iPrevPartsCount = _Block.Parts.Count;

                                int[] temp = new int[iPrevPartsCount + _Shape.Parts.Count];

                                if (BlockPartOffset != null)
                                    Array.Copy(BlockPartOffset, temp, Math.Min(BlockPartOffset.Length, temp.Length));

                                BlockPartOffset = temp;

                                Points = 0;

                                // Iterate for each part
                                for (i = 0; i <= _Shape.Parts.Count - 1; i++)
                                {
                                    Pts = (PointF[])_Shape.Parts[i];
                                    _Block.Parts.Add(Pts);
                                    BlockPartOffset[iPrevPartsCount + i] = Pts.Length;
                                    Points += Pts.Length;
                                    BlockPointCount += Pts.Length;
                                }
                            }
                            break;
                    }
                }
            }

            _Block.Extent = _Extent;
            RecordNumber = 1;
            //*** All Shape will be added as parts of a single block
            ContentLength = ((11 + _Block.Parts.Count) * 2) + (BlockPointCount * 8);
            PolygonRecordWriter(ref oShpBinaryWriter, RecordNumber, ContentLength, BlockPointCount, BlockPartOffset, _Block, SHPFileHeader.ShapeType);
            IndexRecordWriter(ref oShxBinaryWriter, Offset, ContentLength);
            //*** Write SHX Record
            DbaseRecordWriter(ref oDbfBinaryWriter, sBlockId, sBlockName);
            //*** Write DBF Record

            if (ShapeType == ShapeType.Polygon | ShapeType == ShapeType.PolyLine)
            {
                SHPFileHeader.BoundingBoxXMin = _Extent.X;
                SHPFileHeader.BoundingBoxYMin = _Extent.Y;
                SHPFileHeader.BoundingBoxXMax = _Extent.X + _Extent.Width;
                SHPFileHeader.BoundingBoxYMax = _Extent.Y + _Extent.Height;
            }

            //*** Reset FileLength and Extent in Shp File Header
            oShpStream.Position = 24;
            oShpBinaryWriter.Write(BigToLittleEndian((int)oShpStream.Length / 2));
            oShpStream.Position = 36;
            oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxXMin);
            oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxYMin);
            oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxXMax);
            oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxYMax);

            //*** Reset FileLength and Extent in Shx File Header
            oShxStream.Position = 24;
            oShxBinaryWriter.Write(BigToLittleEndian((int)oShxStream.Length / 2));
            oShxStream.Position = 36;
            oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxXMin);
            oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxYMin);
            oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxXMax);
            oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxYMax);


            //*** Dispose
            oShpBinaryWriter.Close();
            oShxBinaryWriter.Close();
            oDbfBinaryWriter.Close();
            oShpStream.Close();
            oShxStream.Close();
            oDbfStream.Close();
            oShpBinaryWriter = null;
            oShxBinaryWriter = null;
            oDbfBinaryWriter = null;
            // _Extent = null;
            ShpInfo = null;
        }

        //public static void CreateBlockFile(string sBlockId, string sBlockName, Hashtable Area_Shp, string DestFilePath, string DestFileName, ShapeType ShapeType)
        //{
        //    Hashtable Shp_Area = new Hashtable();

        //    //- Get Encoding used in Current CultureInfo of OS.
        //    System.Text.Encoding FileEncoding = System.Text.Encoding.Default;

        //    //*** Build ShapeFile - AreaId Collection with unique Shape and comma delimited Areaids
        //    IDictionaryEnumerator dicEnumerator = Area_Shp.GetEnumerator();
        //    while (dicEnumerator.MoveNext())
        //    {
        //        if (Shp_Area.ContainsKey(dicEnumerator.Value))
        //        {
        //            Shp_Area[dicEnumerator.Value] = Shp_Area[dicEnumerator.Value].ToString() + "," + dicEnumerator.Key.ToString();
        //        }
        //        else
        //        {
        //            Shp_Area.Add(dicEnumerator.Value, dicEnumerator.Key);
        //        }
        //    }


        //    FileHeader SHPFileHeader = new FileHeader();
        //    FileStream oShpStream = new FileStream(DestFilePath + "\\" + DestFileName + ".shp", FileMode.Create);
        //    BinaryWriter oShpBinaryWriter = new BinaryWriter(oShpStream);

        //    FileStream oShxStream = new FileStream(DestFilePath + "\\" + DestFileName + ".shx", FileMode.Create);
        //    BinaryWriter oShxBinaryWriter = new BinaryWriter(oShxStream);

        //    FileStream oDbfStream = new FileStream(DestFilePath + "\\" + DestFileName + ".dbf", FileMode.Create);
        //    BinaryWriter oDbfBinaryWriter = new BinaryWriter(oDbfStream, FileEncoding);


        //    SHPFileHeader.FileCode = BigToLittleEndian(9994);
        //    SHPFileHeader.Version = 1000;
        //    SHPFileHeader.ShapeType = (int)ShapeType;
        //    ShapeFileHeaderWriter(ref oShpBinaryWriter, SHPFileHeader);
        //    //*** Write .Shp File Header
        //    ShapeFileHeaderWriter(ref oShxBinaryWriter, SHPFileHeader);
        //    //*** Write .Shx File Header
        //    DbaseFileHeaderWriter(ref oDbfBinaryWriter, 1, FileEncoding);
        //    //Area_Shp.Count '*** Write .dbf File Header


        //    int i;
        //    int j;
        //    string[] AreaId;
        //    string SrcFile;
        //    string SrcFilePath;
        //    string SrcFileName;
        //    ShapeInfo ShpInfo;
        //    Shape _Shape = new Shape();
        //    Shape _Block = new Shape();
        //    RectangleF _Extent = new RectangleF();

        //    PointF[] Pts;
        //    int Offset = 50;
        //    int RecordNumber;
        //    int ContentLength;
        //    int Points;
        //    int BlockPointCount = 0;
        //    int[] BlockPartOffset = null;

        //    dicEnumerator = Shp_Area.GetEnumerator();
        //    while (dicEnumerator.MoveNext())
        //    {
        //        SrcFile = dicEnumerator.Key.ToString();
        //        AreaId = dicEnumerator.Value.ToString().Split(","[0]);

        //        if (File.Exists(SrcFile))
        //        {
        //            SrcFilePath = Path.GetDirectoryName(SrcFile);
        //            SrcFileName = Path.GetFileNameWithoutExtension(SrcFile);
        //            ShpInfo = GetShapeInfo(SrcFilePath, SrcFileName);
        //            switch (ShapeType)
        //            {
        //                case ShapeType.Point:
        //                    break;
        //                case ShapeType.Polygon:
        //                case ShapeType.PolyLine:
        //                    for (j = 0; j <= AreaId.Length - 1; j++)
        //                    {
        //                        if (ShpInfo.Records.ContainsKey(AreaId[j]))
        //                        {
        //                            _Shape = (Shape)ShpInfo.Records[AreaId[j]];
        //                            if (_Extent.IsEmpty)
        //                            {
        //                                _Extent = _Shape.Extent;
        //                            }
        //                            else
        //                            {
        //                                _Extent = RectangleF.Union(_Extent, _Shape.Extent);
        //                            }
        //                            int iPrevPartsCount = _Block.Parts.Count;

        //                            //ReDim Preserve BlockPartOffset(iPrevPartsCount + _Shape.Parts.Count - 1)


        //                            //int[] YourArray;
        //                            //...
        //                            //int[] temp = new int[i + 1];
        //                            //if (YourArray != null)
        //                            //    Array.Copy(YourArray, temp, Math.Min(YourArray.Length, temp.Length));
        //                            //YourArray = temp;

        //                            int[] temp = new int[iPrevPartsCount + _Shape.Parts.Count];

        //                            if (BlockPartOffset != null)
        //                                Array.Copy(BlockPartOffset, temp, Math.Min(BlockPartOffset.Length, temp.Length));

        //                            BlockPartOffset = temp;

        //                            Points = 0;
        //                            for (i = 0; i <= _Shape.Parts.Count - 1; i++)
        //                            {
        //                                Pts = (PointF[])_Shape.Parts[i];
        //                                _Block.Parts.Add(Pts);
        //                                BlockPartOffset[iPrevPartsCount + i] = Pts.Length;
        //                                Points += Pts.Length;
        //                                BlockPointCount += Pts.Length;
        //                            }
        //                        }
        //                    }

        //                    break;
        //            }
        //        }
        //    }

        //    _Block.Extent = _Extent;
        //    RecordNumber = 1;
        //    //*** All Shape will be added as parts of a single block
        //    ContentLength = ((11 + _Block.Parts.Count) * 2) + (BlockPointCount * 8);
        //    PolygonRecordWriter(ref oShpBinaryWriter, RecordNumber, ContentLength, BlockPointCount, BlockPartOffset, _Block, SHPFileHeader.ShapeType);
        //    IndexRecordWriter(ref oShxBinaryWriter, Offset, ContentLength);
        //    //*** Write SHX Record
        //    DbaseRecordWriter(ref oDbfBinaryWriter, sBlockId, sBlockName);
        //    //*** Write DBF Record

        //    if (ShapeType == ShapeType.Polygon | ShapeType == ShapeType.PolyLine)
        //    {
        //        SHPFileHeader.BoundingBoxXMin = _Extent.X;
        //        SHPFileHeader.BoundingBoxYMin = _Extent.Y;
        //        SHPFileHeader.BoundingBoxXMax = _Extent.X + _Extent.Width;
        //        SHPFileHeader.BoundingBoxYMax = _Extent.Y + _Extent.Height;
        //    }

        //    //*** Reset FileLength and Extent in Shp File Header
        //    oShpStream.Position = 24;
        //    oShpBinaryWriter.Write(BigToLittleEndian((int)oShpStream.Length / 2));
        //    oShpStream.Position = 36;
        //    oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxXMin);
        //    oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxYMin);
        //    oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxXMax);
        //    oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxYMax);

        //    //*** Reset FileLength and Extent in Shx File Header
        //    oShxStream.Position = 24;
        //    oShxBinaryWriter.Write(BigToLittleEndian((int)oShxStream.Length / 2));
        //    oShxStream.Position = 36;
        //    oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxXMin);
        //    oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxYMin);
        //    oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxXMax);
        //    oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxYMax);


        //    //*** Dispose
        //    oShpBinaryWriter.Close();
        //    oShxBinaryWriter.Close();
        //    oDbfBinaryWriter.Close();
        //    oShpStream.Close();
        //    oShxStream.Close();
        //    oDbfStream.Close();
        //    oShpBinaryWriter = null;
        //    oShxBinaryWriter = null;
        //    oDbfBinaryWriter = null;
        //    // _Extent = null;
        //    ShpInfo = null;
        //}



        ////*** Create Shape File for selected polygon from multiple shape file
        //[Obsolete]
        //public static void CreateShapeFile(Hashtable Area_Shp, string DestFilePath, string DestFileName, ShapeType ShapeType)
        //{
        //    Hashtable Shp_Area = new Hashtable();
        //    System.Text.Encoding FileEncoding = System.Text.Encoding.Default;

        //    //*** Build ShapeFile - AreaId Collection with unique Shape and comma delimited Areaids
        //    IDictionaryEnumerator dicEnumerator = Area_Shp.GetEnumerator();
        //    while (dicEnumerator.MoveNext())
        //    {
        //        if (Shp_Area.ContainsKey(dicEnumerator.Value))
        //        {
        //            Shp_Area[dicEnumerator.Value] = Shp_Area[dicEnumerator.Value].ToString() + "," + dicEnumerator.Key.ToString();
        //        }
        //        else
        //        {
        //            Shp_Area.Add(dicEnumerator.Value, dicEnumerator.Key);
        //        }
        //    }


        //    FileHeader SHPFileHeader = new FileHeader();
        //    FileStream oShpStream = new FileStream(DestFilePath + "\\" + DestFileName + ".shp", FileMode.Create);
        //    BinaryWriter oShpBinaryWriter = new BinaryWriter(oShpStream);

        //    FileStream oShxStream = new FileStream(DestFilePath + "\\" + DestFileName + ".shx", FileMode.Create);
        //    BinaryWriter oShxBinaryWriter = new BinaryWriter(oShxStream);

        //    FileStream oDbfStream = new FileStream(DestFilePath + "\\" + DestFileName + ".dbf", FileMode.Create);
        //    BinaryWriter oDbfBinaryWriter = new BinaryWriter(oDbfStream, FileEncoding);


        //    SHPFileHeader.FileCode = BigToLittleEndian(9994);
        //    SHPFileHeader.Version = 1000;
        //    SHPFileHeader.ShapeType = (int)ShapeType;
        //    ShapeFileHeaderWriter(ref oShpBinaryWriter, SHPFileHeader);
        //    //*** Write .Shp File Header
        //    ShapeFileHeaderWriter(ref oShxBinaryWriter, SHPFileHeader);
        //    //*** Write .Shx File Header
        //    DbaseFileHeaderWriter(ref oDbfBinaryWriter, Area_Shp.Count, FileEncoding);
        //    //*** Write .dbf File Header


        //    int i;
        //    int j;
        //    string[] AreaId;
        //    string SrcFile;
        //    string SrcFilePath;
        //    string SrcFileName;
        //    ShapeInfo ShpInfo;
        //    Shape _Shape = new Shape();
        //    RectangleF _Extent = new RectangleF();

        //    PointF[] Pts;
        //    int Offset = 50;
        //    int RecordNumber = 0;
        //    int ContentLength;
        //    int Points;


        //    dicEnumerator = Shp_Area.GetEnumerator();
        //    while (dicEnumerator.MoveNext())
        //    {
        //        SrcFile = dicEnumerator.Key.ToString();
        //        AreaId = dicEnumerator.Value.ToString().Split(","[0]);

        //        if (File.Exists(SrcFile))
        //        {
        //            SrcFilePath = Path.GetDirectoryName(SrcFile);
        //            SrcFileName = Path.GetFileNameWithoutExtension(SrcFile);
        //            ShpInfo = GetShapeInfo(SrcFilePath, SrcFileName);
        //            switch (ShapeType)
        //            {
        //                case ShapeType.Point:
        //                    PointF Pt;
        //                    ContentLength = 10;
        //                    for (j = 0; j <= AreaId.Length - 1; j++)
        //                    {
        //                        if (ShpInfo.Records.ContainsKey(AreaId[j]))
        //                        {
        //                            _Shape = (Shape)ShpInfo.Records[AreaId[j]];
        //                            Pt = (PointF)_Shape.Parts[0];

        //                            if (RecordNumber == 0)
        //                            {
        //                                SHPFileHeader.BoundingBoxXMin = Pt.X;
        //                                SHPFileHeader.BoundingBoxYMin = Pt.Y;
        //                                SHPFileHeader.BoundingBoxXMax = Pt.X;
        //                                SHPFileHeader.BoundingBoxYMax = Pt.Y;
        //                            }
        //                            else
        //                            {
        //                                SHPFileHeader.BoundingBoxXMin = Math.Min(SHPFileHeader.BoundingBoxXMin, Pt.X);
        //                                SHPFileHeader.BoundingBoxYMin = Math.Min(SHPFileHeader.BoundingBoxYMin, Pt.Y);
        //                                SHPFileHeader.BoundingBoxXMax = Math.Max(SHPFileHeader.BoundingBoxXMax, Pt.X);
        //                                SHPFileHeader.BoundingBoxYMax = Math.Max(SHPFileHeader.BoundingBoxYMax, Pt.Y);
        //                            }

        //                            RecordNumber += 1;
        //                            oShpBinaryWriter.Write(BigToLittleEndian(RecordNumber));
        //                            oShpBinaryWriter.Write(BigToLittleEndian(ContentLength));
        //                            oShpBinaryWriter.Write((int)1);
        //                            //Shape Type
        //                            oShpBinaryWriter.Write((double)Pt.X);
        //                            oShpBinaryWriter.Write((double)Pt.Y);
        //                            IndexRecordWriter(ref oShxBinaryWriter, Offset, ContentLength);
        //                            //*** Write SHX Record
        //                            Offset += (4 + ContentLength);
        //                            //*** Add 4 for record header '*** Set Offset for next SHX record
        //                            DbaseRecordWriter(ref oDbfBinaryWriter, _Shape.AreaId, _Shape.AreaName);
        //                            //*** Write DBF Record
        //                        }
        //                    }

        //                    // Pt = null;
        //                    break;
        //                case ShapeType.Polygon:
        //                case ShapeType.PolyLine:
        //                    for (j = 0; j <= AreaId.Length - 1; j++)
        //                    {
        //                        if (ShpInfo.Records.ContainsKey(AreaId[j]))
        //                        {
        //                            _Shape = (Shape)ShpInfo.Records[AreaId[j]];
        //                            if (_Extent.IsEmpty)
        //                            {
        //                                _Extent = _Shape.Extent;
        //                            }
        //                            else
        //                            {
        //                                _Extent = RectangleF.Union(_Extent, _Shape.Extent);
        //                            }

        //                            int[] NumPoints = new int[_Shape.Parts.Count];
        //                            Points = 0;
        //                            for (i = 0; i <= _Shape.Parts.Count - 1; i++)
        //                            {
        //                                Pts = (PointF[])_Shape.Parts[i];
        //                                NumPoints[i] = Pts.Length;
        //                                Points += Pts.Length;
        //                            }
        //                            ContentLength = ((11 + _Shape.Parts.Count) * 2) + (Points * 8);
        //                            RecordNumber += 1;
        //                            PolygonRecordWriter(ref oShpBinaryWriter, RecordNumber, ContentLength, Points, NumPoints, _Shape, SHPFileHeader.ShapeType);
        //                            IndexRecordWriter(ref oShxBinaryWriter, Offset, ContentLength);
        //                            //*** Write SHX Record
        //                            Offset += (4 + ContentLength);
        //                            //*** Add 4 for record header '*** Set Offset for next SHX record
        //                            DbaseRecordWriter(ref oDbfBinaryWriter, _Shape.AreaId, _Shape.AreaName);
        //                            //*** Write DBF Record
        //                        }
        //                    }

        //                    break;
        //            }
        //        }
        //    }

        //    if (ShapeType == ShapeType.Polygon | ShapeType == ShapeType.PolyLine)
        //    {
        //        SHPFileHeader.BoundingBoxXMin = _Extent.X;
        //        SHPFileHeader.BoundingBoxYMin = _Extent.Y;
        //        SHPFileHeader.BoundingBoxXMax = _Extent.X + _Extent.Width;
        //        SHPFileHeader.BoundingBoxYMax = _Extent.Y + _Extent.Height;
        //    }

        //    //*** Reset FileLength and Extent in Shp File Header
        //    oShpStream.Position = 24;
        //    oShpBinaryWriter.Write(BigToLittleEndian((int)oShpStream.Length / 2));
        //    oShpStream.Position = 36;
        //    oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxXMin);
        //    oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxYMin);
        //    oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxXMax);
        //    oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxYMax);

        //    //*** Reset FileLength and Extent in Shx File Header
        //    oShxStream.Position = 24;
        //    oShxBinaryWriter.Write(BigToLittleEndian((int)oShxStream.Length / 2));
        //    oShxStream.Position = 36;
        //    oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxXMin);
        //    oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxYMin);
        //    oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxXMax);
        //    oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxYMax);


        //    //*** Dispose
        //    oShpBinaryWriter.Close();
        //    oShxBinaryWriter.Close();
        //    oDbfBinaryWriter.Close();
        //    oShpStream.Close();
        //    oShxStream.Close();
        //    oDbfStream.Close();
        //    oShpBinaryWriter = null;
        //    oShxBinaryWriter = null;
        //    oDbfBinaryWriter = null;
        //    // _Extent = null;
        //    ShpInfo = null;
        //}

        //[Obsolete]
        //public static void CreateShapeFile(Hashtable Area_Shp, string DestFilePath, string DestFileName)
        //{
        //    ShapeType ShapeType = ShapeType.Polygon;
        //    CreateShapeFile(Area_Shp, DestFilePath, DestFileName, ShapeType);
        //}


        //*** Create Shape File for selected polygon from single shape file - Used for Split process

        public static void CreateShapeFile(string AreaIds, string SrcFilePath, string SrcFileName, string DestFilePath, string DestFileName)
        {
            ShapeInfo ShpInfo = GetShapeInfo(SrcFilePath, SrcFileName);
            string[] AreaId;
            AreaId = AreaIds.Split(","[0]);


            FileHeader SHPFileHeader = new FileHeader();
            FileStream oShpStream = new FileStream(DestFilePath + "\\" + DestFileName + ".shp", FileMode.Create);
            BinaryWriter oShpBinaryWriter = new BinaryWriter(oShpStream);

            FileStream oShxStream = new FileStream(DestFilePath + "\\" + DestFileName + ".shx", FileMode.Create);
            BinaryWriter oShxBinaryWriter = new BinaryWriter(oShxStream);

            FileStream oDbfStream = new FileStream(DestFilePath + "\\" + DestFileName + ".dbf", FileMode.Create);

            //- Get Encoding used in source Shape file (.dbf)
            System.Text.Encoding FileEncoding = ShapeFileReader.GetEncodingInDBF(SrcFilePath + "\\" + SrcFileName + ".dbf");
            BinaryWriter oDbfBinaryWriter = new BinaryWriter(oDbfStream, FileEncoding); //System.Text.Encoding.ASCII

            SHPFileHeader.FileCode = BigToLittleEndian(9994);
            SHPFileHeader.Version = 1000;
            switch (ShpInfo.ShapeType)
            {
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.Point:
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PointCustom:
                    SHPFileHeader.ShapeType = (int)ShapeType.Point;
                    break;
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PolyLine:
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PolyLineCustom:
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PolyLineFeature:
                    SHPFileHeader.ShapeType = (int)ShapeType.PolyLine;
                    break;
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.Polygon:
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PolygonBuffer:
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PolygonCustom:
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PolygonFeature:
                    SHPFileHeader.ShapeType = (int)ShapeType.Polygon;
                    break;
            }
            ShapeFileHeaderWriter(ref oShpBinaryWriter, SHPFileHeader);
            //*** Write .Shp File Header
            ShapeFileHeaderWriter(ref oShxBinaryWriter, SHPFileHeader);
            //*** Write .Shx File Header
            DbaseFileHeaderWriter(ref oDbfBinaryWriter, AreaId.Length, FileEncoding);
            //*** Write .dbf File Header


            int i;
            int j;
            Shape _Shape = new Shape();
            PointF[] Pts;
            int Offset = 50;
            int RecordNumber = 0;
            int ContentLength;
            int Points;



            switch (SHPFileHeader.ShapeType)
            {
                case 1:
                    PointF Pt;
                    ContentLength = 10;
                    for (j = 0; j <= AreaId.Length - 1; j++)
                    {
                        if (ShpInfo.Records.ContainsKey(AreaId[j]))
                        {
                            _Shape = (Shape)ShpInfo.Records[AreaId[j]];
                            Pt = (PointF)_Shape.Parts[0];

                            if (j == 0)
                            {
                                SHPFileHeader.BoundingBoxXMin = Pt.X;
                                SHPFileHeader.BoundingBoxYMin = Pt.Y;
                                SHPFileHeader.BoundingBoxXMax = Pt.X;
                                SHPFileHeader.BoundingBoxYMax = Pt.Y;
                            }
                            else
                            {
                                SHPFileHeader.BoundingBoxXMin = Math.Min(SHPFileHeader.BoundingBoxXMin, Pt.X);
                                SHPFileHeader.BoundingBoxYMin = Math.Min(SHPFileHeader.BoundingBoxYMin, Pt.Y);
                                SHPFileHeader.BoundingBoxXMax = Math.Max(SHPFileHeader.BoundingBoxXMax, Pt.X);
                                SHPFileHeader.BoundingBoxYMax = Math.Max(SHPFileHeader.BoundingBoxYMax, Pt.Y);
                            }


                            RecordNumber += 1;
                            oShpBinaryWriter.Write(BigToLittleEndian(RecordNumber));
                            oShpBinaryWriter.Write(BigToLittleEndian(ContentLength));
                            oShpBinaryWriter.Write((int)1);
                            //Shape Type
                            oShpBinaryWriter.Write((double)Pt.X);
                            oShpBinaryWriter.Write((double)Pt.Y);
                            IndexRecordWriter(ref oShxBinaryWriter, Offset, ContentLength);
                            //*** Write SHX Record
                            Offset += (4 + ContentLength);
                            //*** Add 4 for record header '*** Set Offset for next SHX record
                            DbaseRecordWriter(ref oDbfBinaryWriter, _Shape.AreaId, _Shape.AreaName);
                            //*** Write DBF Record
                        }
                    }

                    break;

                case 3:
                case 5:
                    RectangleF _Extent = new RectangleF();
                    for (j = 0; j <= AreaId.Length - 1; j++)
                    {
                        if (ShpInfo.Records.ContainsKey(AreaId[j]))
                        {
                            _Shape = (Shape)ShpInfo.Records[AreaId[j]];
                            if (_Extent.IsEmpty)
                            {
                                _Extent = _Shape.Extent;
                            }
                            else
                            {
                                _Extent = RectangleF.Union(_Extent, _Shape.Extent);
                            }

                            int[] NumPoints = new int[_Shape.Parts.Count];
                            Points = 0;
                            for (i = 0; i <= _Shape.Parts.Count - 1; i++)
                            {
                                Pts = (PointF[])_Shape.Parts[i];
                                NumPoints[i] = Pts.Length;
                                Points += Pts.Length;
                            }
                            ContentLength = ((11 + _Shape.Parts.Count) * 2) + (Points * 8);
                            RecordNumber += 1;
                            PolygonRecordWriter(ref oShpBinaryWriter, RecordNumber, ContentLength, Points, NumPoints, _Shape, SHPFileHeader.ShapeType);
                            IndexRecordWriter(ref oShxBinaryWriter, Offset, ContentLength);
                            //*** Write SHX Record
                            Offset += (4 + ContentLength);
                            //*** Add 4 for record header '*** Set Offset for next SHX record
                            DbaseRecordWriter(ref oDbfBinaryWriter, _Shape.AreaId, _Shape.AreaName);
                            //*** Write DBF Record
                        }
                    }

                    SHPFileHeader.BoundingBoxXMin = _Extent.X;
                    SHPFileHeader.BoundingBoxYMin = _Extent.Y;
                    SHPFileHeader.BoundingBoxXMax = _Extent.X + _Extent.Width;
                    SHPFileHeader.BoundingBoxYMax = _Extent.Y + _Extent.Height;
                    // _Extent = null;
                    break;
            }

            //*** Reset FileLength and Extent in Shp File Header
            oShpStream.Position = 24;
            oShpBinaryWriter.Write(BigToLittleEndian((int)oShpStream.Length / 2));
            oShpStream.Position = 36;
            oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxXMin);
            oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxYMin);
            oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxXMax);
            oShpBinaryWriter.Write(SHPFileHeader.BoundingBoxYMax);

            //*** Reset FileLength and Extent in Shx File Header
            oShxStream.Position = 24;
            oShxBinaryWriter.Write(BigToLittleEndian((int)oShxStream.Length / 2));
            oShxStream.Position = 36;
            oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxXMin);
            oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxYMin);
            oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxXMax);
            oShxBinaryWriter.Write(SHPFileHeader.BoundingBoxYMax);


            //*** Dispose
            oShpBinaryWriter.Close();
            oShxBinaryWriter.Close();
            oDbfBinaryWriter.Close();
            oShpStream.Close();
            oShxStream.Close();
            oDbfStream.Close();
            oShpBinaryWriter = null;
            oShxBinaryWriter = null;
            oDbfBinaryWriter = null;
            ShpInfo = null;
        }

        //*** Create Shape File from Layer Object
        [Obsolete]
        public static void CreateShapefile(Layer _Layer, string FilePath, string FileName)
        {
            System.Text.Encoding FileEncoding = System.Text.Encoding.Default;

            FileHeader SHPFileHeader = new FileHeader();
            FileStream oShpStream = new FileStream(FilePath + "\\" + FileName + ".shp", FileMode.Create);
            BinaryWriter oShpBinaryWriter = new BinaryWriter(oShpStream);

            FileStream oShxStream = new FileStream(FilePath + "\\" + FileName + ".shx", FileMode.Create);
            BinaryWriter oShxBinaryWriter = new BinaryWriter(oShxStream);

            FileStream oDbfStream = new FileStream(FilePath + "\\" + FileName + ".dbf", FileMode.Create);
            BinaryWriter oDbfBinaryWriter = new BinaryWriter(oDbfStream, FileEncoding);
            SHPFileHeader.FileCode = BigToLittleEndian(9994);
            SHPFileHeader.Version = 1000;
            switch (_Layer.LayerType)
            {

                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.Point:
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PointCustom:
                    SHPFileHeader.ShapeType = (int)ShapeType.Point;
                    break;
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PolyLine:
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PolyLineCustom:
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PolyLineFeature:
                    SHPFileHeader.ShapeType = (int)ShapeType.PolyLine;
                    break;
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.Polygon:
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PolygonBuffer:
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PolygonCustom:
                case DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.PolygonFeature:
                    SHPFileHeader.ShapeType = (int)ShapeType.Polygon;
                    break;
            }
            SHPFileHeader.BoundingBoxXMin = _Layer.Extent.X;
            SHPFileHeader.BoundingBoxYMin = _Layer.Extent.Y;
            SHPFileHeader.BoundingBoxXMax = _Layer.Extent.X + _Layer.Extent.Width;
            SHPFileHeader.BoundingBoxYMax = _Layer.Extent.Y + _Layer.Extent.Height;

            ShapeFileHeaderWriter(ref oShpBinaryWriter, SHPFileHeader);
            //*** Write .Shp File Header
            ShapeFileHeaderWriter(ref oShxBinaryWriter, SHPFileHeader);
            //*** Write .Shx File Header
            DbaseFileHeaderWriter(ref oDbfBinaryWriter, _Layer.RecordCount, FileEncoding);
            //*** Write .dbf File Header

            int i;
            Shape _Shape = new Shape();
            PointF[] Pts;
            int Offset = 50;
            int RecordNumber = 0;
            int ContentLength;
            int Points;

            IDictionaryEnumerator dicEnumerator = _Layer.Records.GetEnumerator();
            switch (SHPFileHeader.ShapeType)
            {
                case 1:
                    PointF Pt;
                    ContentLength = 10;
                    while (dicEnumerator.MoveNext())
                    {
                        _Shape = (Shape)dicEnumerator.Value;
                        Pt = (PointF)_Shape.Parts[0];
                        RecordNumber += 1;
                        oShpBinaryWriter.Write(BigToLittleEndian(RecordNumber));
                        oShpBinaryWriter.Write(BigToLittleEndian(ContentLength));
                        oShpBinaryWriter.Write((int)1);
                        //Shape Type
                        oShpBinaryWriter.Write((double)Pt.X);
                        oShpBinaryWriter.Write((double)Pt.Y);
                        IndexRecordWriter(ref oShxBinaryWriter, Offset, ContentLength);
                        //*** Write SHX Record
                        Offset += (4 + ContentLength);
                        //*** Add 4 for record header '*** Set Offset for next SHX record
                        DbaseRecordWriter(ref oDbfBinaryWriter, _Shape.AreaId, _Shape.AreaName);
                        //*** Write DBF Record
                    }

                    break;
                case 3:
                case 5:
                    while (dicEnumerator.MoveNext())
                    {
                        _Shape = (Shape)dicEnumerator.Value;
                        int[] NumPoints = new int[_Shape.Parts.Count];
                        Points = 0;
                        for (i = 0; i <= _Shape.Parts.Count - 1; i++)
                        {
                            Pts = (PointF[])_Shape.Parts[i];
                            NumPoints[i] = Pts.Length;
                            Points += Pts.Length;
                        }
                        ContentLength = ((11 + _Shape.Parts.Count) * 2) + (Points * 8);
                        RecordNumber += 1;
                        PolygonRecordWriter(ref oShpBinaryWriter, RecordNumber, ContentLength, Points, NumPoints, _Shape, SHPFileHeader.ShapeType);
                        IndexRecordWriter(ref oShxBinaryWriter, Offset, ContentLength);
                        //*** Write SHX Record
                        Offset += (4 + ContentLength);
                        //*** Add 4 for record header '*** Set Offset for next SHX record
                        DbaseRecordWriter(ref oDbfBinaryWriter, _Shape.AreaId, _Shape.AreaName);
                        //*** Write DBF Record
                    }

                    break;
            }

            //*** Reset FileLength in Shp File Header
            oShpStream.Position = 24;
            oShpBinaryWriter.Write(BigToLittleEndian((int)oShpStream.Length / 2));

            //*** Reset FileLength in Shx File Header
            oShxStream.Position = 24;
            oShxBinaryWriter.Write(BigToLittleEndian((int)oShxStream.Length / 2));

            //*** Dispose
            oShpBinaryWriter.Close();
            oShxBinaryWriter.Close();
            oDbfBinaryWriter.Close();
            oShpStream.Close();
            oShxStream.Close();
            oDbfStream.Close();
            oShpBinaryWriter = null;
            oShxBinaryWriter = null;
            oDbfBinaryWriter = null;
            oShpStream = null;
            oShxStream = null;
            oDbfStream = null;
            dicEnumerator = null;
        }

        private static void ShapeFileHeaderWriter(ref System.IO.BinaryWriter bWriter, FileHeader SHPFileHeader)
        {
            bWriter.Write(SHPFileHeader.FileCode);
            bWriter.Write(SHPFileHeader.u1);
            bWriter.Write(SHPFileHeader.u2);
            bWriter.Write(SHPFileHeader.u3);
            bWriter.Write(SHPFileHeader.u4);
            bWriter.Write(SHPFileHeader.u5);
            bWriter.Write(SHPFileHeader.FileLength);
            bWriter.Write(SHPFileHeader.Version);
            bWriter.Write(SHPFileHeader.ShapeType);
            bWriter.Write(SHPFileHeader.BoundingBoxXMin);
            bWriter.Write(SHPFileHeader.BoundingBoxYMin);
            bWriter.Write(SHPFileHeader.BoundingBoxXMax);
            bWriter.Write(SHPFileHeader.BoundingBoxYMax);
            bWriter.Write(SHPFileHeader.BoundingBoxZMin);
            bWriter.Write(SHPFileHeader.BoundingBoxZMax);
            bWriter.Write(SHPFileHeader.BoundingBoxMMin);
            bWriter.Write(SHPFileHeader.BoundingBoxMMax);
        }
        private static void DbaseFileHeaderWriter(ref System.IO.BinaryWriter bWriter, int RecordCount, System.Text.Encoding encodingUsedForCodePage)
        {

            //byte[] Reserved = new byte[19];
            int dbfFldCount = 2;         //ID_(255) and NAME1_(60)
            bWriter.Write((byte)3);         //dbfVersion As Byte


            bWriter.Write((byte)(DateTime.Now.Year - 1900));
            //updateYear As Byte 'The year value in the dBASE header must be the year since 1900.
            bWriter.Write((byte)DateTime.Now.Month);
            //updateMonth As Byte
            bWriter.Write((byte)DateTime.Now.Day);
            //updateDay As Byte
            bWriter.Write(RecordCount);
            //numberRecords As Int32
            bWriter.Write((short)(33 + (dbfFldCount * 32)));
            //headerLength As Int16 (Short)
            bWriter.Write((short)101);
            //recordLength As Int16 (Short) 255 for ID_ and 60 for NAME1_

            //reserved(20) As Byte

            bWriter.Write(new byte[17]);
            //- Next 17 Bytes are reserved.

            bWriter.Write(ShapeFileReader.GetByteValueByEncodingCodePage(encodingUsedForCodePage.CodePage));
            //- Set Encoding (pageCode) information at 29th positon.

            bWriter.Write(new byte[2]);
            //- Next 2 Bytes are reserved.

            //*** Field Descriptor1 32 Bytes
            char[] FldID;
            FldID = "ID_".ToCharArray();
            char[] FldIDTemp = new char[11];    //ReDim Preserve FldID(10) equivalent in C#
            Array.Copy(FldID, FldIDTemp, FldID.Length);
            FldID = FldIDTemp;     // FldIDTemp elements are copied back into FldID[]

            bWriter.Write(FldID);
            bWriter.Write("C"[0]);


            bWriter.Write(new byte[4]);         //reserved(4) As Byte
            bWriter.Write((byte)ID_FIELD_LENGTH);          //FieldLength As Byte

            bWriter.Write(new byte[15]);
            //reserved(15) As Byte

            //*** Field Descriptor1 32 Bytes
            char[] FldName;
            FldName = "NAME1_".ToCharArray();
            char[] FldNameTemp = new char[11];    //ReDim Preserve FldID(10) equivalent in C#
            Array.Copy(FldName, FldNameTemp, FldName.Length);
            FldName = FldNameTemp;     // FldNameTemp elements are copied back into FldName[]

            bWriter.Write(FldName);
            bWriter.Write("C"[0]);
            bWriter.Write(new byte[4]);          //reserved(4) As Byte
            bWriter.Write((byte)NAME1_FIELD_LENGTH);          //FieldLength As Byte

            bWriter.Write(new byte[15]);     //reserved(15) As Byte

            //Dbf Header Terminator
            bWriter.Write((byte)13);
            //field terminator as Byte

            //Database file structure
            //The structure of a dBASE III database file is composed of a header and data records.
            //The layout is given below.

            //dBASE III DATABASE FILE HEADER:
            //+---------+-------------------+---------------------------------+
            //| BYTE   | CONTENTS      | MEANING                               |
            //+---------+-------------------+---------------------------------+
            //| 0      | 1 byte        | dBASE III version number              |
            //|        |               | (03H without a .DBT file)             |
            //|        |               | (83H with a .DBT file)                |
            //+---------+-------------------+---------------------------------+
            //| 1-3    | 3 bytes       | date of last update                   |
            //|        |               | (YY MM DD) in binary format           |
            //+---------+-------------------+---------------------------------+
            //| 4-7    | 32 bit number | number of records in data file        |
            //+---------+-------------------+---------------------------------+
            //| 8-9    | 16 bit number | length of header structure           |
            //+---------+-------------------+---------------------------------+
            //| 10-11  | 16 bit number | length of the record                  |
            //+---------+-------------------+---------------------------------+
            //| 12-31  | 20 bytes      | reserved bytes (version 1.00)         |
            //+------ ---+-------------------+---------------------------------+
            //| 32-n   | 32 bytes each | field descriptor array |
            //|        |               | (see below) | --+
            //+---------+-------------------+---------------------------------+ |
            //| n+1    | 1 byte        | 0DH as the field terminator | |
            //+---------+-------------------+---------------------------------+ |
            // |
            // |
            //A FIELD DESCRIPTOR: <------------------------------------------+
            //+---------+-------------------+---------------------------------+
            //| BYTE   | CONTENTS           | MEANING                         |
            //+---------+-------------------+---------------------------------+
            //| 0-10   | 11 bytes           | field name in ASCII zero-filled |
            //+---------+-------------------+---------------------------------+
            //| 11     | 1 byte             | field type in ASCII             |
            //|        |                    | (C N L D or M)                  |
            //+---------+-------------------+---------------------------------+
            //| 12-15  | 32 bit number      | field data address              |
            //|        |                    | (address is set in memory)      |
            //+---------+-------------------+---------------------------------+
            //| 16     | 1 byte             | field length in binary          |
            //+---------+-------------------+---------------------------------+
            //| 17     | 1 byte             | field decimal count in binary   |
            //+---------+-------------------+---------------------------------+
            //| 18-31  | 14 bytes           | reserved bytes (version 1.00)   |
            //+---------+-------------------+---------------------------------+


            //The data records are layed out as follows:

            //     1. Data records are preceeded by one byte that is a space (20 Hex = 32 Dec) if the
            //     record is not deleted and an asterisk (2A Hex = 42 Dec) if it is deleted.

            //     2. Data fields are packed into records with no field separators or
            //     record terminators.

            //     3. Data types are stored in ASCII format as follows:

            //     DATA TYPE DATA RECORD STORAGE
            //     --------- --------------------------------------------
            //     Character (ASCII characters)
            //     Numeric - . 0 1 2 3 4 5 6 7 8 9
            //     Logical ? Y y N n T t F f (? when not initialized)
            //     Memo (10 digits representing a .DBT block number)
            //     Date (8 digits in YYYYMMDD format, such as
            //     19840704 for July 4, 1984)


        }

        public static DataTable GetDbfTable(string dbfPath)
        {
            FileStream dbfStream;
            BinaryReader dbfReader;
            System.Text.Encoding FileEncoding = ShapeFileReader.GetEncodingInDBF(dbfPath);
            dbfStream = File.OpenRead(dbfPath);
            dbfReader = new BinaryReader(dbfStream, FileEncoding);

            int i;
            int j;

            //*** Read dbf header and set Id and Label Field
            string sFieldId = "";
            string sFieldLabel = "";

            byte dbfVersion = dbfReader.ReadByte();
            //3
            byte updateYear = dbfReader.ReadByte();
            byte updateMonth = dbfReader.ReadByte();
            byte updateDay = dbfReader.ReadByte();

            Int32 numberRecords = dbfReader.ReadInt32();
            //
            short headerLength = dbfReader.ReadInt16();
            //
            short recordLength = dbfReader.ReadInt16();
            byte[] reserved = dbfReader.ReadBytes(20);
            Int32 numberFields = (int)(headerLength - 33) / 32;

            DBF_Field_Header[] fieldHeaders = new DBF_Field_Header[numberFields];
            string FieldName;

            DataTable _DT = new DataTable("tblMapArea");

            _DT.Columns.Add("AreaId", Type.GetType("System.String"));
            //MaxLen - 255
            _DT.Columns.Add("AreaName", Type.GetType("System.String"));
            //MaxLen - 60
            //*** validation for max lenght
            _DT.Columns[0].MaxLength = 255;
            _DT.Columns[1].MaxLength = 60;

            for (i = 0; i <= numberFields - 1; i++)
            {
                char[] fieldNameChars = dbfReader.ReadChars(10);
                char fieldNameTerminator = dbfReader.ReadChar();
                FieldName = new string(fieldNameChars).ToUpper();
                fieldHeaders[i].FieldName = FieldName;
                fieldHeaders[i].FieldType = dbfReader.ReadChar();
                //http://www.dbase.com/KnowledgeBase/int/db7_file_fmt.htm
                //BugFix 12 July 2006 ID_ and NAME1_ field may not be the first and second field
                if (FieldName.Length >= 3 && fieldHeaders[i].FieldName.Replace("\0", "").Trim() == ID_FIELD_NAME)
                {
                    if (FieldName[0] == 'I' & FieldName[1] == 'D' & FieldName[2] == '_')
                        sFieldId = fieldHeaders[i].FieldName;
                }
                if (FieldName.Length >= 6 && fieldHeaders[i].FieldName.Replace("\0", "").Trim() == NAME1_FIELD_NAME)
                {
                    if (FieldName[0] == 'N' & FieldName[1] == 'A' & FieldName[2] == 'M' & FieldName[3] == 'E' & FieldName[4] == '1' & FieldName[5] == '_')
                        sFieldLabel = fieldHeaders[i].FieldName;
                }
                byte[] reserved1 = dbfReader.ReadBytes(4);
                fieldHeaders[i].FieldLength = dbfReader.ReadByte();
                byte[] reserved2 = dbfReader.ReadBytes(15);
            }
            byte headerTerminator = dbfReader.ReadByte();
            //13

            if (sFieldId == "")
                sFieldId = fieldHeaders[0].FieldName;
            if (sFieldLabel == "")
            {
                if (fieldHeaders.Length > 1)
                {
                    sFieldLabel = fieldHeaders[1].FieldName;
                }
                else
                {
                    sFieldLabel = fieldHeaders[0].FieldName;
                }
            }

            string AreaId;
            string AreaName;
            for (i = 0; i <= numberRecords - 1; i++)
            {
                byte isValid = dbfReader.ReadByte();
                //32
                DataRow _Row = _DT.NewRow();
                for (j = 0; j <= fieldHeaders.Length - 1; j++)
                {
                    char[] fieldDataChars = dbfReader.ReadChars(fieldHeaders[j].FieldLength);
                    if (fieldHeaders[j].FieldName == sFieldId)
                    {
                        AreaId = new string(fieldDataChars).Trim();
                        AreaId = AreaId.Substring(0, Math.Min(AreaId.Length, _DT.Columns[0].MaxLength));
                        //Limit the string to MaxLength
                        _Row["AreaId"] = AreaId;
                    }
                    else if (fieldHeaders[j].FieldName == sFieldLabel)
                    {
                        AreaName = new string(fieldDataChars).Trim();
                        AreaName = AreaName.Substring(0, Math.Min(AreaName.Length, _DT.Columns[1].MaxLength));
                        //Limit the string to MaxLength
                        _Row["AreaName"] = AreaName;
                    }
                    string fieldData = new string(fieldDataChars).Trim();
                }
                _DT.Rows.Add(_Row);
                _Row = null;
            }

            dbfStream.Close();
            dbfStream = null;
            dbfReader.Close();
            dbfReader = null;
            return _DT;
        }
        public static void SetDbfTable(DataTable DT, string dbfPath)
        {
            System.Text.Encoding FileEncoding = System.Text.Encoding.GetEncoding(1252); //- Default
            if (File.Exists(dbfPath))
            {
                //- Get Encoding (CodePage) used in source Shape file (.dbf)
                FileEncoding = ShapeFileReader.GetEncodingInDBF(dbfPath);
            }

            FileStream oDbfStream = new FileStream(dbfPath, FileMode.Create);
            BinaryWriter oDbfBinaryWriter = new BinaryWriter(oDbfStream, FileEncoding);       //System.Text.Encoding.ASCII
            DbaseFileHeaderWriter(ref oDbfBinaryWriter, DT.Rows.Count, FileEncoding);
            //*** Write .dbf File Header
            int i;
            //*** This process will remove any fields other than ID_ and Name1_ available in original dbf file
            for (i = 0; i <= DT.Rows.Count - 1; i++)
            {
                DbaseRecordWriter(ref oDbfBinaryWriter, DT.Rows[i][0].ToString(), DT.Rows[i][1].ToString());
                //*** Write DBF Record
            }
            oDbfBinaryWriter.Close();
            oDbfBinaryWriter = null;
            oDbfStream.Close();
            oDbfStream = null;
        }

        private static void PolygonRecordWriter(ref System.IO.BinaryWriter bWriter, int RecordNumber, int ContentLength, int Points, int[] NumPoints, Shape _Shape, int ShapeType)
        {
            int i;
            int j;
            PointF[] Pt;
            int Parts;


            //*** Write Record Header
            //Position Field Value Type Byte Order
            //Byte 0 Record Number Record Number Integer Big
            //Byte 4 Content Length Content Length Integer Big
            bWriter.Write(BigToLittleEndian(RecordNumber));
            bWriter.Write(BigToLittleEndian(ContentLength));

            //*** Write Record Content
            //Position Field Value Type Number Byte Order
            //Byte 0 Shape Type 5 Integer 1 Little
            //Byte 4 Box Box Double 4 Little
            //Byte 36 NumParts NumParts Integer 1 Little
            //Byte 40 NumPoints NumPoints Integer 1 Little
            //Byte 44 Parts Parts Integer NumParts Little
            //Byte X Points Points Point NumPoints Little

            bWriter.Write(ShapeType);
            //Shape Type
            bWriter.Write((double)_Shape.Extent.Left);
            //Bounding Box
            bWriter.Write((double)_Shape.Extent.Top);
            bWriter.Write((double)_Shape.Extent.Left + _Shape.Extent.Width);
            bWriter.Write((double)_Shape.Extent.Top + _Shape.Extent.Height);
            bWriter.Write(_Shape.Parts.Count);
            //NumParts

            bWriter.Write(Points);
            //NumPoints
            Parts = 0;
            bWriter.Write(Parts);
            //Parts
            for (i = 1; i <= NumPoints.Length - 1; i++)
            {
                Parts += NumPoints[i - 1];
                bWriter.Write(Parts);
                //Parts
            }

            for (i = 0; i <= _Shape.Parts.Count - 1; i++)
            {
                Pt = (PointF[])_Shape.Parts[i];
                for (j = 0; j <= Pt.Length - 1; j++)
                {
                    bWriter.Write((double)Pt[j].X);
                    //Points
                    bWriter.Write((double)Pt[j].Y);
                }
            }
        }
        private static void IndexRecordWriter(ref System.IO.BinaryWriter bWriter, int RecOffset, int RecContentLength)
        {
            bWriter.Write(BigToLittleEndian(RecOffset));
            bWriter.Write(BigToLittleEndian(RecContentLength));
            //Position Field Value Type Byte Order
            //Byte 0 Offset Offset Integer Big
            //Byte 4 Content Length Content Length Integer Big
        }
        private static void DbaseRecordWriter(ref System.IO.BinaryWriter bWriter, string Id, string Name)
        {
            char[] AreaID;
            char[] AreaName;
            if (Id.Length < ID_FIELD_LENGTH)
            {
                AreaID = (Id + new string(" "[0], ID_FIELD_LENGTH - Id.Length)).ToCharArray();
            }
            else
            {
                AreaID = Id.ToCharArray();
                //Truncate to size if string exceeds 255 Char
                char[] AreaIDTemp = new char[ID_FIELD_LENGTH];  // ReDim not supported in C#, so AreaID[] is copied into temp Array(resized), then Temp Array elements are copied back into original Array
                Array.Copy(AreaID, AreaIDTemp, AreaID.Length);
                AreaID = AreaIDTemp;
            }

            if (Name.Length < NAME1_FIELD_LENGTH)
            {
                AreaName = (Name + new string(" "[0], NAME1_FIELD_LENGTH - Name.Length)).ToCharArray();
            }
            else
            {
                AreaName = Name.ToCharArray();
                char[] AreaNameTemp = new char[NAME1_FIELD_LENGTH];  // ReDim not supported in C#, so AreaName[] is copied into temp Array(resized), then Temp Array elements are copied back into original Array
                Array.Copy(AreaName, AreaNameTemp, AreaName.Length); //- Limit AreaName upto 60 char
                AreaName = AreaNameTemp;
            }

            //Data records are preceeded by one byte that is a space (20 Hex = 32 Dec) if the record is not deleted and an asterisk (2A Hex = 42 Dec) if it is deleted.
            bWriter.Write((byte)32);
            bWriter.Write(AreaID);
            bWriter.Write(AreaName);

        }
        # endregion

        # region "ShapeInfo"
        public static ShapeInfo GetShapeInfoEx(string p_SourceDir, string p_FileName)
        {
            ShapeInfo RetVal = null;
            FileStream _SHXStream;
            BinaryReader _SHXBinaryReader;
            FileHeader _SHXFileHeader = new FileHeader();
            RetVal = new ShapeInfo();
            try
            {
                _SHXStream = File.OpenRead(p_SourceDir + "\\" + p_FileName + ".shx");
                _SHXBinaryReader = new BinaryReader(_SHXStream);
                _SHXFileHeader = ReadHeaderInfo(ref _SHXBinaryReader);
                {
                    RetVal.ID = p_FileName;
                    RetVal.Extent = new RectangleF((float)_SHXFileHeader.BoundingBoxXMin, (float)_SHXFileHeader.BoundingBoxYMin, (float)Math.Abs(_SHXFileHeader.BoundingBoxXMax - _SHXFileHeader.BoundingBoxXMin), (float)Math.Abs(_SHXFileHeader.BoundingBoxYMax - _SHXFileHeader.BoundingBoxYMin));
                    RetVal.RecordCount = (int)(_SHXFileHeader.FileLength - 50) / 4;
                    RetVal.ShapeType = (DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType)_SHXFileHeader.ShapeType;
                }
            }
            finally
            {
                // _SHXStream.Close();
                _SHXStream = null;
                // _SHXBinaryReader.Close();
                _SHXBinaryReader = null;
                //_SHXFileHeader = null;
            }
            return RetVal;

        }
        public static ShapeInfo GetShapeInfo(string p_SourceDir, string p_FileName)
        {
            ShapeInfo RetVal = null;

            //-- Check for shape file set existence
            if (File.Exists(p_SourceDir + "\\" + p_FileName + ".shp") && File.Exists(p_SourceDir + "\\" + p_FileName + ".shx") && File.Exists(p_SourceDir + "\\" + p_FileName + ".dbf"))
            {

                FileHeader SHPFileHeader = new FileHeader();
                SHPRecordHeader SHPRecordHeader = new SHPRecordHeader();
                FileStream objSHPStream = null;
                BinaryReader objSHPBinaryReader = null;

                FileHeader SHXFileHeader = new FileHeader();
                SHXRecordHeader SHXRecordHeader = new SHXRecordHeader();
                FileStream objSHXStream = null;
                BinaryReader objSHXBinaryReader = null;

                FileStream dbfStream = null;
                BinaryReader dbfReader = null;
                System.Text.Encoding FileEncoding;      // To hold encoding type on the basis of code Page value defined in .dbf header.

                int i;
                int j;
                int k;
                try
                {
                    objSHPStream = File.OpenRead(p_SourceDir + "\\" + p_FileName + ".shp");
                    objSHPBinaryReader = new BinaryReader(objSHPStream);
                    objSHXStream = File.OpenRead(p_SourceDir + "\\" + p_FileName + ".shx");
                    objSHXBinaryReader = new BinaryReader(objSHXStream);

                    //-- get Encoding used to create dbf file. This PageCode information is in 29th position of dbfReader. 
                    FileEncoding = ShapeFileReader.GetEncodingInDBF(p_SourceDir + "\\" + p_FileName + ".dbf");
                    dbfStream = File.OpenRead(p_SourceDir + "\\" + p_FileName + ".dbf");
                    //dbfReader = new BinaryReader(dbfStream, System.Text.Encoding.GetEncoding(65001)); // 65001 is the code page for Unicode UTF-8 encoding taken as default'

                    dbfReader = new BinaryReader(dbfStream, FileEncoding);

                    SHPFileHeader = ReadHeaderInfo(ref objSHPBinaryReader);
                    SHXFileHeader = ReadHeaderInfo(ref objSHXBinaryReader);

                    ShapeInfo _ShapeInfo = new ShapeInfo();
                    _ShapeInfo.ID = p_FileName;
                    _ShapeInfo.Extent = new RectangleF((float)SHPFileHeader.BoundingBoxXMin, (float)SHPFileHeader.BoundingBoxYMin, (float)Math.Abs(SHPFileHeader.BoundingBoxXMax - SHPFileHeader.BoundingBoxXMin), (float)Math.Abs(SHPFileHeader.BoundingBoxYMax - SHPFileHeader.BoundingBoxYMin));
                    _ShapeInfo.RecordCount = (int)(SHXFileHeader.FileLength - 50) / 4;

                    _ShapeInfo.ShapeType = (DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType)SHPFileHeader.ShapeType;

                    if ((int)(_ShapeInfo.ShapeType) == (int)(ShapeFileReader.ShapeType.PointZ))
                    {
                        _ShapeInfo.ShapeType = DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.ShapeType.Point;
                    }


                    //*** Read dbf header and set Id and Label Field
                    string sFieldId = "";
                    string sFieldLabel = "";

                    byte dbfVersion = dbfReader.ReadByte();
                    //3
                    byte updateYear = dbfReader.ReadByte();
                    byte updateMonth = dbfReader.ReadByte();
                    byte updateDay = dbfReader.ReadByte();

                    Int32 numberRecords = dbfReader.ReadInt32();
                    //
                    short headerLength = dbfReader.ReadInt16();
                    //
                    short recordLength = dbfReader.ReadInt16();
                    //**** In Reserved Bytes, there is a byte (flag) at 29th postion of stream which represent code page information, that can be used to get Encoding type.
                    dbfStream.Seek(29, SeekOrigin.Begin);     // Seek to 29th postion to fetch code page .
                    FileEncoding = GetDbaseEncodingType(dbfReader.ReadByte());   // Reads code page and gets the corresponding encoding type.
                    dbfStream.Seek(32, SeekOrigin.Begin);  // Move to past the reserved bytes.
                    // byte[] reserved = dbfReader.ReadBytes(20);
                    Int32 numberFields = (int)(headerLength - 33) / 32;

                    DBF_Field_Header[] fieldHeaders = new DBF_Field_Header[numberFields];
                    string FieldName;
                    for (i = 0; i <= numberFields - 1; i++)
                    {
                        //byte[] testBytes = FileEncoding.GetBytes(dbfReader.ReadChars(10));
                        ////dbfReader.BaseStream.Position -= 10;
                        //testBytes = System.Text.Encoding.Convert(System.Text.Encoding.UTF8, FileEncoding, testBytes);
                        try
                        {
                            char[] fieldNameChars = dbfReader.ReadChars(10);
                            char fieldNameTerminator = dbfReader.ReadChar();
                            FieldName = new string(fieldNameChars).ToUpper();
                            fieldHeaders[i].FieldName = FieldName;
                            fieldHeaders[i].FieldType = dbfReader.ReadChar();

                            if (FieldName.Length >= 3 && fieldHeaders[i].FieldName.Replace("\0","").Trim() == ID_FIELD_NAME)
                            {
                                if (FieldName[0] == 'I' & FieldName[1] == 'D' & FieldName[2] == '_')
                                    sFieldId = fieldHeaders[i].FieldName;
                            }

                            if (FieldName.Length >= 6 && fieldHeaders[i].FieldName.Replace("\0", "").Trim() == NAME1_FIELD_NAME)
                            {
                                if (FieldName[0] == 'N' & FieldName[1] == 'A' & FieldName[2] == 'M' & FieldName[3] == 'E' & FieldName[4] == '1' & FieldName[5] == '_')
                                    sFieldLabel = fieldHeaders[i].FieldName;
                            }

                            byte[] reserved1 = dbfReader.ReadBytes(4);
                            fieldHeaders[i].FieldLength = dbfReader.ReadByte();
                            byte[] reserved2 = dbfReader.ReadBytes(15);
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }

                    }
                    byte headerTerminator = dbfReader.ReadByte();
                    //13

                    if (sFieldId == "")
                        sFieldId = fieldHeaders[0].FieldName;
                    if (sFieldLabel == "")
                    {
                        if (fieldHeaders.Length > 1)
                        {
                            sFieldLabel = fieldHeaders[1].FieldName;
                        }
                        else
                        {
                            sFieldLabel = fieldHeaders[0].FieldName;
                        }
                    }

                    int numParts;
                    int numPoints;
                    int[] theParts;
                    PointF[] Pt;
                    float Ydiff = 0;
                    //*** Add Ydiff to make all Y coordinates positve for area calculation. if extent of polygon goes below equator
                    //*** Special Handling for centroids of areas lying in 3rd quadrent
                    //*** http://astronomy.swin.edu.au/~pbourke/geometry/polyarea/
                    //*** http://www.geog.ubc.ca/courses/klink/gis.notes/ncgia/u33.html
                    //*** X = S ((y(i) - y(i+1)) (x(i)2 + x(i)x(i+1) + x(i+1)2)/6A) Y = S ((x(i+1) - x(i)) (y(i)2 + y(i)y(i+1) + y(i+1)2)/6A) where A is the area of the polygon
                    //*** note: as with the area algorithm, the polygon must be coded clockwise and all y coordinates must be non- negative

                    for (i = 0; i <= _ShapeInfo.RecordCount - 1; i++)
                    {

                        SHXRecordHeader.Offset = BigToLittleEndian(objSHXBinaryReader.ReadInt32());
                        SHXRecordHeader.ContentLength = BigToLittleEndian(objSHXBinaryReader.ReadInt32());

                        objSHPStream.Seek(SHXRecordHeader.Offset * 2, SeekOrigin.Begin);

                        SHPRecordHeader.RecordNumber = BigToLittleEndian(objSHPBinaryReader.ReadInt32());
                        SHPRecordHeader.ContentLength = BigToLittleEndian(objSHPBinaryReader.ReadInt32());

                        objSHPBinaryReader.ReadInt32();
                        //ShapeType


                        byte isValid = dbfReader.ReadByte();
                        //32

                        Shape _Shape = new Shape();
                        RectangleF _Extent = new RectangleF();

                        {
                            for (j = 0; j <= fieldHeaders.Length - 1; j++)
                            {
                                char[] fieldDataChars;
                                if (fieldHeaders[j].FieldName == sFieldId)
                                {
                                    //Fetch AreaID from .dbf file using default encoding which is set to UTF-8.
                                    fieldDataChars = dbfReader.ReadChars(fieldHeaders[j].FieldLength);
                                    _Shape.AreaId = new string(fieldDataChars).Trim();
                                }
                                else if (fieldHeaders[j].FieldName == sFieldLabel)
                                {
                                    //Fetch the value from "NAME1_" column in its original encoded characters using extracted encoding type.
                                    fieldDataChars = FileEncoding.GetString(dbfReader.ReadBytes(fieldHeaders[j].FieldLength)).Replace("\0", "").Trim().ToCharArray();
                                    _Shape.AreaName = new string(fieldDataChars).Trim();
                                }
                                else
                                {
                                    fieldDataChars = dbfReader.ReadChars(fieldHeaders[j].FieldLength);
                                }
                                //string fieldData = new string(fieldDataChars).Trim(); // unused statement
                            }

                            switch (SHPFileHeader.ShapeType)
                            {
                                case (int)ShapeType.Point:
                                case (int)(ShapeFileReader.ShapeType.PointZ):
                                    PointF _Pt = new PointF();
                                    _Pt.X = (float)objSHPBinaryReader.ReadDouble();
                                    _Pt.Y = (float)objSHPBinaryReader.ReadDouble();
                                    _Shape.Parts.Add(_Pt);
                                    break;
                                case (int)ShapeType.PolyLine:
                                    _Extent.X = (float)objSHPBinaryReader.ReadDouble();
                                    //Byte 04 Minx
                                    _Extent.Y = (float)objSHPBinaryReader.ReadDouble();
                                    //Byte 12 MinY
                                    _Extent.Width = (float)Math.Abs(_Extent.X - objSHPBinaryReader.ReadDouble());
                                    //Byte 20 MaxX
                                    _Extent.Height = (float)Math.Abs(_Extent.Y - objSHPBinaryReader.ReadDouble());
                                    //Byte 28 MaxY
                                    _Shape.Extent = _Extent;

                                    numParts = objSHPBinaryReader.ReadInt32();
                                    //Byte 36 Num Parts
                                    numPoints = objSHPBinaryReader.ReadInt32();
                                    //Byte 40 Num Points

                                    theParts = new int[numParts];
                                    for (j = 0; j <= numParts - 1; j++)
                                    {
                                        theParts[j] = objSHPBinaryReader.ReadInt32();
                                    }


                                    for (j = 0; j <= numParts - 1; j++)
                                    {
                                        if ((j != numParts - 1))
                                        {
                                            Pt = new PointF[theParts[j + 1] - theParts[j]];
                                            for (k = theParts[j]; k <= theParts[j + 1] - 1; k++)
                                            {
                                                Pt[k - theParts[j]].X = (float)objSHPBinaryReader.ReadDouble();
                                                Pt[k - theParts[j]].Y = (float)objSHPBinaryReader.ReadDouble();
                                            }
                                        }
                                        else
                                        {
                                            Pt = new PointF[numPoints - theParts[j]];
                                            for (k = theParts[j]; k <= numPoints - 1; k++)
                                            {
                                                Pt[k - theParts[j]].X = (float)objSHPBinaryReader.ReadDouble();
                                                Pt[k - theParts[j]].Y = (float)objSHPBinaryReader.ReadDouble();
                                            }
                                        }
                                        _Shape.Parts.Add(Pt);
                                    }

                                    break;

                                case (int)ShapeType.Polygon:
                                    //*** Bugfix 27 Sep 2006 Centroid calulation for very small polygon 'Mayanmar problem
                                    double second_factor;
                                    //*** For centroid calculation
                                    double[] polygon_area;
                                    //*** SignedPolygonArea
                                    double[] Cx;
                                    double[] Cy;

                                    _Extent.X = (float)objSHPBinaryReader.ReadDouble();
                                    //Byte 04 Minx
                                    _Extent.Y = (float)objSHPBinaryReader.ReadDouble();
                                    //Byte 12 MinY
                                    _Extent.Width = (float)Math.Abs(_Extent.X - objSHPBinaryReader.ReadDouble());
                                    //Byte 20 MaxX
                                    _Extent.Height = (float)Math.Abs(_Extent.Y - objSHPBinaryReader.ReadDouble());
                                    //Byte 28 MaxY
                                    _Shape.Extent = _Extent;

                                    if (_Extent.Y < 0)
                                        Ydiff = -(_Extent.Y);


                                    numParts = objSHPBinaryReader.ReadInt32();
                                    //Byte 36 Num Parts
                                    numPoints = objSHPBinaryReader.ReadInt32();
                                    //Byte 40 Num Points

                                    theParts = new int[numParts];
                                    for (j = 0; j <= numParts - 1; j++)
                                    {
                                        theParts[j] = objSHPBinaryReader.ReadInt32();
                                    }


                                    Cx = new double[numParts];
                                    Cy = new double[numParts];
                                    polygon_area = new double[numParts];


                                    for (j = 0; j <= numParts - 1; j++)
                                    {
                                        if ((j != numParts - 1))
                                        {
                                            Pt = new PointF[theParts[j + 1] - theParts[j]];
                                            for (k = theParts[j]; k <= theParts[j + 1] - 1; k++)
                                            {
                                                Pt[k - theParts[j]].X = (float)objSHPBinaryReader.ReadDouble();
                                                Pt[k - theParts[j]].Y = (float)objSHPBinaryReader.ReadDouble();
                                                if (k > theParts[j])
                                                {
                                                    polygon_area[j] = polygon_area[j] + (Pt[k - theParts[j]].X - Pt[k - theParts[j] - 1].X) * ((Pt[k - theParts[j]].Y + Ydiff) + (Pt[k - theParts[j] - 1].Y + Ydiff)) / 2;
                                                    second_factor = Pt[k - theParts[j] - 1].X * (Pt[k - theParts[j]].Y + Ydiff) - Pt[k - theParts[j]].X * (Pt[k - theParts[j] - 1].Y + Ydiff);
                                                    Cx[j] = Cx[j] + (Pt[k - theParts[j] - 1].X + Pt[k - theParts[j]].X) * second_factor;
                                                    Cy[j] = Cy[j] + ((Pt[k - theParts[j] - 1].Y + Ydiff) + (Pt[k - theParts[j]].Y + Ydiff)) * second_factor;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Pt = new PointF[numPoints - theParts[j]];
                                            for (k = theParts[j]; k <= numPoints - 1; k++)
                                            {
                                                Pt[k - theParts[j]].X = (float)objSHPBinaryReader.ReadDouble();
                                                Pt[k - theParts[j]].Y = (float)objSHPBinaryReader.ReadDouble();
                                                if (k > theParts[j])
                                                {
                                                    polygon_area[j] = polygon_area[j] + (Pt[k - theParts[j]].X - Pt[k - theParts[j] - 1].X) * ((Pt[k - theParts[j]].Y + Ydiff) + (Pt[k - theParts[j] - 1].Y + Ydiff)) / 2;
                                                    second_factor = Pt[k - theParts[j] - 1].X * (Pt[k - theParts[j]].Y + Ydiff) - Pt[k - theParts[j]].X * (Pt[k - theParts[j] - 1].Y + Ydiff);
                                                    Cx[j] = Cx[j] + (Pt[k - theParts[j] - 1].X + Pt[k - theParts[j]].X) * second_factor;
                                                    Cy[j] = Cy[j] + ((Pt[k - theParts[j] - 1].Y + Ydiff) + (Pt[k - theParts[j]].Y + Ydiff)) * second_factor;
                                                }
                                            }
                                        }
                                        _Shape.Parts.Add(Pt);
                                        if (polygon_area[j] == 0)
                                        {
                                            Cx[j] = 0;
                                            //*** Indonesia problem
                                            Cy[j] = 0;
                                        }
                                        else
                                        {
                                            Cx[j] = Cx[j] / 6 / polygon_area[j];
                                            // Divide by 6 times the polygon's area.
                                            Cy[j] = Cy[j] / 6 / polygon_area[j];
                                        }
                                    }


                                    double SumCx = 0;
                                    double SumCy = 0;
                                    double SumA = 0;
                                    for (j = 0; j <= numParts - 1; j++)
                                    {
                                        SumCx = SumCx + Cx[j] * polygon_area[j];
                                        SumCy = SumCy + Cy[j] * polygon_area[j];
                                        SumA = SumA + polygon_area[j];
                                    }

                                    SumCx = SumCx / SumA;
                                    SumCy = SumCy / SumA;
                                    if (SumCx < 0 | SumCy < 0)
                                    {
                                        //*** If the values are negative, the polygon is oriented counterclockwise. Reverse the signs.
                                        SumCx = -SumCx;
                                        SumCy = -SumCy;
                                    }



                                    _Shape.Centroid = new PointF((float)SumCx, (float)SumCy - Ydiff);
                                    break;
                            }
                            _ShapeInfo.Records.Add(_Shape.AreaId, _Shape);
                            _Shape = null;
                        }
                    }

                    RetVal = _ShapeInfo;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    //-- Deallocate the memory used for garbage collector to remove the memory utilized
                    objSHPStream.Close();
                    objSHPStream = null;
                    objSHXStream.Close();
                    objSHXStream = null;
                    dbfStream.Close();
                    dbfStream = null;

                    objSHPBinaryReader.Close();
                    objSHPBinaryReader = null;
                    objSHXBinaryReader.Close();
                    objSHXBinaryReader = null;
                    dbfReader.Close();
                    dbfReader = null;
                    // SHPFileHeader = null;
                    // SHXFileHeader = null;
                    // SHXRecordHeader = null;
                    // SHPRecordHeader = null;
                }
            }


            return RetVal;
        }
        private static FileHeader ReadHeaderInfo(ref System.IO.BinaryReader objReader)
        {
            FileHeader _Header = new FileHeader();
            try
            {
                {
                    _Header.FileCode = BigToLittleEndian(objReader.ReadInt32());
                    objReader.ReadBytes(20);
                    //*** Skip the unused bytes
                    _Header.FileLength = BigToLittleEndian(objReader.ReadInt32());
                    _Header.Version = objReader.ReadInt32();
                    _Header.ShapeType = objReader.ReadInt32();
                    _Header.BoundingBoxXMin = objReader.ReadDouble();
                    _Header.BoundingBoxYMin = objReader.ReadDouble();
                    _Header.BoundingBoxXMax = objReader.ReadDouble();
                    _Header.BoundingBoxYMax = objReader.ReadDouble();
                    _Header.BoundingBoxZMin = objReader.ReadDouble();
                    _Header.BoundingBoxZMax = objReader.ReadDouble();
                    _Header.BoundingBoxMMin = objReader.ReadDouble();
                    _Header.BoundingBoxMMax = objReader.ReadDouble();
                }
                return _Header;
            }
            finally
            {
                //_Header = null;
            }

        }

        static internal int BigToLittleEndian(int value)
        {
            return System.Drawing.Color.FromArgb(System.Drawing.Color.FromArgb(value).B, System.Drawing.Color.FromArgb(value).G, System.Drawing.Color.FromArgb(value).R, System.Drawing.Color.FromArgb(value).A).ToArgb();
        }

        private static System.Text.Encoding GetEncodingInDBF(string dbfFilePath)
        {
            System.Text.Encoding RetVal = null;

            FileStream dbfStream = null;
            BinaryReader dbfReader = null;
            try
            {
                dbfStream = File.OpenRead(dbfFilePath);
                dbfReader = new BinaryReader(dbfStream, System.Text.Encoding.GetEncoding(1252));


                dbfStream.Seek(29, SeekOrigin.Begin);     // Seek to 29th postion to fetch code page .
                RetVal = GetDbaseEncodingType(dbfReader.ReadByte());   // Reads code page and gets the corresponding encoding type.
                dbfStream.Seek(32, SeekOrigin.Begin);  // Move to past the reserved bytes.
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                //-- Deallocate the memory used for garbage collector to remove the memory utilized
                dbfStream.Close();
                dbfStream = null;

                dbfReader.Close();
                dbfReader = null;
            }

            return RetVal;
        }

        /// <summary>
        ///  To get Encoding type on the basis of codePage information passed in argument.
        /// </summary>
        /// <param name="CodePage">A byte variable, that contains Code Page information fetched from .dbf header.</param>
        /// <returns>Encoding Type</returns>
        /// <remarks>Code Pages can be added in code for more language support in future. Reference MSDN link: ms-help://MS.VSCC.v80/MS.MSDN.v80/MS.NETDEVFX.v20.en/cpref12/html/T_System_Text_Encoding.htm .</remarks>
        private static System.Text.Encoding GetDbaseEncodingType(byte CodePage)
        {
            switch (CodePage)
            {
                case 0x01: return System.Text.Encoding.GetEncoding(437); //DOS USA code page 437 
                case 0x02: return System.Text.Encoding.GetEncoding(850); // DOS Multilingual code page 850 
                case 0x03: return System.Text.Encoding.GetEncoding(1252); // Windows ANSI code page 1252 
                case 0x04: return System.Text.Encoding.GetEncoding(10000); // Standard Macintosh 
                case 0x08: return System.Text.Encoding.GetEncoding(865); // Danish OEM
                case 0x09: return System.Text.Encoding.GetEncoding(437); // Dutch OEM
                case 0x0A: return System.Text.Encoding.GetEncoding(850); // Dutch OEM Secondary codepage
                case 0x0B: return System.Text.Encoding.GetEncoding(437); // Finnish OEM
                case 0x0D: return System.Text.Encoding.GetEncoding(437); // French OEM
                case 0x0E: return System.Text.Encoding.GetEncoding(850); // French OEM Secondary codepage
                case 0x0F: return System.Text.Encoding.GetEncoding(437); // German OEM
                case 0x10: return System.Text.Encoding.GetEncoding(850); // German OEM Secondary codepage
                case 0x11: return System.Text.Encoding.GetEncoding(437); // Italian OEM
                case 0x12: return System.Text.Encoding.GetEncoding(850); // Italian OEM Secondary codepage
                case 0x13: return System.Text.Encoding.GetEncoding(932); // Japanese Shift-JIS
                case 0x14: return System.Text.Encoding.GetEncoding(850); // Spanish OEM secondary codepage
                case 0x15: return System.Text.Encoding.GetEncoding(437); // Swedish OEM
                case 0x16: return System.Text.Encoding.GetEncoding(850); // Swedish OEM secondary codepage
                case 0x17: return System.Text.Encoding.GetEncoding(865); // Norwegian OEM
                case 0x18: return System.Text.Encoding.GetEncoding(437); // Spanish OEM
                case 0x19: return System.Text.Encoding.GetEncoding(437); // English OEM (Britain)
                case 0x1A: return System.Text.Encoding.GetEncoding(850); // English OEM (Britain) secondary codepage
                case 0x1B: return System.Text.Encoding.GetEncoding(437); // English OEM (U.S.)
                case 0x1C: return System.Text.Encoding.GetEncoding(863); // French OEM (Canada)
                case 0x1D: return System.Text.Encoding.GetEncoding(850); // French OEM secondary codepage
                case 0x1F: return System.Text.Encoding.GetEncoding(852); // Czech OEM
                case 0x22: return System.Text.Encoding.GetEncoding(852); // Hungarian OEM
                case 0x23: return System.Text.Encoding.GetEncoding(852); // Polish OEM
                case 0x24: return System.Text.Encoding.GetEncoding(860); // Portuguese OEM
                case 0x25: return System.Text.Encoding.GetEncoding(850); // Portuguese OEM secondary codepage
                case 0x26: return System.Text.Encoding.GetEncoding(866); // Russian OEM
                case 0x37: return System.Text.Encoding.GetEncoding(850); // English OEM (U.S.) secondary codepage
                case 0x40: return System.Text.Encoding.GetEncoding(852); // Romanian OEM
                case 0x4D: return System.Text.Encoding.GetEncoding(936); // Chinese GBK (PRC)
                case 0x4E: return System.Text.Encoding.GetEncoding(949); // Korean (ANSI/OEM)
                case 0x4F: return System.Text.Encoding.GetEncoding(950); // Chinese Big5 (Taiwan)
                case 0x50: return System.Text.Encoding.GetEncoding(874); // Thai (ANSI/OEM)
                case 0x57: return System.Text.Encoding.GetEncoding(1252); // ANSI
                case 0x58: return System.Text.Encoding.GetEncoding(1252); // Western European ANSI
                case 0x59: return System.Text.Encoding.GetEncoding(1252); // Spanish ANSI
                case 0x64: return System.Text.Encoding.GetEncoding(852); // Eastern European MS–DOS
                case 0x65: return System.Text.Encoding.GetEncoding(866); // Russian MS–DOS
                case 0x66: return System.Text.Encoding.GetEncoding(865); // Nordic MS–DOS
                case 0x67: return System.Text.Encoding.GetEncoding(861); // Icelandic MS–DOS
                case 0x68: return System.Text.Encoding.GetEncoding(895); // Kamenicky (Czech) MS-DOS 
                case 0x69: return System.Text.Encoding.GetEncoding(620); // Mazovia (Polish) MS-DOS 
                case 0x6A: return System.Text.Encoding.GetEncoding(737); // Greek MS–DOS (437G)
                case 0x6B: return System.Text.Encoding.GetEncoding(857); // Turkish MS–DOS
                case 0x6C: return System.Text.Encoding.GetEncoding(863); // French–Canadian MS–DOS
                case 0x78: return System.Text.Encoding.GetEncoding(950); // Taiwan Big 5
                case 0x79: return System.Text.Encoding.GetEncoding(949); // Hangul (Wansung)
                case 0x7A: return System.Text.Encoding.GetEncoding(936); // PRC GBK
                case 0x7B: return System.Text.Encoding.GetEncoding(932); // Japanese Shift-JIS
                case 0x7C: return System.Text.Encoding.GetEncoding(874); // Thai Windows/MS–DOS
                case 0x7D: return System.Text.Encoding.GetEncoding(1255); // Hebrew Windows 
                case 0x7E: return System.Text.Encoding.GetEncoding(1256); // Arabic Windows 
                case 0x86: return System.Text.Encoding.GetEncoding(737); // Greek OEM
                case 0x87: return System.Text.Encoding.GetEncoding(852); // Slovenian OEM
                case 0x88: return System.Text.Encoding.GetEncoding(857); // Turkish OEM
                case 0x96: return System.Text.Encoding.GetEncoding(10007); // Russian Macintosh 
                case 0x97: return System.Text.Encoding.GetEncoding(10029); // Eastern European Macintosh 
                case 0x98: return System.Text.Encoding.GetEncoding(10006); // Greek Macintosh 
                case 0xC8: return System.Text.Encoding.GetEncoding(1250); // Eastern European Windows
                case 0xC9: return System.Text.Encoding.GetEncoding(1251); // Russian Windows
                case 0xCA: return System.Text.Encoding.GetEncoding(1254); // Turkish Windows
                case 0xCB: return System.Text.Encoding.GetEncoding(1253); // Greek Windows
                case 0xCC: return System.Text.Encoding.GetEncoding(1257); // Baltic Windows
                default:
                    return System.Text.Encoding.GetEncoding(1252);  // Default ANSI
            }

        }

        /// <summary>
        ///  To get byte value representing codePage information of encoding.
        /// </summary>
        /// <remarks>Code Pages can be added in code for more language support in future. Reference MSDN link: ms-help://MS.VSCC.v80/MS.MSDN.v80/MS.NETDEVFX.v20.en/cpref12/html/T_System_Text_Encoding.htm .</remarks>
        private static byte GetByteValueByEncodingCodePage(int CodePage)
        {
            //Reference MSDN link: ms-help://MS.VSCC.v80/MS.MSDN.v80/MS.NETDEVFX.v20.en/cpref12/html/T_System_Text_Encoding.htm 

            switch (CodePage)
            {
                case 437: return 0x01; //DOS USA code page 437 , Dutch OEM, Finnish OEM

                case 850:
                    // DOS Multilingual code page 850 , Dutch OEM Secondary codepage, French OEM secondary codepage
                    return 0x02;

                case 10000: return 0x04; // Standard Macintosh 
                case 865: return 0x08; // Danish OEM                
                case 932: return 0x13; // Japanese Shift-JIS
                case 863: return 0x1C; // French OEM (Canada)
                case 852: return 0x1F; // Czech OEM, Hungarian OEM, Polish OEM, Eastern European MS–DOS

                case 860: return 0x24; // Portuguese OEM

                case 866: return 0x26; // Russian OEM, Russian MS–DOS

                case 936: return 0x4D; // Chinese GBK (PRC)

                case 949: return 0x4E; // Korean (ANSI/OEM)
                case 950: return 0x4F; // Chinese Big5 (Taiwan)

                case 874: return 0x50; // Thai (ANSI/OEM), Thai Windows/MS–DOS

                case 861: return 0x67; // Icelandic MS–DOS
                case 895: return 0x68; // Kamenicky (Czech) MS-DOS 

                case 620: return 0x69; // Mazovia (Polish) MS-DOS 
                case 737: return 0x6A; // Greek MS–DOS (437G)
                case 857: return 0x6B; // Turkish MS–DOS

                case 10007: return 0x96; // Russian Macintosh 
                case 10029: return 0x97; // Eastern European Macintosh 
                case 10006: return 0x98; // Greek Macintosh 

                case 1250: return 0xC8; // Eastern European Windows
                case 1251: return 0xC9; // Russian Windows
                case 1254: return 0xCA; // Turkish Windows
                case 1253: return 0xCB; // Greek Windows
                case 1255: return 0x7D; // Hebrew Windows 
                case 1256: return 0x7E; // Arabic Windows 
                case 1257: return 0xCC; // Baltic Windows

                case 1252:
                    return 0x03;  // Default ANSI, Windows ANSI
                default:
                    return 0x57;  // Default ANSI, Western European ANSI,  Spanish ANSI
            }

        }
        # endregion

        #region "-- Utility Encrypt/Decrypt --"

        /// <summary>
        /// It encrypt OR Decrypt shapeFile (.shp) by manupulating file header information.
        /// </summary>
        /// <param name="encrypt">true if encrypt , else decrypt</param>
        /// <param name="FilePath">fileName with path.</param>
        public static void EncryptDecryptShapeFile(bool encrypt, string FilePath)
        {
            //   ' This module was added as a part of CensusInfo project to provide Encryption-Decryption utility for shape files.
            //' This Utility takes advantage of fixed file headers of shape file where first byte represent File Code
            //' and its value is fixed (9994). Position at Byte4 is unused with value 0.

            //'Description of the Main File Header of shape file
            //'Position    Field       Value       Type    Order
            //'Byte0       File Code   9994        Integer Big
            //'Byte4       Unused      0           Integer Big
            //'Byte8       Unused      0           Integer Big
            //'Byte12      Unused      0           Integer Big
            //'Byte16      Unused      0           Integer Big
            //'Byte20      Unused      0           Integer Big
            //'Byte24      File       Length File Length Integer Big

            string sTemp;
            long nSize;
            byte[] bBytes;

            string FolderPath;
            FileStream SHPMain;
            FileStream SHPTemp;

            BinaryReader SHPReader;
            byte tempByte;
            bool ChangesDone = false;

            FolderPath = Path.GetDirectoryName(FilePath);
            //No check for existance of folder and Shape file, as its being checked in calling procedure
            //sTemp = String(260, 0)                                          'Create a buffer to store temp filename
            //GetTempFileName(FolderPath, "", 0, sTemp)                        'Get a temporary filename
            sTemp = Path.Combine(FolderPath, DateTime.Now.Ticks.ToString());

            //Get file handles of tempfile (for writing) and shape file (for reading)
            SHPMain = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite);
            SHPTemp = new FileStream(sTemp, FileMode.CreateNew, FileAccess.ReadWrite);

            nSize = new FileInfo(FilePath).Length;
            //Get the Shape file size
            //SetFilePointer(hShpFile, 0, 0, FILE_BEGIN)                       'Set the file pointer
            //ReDim bBytes(nSize - 1)                                'Create an array of bytes
            SHPReader = new BinaryReader(SHPMain);
            bBytes = SHPReader.ReadBytes((int)nSize);


            //Logic for Encryption / Decryption
            // 1st byte is swapped with 4th byte , and 3rd byte is swapped with 2nd byte.
            // 5th byte value, which was always 0, is changed to 255 when encyption.

            if (encrypt == true && bBytes[4] == 0)      // Encrypt file header
            {

                tempByte = bBytes[0];
                bBytes[0] = bBytes[3];

                bBytes[3] = tempByte;

                tempByte = bBytes[1];
                bBytes[1] = bBytes[2];

                bBytes[2] = tempByte;

                // Initially 5th byte is always 0, convert it to 255 as a sign of encrypt
                bBytes[4] = 255;

                ChangesDone = true;
            }
            else                 // Decrypt file header
            {
                if (bBytes[4] == 255)   // encrypted shape file must have 255 value in 5th byte.
                {
                    //original shape file configuration
                    tempByte = bBytes[0];
                    bBytes[0] = bBytes[3];

                    bBytes[3] = tempByte;

                    tempByte = bBytes[1];
                    bBytes[1] = bBytes[2];

                    bBytes[2] = tempByte;

                    bBytes[4] = 0;

                    ChangesDone = true;
                }
            }

            // Write manupulated bytes array into TempshapeFile .
            SHPTemp.Write(bBytes, 0, bBytes.Length);

            SHPTemp.Close();
            SHPMain.Close();

            if (ChangesDone = true)
            {
                //Copy Temp file over original Shape file and If the function succeeds return a nonzero value.
                File.Copy(sTemp, FilePath, true);
            }

            File.Delete(sTemp);   //Delete the Tempfile

        }

        #endregion
    }
}
