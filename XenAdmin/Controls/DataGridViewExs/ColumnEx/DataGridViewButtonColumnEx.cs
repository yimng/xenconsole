﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms.VisualStyles;

/*
 * @Author: 江心逐浪 [Jonson]
 * 
 * @E-Mail: jiangshan_111@126.com   
 * 
 * @Content:
 *      此类主要是在DataGridView的Button列中扩展出一个更便于控制的Button列;
 *       1> 主要是绘制大小固定的按钮，并且添加一些按钮的事件等;
 *       2> 按钮可以支持根据内容来筛选显示,增加转换器;
 *       3> 按钮的点击事件可以直接抛出。
 */
namespace XenAdmin.Controls.DataGridViewExs
{
    /// <summary>
    /// 扩展列
    /// </summary>
    public class DataGridViewButtonColumnEx : DataGridViewColumn
    {
        static Size DefaultButtonSize = new Size(75, 21);

        private Size m_ButtonSize = DefaultButtonSize;
        [Category("Jonson Design"), Browsable(true), Localizable(true), Description("按钮大小模板。")]
        public Size ButtonSize
        {
            get { return m_ButtonSize; }
            set { m_ButtonSize = value; }
        }

        private String m_ButtonText = Messages.USB_DEVICE_BIND_TEXT; //String.Empty;
        [Category("Jonson Design"), Browsable(true), Localizable(true), Description("按钮默认文本")]
        public String ButtonText
        {
            get { return m_ButtonText; }
            set { m_ButtonText = value; }
        }

        public override object Clone()
        {
            DataGridViewButtonColumnEx column = (DataGridViewButtonColumnEx)base.Clone();
            column.ButtonText = m_ButtonText;
            return column;
        }

        public DataGridViewButtonColumnEx()
            : base()
        {
            this.CellTemplate = new DataGridViewButtonCellEx();
        }
    }


    /// <summary>
    /// 扩展单元格
    /// </summary>
    public class DataGridViewButtonCellEx : DataGridViewButtonCell
    {
        #region "type:designer properties"

        private bool m_enabled = true;
        [Category("Jonson Design"), Browsable(true), Localizable(true), Description("是否可用")]
        public bool Enabled
        {
            get { return m_enabled; }
            set { m_enabled = value; }
        }

        /// <summary>
        /// 这里按钮的可见进行重写，原有的是从column中统一设置，
        /// 通过此属性可以单独设置是否可见
        /// </summary>
        private bool m_visible = true;
        [Category("Jonson Design"), Browsable(true), Localizable(true), Description("是否可见")]
        public new bool Visible
        {
            get { return m_visible; }
            set { m_visible = value; }
        }

        /// <summary>
        /// 如果按钮文本没有单独设置的话，将使用column中设置的文本
        /// </summary>
        private string m_text = string.Empty;
        [Category("Jonson Design"), Browsable(true), Localizable(true), Description("按钮文本")]
        public string Text
        {
            get
            {
                if (string.IsNullOrEmpty(m_text))
                    m_text = this.OwningColumnEx.ButtonText;
                return m_text;
            }
            set
            {
                m_text = value;
            }
        }

        private bool m_bind;
        public bool Bind
        {
            set 
            {
                m_bind = value;
                if (value)
                {
                    this.Text = Messages.USB_DEVICE_BIND_TEXT;
                }
                else
                {
                    this.Text = Messages.USB_DEVICE_UNBIND_TEXT;
                }
            }
            get { return m_bind; }
        }

        /// <summary>
        /// 如果按钮大小没有单独设置的话，将使用column中设置的按钮大小
        /// </summary>
        private Size m_size = Size.Empty;
        [Category("Jonson Design"), Browsable(true), Localizable(true), Description("按钮大小")]
        public new Size Size
        {
            get
            {
                if (m_size.IsEmpty)
                    m_size = this.OwningColumnEx.ButtonSize;
                return m_size;
            }
            set
            {
                m_size = value;
            }
        }

        [Category("Jonson Design"), Browsable(true), Localizable(true), Description("所属列")]
        public DataGridViewButtonColumnEx OwningColumnEx
        {
            get { return this.OwningColumn as DataGridViewButtonColumnEx; }
        }

        [Category("Jonson Design"), Browsable(true), Localizable(true), Description("所属表格")]
        public DataGridViewExs DataGridViewEx
        {
            get { return this.DataGridView as DataGridViewExs; }
        }
        #endregion

        #region "type:protected properties"
        //
        // 当前按钮位置大小
        //
        protected Rectangle m_buttonRegion = Rectangle.Empty;
        //
        // 当前按钮绝对位置大小
        //
        protected Rectangle m_absBtnRegion = Rectangle.Empty;
        //
        // 当前按钮状态
        //
        protected PushButtonState m_curBtnState = PushButtonState.Normal;
        //
        // 是否第一次绘制
        //
        protected bool m_firstDraw = true;
        //
        // 单元格背景色，根据当前是否选中，样式不一样
        //
        protected Brush m_brushCellBack;

        #endregion

        #region "type:paint"

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            //
            // 此段代码主要解决xp下，如果鼠标默认在按钮列上，按钮的默认绘制样式问题
            //
            if (this.m_firstDraw)
            {
                this.m_firstDraw = false;
                this.m_curBtnState = this.Enabled ? PushButtonState.Normal : PushButtonState.Disabled;
            }
            // 是否需要重绘单元格的背景颜色
            m_brushCellBack = this.DataGridView.SelectedCells.Contains(this) ?
                new SolidBrush(cellStyle.SelectionBackColor) : new SolidBrush(cellStyle.BackColor);
            graphics.FillRectangle(m_brushCellBack, cellBounds.X, cellBounds.Y, cellBounds.Width, cellBounds.Height);

            //计算button的区域
            m_buttonRegion = RectangleCommon.GetSmallRectOfRectangle(cellBounds, this.Size, out this.m_absBtnRegion);

            //绘制按钮
            if (m_enabled)
                this.InternalDrawButton(graphics, m_buttonRegion, m_curBtnState, Text);
            else
                this.InternalDrawButton(graphics, m_buttonRegion, PushButtonState.Disabled, Text);

            // 填充单元格的边框
            base.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
        }

        protected void InternalDrawButton(Graphics graphics, Rectangle bounds, PushButtonState buttonState, string buttonText)
        {
            //如果是隐藏的，不绘制
            if (!m_visible) return;

            Color buttonTextColor = SystemColors.ControlText;
            if (buttonState == PushButtonState.Disabled)
                buttonTextColor = SystemColors.GrayText;

            ButtonRenderer.DrawButton(graphics, bounds, buttonState);
            if (buttonText != null)
                TextRenderer.DrawText(graphics, buttonText, this.DataGridView.Font, bounds, buttonTextColor);
        }

        #endregion

        #region "type:change current state"

        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (IsInRegion(e.Location, e.ColumnIndex, e.RowIndex))
            {
                this.m_curBtnState = PushButtonState.Hot;
            }
            else
            {
                this.m_curBtnState = PushButtonState.Normal;
            }
            this.DataGridView.InvalidateCell(this);
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            base.OnMouseLeave(rowIndex);
            this.m_curBtnState = PushButtonState.Normal;
            this.DataGridView.InvalidateCell(this);
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (IsInRegion(e.Location, e.ColumnIndex, e.RowIndex))
            {
                this.m_curBtnState = PushButtonState.Pressed;
            }
            else
            {
                this.m_curBtnState = PushButtonState.Normal;
            }
            this.DataGridView.InvalidateCell(this);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Left && this.Enabled && IsInRegion(e.Location, e.ColumnIndex, e.RowIndex))
            {
                if (this.DataGridViewEx != null)
                    this.DataGridViewEx.OnButtonClicked(e.ColumnIndex, e.RowIndex, this.Value, this.Bind);
            }
        }

        #endregion

        #region "type:others"
        /// <summary>
        /// 是否在Button按钮区域
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected bool IsInRegion(Point p, int columnIndex, int rowIndex)
        {
            Rectangle cellBounds = DataGridView[columnIndex, rowIndex].ContentBounds;
            RectangleCommon.GetSmallRectOfRectangle(cellBounds, this.Size, out this.m_absBtnRegion);
            return this.m_absBtnRegion.Contains(p);
        }

        #endregion

    }
}
