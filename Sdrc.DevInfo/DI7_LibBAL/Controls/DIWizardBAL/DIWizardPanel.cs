using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.DIWizardBAL
{
    /// <summary>
    /// Class contains the properties of the wizard panel
    /// </summary>
    public class DIWizardPanel
    {

        #region " -- Enums --"

        /// <summary>
        /// enum to define the panel type used in DIDataWizard
        /// </summary>
        public enum PanelType
        {            
            Indicator,
            Area,
            TimePeriod,
            Source,
            Database,
            Gallery
        }

        #endregion

        #region " -- Public -- "

        #region " -- New / Dispose "

        #endregion

        #region " -- Properties / Variables -- "

        private PanelType _Type;
        /// <summary>
        /// Gets or sets the panel type.
        /// </summary>
        public PanelType Type
        {
            get 
            { 
                return this._Type; 
            }
            set 
            {
                this._Type = value; 
            }
        }

        private string _BGImageNormal = string.Empty;
        /// <summary>
        /// Gets or sets the panel normal image file name.
        /// </summary>
        public string BGImageNormal
        {
            get 
            {
                return this._BGImageNormal; 
            }
            set 
            {
                this._BGImageNormal = value; 
            }
        }

        private string _BGImageSelected = string.Empty;
        /// <summary>
        /// Gets or sets the panel selected image file name.
        /// </summary>
        public string BGImageSelected
        {
            get 
            { 
                return this._BGImageSelected; 
            }
            set 
            {
                this._BGImageSelected = value; 
            }
        }

        private string _BGImageHover = string.Empty;
        /// <summary>
        /// Gets or sets the panel hower image file name.
        /// </summary>
        public string BGImageHover
        {
            get 
            {
                return this._BGImageHover; 
            }
            set 
            {
                this._BGImageHover = value; 
            }
        }

        private string _Caption = string.Empty;
        /// <summary>
        /// Gets or sets the panel caption.
        /// </summary>
        public string Caption
        {
            get 
            {
                return this._Caption; 
            }
            set 
            {
                this._Caption = value; 
            }
        }

        private string _StatusCaption = string.Empty;
        /// <summary>
        /// Gets or sets the panel status caption.
        /// </summary>
        public string StatusCaption
        {
            get 
            {
                return this._StatusCaption; 
            }
            set 
            {
                this._StatusCaption = value; 
            }
        }

        private string _LinkCaption = string.Empty;
        /// <summary>
        /// Gets or sets the panle link caption.
        /// </summary>
        public string LinkCaption
        {
            get 
            {
                return this._LinkCaption; 
            }
            set 
            {
                this._LinkCaption = value; 
            }
        }

        private string _LinkIconNormal = string.Empty;
        /// <summary>
        /// Gets or sets the panel normal link icon file name.
        /// </summary>
        public string LinkIconNormal
        {
            get 
            {
                return this._LinkIconNormal; 
            }
            set 
            {
                this._LinkIconNormal = value; 
            }
        }

        private string _LinkIconSelected = string.Empty;
        /// <summary>
        /// Gets or sets the panel selected link icon file name.
        /// </summary>
        public string LinkIconSelected
        {
            get 
            {
                return this._LinkIconSelected; 
            }
            set 
            {
                this._LinkIconSelected = value; 
            }
        }

        private string _LinkIconHover = string.Empty;
        /// <summary>
        /// Gets or sets the panle hover link icon file name.
        /// </summary>
        public string LinkIconHover
        {
            get 
            {
                return this._LinkIconHover; 
            }
            set 
            {
                this._LinkIconHover = value; 
            }
        }

        private string _IconNormal = string.Empty;
        /// <summary>
        /// Gets or sets the panel normal icon file name.
        /// </summary>
        public string IconNormal
        {
            get 
            {
                return this._IconNormal; 
            }
            set 
            {
                this._IconNormal = value;
            }
        }

        private string _IconSelected = string.Empty;
        /// <summary>
        /// Gets or sets the panel selected icon file name.
        /// </summary>
        public string IconSelected
        {
            get 
            {
                return this._IconSelected; 
            }
            set 
            {
                this._IconSelected = value; 
            }
        }

        private string _IconHover = string.Empty;
        /// <summary>
        /// Gets or sets the panel hover icon file name.
        /// </summary>
        public string IconHover
        {
            get 
            {
                return this._IconHover; 
            }
            set 
            {
                this._IconHover = value; 
            }
        }

        private bool _Selected = false;
        /// <summary>
        /// Gets or gets the selected panel.
        /// </summary>
        public bool Selected
        {
            get 
            {
                return this._Selected; 
            }
            set 
            { 
                this._Selected = value; 
            }
        }

        private int _Order = 0;
        /// <summary>
        /// Gets or sets the panel order.
        /// </summary>
        public int Order
        {
            get 
            {
                return this._Order; 
            }
            set 
            {
                this._Order = value; 
            }
        }

        private string _GalleryCaption = string.Empty;
        /// <summary>
        /// Gets or sets the Gallery caption.
        /// </summary>
        public string GalleryCaption
        {
            get 
            {
                return this._GalleryCaption; 
            }
            set 
            {
                this._GalleryCaption = value; 
            }
        }

        private string _GalleryStatusCaption = string.Empty;
        /// <summary>
        /// Gets or sets the Gallery status caption.
        /// </summary>
        public string GalleryStatusCaption
        {
            get 
            {
                return this._GalleryStatusCaption; 
            }
            set 
            {
                this._GalleryStatusCaption = value; 
            }
        }
	
	

        #endregion

        #region " -- Methods -- "

        
        #endregion

        #endregion

        #region " -- Private -- "

        #region " -- Methods -- "

        #endregion

        #endregion

    }
}
