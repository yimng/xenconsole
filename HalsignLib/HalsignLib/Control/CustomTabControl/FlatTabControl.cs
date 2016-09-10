using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace CustomTabControl
{
	/// <summary>
	/// Summary description for FlatTabControl.
	/// </summary>
	[ToolboxBitmap(typeof(System.Windows.Forms.TabControl))] //,
		//Designer(typeof(Designers.FlatTabControlDesigner))]
	
	public class FlatTabControl : System.Windows.Forms.TabControl
	{

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private SubClass scUpDown = null;
		private bool bUpDown; // true when the button UpDown is required
		private ImageList leftRightImages = null;
		private const int nMargin = 15;
		private Color mBackColor = SystemColors.Window;

		public FlatTabControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// double buffering
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			bUpDown = false;

			this.ControlAdded += new ControlEventHandler(FlatTabControl_ControlAdded);
			this.ControlRemoved += new ControlEventHandler(FlatTabControl_ControlRemoved);
			this.SelectedIndexChanged += new EventHandler(FlatTabControl_SelectedIndexChanged);

			leftRightImages = new ImageList();
			//leftRightImages.ImageSize = new Size(16, 16); // default

			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FlatTabControl));
            //Bitmap updownImage = ((System.Drawing.Bitmap)(resources.GetObject("TabIcons.bmp")));
            Bitmap updownImage = null;
			if (updownImage != null)
			{
				updownImage.MakeTransparent(Color.Red);
				leftRightImages.Images.AddStrip(updownImage);
			}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}

				leftRightImages.Dispose();
			}
			base.Dispose( disposing );
		}
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Rectangle rect = new Rectangle(0, 0, this.Width, this.GetTabRect(0).Height + 4);
            base.Invalidate(rect);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Rectangle rect = new Rectangle(0, 0, this.Width, this.GetTabRect(0).Height + 4);
            base.Invalidate(rect);
        }

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

            DrawControl(e.Graphics);
		}

        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle rect = base.DisplayRectangle;
                return new Rectangle(rect.Left - 4, rect.Top - 1, rect.Width + 8, rect.Height + 5);
            }
        }

        internal void DrawControl(Graphics g)
        {
            if (!Visible)
                return;

            Rectangle TabControlArea = this.ClientRectangle;
            Rectangle TabArea = this.DisplayRectangle;

            // fill client area
            RenderControlBackground(g, TabControlArea);

            // draw panel border
            int nDelta = SystemInformation.Border3DSize.Width;
            TabArea.Inflate(nDelta, nDelta);
            //RenderTabPanelBorder(g, TabArea);


            // clip region for drawing tabs
            Region rsaved = g.Clip;

            int nWidth = TabArea.Width + nMargin;
            if (bUpDown)
            {
                // exclude updown control for painting
                if (CustomWin32.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rupdown = new Rectangle();
                    CustomWin32.GetWindowRect(scUpDown.Handle, ref rupdown);
                    Rectangle rupdown2 = this.RectangleToClient(rupdown);

                    nWidth = rupdown2.X;
                }
            }

            // draw tabs
            for (int i = 0; i < this.TabCount; i++)
                DrawTab(g, this.TabPages[i], i);

            g.Clip = rsaved;
            //----------------------------


            //----------------------------
            // draw background to cover flat border areas
            //覆盖tabpage和控件边框的空隙
            //if (this.SelectedTab != null)
            //{
            //    TabPage tabPage = this.SelectedTab;
            //    Color color = tabPage.BackColor;
            //    Pen border = new Pen(color);

            //    TabArea.Offset(1, 1);
            //    TabArea.Width -= 2;
            //    TabArea.Height -= 2;

            //    g.DrawRectangle(border, TabArea);
            //    TabArea.Width -= 1;
            //    TabArea.Height -= 1;
            //    g.DrawRectangle(border, TabArea);

            //    border.Dispose();
            //}
            //----------------------------

            //如果边框是两条线，则这里就只需要覆盖底线和右边的竖线，根据实际观察得知
            //if (this.SelectedTab != null)
            //{
            //    TabPage tabPage = this.SelectedTab;
            //    Color color = tabPage.BackColor;
            //    Pen border = new Pen(color);

            //    g.DrawLine(border, new Point(TabArea.X + 2, TabArea.Bottom - 2)
            //        , new Point(TabArea.Right - 2, TabArea.Bottom - 2));

            //    g.DrawLine(border, new Point(TabArea.Right - 2, TabArea.Bottom - 2)
            //    , new Point(TabArea.Right - 2, TabArea.Y + 2));

            //    border.Dispose();
            //}
        }

        /// <summary>
        /// 画tab panel控件容器的边框。可重写。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="tabArea"></param>
        protected virtual void RenderTabPanelBorder(Graphics g, Rectangle tabArea)
        {
            Pen border1 = new Pen(Color.FromArgb(41,50,62));
            g.DrawRectangle(border1, tabArea);
            border1.Dispose();

            //内边框
            Pen border2 = new Pen(Color.FromArgb(119,140,166));
            //Pen border2 = new Pen(Color.FromArgb(255,0,0));
            tabArea.Inflate(-1, -1);
            g.DrawRectangle(border2, tabArea);
            //g.DrawRectangle(border2,tabArea.X+1,tabArea.Y+1,tabArea.Width-3,tabArea.Height-3);
            border2.Dispose();
        }

        /// <summary>
        /// 画整个控件背景。可重写以自定义样式。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="TabControlArea"></param>
        protected  virtual void RenderControlBackground(Graphics g, Rectangle tabControlArea)
        {
            Brush br = new SolidBrush(Color.FromArgb(57, 81, 107));
            g.FillRectangle(br, tabControlArea);
            br.Dispose();
        }

		protected virtual void DrawTab(Graphics g, TabPage tabPage, int nIndex)
		{
			Rectangle recBounds = this.GetTabRect(nIndex);
            RectangleF tabTextArea = new RectangleF(this.GetTabRect(nIndex).X, this.GetTabRect(nIndex).Y + 2, this.GetTabRect(nIndex).Width, this.GetTabRect(nIndex).Height);
            Point cusorPoint = PointToClient(MousePosition);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			bool bSelected = (this.SelectedIndex == nIndex);
            bool bHovered = recBounds.Contains(cusorPoint);

            if (bSelected)
            {
                Point[] pt = new Point[5];

                #region 标签边框
                if (nIndex == 0)
                {
                    if (this.Alignment == TabAlignment.Top)
                    {
                        pt[0] = new Point(recBounds.Left, recBounds.Bottom + 1);
                        pt[1] = new Point(recBounds.Left, recBounds.Top + 3);
                        pt[2] = new Point(recBounds.Right - recBounds.Height / 2, recBounds.Top + 3);
                        pt[3] = new Point(recBounds.Right + recBounds.Height / 2, recBounds.Bottom + 1);
                        pt[4] = new Point(recBounds.Left, recBounds.Bottom + 1);
                    }
                }
                else
                {
                    if (this.Alignment == TabAlignment.Top)
                    {
                        pt[0] = new Point(recBounds.Left, recBounds.Bottom - recBounds.Height / 4 - 4);
                        pt[1] = new Point(recBounds.Left, recBounds.Top + 3);
                        pt[2] = new Point(recBounds.Right - recBounds.Height / 2, recBounds.Top + 3);
                        pt[3] = new Point(recBounds.Right + recBounds.Height / 2, recBounds.Bottom + 1);
                        pt[4] = new Point(recBounds.Left, recBounds.Bottom + 1);
                    }
                }

                //----------------------------
                // fill this tab with background color
                Brush br1 = new SolidBrush(Color.White);
                g.FillPolygon(br1, pt);
                br1.Dispose();

                using (Pen pen1 = new Pen(Color.Black))
                {
                    g.DrawPolygon(pen1, pt);

                }
                #endregion

                //----------------------------
                // clear bottom lines
                Pen pen = new Pen(Color.White, 0.2f);

                switch (this.Alignment)
                {
                    case TabAlignment.Top:
                        if (nIndex == 0)
                        {
                            g.DrawLine(pen, recBounds.Left, recBounds.Bottom + 1, recBounds.Left, recBounds.Top + 3);
                            g.DrawLine(pen, recBounds.Left, recBounds.Top + 3, recBounds.Right - recBounds.Height / 2 - 1, recBounds.Top + 3);
                        }
                        else
                        {
                            g.DrawLine(pen, recBounds.Left, recBounds.Bottom + 1, recBounds.Left, recBounds.Top + 3);
                            g.DrawLine(pen, recBounds.Left, recBounds.Top + 3, recBounds.Right - recBounds.Height / 2 - 1, recBounds.Top + 3);
                        }
                        
                        break;

                    case TabAlignment.Bottom:
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top, recBounds.Right - 1, recBounds.Top);
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top - 1, recBounds.Right - 1, recBounds.Top - 1);
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top - 2, recBounds.Right - 1, recBounds.Top - 2);
                        break;
                }

                pen.Dispose();
                //----------------------------
            }
            else
            {
                Color tabBackColor = Color.Black;
                Color tabBorderColor = Color.Black;
                Color tabBorderInnerColor = Color.Black;

                Point[] pt = new Point[5];

                #region 标签边框
                if (nIndex == 0)
                {
                    if (this.Alignment == TabAlignment.Top)
                    {
                        pt[0] = new Point(recBounds.Left, recBounds.Bottom);
                        pt[1] = new Point(recBounds.Left, recBounds.Top + 3);
                        pt[2] = new Point(recBounds.Right - recBounds.Height / 2, recBounds.Top + 3);
                        pt[3] = new Point(recBounds.Right + recBounds.Height / 2, recBounds.Bottom);
                        pt[4] = new Point(recBounds.Left, recBounds.Bottom);
                    }
                }
                else
                {
                    if (this.Alignment == TabAlignment.Top)
                    {
                        pt[0] = new Point(recBounds.Left, recBounds.Bottom - recBounds.Height / 4 - 4);
                        pt[1] = new Point(recBounds.Left, recBounds.Top + 3);
                        pt[2] = new Point(recBounds.Right - recBounds.Height / 2, recBounds.Top + 3);
                        pt[3] = new Point(recBounds.Right + recBounds.Height / 2, recBounds.Bottom);
                        pt[4] = new Point(recBounds.Left + recBounds.Height / 2, recBounds.Bottom);
                    }
                }

                // fill this tab with background color
                if (bHovered) //如果不是选中，是hover状态
                {
                    Brush br1 = new SolidBrush(Color.FromArgb(153, 165, 181));
                    g.FillPolygon(br1, pt);
                    br1.Dispose();
                }
                else
                {
                    Brush br1 = new SolidBrush(Color.FromArgb(120, 135, 156));
                    g.FillPolygon(br1, pt);
                    br1.Dispose();
                }

                using (Pen pen1 = new Pen(tabBorderInnerColor))
                {
                    g.DrawPolygon(pen1, pt);

                }
                #endregion



                //----------------------------
                // clear bottom lines
                Pen pen = new Pen(Color.FromArgb(200, 200, 200), 0.2f);

                if (nIndex == 0)
                {
                    switch (this.Alignment)
                    {
                        case TabAlignment.Top:
                            g.DrawLine(pen, recBounds.Left, recBounds.Bottom, recBounds.Left, recBounds.Top + 3);
                            g.DrawLine(pen, recBounds.Left, recBounds.Top + 3, recBounds.Right - recBounds.Height / 2 - 1, recBounds.Top + 3);
                            break;

                        case TabAlignment.Bottom:
                            break;
                    }
                }
                else
                {
                    switch (this.Alignment)
                    {
                        case TabAlignment.Top:
                            g.DrawLine(pen, recBounds.Left, recBounds.Bottom - recBounds.Height / 4 - 5, recBounds.Left, recBounds.Top + 3);
                            g.DrawLine(pen, recBounds.Left, recBounds.Top + 3, recBounds.Right - recBounds.Height / 2 - 1, recBounds.Top + 3);
                            break;

                        case TabAlignment.Bottom:
                            break;
                    }
                }

                pen.Dispose();
            }

            
			//----------------------------

			//----------------------------
			// draw tab's icon
            //if ((tabPage.ImageIndex >= 0) && (ImageList != null) && (ImageList.Images[tabPage.ImageIndex] != null))
            //{
            //    int nLeftMargin = 8;
            //    int nRightMargin = 2;

            //    Image img = ImageList.Images[tabPage.ImageIndex];

            //    Rectangle rimage = new Rectangle(recBounds.X + nLeftMargin, recBounds.Y + 1, img.Width, img.Height);

            //    // adjust rectangles
            //    float nAdj = (float)(nLeftMargin + img.Width + nRightMargin);

            //    rimage.Y += (recBounds.Height - img.Height) / 2;
            //    tabTextArea.X += nAdj;
            //    tabTextArea.Width -= nAdj;

            //    // draw icon
            //    g.DrawImage(img, rimage);
            //}
			//----------------------------

			//----------------------------
			// draw string
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;  
			stringFormat.LineAlignment = StringAlignment.Center;

            if (bSelected)
            {
                using (SolidBrush br = new SolidBrush(Color.Black))
                {
                    g.DrawString(tabPage.Text, Font, br, tabTextArea, stringFormat);
                }
            }
            else
            {
                using (SolidBrush br = new SolidBrush(Color.White))
                {
                    g.DrawString(tabPage.Text, Font, br, tabTextArea, stringFormat);
                }
               
            }
			//----------------------------
		}

		internal void DrawIcons(Graphics g)
		{
            //if ((leftRightImages == null) || (leftRightImages.Images.Count != 4))
            //    return;

            ////----------------------------
            //// calc positions
            //Rectangle TabControlArea = this.ClientRectangle;

            //Rectangle r0 = new Rectangle();
            //Win32.GetClientRect(scUpDown.Handle, ref r0);

            //Brush br = new SolidBrush(SystemColors.Control);
            //g.FillRectangle(br, r0);
            //br.Dispose();

            //Pen border = new Pen(SystemColors.ControlDark);
            //Rectangle rborder = r0;
            //rborder.Inflate(-1, -1);
            //g.DrawRectangle(border, rborder);
            //border.Dispose();

            //int nMiddle = (r0.Width / 2);
            //int nTop = (r0.Height - 16) / 2;
            //int nLeft = (nMiddle - 16) / 2;

            //Rectangle r1 = new Rectangle(nLeft, nTop, 16, 16);
            //Rectangle r2 = new Rectangle(nMiddle + nLeft, nTop, 16, 16);
            ////----------------------------

            ////----------------------------
            //// draw buttons
            //Image img = leftRightImages.Images[1];
            //if (img != null)
            //{
            //    if (this.TabCount > 0)
            //    {
            //        Rectangle r3 = this.GetTabRect(0);
            //        if (r3.Left < TabControlArea.Left)
            //            g.DrawImage(img, r1);
            //        else
            //        {
            //            img = leftRightImages.Images[3];
            //            if (img != null)
            //                g.DrawImage(img, r1);
            //        }
            //    }
            //}

            //img = leftRightImages.Images[0];
            //if (img != null)
            //{
            //    if (this.TabCount > 0)
            //    {
            //        Rectangle r3 = this.GetTabRect(this.TabCount - 1);
            //        if (r3.Right > (TabControlArea.Width - r0.Width))
            //            g.DrawImage(img, r2);
            //        else
            //        {
            //            img = leftRightImages.Images[2];
            //            if (img != null)
            //                g.DrawImage(img, r2);
            //        }
            //    }
            //}
			//----------------------------
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			FindUpDown();
		}

		private void FlatTabControl_ControlAdded(object sender, ControlEventArgs e)
		{
			FindUpDown();
			UpdateUpDown();
		}

		private void FlatTabControl_ControlRemoved(object sender, ControlEventArgs e)
		{
			FindUpDown();
			UpdateUpDown();
		}

		private void FlatTabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateUpDown();
			Invalidate();	// we need to update border and background colors
		}

		private void FindUpDown()
		{
			bool bFound = false;

			// find the UpDown control
            IntPtr pWnd = CustomWin32.GetWindow(this.Handle, CustomWin32.GW_CHILD);
			
			while (pWnd != IntPtr.Zero)
			{
				//----------------------------
				// Get the window class name
				char[] className = new char[33];

                int length = CustomWin32.GetClassName(pWnd, className, 32);

				string s = new string(className, 0, length);
				//----------------------------

				if (s == "msctls_updown32")
				{
					bFound = true;

					if (!bUpDown)
					{
						//----------------------------
						// Subclass it
						this.scUpDown = new SubClass(pWnd, true);
						this.scUpDown.SubClassedWndProc += new SubClass.SubClassWndProcEventHandler(scUpDown_SubClassedWndProc);
						//----------------------------

						bUpDown = true;
					}
					break;
				}

                pWnd = CustomWin32.GetWindow(pWnd, CustomWin32.GW_HWNDNEXT);
			}

			if ((!bFound) && (bUpDown))
				bUpDown = false;
		}

		private void UpdateUpDown()
		{
			if (bUpDown)
			{
                if (CustomWin32.IsWindowVisible(scUpDown.Handle))
				{
					Rectangle rect = new Rectangle();

                    CustomWin32.GetClientRect(scUpDown.Handle, ref rect);
                    CustomWin32.InvalidateRect(scUpDown.Handle, ref rect, true);
				}
			}
		}

		#region scUpDown_SubClassedWndProc Event Handler

		private int scUpDown_SubClassedWndProc(ref Message m) 
		{
			switch (m.Msg)
			{
                case CustomWin32.WM_PAINT:
				{
					//------------------------
					// redraw
                    IntPtr hDC = CustomWin32.GetWindowDC(scUpDown.Handle);
					Graphics g = Graphics.FromHdc(hDC);

					DrawIcons(g);

					g.Dispose();
                    CustomWin32.ReleaseDC(scUpDown.Handle, hDC);
					//------------------------

					// return 0 (processed)
					m.Result = IntPtr.Zero;

					//------------------------
					// validate current rect
					Rectangle rect = new Rectangle();

                    CustomWin32.GetClientRect(scUpDown.Handle, ref rect);
                    CustomWin32.ValidateRect(scUpDown.Handle, ref rect);
					//------------------------
				}
				return 1;
			}

			return 0;
		}
		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}


		#endregion

		#region Properties
		
		[Editor(typeof(TabpageExCollectionEditor), typeof(UITypeEditor))]
		public new TabPageCollection TabPages
		{
			get
			{
				return base.TabPages;
			}
		}

		new public TabAlignment Alignment
		{
			get {return base.Alignment;}
			set {
				TabAlignment ta = value;
                //if ((ta != TabAlignment.Top) && (ta != TabAlignment.Bottom))
                //    ta = TabAlignment.Top;
				
				base.Alignment = ta;}
		}

		[Browsable(false)]
		new public bool Multiline
		{
			get {return base.Multiline;}
			set {base.Multiline = false;}
		}

		[Browsable(true)]
		public Color myBackColor
		{
			get {return mBackColor;}
			set {mBackColor = value; this.Invalidate();}
		}

		#endregion

		#region TabpageExCollectionEditor

		internal class TabpageExCollectionEditor : CollectionEditor
		{
			public TabpageExCollectionEditor(System.Type type): base(type)
			{
			}
            
			protected override Type CreateCollectionItemType()
			{
				return typeof(TabPage);
			}
		}
        
		#endregion
	}

	//#endregion
}
