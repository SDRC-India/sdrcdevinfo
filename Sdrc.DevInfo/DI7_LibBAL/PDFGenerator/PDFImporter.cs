using System;
using System.Collections.Generic;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using System.IO;

namespace DevInfo.Lib.DI_LibBAL.PDFGenerator
{
    
    /// <summary>
    /// Use this class to import images and contents into PDF files.
    /// </summary>
    public class PDFImporter
    {        
        #region "-- Private --"

        #region "-- Variables --"

        private string OutputFileNameWPath = string.Empty;
        private Document PDFDocument = null;

        #endregion

        #region "-- Methods --"

        private void CreatePDFDocument()
        {

            try
            {
                // Step1: create pdf document
                this.PDFDocument = new Document();

                // Step2: create a writer that listens to the document
                // and directs a PDF-stream to a file
                PdfWriter.GetInstance(this.PDFDocument, new FileStream(this.OutputFileNameWPath, FileMode.Create));

                // Step3: open pdf document
                this.PDFDocument.Open();

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        #endregion

        #endregion

        #region "-- Public  --"

        #region "-- New/Dispose --"

        public PDFImporter(string outputFileNameWPath)
        {
            this.OutputFileNameWPath = outputFileNameWPath;

            // create document-object
            this.CreatePDFDocument();
        }

        public void Dispose()
        {
            //  close the document
            if (this.PDFDocument != null)
            {
                if (this.PDFDocument.IsOpen())
                {
                    try
                    {
                        this.PDFDocument.Close();
                    }
                    catch (Exception)
                    {
                        //do noting.
                    }
                }
            }
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Sets the page size
        /// </summary>
        /// <param name="PageSize"></param>
        public void SetPageSize(System.Drawing.Size PageSize )
        {
            this.PDFDocument.SetPageSize( new Rectangle(0, 0, PageSize.Width+this.PDFDocument.LeftMargin+this.PDFDocument.RightMargin, PageSize.Height+this.PDFDocument.TopMargin+this.PDFDocument.BottomMargin));
        }

        /// <summary>
        /// Inserts the given image as a new page into PDF document
        /// </summary>
        /// <param name="imageFileNameWPath"></param>
        public void InsertImageIntoNewPage(string imageFileNameWPath)
        {
            Image ImageFile = null;
            try
            {
                if (this.PDFDocument != null)
                {
                    if (this.PDFDocument.IsOpen())
                    {

                        ImageFile = iTextSharp.text.Image.GetInstance(imageFileNameWPath);

                        //ImageFile.scale;
                        this.PDFDocument.NewPage();

                        this.PDFDocument.Add(ImageFile);                          
                    }
                }
            }
            catch (Exception ex)
            {
               // ExceptionFacade.ThrowException(ex);
            }
        }

        #endregion

        #endregion
    }
}
