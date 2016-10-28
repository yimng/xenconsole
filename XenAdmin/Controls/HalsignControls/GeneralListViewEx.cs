using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XenAdmin.Controls
{
	public partial class GeneralListViewEx: ListView
	{
        private const int WM_PAINT = 0x000F;

	    private const int ControlPadding = 4;

	    public GeneralListViewEx()
		{
			InitializeComponent();
            _controls = new ArrayList();
            this.CopyMenuItem = new ToolStripMenuItem(Messages.COPY);
            this.CopyMenuItem.Click += new EventHandler(CopyMenuItem_Click);
		}

        void CopyMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SelectedItems.Count > 0)
            {
                Clipboard.SetDataObject(this.SelectedItems[0].SubItems[1].Text);
            }
        }

        private readonly ArrayList _controls;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAINT)
            {
                foreach (EmbeddedControl c in _controls)
                {
                    Rectangle r = c.MySubItem.Bounds;
                    if (r.Y > 0 && r.Y < this.ClientRectangle.Height)
                    {
                        c.MyControl.Visible = true;
                        c.MyControl.Bounds = new Rectangle(r.X + ControlPadding/2, r.Y + ControlPadding/2,
                                                           r.Width - (4*ControlPadding), r.Height - ControlPadding);
                    }
                    else
                    {
                        c.MyControl.Visible = false;
                    }
                }

                if (this.View == View.Details && this.Columns.Count > 0)
                    this.Columns[this.Columns.Count - 1].Width = -2;
            }
            base.WndProc(ref m);
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            ListViewItemEx item = e.Item as ListViewItemEx;
            if (e.ColumnIndex == 0)
            {
                e.SubItem.ForeColor = Color.FromArgb(0, 0x66, 0xff);
            }
            if (e.ColumnIndex == 1)
            {
                e.SubItem.ForeColor = item == null ? SystemColors.ControlText : item._Color;
            }
            e.DrawDefault = true;
            base.OnDrawSubItem(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Right && this.SelectedItems.Count > 0)
            {
                List<ToolStripMenuItem> tag = this.SelectedItems[0].Tag as List<ToolStripMenuItem>;
                this.contextMenuStrip1.Items.Clear();
                this.contextMenuStrip1.Items.Add(this.CopyMenuItem);
                if (tag != null)
                {
                    this.contextMenuStrip1.Items.Add(new ToolStripSeparator());
                    this.contextMenuStrip1.Items.AddRange(tag.ToArray());
                }
                this.contextMenuStrip1.Show(this, this.PointToClient(Control.MousePosition));
            }
        }

        public void AddControlToSubItem(Control control, ExControlListViewSubItem subitem)
        {
            this.Controls.Add(control);
            subitem.MyControl = control;
            EmbeddedControl ec;
            ec.MyControl = control;
            ec.MySubItem = subitem;
            this._controls.Add(ec);
        }

        public void AddItem(ListViewItemEx item)
        {
            base.Items.Add(item);
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem CopyMenuItem;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            // 
            // GeneralListViewEx
            // 
            this.FullRowSelect = true;
            this.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.MultiSelect = false;
            this.OwnerDraw = true;
            this.ShowItemToolTips = true;
            this.ResumeLayout(false);

        }

        #endregion
	}

    public partial class ListViewItemEx : ListViewItem
    {
        public ListViewItemEx(string[] items, int imageIndex, ListViewGroup group, Color color)
            : base(items, imageIndex, group)
        {
            this.color = color;
        }

        public ListViewItemEx(string[] items, int imageIndex, ListViewGroup group)
            : base(items, imageIndex, group)
        { }

        private Color color;
        public Color _Color
        {
            get { return this.color; }
            set { this.color = value; }
        }
    }

    public class ExControlListViewSubItem : ListViewItem.ListViewSubItem
    {

        private Control _control;

        public ExControlListViewSubItem()
        {

        }

        public Control MyControl
        {
            get { return _control; }
            set { _control = value; }
        }
    }

    public struct EmbeddedControl
    {
        public Control MyControl;
        public ExControlListViewSubItem MySubItem;
    }
}
