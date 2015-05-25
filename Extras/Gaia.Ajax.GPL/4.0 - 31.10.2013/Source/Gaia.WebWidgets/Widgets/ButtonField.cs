/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System;
using System.Web;
using System.Web.UI;
using System.Globalization;
using System.ComponentModel;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets 
{
    using Widgets.DataControls;

    /// <summary>
    /// Represents a field that is displayed as a button in a data-bound control.
    /// </summary>
    [Designer("Gaia.WebWidgets.Design.ButtonFieldDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaWebControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class ButtonField : ASP.ButtonField, IAjaxDataControlField
    {
        private PropertyDescriptor _textFieldDesc;
        private readonly DataControlFieldImpl<ButtonField> _impl;

        /// <summary>
        /// Initializes new instance of the <see cref="ButtonField"/> control.
        /// </summary>
        public ButtonField()
        {
            _impl = new DataControlFieldImpl<ButtonField>(this);
        }

        /// <summary>Returns a new instance of the field class.</summary>
        /// <returns>A new instance of the %ButtonField class.</returns>
        protected override ASP.DataControlField CreateField() 
        {
            return new ButtonField();
        }

        /// <summary>Initializes the field object.</summary>
        /// <param name="sortingEnabled">True if sorting is supported; Otherwise, false.</param>
        /// <param name="control">The data control that contains the field.</param>
        /// <returns>Always returns true.</returns>
        public override bool Initialize(bool sortingEnabled, Control control) 
        {
            _textFieldDesc = null;
            _impl.Initialize(sortingEnabled);
            return base.Initialize(sortingEnabled, control);
        }

        /// <summary>
        /// Initializes the specified cell object with the specified cell type, row state, and row index.
        /// </summary>
        /// <param name="cell">The cell to initialize.</param>
        /// <param name="cellType">One of the %DataControlCellType values.</param>
        /// <param name="rowState">One of the %DataControlRowState values.</param>
        /// <param name="rowIndex">The zero-based index of the row.</param>
        public override void InitializeCell(ASP.DataControlFieldCell cell, ASP.DataControlCellType cellType, ASP.DataControlRowState rowState, int rowIndex) 
        {
            var postBackContainer = Control as ASP.IPostBackContainer;
            _impl.InitializeCell(cell, cellType, rowState, rowIndex);
            if (cellType == ASP.DataControlCellType.Header || cellType == ASP.DataControlCellType.Footer) return;
            
            ASP.IButtonControl control;
            string row = rowIndex.ToString(NumberFormatInfo.InvariantInfo);
            switch (ButtonType) 
            {
                case ASP.ButtonType.Button:
                    control = DataControlFactory.Button(Text, CommandName, row, CausesValidation, postBackContainer);
                    break;

                case ASP.ButtonType.Link:
                    control = DataControlFactory.LinkButton(Text, CommandName, row, CausesValidation, postBackContainer);
                    break;

                default:
                    control = DataControlFactory.ImageButton(Text, ImageUrl, CommandName, row, CausesValidation, postBackContainer);
                    break;
            }
            
            control.ValidationGroup = ValidationGroup;
            var webControl = (ASP.WebControl)control;
            if (DataTextField.Length != 0 && Visible)
                webControl.DataBinding += OnDataBindField;
            cell.Controls.Add(webControl);
        }

        private void OnDataBindField(object sender, EventArgs e)
        {
            var control = (Control)sender;
            var namingContainer = control.NamingContainer;
            
            if (namingContainer == null)
                throw new HttpException("Missing NamingContainer of DataControlField");

            var component = DataBinder.GetDataItem(namingContainer);
            if (component == null && !DesignMode)
                throw new HttpException("DataItem Was Not Found");

            if (_textFieldDesc == null && component != null) 
            {
                _textFieldDesc = TypeDescriptor.GetProperties(component).Find(DataTextField, true);
                if (_textFieldDesc == null && !DesignMode)
                    throw new HttpException("DataTextField Was Not Found");
            }

            string text = null;
            if (_textFieldDesc != null && component != null)
            {
                var value = _textFieldDesc.GetValue(component);
                if (value != null)
                    text = FormatDataTextValue(value);
            }

            if (text == null)
                text = "Sample_Databound_Text";

            ((ASP.IButtonControl) control).Text = text;
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
