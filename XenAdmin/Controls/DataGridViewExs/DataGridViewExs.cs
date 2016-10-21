using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

/*
 * @Author: 江心逐浪
 * 
 * @E-Mail: jiangshan_111@126.com   
 * 
 * @Content:
 *      主要是对DataGridView进行扩展。
 */
namespace XenAdmin.Controls.DataGridViewExs
{
    

    /// <summary>
    /// 扩展的DataGridView
    /// </summary>
    public class DataGridViewExs : DataGridView
    {
        /// <summary>
        ///DataGridViewButtonColumnEx中的按钮点击事件，如果没有这个列，
        ///此事件就没有具体作用
        /// </summary>
        [Category("Jonson Design"),Description("按钮点击事件")]
        public event DataGridViewButtonClicked CellButtonClicked;

        public DataGridViewExs()
            : base()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.UserPaint | 
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
        }

        /// <summary>
        /// checkbox的单元格改变事件
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        internal void OnCheckBoxCellCheckedChange(int columnIndex, int rowIndex, bool value)
        {
            bool existsChecked = false, existsNoChecked = false;
            DataGridViewCheckBoxCellEx cellEx;
            foreach (DataGridViewRow row in this.Rows)
            {
                cellEx = row.Cells[columnIndex] as DataGridViewCheckBoxCellEx;
                if (cellEx == null) return;
                existsChecked |= cellEx.Checked;
                existsNoChecked |= !cellEx.Checked;
            }

            DataGridViewCheckBoxColumnHeaderCellEx headerCellEx =
                this.Columns[columnIndex].HeaderCell as DataGridViewCheckBoxColumnHeaderCellEx;

            if (headerCellEx == null) return;

            CheckState oldState = headerCellEx.CheckedAllState;

            if (existsChecked)
                headerCellEx.CheckedAllState = existsNoChecked ? CheckState.Indeterminate : CheckState.Checked;
            else
                headerCellEx.CheckedAllState = CheckState.Unchecked;

            if (oldState != headerCellEx.CheckedAllState)
                this.InvalidateColumn(columnIndex);
        }

        /// <summary>
        /// 全选中/取消全选中
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="isCheckedAll"></param>
        internal void OnCheckAllCheckedChange(int columnIndex,bool isCheckedAll)
        {
            DataGridViewCheckBoxCellEx cellEx;
            foreach (DataGridViewRow row in this.Rows)
            {
                cellEx = row.Cells[columnIndex] as DataGridViewCheckBoxCellEx;
                if (cellEx == null) continue;
                cellEx.Checked = isCheckedAll;
            }
        }

        /// <summary>
        /// 触发DataGridViewColumnEx中的按钮点击事件
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="value"></param>
        internal void OnButtonClicked(int columnIndex, int rowIndex, object value, bool bind)
        {
            this.OnCellButtonClicked(columnIndex, rowIndex, value, bind);
        }

        protected void OnCellButtonClicked(int columnIndex, int rowIndex, object value, bool bind)
        {
            if (CellButtonClicked != null)
                CellButtonClicked(this, new DataGridViewButtonClickEventArgs(columnIndex, rowIndex, value, bind));
        }
    }
}
