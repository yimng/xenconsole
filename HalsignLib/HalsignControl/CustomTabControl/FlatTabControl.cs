using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace HalsignLib.HalsignControl.CustomTabControl
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
		private const int nMargin = 2;
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

            this.Alignment = TabAlignment.Top;

			bUpDown = false;

			this.ControlAdded += new ControlEventHandler(FlatTabControl_ControlAdded);
			this.ControlRemoved += new ControlEventHandler(FlatTabControl_ControlRemoved);
			this.SelectedIndexChanged += new EventHandler(FlatTabControl_SelectedIndexChanged);

			leftRightImages = new ImageList();

			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FlatTabControl));

            Bitmap upNormalImage = ((System.Drawing.Bitmap)(resources.GetObject("arrow_right_normal")));
            Bitmap downNormalImage = ((System.Drawing.Bitmap)(resources.GetObject("arrow_left_normal")));
            Bitmap upGreyImage = ((System.Drawing.Bitmap)(resources.GetObject("arrow_right_grey")));
            Bitmap downGreyImage = ((System.Drawing.Bitmap)(resources.GetObject("arrow_left_grey")));

            if (upNormalImage != null && downNormalImage != null && upGreyImage != null && downGreyImage!=null)
			{
                leftRightImages.Images.Add(upNormalImage);
                leftRightImages.Images.Add(downNormalImage);
                leftRightImages.Images.Add(upGreyImage);
                leftRightImages.Images.Add(downGreyImage);
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

        //public override Rectangle DisplayRectangle
        //{
        //    get
        //    {
        //        Rectangle rect = base.DisplayRectangle;
        //        return new Rectangle(rect.Left - 4, rect.Top - 1, rect.Width + 8, rect.Height + 5);
        //    }
        //}

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

            Pen border = new Pen(SystemColors.Control);
            TabArea.Inflate(nDelta, nDelta);
            g.DrawRectangle(border, TabArea);
            border.Dispose();

            // clip region for drawing tabs
            Region rsaved = g.Clip;
            Rectangle rreg;

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

            rreg = new Rectangle(TabArea.Left, TabControlArea.Top, nWidth - nMargin, TabControlArea.Height);

            g.SetClip(rreg);

            // draw tabs
            for (int i = 0; i < this.TabCount; i++)
                DrawTab(g, this.TabPages[i], i);

            g.Clip = rsaved;
            //----------------------------
            
            //----------------------------
            // draw background to cover flat border areas
            if (this.SelectedTab != null)
            {
                TabPage tabPage = this.SelectedTab;
                Color color = Color.White;
                border = new Pen(color);

                TabArea.Offset(1, 1);
                TabArea.Width -= 2;
                TabArea.Height -= 2;

                g.DrawRectangle(border, TabArea);
                TabArea.Width -= 1;
                TabArea.Height -= 1;
                g.DrawRectangle(border, TabArea);

                border.Dispose();
            }
        }

        /// <summary>
        /// draw tab panel border
        /// </summary>
        /// <param name="g"></param>
        /// <param name="tabArea"></param>
        protected virtual void RenderTabPanelBorder(Graphics g, Rectangle tabArea)
        {
            Pen border1 = new Pen(Color.FromArgb(41,50,62));
            g.DrawRectangle(border1, tabArea);
            border1.Dispose();

            Pen border2 = new Pen(Color.FromArgb(119, 140, 166));
            tabArea.Inflate(-1, -1);
            g.DrawRectangle(border2, tabArea);
            border2.Dispose();
        }

        /// <summary>
        /// draw control background
        /// </summary>
        /// <param name="g"></param>
        /// <param name="TabControlArea"></param>
        protected  virtual void RenderControlBackground(Graphics g, Rectangle tabControlArea)
        {
            Brush br = new SolidBrush(SystemColors.Control);
            g.FillRectangle(br, tabControlArea);
            br.Dispose();

            Brush brush = new SolidBrush(Color.FromArgb(57, 81, 107));
            Rectangle recBounds = this.GetTabRect(0);
            Rectangle rectangle = new Rectangle(tabControlArea.X, tabControlArea.Y, tabControlArea.Width, recBounds.Height + 2);
            g.FillRectangle(brush, rectangle);
            brush.Dispose();
        }

		protected virtual void DrawTab(Graphics g, TabPage tabPage, int nIndex)
		{
			Rectangle recBounds = this.GetTabRect(nIndex);
            RectangleF tabTextArea = (RectangleF)this.GetTabRect(nIndex);
            Point cusorPoint = PointToClient(MousePosition);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			bool bSelected = (this.SelectedIndex == nIndex);
            bool bHovered = recBounds.Contains(cusorPoint);

            if (bSelected)
            {
                Point[] pt = new Point[5];

                #region tab border
                if (nIndex == 0)
                {
                    pt[0] = new Point(recBounds.Left, recBounds.Bottom);
                    pt[1] = new Point(recBounds.Left, recBounds.Top);
                    pt[2] = new Point(recBounds.Right - recBounds.Height / 2, recBounds.Top);
                    pt[3] = new Point(recBounds.Right + recBounds.Height / 2, recBounds.Bottom);
                    pt[4] = new Point(recBounds.Left, recBounds.Bottom);
                }
                else
                {
                    pt[0] = new Point(recBounds.Left, recBounds.Bottom - recBounds.Height / 2);
                    pt[1] = new Point(recBounds.Left, recBounds.Top);
                    pt[2] = new Point(recBounds.Right - recBounds.Height / 2, recBounds.Top);
                    pt[3] = new Point(recBounds.Right + recBounds.Height / 2, recBounds.Bottom);
                    pt[4] = new Point(recBounds.Left, recBounds.Bottom);
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
                // clear left/top/bottom lines
                Pen pen = new Pen(Color.White, 0.2f);

                g.DrawLine(pen, recBounds.Left, recBounds.Bottom, recBounds.Left, recBounds.Top);
                g.DrawLine(pen, recBounds.Left, recBounds.Top, recBounds.Right - recBounds.Height / 2 - 1, recBounds.Top);
                g.DrawLine(pen, recBounds.Left, recBounds.Bottom, recBounds.Right + recBounds.Height / 2, recBounds.Bottom);

                pen.Dispose();
                //----------------------------
            }
            else
            {
                Color tabBackColor = Color.Black;
                Color tabBorderColor = Color.Black;
                Color tabBorderInnerColor = Color.Black;

                Point[] pt = new Point[5];

                #region tab border
                if (nIndex == 0)
                {
                    pt[0] = new Point(recBounds.Left, recBounds.Bottom);
                    pt[1] = new Point(recBounds.Left, recBounds.Top);
                    pt[2] = new Point(recBounds.Right - recBounds.Height / 2, recBounds.Top);
                    pt[3] = new Point(recBounds.Right + recBounds.Height / 2, recBounds.Bottom);
                    pt[4] = new Point(recBounds.Left, recBounds.Bottom);
                }
                else
                {
                    pt[0] = new Point(recBounds.Left, recBounds.Bottom - recBounds.Height / 2);
                    pt[1] = new Point(recBounds.Left, recBounds.Top);
                    pt[2] = new Point(recBounds.Right - recBounds.Height / 2, recBounds.Top);
                    pt[3] = new Point(recBounds.Right + recBounds.Height / 2, recBounds.Bottom);
                    pt[4] = new Point(recBounds.Left + recBounds.Height / 2, recBounds.Bottom);
                }

                // fill this tab with background color
                if (bHovered)
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
                // clear left/top/bottom lines
                Pen pen = new Pen(Color.FromArgb(200, 200, 200), 0.2f);

                if (nIndex == 0)
                {
                    g.DrawLine(pen, recBounds.Left, recBounds.Bottom, recBounds.Left, recBounds.Top);
                    g.DrawLine(pen, recBounds.Left, recBounds.Top, recBounds.Right - recBounds.Height / 2 - 1, recBounds.Top);
                }
                else
                {
                    g.DrawLine(pen, recBounds.Left, recBounds.Bottom - recBounds.Height / 2 - 1, recBounds.Left, recBounds.Top);
                    g.DrawLine(pen, recBounds.Left, recBounds.Top, recBounds.Right - recBounds.Height / 2 - 1, recBounds.Top);
                }

                pen.Dispose();
            }

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
            if ((leftRightImages == null) || (leftRightImages.Images.Count != 4))
                return;

            //----------------------------
            // calculate positions
            Rectangle TabControlArea = this.ClientRectangle;

            Rectangle r0 = new Rectangle();
            CustomWin32.GetClientRect(scUpDown.Handle, ref r0);

            Brush br = new SolidBrush(Color.FromArgb(120, 135, 156));
            g.FillRectangle(br, r0);
            br.Dispose();

            Pen border = new Pen(SystemColors.ControlDark);
            Rectangle rborder = r0;
            rborder.Inflate(-1, -1);
            g.DrawRectangle(border, rborder);
            border.Dispose();

            int nMiddle = (r0.Width / 2);
            int nTop = (r0.Height - 16) / 2;
            int nLeft = (nMiddle - 16) / 2;

            Rectangle r1 = new Rectangle(nLeft, nTop, 16, 16);
            Rectangle r2 = new Rectangle(nMiddle + nLeft, nTop, 16, 16);
            //----------------------------

            //----------------------------
            // draw buttons
            Image img = leftRightImages.Images[1];
            if (img != null)
            {
                if (this.TabCount > 0)
                {
                    Rectangle r3 = this.GetTabRect(0);
                    if (r3.Left < TabControlArea.Left)
                        g.DrawImage(img, r1);
                    else
                    {
                        img = leftRightImages.Images[3];
                        if (img != null)
                            g.DrawImage(img, r1);
                    }
                }
            }

            img = leftRightImages.Images[0];
            if (img != null)
            {
                if (this.TabCount > 0)
                {
                    Rectangle r3 = this.GetTabRect(this.TabCount - 1);
                    if (r3.Right > (TabControlArea.Width - r0.Width))
                        g.DrawImage(img, r2);
                    else
                    {
                        img = leftRightImages.Images[2];
                        if (img != null)
                            g.DrawImage(img, r2);
                    }
                }
            }
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
            set
            {
                TabAlignment ta = value;
                if ((ta != TabAlignment.Top) && (ta != TabAlignment.Bottom))
                    ta = TabAlignment.Top;

                base.Alignment = ta;
            }
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
}
