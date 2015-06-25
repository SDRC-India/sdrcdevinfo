using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.VideoFrameBAL
{
    public class FrameEffect
    {
        #region " -- Public -- "

        #region " -- New / Dispose -- "

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="effectID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        public FrameEffect(int effectID, int effectTime, string param1, string param2, string param3, string param4, string effectTooltip)
        {
            this._EffectID = effectID;
            this._EffectTime = effectTime;
            this._Param1 = param1;
            this._Param2 = param2;
            this._Param3 = param3;
            this._Param4 = param4;
            this._EffectToolTip = effectTooltip;
        }

        /// <summary>
        /// Constructor only for serilization purposes.
        /// </summary>
        public FrameEffect()
        { }

        #endregion

        #region " -- Properies -- "

        private int _EffectID = 1;
        /// <summary>
        /// Gets or sets the effect Id
        /// </summary>
        public int EffectID
        {
            get 
            { 
                return this._EffectID; 
            }
            set 
            {
                this._EffectID = value; 
            }
        }

        private int _EffectTime = 2;
        /// <summary>
        /// Gets or sets the effect end time
        /// </summary>
        public int EffectTime
        {
            get 
            {
                return this._EffectTime; 
            }
            set 
            {
                this._EffectTime = value; 
            }
        }

        private string _Param1 = string.Empty;
        /// <summary>
        /// Gets or sets the effects param1.
        /// </summary>
        public string Param1
        {
            get 
            { 
                return this._Param1; 
            }
            set 
            {
                this._Param1 = value; 
            }
        }

        private string _Param2 = string.Empty;
        /// <summary>
        /// Gets or sets the effects param2.
        /// </summary>
        public string Param2
        {
            get 
            {
                return this._Param2; 
            }
            set 
            {
                this._Param2 = value; 
            }
        }

        private string _Param3 = string.Empty;
        /// <summary>
        /// Gets or sets the effects param3.
        /// </summary>
        public string Param3
        {
            get 
            {
                return this._Param3; 
            }
            set 
            {
                this._Param3 = value; 
            }
        }

        private string _Param4 = string.Empty;
        /// <summary>
        /// Gets or sets the effects param4.
        /// </summary>
        public string Param4
        {
            get 
            {
                return this._Param4; 
            }
            set 
            {
                this._Param4 = value; 
            }
        }

        private string _EffectToolTip = string.Empty;
        /// <summary>
        /// Gets or sets the effect tool tip.
        /// </summary>
        public string EffectToolTip
        {
            get 
            {
                return this._EffectToolTip; 
            }
            set 
            {
                this._EffectToolTip = value; 
            }
        }

        #endregion

        #endregion

    }
}
