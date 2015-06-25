using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.Controls.TreeSelectionBAL
{
    /// <summary>
    /// Provides the information of the selected node
    /// </summary>
    public class SelectedNodeInfo
    {
        #region "-- public --"

        #region "-- Variables --"

        private int _NId = 0;
        /// <summary>
        /// Gets or sets NId
        /// </summary>
        public int NId
        {
            get { return this._NId; }
            set { this._NId = value; }
        }

        private string _Text = string.Empty;
        /// <summary>
        /// Gets or sets node's text 
        /// </summary>
        public string Text
        {
            get { return this._Text; }
            set { this._Text = value; }
        }

        private string _GId = string.Empty;
        /// <summary>
        /// Gets or Sets GID
        /// </summary>
        public string GId
        {
            get { return this._GId; }
            set { this._GId = value; }
        }

        private int _Level = 0;
        /// <summary>
        /// Gets or sets level
        /// </summary>
        public int Level
        {
            get { return this._Level; }
            set { this._Level = value; }
        }

        private int _ParentNid = 0;
        /// <summary>
        /// Gets or sets the parent NId.
        /// </summary>
        public int ParentNid
        {
            get { return this._ParentNid; }
            set { this._ParentNid = value; }
        }

        private bool _Show = true;
        /// <summary>
        /// Gets or sets the Node visibility
        /// </summary>
        public bool Show
        {
            get { return _Show; }
            set { _Show = value; }
        }
	
	

        #endregion

        #region "-- New/Dispose --"

        /// <summary>
        /// Only for serliazation purpose
        /// </summary>
        public SelectedNodeInfo()
        { 
            //--Do Nothing
        }

        public SelectedNodeInfo(int NId, string text, string GId, int level)
        {
            this._NId = NId;
            this._Text = text;
            this._GId = GId;
            this._Level = level;
        }

        public SelectedNodeInfo(int NId, string text, string GId, int level, int parentNId)
        {
            this._NId = NId;
            this._Text = text;
            this._GId = GId;
            this._Level = level;
            this._ParentNid = parentNId;
        }

        public SelectedNodeInfo(int NId, string text, string GId, int level, int parentNId, bool show)
        {
            this._NId = NId;
            this._Text = text;
            this._GId = GId;
            this._Level = level;
            this._ParentNid = parentNId;
            this._Show = show;
        }

        #endregion

        #region " -- Methods -- "

        public static bool Sort(List<SelectedNodeInfo> selectedNode)
        {
            bool Retval = true;
            try
            {
                selectedNode.Sort(delegate(SelectedNodeInfo CompareNId1, SelectedNodeInfo CompareNId2) { return CompareNId1.Level.CompareTo(CompareNId2.Level); });

                for (int i = 0; i < selectedNode.Count-2; i++)
                {
                    int Index = i;
                    for (int j = i + 1; j < selectedNode.Count-1; j++)
                    {
                        if (selectedNode[i].Level == selectedNode[j].Level && string.Compare(selectedNode[i].Text.ToLower(), selectedNode[j].Text.ToLower()) >= 0)
                        {
                            Index = j;
                        }                        
                    }
                    SelectedNodeInfo TempNId = new SelectedNodeInfo(selectedNode[Index].NId, selectedNode[Index].Text, selectedNode[Index].GId, selectedNode[Index].Level, selectedNode[Index].ParentNid);
                    selectedNode[Index] = selectedNode[i];
                    selectedNode[i] = TempNId;
                }
            }
            catch (Exception)
            {
                Retval = false;
            }
            return Retval;
        }  

        #endregion

        #endregion
    }
}
