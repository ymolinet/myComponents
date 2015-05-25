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
using System.ComponentModel;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Represents a Boolean field that is displayed as a check box in a data-bound control.
    /// </summary>
    [Designer("Gaia.WebWidgets.Design.CheckBoxFieldDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaWebControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class CheckBoxField : ASP.CheckBoxField, IAjaxBoundField
    {
        private readonly BoundFieldImpl<CheckBoxField> _impl;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CheckBoxField()
        {
            _impl = new BoundFieldImpl<CheckBoxField>(this);
        }

        /// <summary>Initializes the field object.</summary>
        /// <param name="enableSorting">True if sorting is supported; Otherwise, false.</param>
        /// <param name="control">The data control that contains the field.</param>
        /// <returns>Always returns true.</returns>
        public override bool Initialize(bool enableSorting, Control control) 
        {
            _impl.Initialize(enableSorting);
            return base.Initialize(enableSorting, control);
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
            _impl.InitializeCell(cell, cellType, rowState, rowIndex);
        }

        /// <summary>
        /// Initializes the specified cell object with the specified row state.
        /// </summary>
        /// <param name="cell">The cell to initialize.</param>
        /// <param name="rowState">One of the %DataControlRowState values.</param>
        protected override void InitializeDataCell(ASP.DataControlFieldCell cell, ASP.DataControlRowState rowState)
        {
            CheckBox checkBox = null;
            bool canDataBindField = false;

            bool isEdit = ((rowState & ASP.DataControlRowState.Edit) != ASP.DataControlRowState.Normal);
            if ((isEdit && !ReadOnly) || ((rowState & ASP.DataControlRowState.Insert) != ASP.DataControlRowState.Normal))
            {
                checkBox = new CheckBox {ToolTip = HeaderText};
                canDataBindField = DataField.Length != 0 && isEdit;
            }
            else if (DataField.Length != 0)
            {
                checkBox = new CheckBox {Text = Text, Enabled = false};
                canDataBindField = true;
            }
            
            if (checkBox != null)
                cell.Controls.Add(checkBox);
            
            if (canDataBindField && Visible)
                checkBox.DataBinding += OnDataBindField;
        }

        /// <summary>
        /// Binds the value of a field to a check box in the <see cref="T:Gaia.WebWidgets.CheckBoxField"/> object.
        /// </summary>
        /// <param name="sender">The source of the event. 
        /// </param><param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. 
        /// </param><exception cref="T:System.Web.HttpException">The control to which the field value is bound is not a <see cref="T:System.Web.UI.WebControls.CheckBox"/> control.
        /// - or -
        /// The field value cannot be converted to a Boolean value. 
        /// </exception>
        protected override void OnDataBindField(object sender, EventArgs e)
        {
            var checkBox = sender as ASP.CheckBox;
            if (checkBox == null)
                throw new HttpException("CheckBoxField wrong control type during DataBind.");

            checkBox.Text = Text;
            SetCheckBoxValue(checkBox, GetValue(checkBox.NamingContainer));
        }

        /// <summary>
        /// Gets or sets the text that is displayed in the header of a data control.
        /// </summary>
        public override string HeaderText
        {
            get { return _impl.HeaderText;  }
            set { _impl.HeaderText = value; }
        }

        /// <summary>Returns a new instance of the field class.</summary>
        /// <returns>A new instance of the %CheckBoxField class.</returns>
        protected override ASP.DataControlField CreateField()
        {
            return new CheckBoxField();
        }

        /// <summary>
        /// Sets the state of the specified <paramref name="checkBox"/> according to the specified <paramref name="value"/>.
        /// </summary>
        internal static void SetCheckBoxValue(ASP.CheckBox checkBox, object value)
        {
            if (value == null || Convert.IsDBNull(value))
                checkBox.Checked = false;
            else if (value is bool)
                checkBox.Checked = (bool)value;
            else
            {
                try
                {
                    checkBox.Checked = bool.Parse(value.ToString());
                }
                catch (FormatException exception)
                {
                    throw new HttpException("Could not parse DataField value as boolean.", exception);
                }
            }
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

        #region [ -- IAjaxBoundField implementation -- ]

        /// <summary>
        /// Gets a dictionary of state information that allows you to save and 
        /// restore the view state of a <see cref="BoundField"/> object across multiple requests for the same page.
        /// </summary>
        StateBag IAjaxBoundField.ViewState
        {
            get { return ViewState; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether HTML encoding is supported by a <see cref="BoundField"/> object.
        /// </summary>
        bool IAjaxBoundField.SupportsHtmlEncode
        {
            get { return SupportsHtmlEncode; }
        }

        /// <summary>
        /// Raises the FieldChanged event.
        /// </summary>
        void IAjaxBoundField.OnFieldChanged()
        {
            OnFieldChanged();
        }

        /// <summary>
        /// Initializes the specified <see cref="ASP.DataControlFieldCell"/> object to the specified row state.
        /// </summary>
        /// <param name="cell">The <see cref="ASP.DataControlFieldCell"/> to initialize.</param>
        /// <param name="rowState">One of the <see cref="ASP.DataControlRowState"/> values.</param>
        void IAjaxBoundField.InitializeDataCell(ASP.DataControlFieldCell cell, ASP.DataControlRowState rowState)
        {
            InitializeDataCell(cell, rowState);
        }

        #endregion
    }
}
