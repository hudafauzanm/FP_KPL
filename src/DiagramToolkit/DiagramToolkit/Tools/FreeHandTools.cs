using DiagramToolkit.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiagramToolkit.Tools
{
    public class FreeHandTool : ToolStripButton, ITool
    {
        private ICanvas canvas;
        private FreeHand lineSegment;

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

        public FreeHandTool()
        {
            this.Name = "FreeHand tool";
            this.ToolTipText = "FreeHand tool";
            this.Image = IconSet.download;
            this.CheckOnClick = true;
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lineSegment = new FreeHand(new System.Drawing.Point(e.X, e.Y), new System.Drawing.Point(e.X, e.Y));
                canvas.AddDrawingObject(lineSegment);
            }
            else if(e.Button == MouseButtons.Right)
            {
                canvas.DeselectAllObjects();
                DrawingObject selectedObject = canvas.SelectObjectAt(e.X, e.Y);
                if(selectedObject !=null)
                selectedObject.Flip();
            }
        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.lineSegment != null)
                {
                    //lineSegment.SetLastPoint(new System.Drawing.Point(e.X, e.Y));
                    lineSegment.AddPoint(new System.Drawing.Point(e.X, e.Y), -2);
                }
                
            }
        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {
            if (this.lineSegment != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    lineSegment.SetLastPoint(new System.Drawing.Point(e.X, e.Y));
                    lineSegment.Select();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    canvas.DeselectAllObjects();
                }
            }
        }

        public void ToolMouseDoubleClick(object sender, MouseEventArgs e)
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