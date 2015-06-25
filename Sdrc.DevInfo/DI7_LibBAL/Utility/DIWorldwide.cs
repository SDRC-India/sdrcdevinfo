using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// This class acts as wrapper class for diWorldwide user registration.
    /// <para>This class internally calls the web methods provided in diWorldwide webservice.</para>
    /// </summary>
    public class DIWorldwide
    {

        private static string WorldwideUserGID = string.Empty;
        private static bool WorldwideOnlineStatus = false;

        private static void ThreadUpdateWorldwideStatus()
        {
            //-- It updates online/ Offline status in DiWorldWide forum
             try
            {
                DIWWSStatus.UserOnlineStatus DiWWSStatus = new DIWWSStatus.UserOnlineStatus();

                if (DiWWSStatus != null)
                {
                    if (String.IsNullOrEmpty(DIWorldwide.WorldwideUserGID) == false)
                    {
                        DiWWSStatus.SetStatus(DIWorldwide.WorldwideOnlineStatus, DIWorldwide.WorldwideUserGID);
                    }
                }

                //-- Set GID blank
                DIWorldwide.WorldwideUserGID = string.Empty;
            }
            catch
            {

            }
        }


        /// <summary>
          /// It updates online/ Offline status in DiWorldWide forum
          /// </summary>
        public static void  SetWorldwideUserStatus(bool online, string userGUID)
        {
            //-- Set private variables
            DIWorldwide.WorldwideOnlineStatus = online;
            DIWorldwide.WorldwideUserGID = userGUID;

            //-- update in seperate thread
            Thread WorldwideThread = new Thread(new ThreadStart(ThreadUpdateWorldwideStatus));
            WorldwideThread.Priority = ThreadPriority.Lowest;
            WorldwideThread.Start();
        }
        
        /// <summary>
        /// Sets the relationships between userGID and Adaption in diWorldwide server.
        /// </summary>
        /// <param name="userGUID"></param>
        /// <param name="currentAdaptationName"></param>
        /// <param name="currentAdaptationVersion"></param>
        public static void SetUserAdaptationInfo(string userGUID, string currentAdaptationName, string currentAdaptationVersion)
        {
            try
            {
                DIWWSRegistration.UserRegistration WorldwideInfo = new DIWWSRegistration.UserRegistration();

                if ((WorldwideInfo != null))
                {
                    WorldwideInfo.SetUserAdaptationInfo(userGUID, currentAdaptationName, currentAdaptationVersion);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Registers the user to the diWorldwide Web service
        /// </summary>
        /// <param name="userFirstName"></param>
        /// <param name="userLastName"></param>
        /// <param name="email"></param>
        /// <param name="organisation"></param>
        /// <param name="country"></param>
        /// <returns>On Success will return GID created for the registered User else return -1</returns>
        /// <remarks></remarks>
        public static string RegisterUser(string userFirstName, string userLastName, string email, string password, string organisation, string country)
        {

            string retVal = null;

            try
            {
                DIWWSRegistration.UserRegistration WorldWideRegister = new DIWWSRegistration.UserRegistration();

                // -- Call the diWorldWide Web Service to register the User 

                //'-- Register user by calling RegisterUser() of webservice Class.
                //   RegisterUser() returns UserGID if successfuly done.
                retVal = (string)(WorldWideRegister.RegisterUser(userFirstName, userLastName, email, password, organisation, country));

                if (string.IsNullOrEmpty(retVal))
                {
                    retVal = "-1";
                }
            }

            catch (Exception ex)
            {
                retVal = "-1";
            }

            return retVal;

        }

        /// <summary>
        /// Gets the country list available in diWorldwide server.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCountryList()
        {
            DataTable RetVal = null;

            try
            {
                DIWWSUtility.Utility WorldWideUtility = new DIWWSUtility.Utility();
                
                //RetVal = WorldWideUtility.GetCountryList().Tables[0];
            }
            catch 
            {
                
            }

            return RetVal;
        }

        public static DataTable GetDIWorldwideUser(string userGUID)
        {
            DataTable DtUserDetails = null;

            try
            {
                DIWWSRegistration.UserRegistration WorldWideRegister = new DIWWSRegistration.UserRegistration();

                DtUserDetails = WorldWideRegister.GetUserInformation(userGUID);
            }
            catch
            {

            }

            return DtUserDetails;
        }

        public static bool UpdateUser(string userGUID, string userFirstName, string userLastName, string email, string organisation, string country)
        {
            bool retVal = false;

            try
            {
                DIWWSRegistration.UserRegistration WorldWideRegister = new DIWWSRegistration.UserRegistration();

                // -- Call the diWorldWide Web Service to update User details

                //'-- Update user by calling webservice Class.
                object oRet = (string)(WorldWideRegister.UpdateUserInfo(userGUID, userFirstName, userLastName, email, organisation, country));

                if (oRet != null)
                {
                    retVal = true;
                }
            }

            catch (Exception ex)
            {
                retVal = false;
            }

            return retVal;

        }

    }
}
