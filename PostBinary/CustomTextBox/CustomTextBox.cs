﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms.Design;
namespace MyControls
{    
    public partial class CustomTextBox : UserControl
    {
        private int startPosition = 0;
        private int endPosition = 0;
        private bool displayError = false;
        private float displayableLettersNumber;
        
        private int length;
        [Category("Отображение"), Description("Текущее количество символов"), DisplayName("TextLength")]
        [DefaultValue(0)]
        public int Length 
        {
            get { return length; }
            set { length = value; }
        }
        
        private Timer caretTimer = new Timer();
        private bool drawCaret;
        
        private int textPadding = 2;
        
        private int fontSize = 9;
        [Category("Отображение"), Description("Размер шрифта"), DisplayName("FontSize")]
        [DefaultValue(9)]
        public int FontSize
        {
            get { return fontSize; }
            set
            {
                if (value <= 15)
                    fontSize = value;
                else
                    fontSize = 15;
                if (value < 9)
                    fontSize = 9;
            }
        }

        private int maxTextLength;
        [Category("Отображение"), Description("Максимальное количество символов"), DisplayName("MaxTextLength")]
        [DefaultValue(32565)]
        public int MaxTextLength
        {
            get { return maxTextLength; }
            set {
                if (value <= Int32.MaxValue)
                    maxTextLength = value;
                else
                    maxTextLength = Int32.MaxValue;
                if (value < 0)
                    maxTextLength = 0;
            }
        }
       
        private Size windowSize;
        public Size WindowSize
        {
            get { return windowSize; }
            set {
                windowSize = value;
            }
        }
        private int currCaretPos = 0; 
        // FLAGS BEGIN
        [DefaultValue(false)]
        private bool Entered; // When control gets focus
        
        // FLAGS END

        //private String text;

        [Category("Ошибка"), Description("Отображать ошибку"), DisplayName("DisplayError")]
        [DefaultValue(false)]
        public bool DisplayError
        {
            get { return displayError; }
            set { displayError = value;}
        }

        [Category("Ошибка"), Description("Начальная позиция символов ошибки"), DisplayName("StartPosition")]
        [DefaultValue(0)]
        public int StartPosition
        {
            get { return startPosition; }
            set { 
                if ( (value >= 0) || (value < this.Length) )
                    {
                        startPosition = value;
                    }
                }
        }

        [Category("Ошибка"), Description("Конечная позиция символов ошибки"), DisplayName("EndPosition")]
        [DefaultValue(0)]
        public int EndPosition
        {
            get { return endPosition; }
            set
            {
                if ( ((value >= 0) || (value <= this.Length)) && (value > startPosition) )
                {
                    endPosition = value;
                }
            }
        }
        
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            Entered = true;
            ClientSize = new Size(Size.Width, 200);
            Select();
            Invalidate();
            caretTimer.Start();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }
      
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);            
            Console.WriteLine("OnLeave");
            Entered = false;
            caretTimer.Stop();
            ClientSize = new Size(Size.Width, 20);
            drawCaret = false;
            Invalidate();
        }
        protected override void OnClick(EventArgs e)
        {
            Console.WriteLine("OnClick");
            base.OnClick(e);
            Entered = true;
            //currCaretPosition 
            Console.WriteLine("OnClick");
            Focus();
            caretTimer.Start();
            Invalidate();
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }
        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            this.Cursor = Cursors.IBeam;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.Cursor = Cursors.Arrow;
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            return base.ProcessKeyPreview(ref m);
        }

        protected override void OnTabIndexChanged(EventArgs e)
        {
            Console.WriteLine("OnTabIndexChanged");
            base.OnTabIndexChanged(e);
        }
        protected override void OnTabStopChanged(EventArgs e)
        {
            Console.WriteLine("OnTabStopChanged");
            base.OnTabStopChanged(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            Console.WriteLine("OnKeyDown["+Text+"]");
            Console.WriteLine(e.KeyValue);
              
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    
                    if (e.Shift)
                    {

                    }
                    else
                    {
                    }
                    break;
            }
            if (Text.Length != 0)
            {
                if ((e.KeyCode == Keys.Left) && (currCaretPos > 0))
                {
                    if (currCaretPos > 0)
                        currCaretPos--;
                    
                    e.SuppressKeyPress = true;
                }
                if ((e.KeyCode == Keys.Right) && (currCaretPos < 30)) // count Width CORRECTLY
                {
                    if (currCaretPos < Text.Length)
                        currCaretPos++;
                    
                    e.SuppressKeyPress = true;
                }
            }
            if (Length + 1 >= maxTextLength)
                return;
        }
        
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            // LEFT = 37
            // UP = 38
            // RIGHT = 39
            // UP = 40
            Console.WriteLine("OnKeyPress[" + Text + "]");
            if ((e.KeyChar == 37) || (e.KeyChar == 38) || (e.KeyChar == 38) || (e.KeyChar == 40))
                e.KeyChar = '\0';
            
            Console.WriteLine(e.KeyChar);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            bool flag = true;
            Console.WriteLine("OnKeyUp[" + Text + "]");
            if (flag)
            {
                if ( (e.KeyCode != Keys.Left) && (e.KeyCode != Keys.Right) && (e.KeyCode != Keys.Up) && (e.KeyCode != Keys.Down) )
                {   
                    String tempText = Text.Substring(0, currCaretPos);
                    tempText = String.Concat(tempText, ((char)(e.KeyValue)).ToString());
                    Text = tempText + Text.Substring(currCaretPos);
                    currCaretPos++;
                }
            }            

            Invalidate();
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
           // base.OnPaint(e);
            
            if (displayError)
            {
               // String text = Text;
                //SizeF textSize = e.Graphics.MeasureString(text, Font);
                //e.Graphics
                //System.Drawing.Font f = new System.Drawing.Font(FontFamily.GenericSansSerif, 11F);
                //e.Graphics.DrawString(Text.Substring(0, startPosition), f, Brushes.Black, ClientRectangle);

                //Rectangle temp = new Rectangle(ClientRectangle.Location.X - text.Substring(0, startPosition).Length * 11, ClientRectangle.Location.Y, ClientRectangle.Width, ClientRectangle.Height);

                //e.Graphics.DrawString(text.Substring(startPosition, endPosition), f, Brushes.Red, temp);
                //e.Graphics.DrawString(text.Substring(endPosition, Text.Length), f, Brushes.Black, ClientRectangle);
                //f.Dispose();
            }
            else
            { 
             
            }

            /*
            if (Focus())
                ClientSize = WindowSize;
            else
                ClientSize = ClientRectangle.Size;*/

            // Paint usual TextBox
            System.Drawing.Pen borderTop;
            System.Drawing.Pen borderLRB;
            System.Drawing.Pen inner;
            if (Entered)
            {
                borderTop = new System.Drawing.Pen(System.Drawing.Color.FromArgb(60, 120, 180));
                borderLRB = new System.Drawing.Pen(System.Drawing.Color.FromArgb(180, 205, 230));
                inner = new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 255, 255));
            }
            else
            {
                borderTop = new System.Drawing.Pen(System.Drawing.Color.FromArgb(170, 175, 180));
                borderLRB = new System.Drawing.Pen(System.Drawing.Color.FromArgb(225, 235, 240));
                inner = new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 255, 255));
            }
            //System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
            e.Graphics.DrawLine(borderTop, 1, 0, ClientSize.Width - 1, 0); // Top  Border
            e.Graphics.DrawLine(borderLRB, 0, 0, 0, ClientSize.Height);// Left Border
            e.Graphics.DrawLine(borderLRB, ClientSize.Width - 1, 0, ClientSize.Width - 1, ClientSize.Height);// Right Border
            e.Graphics.DrawLine(borderLRB, 0, ClientSize.Height - 1, ClientSize.Width, ClientSize.Height - 1);// Bottom
            Rectangle rect = new Rectangle(1, 1, ClientSize.Width - 2, ClientSize.Height - 2);
            //e.Graphics.DrawRectangle(inner, rect);
            e.Graphics.FillRectangle(Brushes.White, rect);
            borderLRB.Dispose(); borderTop.Dispose(); inner.Dispose();

            //Draw Carret
            
            if (drawCaret)
            {
                System.Drawing.Pen pCaret = new Pen(System.Drawing.Color.FromArgb(0, 0, 0));
                if (currCaretPos > Text.Length)
                    currCaretPos = Text.Length;
                float x = e.Graphics.MeasureString(Text.Substring(0, currCaretPos), this.Font).Width;
                float y = e.Graphics.MeasureString("0", Font).Height ;
                e.Graphics.DrawLine(pCaret, 4 + x , 4, 4 + x , y); // Top  Border
                //e.Graphics.DrawLine(pCaret, new Point(4, 3), new Point(19, ClientSize.Height - 2)); // Top  Border
                pCaret.Dispose(); 
            }
            
            if (Text.Length > 0)
            { 
                System.Drawing.Font fnt = new System.Drawing.Font(FontFamily.GenericSansSerif, fontSize);
                //for (int i = 0; i< 10 || i < this.Text.Length ; i++) //(260 - textPadding * 2)/9
                
                    //if ((this.Text[i] != '\n') || (this.Text[i] != '\b') || (this.Text[i] != '\r'))
                e.Graphics.DrawString(Text.Substring(0, Text.Length), fnt, new SolidBrush(Color.Black), 4 , 2);
                //if (Text.Length>2)
                //e.Graphics.DrawString(Text.Substring(Text.Length / 2, Text.Length), fnt, new SolidBrush(Color.Red), e.Graphics.MeasureString(Text.Substring(Text.Length/2, Text.Length), fnt).Width, 2);
                
                fnt.Dispose();
            }

        }

        private void caretTimer_Tick(System.Object sender, System.EventArgs e)
        {
            if (!drawCaret)
                drawCaret = true;
            else
                drawCaret = false;
            //Invalidate();
            PaintEventArgs ev = new PaintEventArgs(this.CreateGraphics(),ClientRectangle);
            OnPaint(ev);
            ev.Dispose();
        }
       
        public CustomTextBox()
        {
            SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
            caretTimer.Interval = 700;
            caretTimer.Tick +=new EventHandler(caretTimer_Tick);
            maxTextLength = Text.Length;
            InitializeComponent();
            // How much space fill letter
            Graphics g = this.CreateGraphics();
            System.Drawing.Font fnt = new System.Drawing.Font(FontFamily.GenericSansSerif, fontSize);
            displayableLettersNumber = g.MeasureString("1", fnt).Width;
        }

    }
}