using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
   public class NotesProfilesInfo
    {

       private int _ProfileNId;
       /// <summary>
       /// Get And Set ProfileNid
       /// </summary>
       public int Profile_NId
        {
            get { return this._ProfileNId ; }
            set { this._ProfileNId = value; }
        }

       private string _ProfileName;
       /// <summary>
       /// Get and Set Profile Name
       /// </summary>
       public string Profile_Name
        {
            get { return this._ProfileName ; }
            set { this._ProfileName = value; }
        }

        private string _ProfileEMail;
       /// <summary>
       /// Get and Set Email
       /// </summary>
        public string Profile_EMail
        {
            get { return _ProfileEMail; }
            set { _ProfileEMail = value; }
        }

        private string  _ProfileCountry;
       /// <summary>
        /// Get and Set Notes Profile Country
       /// </summary>
        public string  Profile_Country
        {
            get { return this._ProfileCountry; }
            set { this._ProfileCountry = value; }
        }
       
        private string  _ProfileOrg;
       /// <summary>
        /// Get and Set Notes Profile Oraganization Name 
       /// </summary>
        public string  ProfileOrganization
        {
            get { return this._ProfileOrg; }
            set { this._ProfileOrg = value; }
        }

        private string _ProfileOrgType;
       /// <summary>
       /// Get and Set Notes Profile Organization Type
       /// </summary>
        public string Profile_Org_Type
        {
            get { return this._ProfileOrgType; }
            set { this._ProfileOrgType = value; }
        }
	
	
	
    }
}
