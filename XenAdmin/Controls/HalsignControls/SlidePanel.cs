using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace XenAdmin.Controls
{
	public partial class SlidePanel: Panel
	{
        // Fields
        private readonly Color LINK_COLOR = Color.FromArgb(0, 0x66, 0xff);
        private const int mBufferSize = 6;
        private bool mCanLayout;
        private int mClientAreaHeight;
        private readonly Image mCloseImage = null;
        public bool mCollapsed;
        private readonly int mCollapsedHeight = 0x16;
        private bool mCollapseSlideDown;
        private readonly EventHandler mControlResizedHandler = null;
        private int mDeltaY;
        private int mExpandedHeight;
        private string mHeaderText;
        private Rectangle mLinkRect;
        private string mLinkText;
        private bool mLoading;
        private readonly Image mOpenImage = null;

        // Events
        public event LinkLabelLinkClickedEventHandler LinkClicked;

		public SlidePanel()
		{
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.BackColor = Color.White;
            this.mCollapsed = false;
            this.mCanLayout = false;
            this.mCollapseSlideDown = false;
            this.mDeltaY = base.Height - this.mCollapsedHeight;
		}

        private void ControlSizeLocationChanged(object sender, EventArgs e)
        {
            if (!base.DesignMode && this.mCanLayout)
            {
                Control control = (Control)sender;
                if ((control.Bottom + 6) > base.Height)
                {
                    base.Controls.Remove(control);
                    base.Height = control.Bottom + 6;
                    base.Controls.Add(control);
                }
            }
        }

        private void DoExpandCollapse()
        {
            this.mCanLayout = false;
            AnchorStyles anchor = this.Anchor;
            if (this.Dock == DockStyle.None)
            {
                this.Anchor = AnchorStyles.None;
                this.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            }
            if (this.mCollapsed)
            {
                base.Height = this.mExpandedHeight;
                if (this.mCollapseSlideDown)
                {
                    base.Location = new Point(base.Location.X, base.Location.Y - this.mClientAreaHeight);
                }
                base.Parent.Invalidate();
            }
            else
            {
                if (this.mCollapseSlideDown)
                {
                    this.mClientAreaHeight = base.Height - this.mCollapsedHeight;
                    base.Location = new Point(base.Location.X, base.Location.Y + this.mClientAreaHeight);
                }
                this.mExpandedHeight = base.Height;
                base.Height = this.mCollapsedHeight;
            }
            this.mDeltaY = this.mExpandedHeight - this.mCollapsedHeight;
            this.mCollapsed = !this.mCollapsed;
            if (this.Dock == DockStyle.None)
            {
                this.Anchor = anchor;
            }
            this.LayoutSiblings();
            this.mCanLayout = true;
        }

        private Cursor GetCurrentCursor()
        {
            if (!this.mLoading)
            {
                return Cursors.Default;
            }
            return Cursors.AppStarting;
        }

        private void LayoutSiblings()
        {
            if (base.Parent != null)
            {
                base.Parent.SuspendLayout();
                foreach (Control control in base.Parent.Controls)
                {
                    if (((control.GetType().Name == "SlidePanel") && (control.Location.X == base.Location.X)) && (control.Location.Y > base.Location.Y))
                    {
                        int mDeltaY = this.mDeltaY;
                        if (this.mCollapsed)
                        {
                            mDeltaY = -mDeltaY;
                        }
                        control.Top += mDeltaY;
                    }
                }
                base.Parent.ResumeLayout();
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            try
            {
                int num = 0;
                Control control = e.Control;
                control.Resize += this.mControlResizedHandler;
                control.LocationChanged += this.mControlResizedHandler;
                num = control.Location.Y + control.Height;
                this.mDeltaY = 0;
                if (num >= base.Height)
                {
                    this.mExpandedHeight = num + 6;
                    this.mDeltaY = this.mExpandedHeight - base.Height;
                    base.Height = this.mExpandedHeight;
                }
                this.LayoutSiblings();
            }
            catch (Exception)
            {
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            try
            {
                Control control = e.Control;
                control.Resize -= this.mControlResizedHandler;
                control.LocationChanged -= this.mControlResizedHandler;
                base.Height -= control.Height;
                this.mDeltaY = -control.Height;
                this.LayoutSiblings();
            }
            catch
            {
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.mLinkText))
            {
                if (this.mLinkRect.Contains(e.X, e.Y))
                {
                    this.Cursor = Cursors.Hand;
                }
                else
                {
                    this.Cursor = this.GetCurrentCursor();
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            int y = e.Y;
            if (((this.LinkClicked != null) && !string.IsNullOrEmpty(this.mLinkText)) && this.mLinkRect.Contains(e.X, e.Y))
            {
                LinkLabel.Link link = new LinkLabel.Link(0, this.mLinkText.Length)
                {
                    Description = this.mLinkText,
                    Name = this.mLinkText
                };
                LinkLabelLinkClickedEventArgs args = new LinkLabelLinkClickedEventArgs(link, e.Button);
                this.LinkClicked(this, args);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int num;
            Image mOpenImage;
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            if (this.mCollapsed)
            {
                num = this.mCollapsedHeight - 1;
                mOpenImage = this.mOpenImage;
            }
            else
            {
                num = base.Height - 1;
                mOpenImage = this.mCloseImage;
            }
            LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, this.mCollapsedHeight), Color.FromArgb(0xcb, 0xcd, 0xcd), Color.FromArgb(0xec, 0xed, 0xee));
            graphics.FillRectangle(brush, 0, 0, base.Width, this.mCollapsedHeight);
            using (Pen pen = new Pen(Color.FromArgb(0xff, 0xff, 0xff)))
            {
                graphics.DrawLine(pen, 1, 1, base.Width - 2, 1);
            }
            using (Pen pen2 = new Pen(Color.FromArgb(0x7a, 0x7a, 0x7a)))
            {
                graphics.DrawLine(pen2, 0, this.mCollapsedHeight - 1, base.Width - 1, this.mCollapsedHeight - 1);
                graphics.DrawRectangle(pen2, 0, 0, base.Width - 1, num);
            }
            using (Brush brush2 = new SolidBrush(Color.FromArgb(15, 0x3f, 0x6d)))
            {
                Font font = new Font(this.Font, FontStyle.Bold);
                graphics.DrawString(this.mHeaderText, font, brush2, (float)4f, (float)4f);
            }
            if (!string.IsNullOrEmpty(this.mLinkText))
            {
                using (Brush brush3 = new SolidBrush(this.LINK_COLOR))
                {
                    Font font2 = this.Font;
                    SizeF ef = e.Graphics.MeasureString(this.mLinkText, font2);
                    this.mLinkRect = new Rectangle((base.Width - 4) - ((int)ef.Width), 4, (int)(ef.Width + 0.5), (int)(ef.Height + 0.5));
                    graphics.DrawString(this.mLinkText, font2, brush3, this.mLinkRect);
                }
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            this.mCanLayout = true;
        }

        public void ShrinkWrap()
        {
            int bottom = 0;
            foreach (Control control in base.Controls)
            {
                if (control.Bottom > bottom)
                {
                    bottom = control.Bottom;
                }
            }
            base.Height = bottom + 6;
        }

        // Properties
        public string HeaderLink
        {
            get
            {
                return this.mLinkText;
            }
            set
            {
                this.mLinkText = value;
                base.Invalidate();
            }
        }

        public string HeaderText
        {
            get
            {
                return this.mHeaderText;
            }
            set
            {
                this.mHeaderText = value;
                base.Invalidate();
            }
        }

        public bool Loading
        {
            get
            {
                return this.mLoading;
            }
            set
            {
                this.mLoading = value;
                this.Cursor = this.GetCurrentCursor();
            }
        }

        public bool SlideDownOnCollapse
        {
            get
            {
                return this.mCollapseSlideDown;
            }
            set
            {
                this.mCollapseSlideDown = value;
            }
        }

	}
}
