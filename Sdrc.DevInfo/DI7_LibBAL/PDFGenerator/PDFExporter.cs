using System;
using System.Collections.Generic;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using System.IO;
using System.Drawing.Imaging;

namespace DevInfo.Lib.DI_LibBAL.PDFGenerator
{
    /// <summary>
    /// Use this class to export images and contents from PDF files.
    /// </summary>
    public class PDFExporter
    {

        #region "-- Public --"

        #region "-- Methods --"
        
        /// <summary>
        /// Extracts the images from the given PDF file and save it into the given images folder
        /// </summary>
        /// <param name="sourcePdfFile"></param>
        /// <param name="imagesFolderPath"></param>
        public  List<string> ExtractImages(string sourcePdfFile, string imagesFolderPath)
        {
            List<string> RetVal = new List<string>();
            string ImageFileName = string.Empty;
            RandomAccessFileOrArray raf = null;
            PdfReader reader = null;
            PdfObject pdfObj = null;
            PdfStream pdfStream = null;
            int ImageNo = 1;

            try
            {
                raf = new RandomAccessFileOrArray(sourcePdfFile);

                reader = new PdfReader(raf, null);
                for (int i = 0; i < reader.XrefSize; i++)
                {
                    pdfObj = reader.GetPdfObject(i);

                    if (pdfObj != null && pdfObj.IsStream())
                    {
                        pdfStream = (PdfStream)(pdfObj);

                        PdfObject subtype = pdfStream.Get(PdfName.SUBTYPE);
                        if (subtype != null && subtype.ToString() == PdfName.IMAGE.ToString())
                        {

                            Byte[] bytes = PdfReader.GetStreamBytesRaw((PRStream)(pdfStream));


                            if (bytes != null)
                            {
                                try
                                {

                                    using (System.IO.MemoryStream memStream = new System.IO.MemoryStream(bytes))
                                    {

                                        memStream.Position = 0;

                                        System.Drawing.Image img = System.Drawing.Image.FromStream(memStream);
                                        ImageFileName = imagesFolderPath + "\\" + Path.GetFileNameWithoutExtension(sourcePdfFile) + "_" + ImageNo + ".png";
                                        img.Save(ImageFileName);

                                        RetVal.Add(imagesFolderPath + "\\" + Path.GetFileNameWithoutExtension(sourcePdfFile) + "_" + ImageNo + ".png");

                                        ImageNo++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //'Most likely the image is in an unsupported format

                                    //'Do nothing

                                    //'You can add your own code to handle this exception if you want to

                                }

                            }

                        }

                    }

                }

                reader.Close();
            }
            catch (Exception ex)
            {
                //ExceptionFacade.ThrowException(ex);

            }
            return RetVal;
        }

        #endregion

        #endregion
    }
}
