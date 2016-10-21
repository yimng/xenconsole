using System;
using System.Collections.Generic;
using System.Text;

namespace XenAdmin.Controls.DataGridViewExs
{
    /// <summary>
    /// 用于DataGridViewButtonColumnEx的按钮点击事件
    /// </summary>
    public class DataGridViewButtonClickEventArgs : EventArgs
    {
        public int ColumnIndex { get; set; }

        public int RowIndex { get; set; }

        public object Value { get; set; }

        public bool Bind { get; set; }

        public DataGridViewButtonClickEventArgs(int columnIndex, int rowIndex, object value, bool bind)
        {
            this.ColumnIndex = columnIndex;
            this.RowIndex = rowIndex;
            this.Value = value;
            this.Bind = bind;
        }
    }
}
