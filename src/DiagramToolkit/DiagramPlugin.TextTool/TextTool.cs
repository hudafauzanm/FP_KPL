﻿using DiagramToolkit;
using DiagramPlugin.Shapes;
using System;
using System.Windows.Forms;

namespace DiagramPlugin.TextTool
{
    public class TextTool : ToolStripButton, ITool, IPlugin
    {
        private Text text;
        private ICanvas canvas;

        public Cursor Cursor
        {
            get
            {
                return Cursors.Arrow;
            }
        }

        public ICanvas TargetCanvas
        {
            get
            {
                return this.canvas;
            }

            set
            {
                this.canvas = value;
            }
        }

        public IPluginHost Host
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public TextTool()
        {
            this.Name = "Text tool";
            this.ToolTipText = "Text tool";
            this.Image = IconSet.font;
            this.CheckOnClick = true;
        }

        public void ToolMouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                text = new Text();
                text.Value = "Text";
                text.X = e.X;
                text.Y = e.Y;

                DrawingObject obj = canvas.SelectObjectAt(e.X, e.Y);

                if (obj == null)
                {
                    canvas.AddDrawingObject(text);
                }
                else
                {
                    bool allowed = obj.Add(text);

                    if (!allowed)
                    {
                        canvas.AddDrawingObject(text);
                    }
                }

            }
        }
        public void ToolMouseMove(object sender, MouseEventArgs e)
        {

        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {

        }

        public void ToolKeyUp(object sender, KeyEventArgs e)
        {

        }

        public void ToolKeyDown(object sender, KeyEventArgs e)
        {

        }

        public void ToolHotKeysDown(object sender, Keys e)
        {

        }
    }
}
