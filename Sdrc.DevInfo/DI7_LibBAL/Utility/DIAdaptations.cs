using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// Class for DevInfo adaptations primary information
    /// </summary>
    public class DIAdaptations
    {

        #region " -- Private -- "

        #region " -- Methods -- "


        /// <summary>
        /// Get DIAdaptation instance for collection based on adaptation name
        /// </summary>
        /// <param name="diAdaptations"></param>
        /// <param name="adaptationName"></param>
        /// <returns></returns>
        private static DIAdaptation GetDIAdaptation(DIAdaptations diAdaptations, string adaptationName)
        {
            DIAdaptation RetVal = null;
            foreach (DIAdaptation diAdaptation in diAdaptations.Adaptations)
            {
                if (string.Compare(adaptationName, diAdaptation.AdaptationName, true) == 0)
                {
                    RetVal = diAdaptation;
                    break;
                }
            }

            return RetVal;
        }

        #endregion

        #endregion

        #region " -- Public -- "

        #region " -- Constants -- "
        /// <summary>
        /// DIAdaptations.xml
        /// </summary>
        public const string DI_ADAPTATIONS_FILENAME = "DIAdaptations.xml";
        #endregion

        #region " -- Constructor -- "
        public DIAdaptations()
        {
        }

        #endregion

        #region " -- Properties -- "

        private string _WorldwideUserGId = string.Empty;
        /// <summary>
        /// Get or set the User GId. Default = Empty
        /// </summary>
        public string WorldwideUserGId
        {
            get { return _WorldwideUserGId; }
            set { _WorldwideUserGId = value; }
        }

        private bool _WorldWideUserRegisteredComplete = false;
        /// <summary>
        /// Gets or sets the value whether WorldWide registeration is complete.
        /// </summary>
        public bool WorldWideUserRegisteredComplete
        {
            get { return _WorldWideUserRegisteredComplete; }
            set { _WorldWideUserRegisteredComplete = value; }
        }

        private List<string> _WorldWideAdaptations = new List<string>();
        /// <summary>
        /// Gets or sets the collection of adaptations used in Worldwide.
        /// </summary>
        public List<string> WorldWideAdaptations
        {
            get { return _WorldWideAdaptations; }
            set { _WorldWideAdaptations = value; }
        }

        private DIAdaptation _LastUsedAdaptation = null;
        /// <summary>
        /// Gets or sets the last used adaptation
        /// </summary>
        public DIAdaptation LastUsedAdaptation
        {
            get { return _LastUsedAdaptation; }
            set { _LastUsedAdaptation = value; }
        }

        private GenericCollection<DIAdaptation> _Adaptations = new GenericCollection<DIAdaptation>();
        /// <summary>
        /// Bottom panel buttons collection
        /// </summary>
        public GenericCollection<DIAdaptation> Adaptations
        {
            get { return _Adaptations; }
            set { _Adaptations = value; }
        }

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// deserialize DIadaptaions xml file .
        /// </summary>
        /// <param name="fileName">Path of DIAdaptations.xml. Path.Combine(SideBar Exe Startup Path, DIAdaptations.DI_ADAPTATIONS_FILENAME) </param>
        /// <returns></returns>
        public static DIAdaptations Load(string filenameWithPath)
        {
            DIAdaptations Retval;
            try
            {
                XmlSerializer DIAdaptationsSerializer = new XmlSerializer(typeof(DIAdaptations));
                StreamReader DIAdaptationsReader = new StreamReader(filenameWithPath);
                Retval = (DIAdaptations)DIAdaptationsSerializer.Deserialize(DIAdaptationsReader);
                DIAdaptationsReader.Close();

                //-- Also validate for existance of other Adaptations. 
                //-- Some of them may be uninsatlled and  AdaptationFolder may no longer be available then remove them from collection
                try
                {
                    foreach (DIAdaptation diAdaptation in Retval.Adaptations)
                    {
                        if (!System.IO.Directory.Exists(diAdaptation.AdaptationFolder))
                        {
                            //-- If uninstalled adaptaion was last used adaptation then reset 
                            if (string.Compare(diAdaptation.AdaptationFolder, Retval.LastUsedAdaptation.AdaptationFolder)== 0)
                            {
                                Retval.LastUsedAdaptation = null;
                            }
                            Retval.Adaptations.Remove(DIAdaptations.GetDIAdaptation(Retval, diAdaptation.AdaptationName));
                            break;
                        }
                    }

                    //-- If no last used adaptaion then set the first adaptaion in collection as last used
                    if (Retval.LastUsedAdaptation == null && Retval.Adaptations.Count > 0)
                    {
                        Retval.LastUsedAdaptation = Retval.Adaptations[0];
                    }

                    Retval.Save(filenameWithPath);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }

            }
            catch (Exception)
            {
                Retval = null;
            }
            return Retval;
        }

        /// <summary>
        /// Set the Last used adaptation in DIAdaptations.xml. If file does not exists then create one
        /// </summary>
        /// <param name="adaptationName"></param>
        /// <param name="adaptationFolder"></param>
        public static void SetDIAdaptations(string executableFolder, string adaptationName, string adaptationVersion, string adaptationFolder)
        {
            try
            {
                string DIAdaptationsPath = Path.Combine(executableFolder, DIAdaptations.DI_ADAPTATIONS_FILENAME);
                DIAdaptations oDIAdaptations = null;
                DIAdaptation oDIAdaptation = null;

                //-- Check for existance of C:\DevInfo\DevInfo 6.0\DIAdaptations.xml
                //-- If DIAdaptation.xml doesn't exist then create one
                if (File.Exists(DIAdaptationsPath))
                {
                    //-- Load serialised DIAdaptation.xml
                    oDIAdaptations = DIAdaptations.Load(DIAdaptationsPath);

                    //-- Get current adaption in collection
                    oDIAdaptation = DIAdaptations.GetDIAdaptation(oDIAdaptations, adaptationName);
                }
                else
                {
                    oDIAdaptations = new DIAdaptations();
                }

                //-- If current adaptation doesnt exists in collection then add it to collection
                if ((oDIAdaptation == null))
                {
                    oDIAdaptation = new DIAdaptation(adaptationName,adaptationVersion, adaptationFolder);
                    oDIAdaptations.Adaptations.Add(oDIAdaptation);
                }
                oDIAdaptations.LastUsedAdaptation = oDIAdaptation;

                oDIAdaptations.Save(DIAdaptationsPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }


        /// <summary>
        /// Save the DIAdaptations class in the form of XML.
        /// </summary>
        /// <param name="fileName">file name with path</param>
        public void Save(string filenameWithPath)
        {
            XmlSerializer DIAdaptationsSerializer = new XmlSerializer(typeof(DIAdaptations));
            StreamWriter DIAdaptationsWriter = new StreamWriter(filenameWithPath);
            DIAdaptationsSerializer.Serialize(DIAdaptationsWriter, this);
            DIAdaptationsWriter.Close();
        }
        #endregion

        #endregion
    }


}
