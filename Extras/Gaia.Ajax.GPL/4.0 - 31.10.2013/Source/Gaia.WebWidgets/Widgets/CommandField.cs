/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.Web.UI;
using System.Globalization;
using System.ComponentModel;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    using Widgets.DataControls;

    /// <summary>
    /// Represents a special field that displays command buttons to perform 
    /// selecting, editing, inserting, or deleting operations in a data-bound control.
    /// </summary>
    [Designer("Gaia.WebWidgets.Design.CommandFieldDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaWebControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class CommandField : ASP.CommandField, IAjaxDataControlField
    {
        private readonly DataControlFieldImpl<CommandField> _impl;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandField"/> class.
        /// </summary>
        public CommandField()
        {
            _impl = new DataControlFieldImpl<CommandField>(this);
        }

        /// <summary>
        /// Performs basic instance initialization for a data control field.
        /// </summary>
        /// <param name="sortingEnabled">A value that indicates whether the control supports the sorting of columns of data.</param>
        /// <param name="control">The data control that owns the <see cref="T:System.Web.UI.WebControls.DataControlField"/>.</param>
        /// <returns>Always returns false.</returns>
        public override bool Initialize(bool sortingEnabled, Control control) 
        {
            _impl.Initialize(sortingEnabled);
            return base.Initialize(sortingEnabled, control);
        }

        /// <summary>
        /// Creates an empty <see cref="T:System.Web.UI.WebControls.CommandField"/> object.
        /// </summary>
        /// <returns>
        /// An empty <see cref="T:System.Web.UI.WebControls.CommandField"/>.
        /// </returns>
        protected override ASP.DataControlField CreateField() 
        {
            return new CommandField();
        }

        /// <summary>
        /// Initializes the specified <see cref="T:System.Web.UI.WebControls.DataControlFieldCell"/> object to the specified row state.
        /// </summary>
        /// <param name="cell">The <see cref="T:System.Web.UI.WebControls.DataControlFieldCell"/> to initialize.</param>
        /// <param name="cellType">One of the <see cref="T:System.Web.UI.WebControls.DataControlCellType"/> values.</param>
        /// <param name="rowState">One of the <see cref="T:System.Web.UI.WebControls.DataControlRowState"/> values.</param>
        /// <param name="rowIndex">The zero-based index of the row that contains the cell.</param>
        public override void InitializeCell(ASP.DataControlFieldCell cell, ASP.DataControlCellType cellType, ASP.DataControlRowState rowState, int rowIndex) 
        {
            if (cellType != ASP.DataControlCellType.DataCell)
            {
                _impl.InitializeCell(cell, cellType, rowState, rowIndex);
                return;
            }

            var showEditButton = ShowEditButton;
            var showDeleteButton = ShowDeleteButton;
            var showInsertButton = ShowInsertButton;
            var showSelectButton = ShowSelectButton;
            var showCancelButton = ShowCancelButton;
            var causesValidation = CausesValidation;
            var validationGroup = ValidationGroup;

            if ((rowState & (ASP.DataControlRowState.Insert | ASP.DataControlRowState.Edit)) != ASP.DataControlRowState.Normal) 
            {
                if (((rowState & ASP.DataControlRowState.Edit) != ASP.DataControlRowState.Normal) && showEditButton) 
                {
                    AddButtonToCell(cell, "Update", UpdateText, causesValidation, validationGroup, rowIndex, UpdateImageUrl, false);
                    if (showCancelButton)
                        AddButtonToCell(cell, "Cancel", CancelText, false, string.Empty, rowIndex, CancelImageUrl, true);
                }
                
                if (((rowState & ASP.DataControlRowState.Insert) != ASP.DataControlRowState.Normal) && showInsertButton) 
                {
                    AddButtonToCell(cell, "Insert", InsertText, causesValidation, validationGroup, rowIndex, InsertImageUrl, false);
                    if (showCancelButton)
                        AddButtonToCell(cell, "Cancel", CancelText, false, string.Empty, rowIndex, CancelImageUrl, true);
                }
            } 
            else 
            {
                var firstButton = true;

                if (showEditButton) 
                {
                    AddButtonToCell(cell, "Edit", EditText, false, string.Empty, rowIndex, EditImageUrl, false);
                    firstButton = false;
                }

                if (showDeleteButton) 
                {
                    AddButtonToCell(cell, "Delete", DeleteText, false, string.Empty, rowIndex, DeleteImageUrl, !firstButton);
                    firstButton = false;
                }
                
                if (showInsertButton) 
                {
                    AddButtonToCell(cell, "New", NewText, false, string.Empty, rowIndex, NewImageUrl, !firstButton);
                    firstButton = false;
                }

                if (showSelectButton)
                    AddButtonToCell(cell, "Select", SelectText, false, string.Empty, rowIndex, SelectImageUrl, !firstButton);
            }
        }

        /// <summary>
        /// Adds the button to cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="commandName">Name of the command.</param>
        /// <param name="buttonText">The button text.</param>
        /// <param name="causesValidation">if set to <c>true</c> [causes validation].</param>
        /// <param name="validationGroup">The validation group.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="hasPreSpace">if set to <c>true</c> [has pre space].</param>
        protected virtual void AddButtonToCell(ASP.DataControlFieldCell cell, string commandName, string buttonText, bool causesValidation, string validationGroup, int rowIndex, string imageUrl, bool hasPreSpace)
        {
            ASP.IButtonControl control;
            var container = Control as ASP.IPostBackContainer;
            var commandArgument = rowIndex.ToString(NumberFormatInfo.InvariantInfo);
            
            switch (ButtonType) 
            {
                case ASP.ButtonType.Button:
                    control = DataControlFactory.Button(buttonText, commandName, commandArgument, causesValidation, container);
                    break;

                case ASP.ButtonType.Link:
                    control = DataControlFactory.LinkButton(buttonText, commandName, commandArgument, causesValidation, container);
                    break;

                default:
                    control = DataControlFactory.ImageButton(buttonText, imageUrl, commandName, commandArgument, causesValidation, container);
                    break;
            }

            control.ValidationGroup = validationGroup;
            if (hasPreSpace)
                cell.Controls.Add(new LiteralControl("&nbsp;"));
            cell.Controls.Add((Control) control);
        }

        #region [ -- IAjaxDataControlField implementation -- ]

        /// <summary>
        /// Gets a reference to the data control that the <see cref="ASP.DataControlField"/> object is associated with.
        /// </summary>
        Control IAjaxDataControlField.Control
        {
            get { return Control; }
        }

        #endregion
    }
}
