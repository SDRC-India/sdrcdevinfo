using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Utility.MRU
{
        /// <summary>
        /// Returns the Most Resently Used Loations
        /// </summary>
        public class MRUFile:IDisposable    
        {

           #region "--Private--"

            #region "-- Variables --"

            private string PrefFileNameWPath=string.Empty;

            #endregion

            #region "-- New / Dispose --"

            private MRUFile()
            {
                // do not implement this
            }

            #endregion

            #region "--Methods--"

            private string GetMRUString(MRUKey mruKeyValue)
            {
                string MruKeyValue = string.Empty;

                if (mruKeyValue == MRUKey.MRU_DATABASES)
                {
                    MruKeyValue = "mru_databases";
                }
                else if (mruKeyValue == MRUKey.MRU_DI4_DATABASES)
                {
                    MruKeyValue = "mru_di4_databases";
                }
                else if (mruKeyValue == MRUKey.MRU_LANGUAGE)
                {
                    MruKeyValue = "mru_language";
                }
                else if (mruKeyValue == MRUKey.MRU_REPORTS)
                {
                    MruKeyValue = "mru_reports";
                }
                else if (mruKeyValue == MRUKey.MRU_SPREADSHEETS)
                {
                    MruKeyValue = "mru_spreadsheets";
                }
                else if (mruKeyValue == MRUKey.MRU_TEMPLATES)
                {
                    MruKeyValue = "mru_templates";
                }
                else if (mruKeyValue == MRUKey.MRU_SPS)
                {
                    MruKeyValue = "mru_spss_sps";
                }
                else if (mruKeyValue == MRUKey.MRU_SPO)
                {
                    MruKeyValue = "mru_spss_spo";
                }
                else if (mruKeyValue == MRUKey.MRU_DX_FREE_FORMAT)
                {
                    MruKeyValue = "mru_xls_free";
                }
                else if (mruKeyValue == MRUKey.MRU_DX_SMS)
                {
                    MruKeyValue = "mru_sms";
                }
                else if (mruKeyValue == MRUKey.MRU_DX_DESKTOP_DATACAPTURE)
                {
                    MruKeyValue = "mru_desktop_datacapture";
                }
                else if (mruKeyValue == MRUKey.MRU_PDA_FORMAT)
                {
                    MruKeyValue = "mru_pda_format";
                }
                else if (mruKeyValue == MRUKey.MRU_IMPORT_COMMENTS)
                {
                    MruKeyValue = "mru_import_Comments";
                }
                else if (mruKeyValue == MRUKey.MRU_CRIS)
                {
                    MruKeyValue = "mru_dx_cris";
                }
                else if (mruKeyValue == MRUKey.MRU_STATA_SMCL)
                {
                    MruKeyValue = "mru_dx_stata_smcl";
                }
                else if (mruKeyValue == MRUKey.MRU_SAS_XLS)
                {
                    MruKeyValue = "mru_sas_xls"; // dont change this
                }
                else if (mruKeyValue == MRUKey.MRU_STATA_DAT)
                {
                    MruKeyValue = "mru_stata"; // dont change this
                }

                return MruKeyValue;
            }

           
            #endregion

            #endregion

            #region "-- Public --"

            #region "-- New / Dispose --"

            public MRUFile(string prefFileNameWPath)
            {
                this.PrefFileNameWPath = prefFileNameWPath;
                DIPrefFile.Open(this.PrefFileNameWPath);
            }

            #endregion

            /// <summary>
            /// Returns the MRU values
            /// </summary>
            /// <param name="mruKeyValue">Set MRU Key</param>
            public List<string> GetMRUValues(MRUKey mruKeyValue)
            {
                //-- this method fetch particular MruKey and add keys to oCtrl 
                List<string> RetVal=new List<string>();
                string MruValue = this.GetMRUString(mruKeyValue);
                string Value=string.Empty;
                int i;
                             

                //-- if (pref.xml file not exit) exit
                if (File.Exists(this.PrefFileNameWPath))
                {

                    try
                    {
                        for (i = 1; i <= 4; i++)
                        {
                            Value = DIPrefFile.GetValue(MruValue, "mru" + i);
                            if (!string.IsNullOrEmpty(Value))
                            {
                                RetVal.Add(Value);
                            }
                        }
                    }
                    catch (Exception )
                    {
                    }
                }
                    return RetVal;
            }
     
            /// <summary>
            /// Set the new value in MRU and save it into pref file.
            /// </summary>
            /// <param name="mruKeyValue">MRUKey eMruKey</param>
            /// <param name="newValue">new mru value</param>
            public void SetMRU(MRUKey mruKeyValue, string newValue)
            {
                int i;
                bool IsAlreadyExists = false;
                string MruValue =this.GetMRUString(mruKeyValue);
                string Value=string.Empty;

                if ((File.Exists(this.PrefFileNameWPath)))
                {
                    try
                    {
                        for (i = 1; i <= 4; i++)
                        {
                            // get value from pref file
                            Value = DIPrefFile.GetValue(MruValue, MRUConstants.MRUString + i);

                            if (!string.IsNullOrEmpty(Value))
                            {
                                if (Value.ToUpper() == newValue.ToUpper())
                                {
                                    IsAlreadyExists = true;
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception )
                    {
                        // do nothing
                    }

                    try
                    {
                        if (IsAlreadyExists == false)
                        {
                            for (i = 4; i >= 2; i--)
                            {
                                DIPrefFile.UpdateValue(MruValue, MRUConstants.MRUString + i, DIPrefFile.GetValue(MruValue, MRUConstants.MRUString + (i - 1)));
                            }
                            DIPrefFile.UpdateValue(MruValue, MRUConstants.MRUString + "1", newValue);
                        }
                        else
                        {
                            for (i = 1; i <= 4; i++)
                            {
                                Value = DIPrefFile.GetValue(MruValue, MRUConstants.MRUString + i);
                                if (!string.IsNullOrEmpty(Value))
                                {
                                    if (Value.ToUpper() == newValue.ToUpper())
                                    {
                                        string mru_1 = DIPrefFile.GetValue(MruValue, MRUConstants.MRUString + "1");
                                        DIPrefFile.UpdateValue(MruValue, "mru1", newValue);
                                        DIPrefFile.UpdateValue(MruValue, "mru" + i, mru_1);
                                        break;
                                    }
                                }
                            }

                        }

                       
                    }
                    catch (Exception )
                    {
                    }
                 
                }
            }

            #endregion


            #region IDisposable Members

            public void Dispose()
            {
                try
                {
                    DIPrefFile.Close();
                }
                catch (Exception)
                {
                                     
                }
                
            }

            #endregion
        }


}
