using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramToolkit.Shapes
{
    class FreeHand : DrawingObject
    {
        private Pen pen;
        List<Point> freehandPoint;

        const double EPSILON = 3.0;

        public FreeHand(Point startPoint, Point endPoint)
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;
            freehandPoint = new List<Point>();
            freehandPoint.Add(startPoint);
            freehandPoint.Add(endPoint);

        }



        public void SetLastPoint(Point point)
        {
            freehandPoint[freehandPoint.Count - 1] = point;
        }

        public FreeHand(Point startPoint)
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;
            freehandPoint = new List<Point>();
            freehandPoint.Add(startPoint);

        }



        public override bool Intersect(int xTest, int yTest)
        {

            for (int i = 0; i < freehandPoint.Count-1; i++)
            {
                Point Startpoint = freehandPoint[i];
                Point Endpoint = freehandPoint[i+1];
                if (inLine(Startpoint, Endpoint, xTest, yTest))
                    return true;
            }
            return false;
            
        }

        public double GetSlope(Point Startpoint, Point Endpoint)
        {
            double m = (double)(Endpoint.Y - Startpoint.Y) / (double)(Endpoint.X - Startpoint.X);
            return m;
        }


        public override void RenderOnEditingView()
        {
            pen.Color = Color.Blue;
            pen.Width = 1.5f;
            pen.DashStyle = DashStyle.Solid;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;

                for (int i = 0; i < freehandPoint.Count - 1; i++)
                {
                    Point startPoint = freehandPoint[i];
                    Point endPoint = freehandPoint[i + 1];
                    this.GetGraphics().DrawLine(pen, startPoint, endPoint);
                }


            }
        }

        public override void RenderOnPreview()
        {
            pen.Color = Color.Red;
            pen.Width = 1.5f;
            pen.DashStyle = DashStyle.DashDotDot;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                for (int i = 0; i < freehandPoint.Count - 1; i++)
                {
                    Point startPoint = freehandPoint[i];
                    Point endPoint = freehandPoint[i + 1];
                    this.GetGraphics().DrawLine(pen, startPoint, endPoint);
                }
            }
        }

        public override void RenderOnStaticView()
        {
            pen.Color = Color.Black;
            pen.Width = 1.5f;
            pen.DashStyle = DashStyle.Solid;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                for (int i = 0; i < freehandPoint.Count - 1; i++)
                {
                    Point startPoint = freehandPoint[i];
                    Point endPoint = freehandPoint[i + 1];
                    this.GetGraphics().DrawLine(pen, startPoint, endPoint);
                }
            }
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            for (int i = 0; i < freehandPoint.Count; i++)
            {
                freehandPoint[i] = new Point(freehandPoint[i].X + xAmount, freehandPoint[i].Y + yAmount);
            }
        }

        public override void AddPoint(Point point, int position)
        {
            if (position == freehandPoint.Count)
                freehandPoint.Add(point);
            else if (position == -2)
            {
                freehandPoint.Add(point);
            }
            else
                freehandPoint.Insert(position + 1, point);
        }

        public bool inLine(Point Startpoint, Point Endpoint, int xTest, int yTest)
        {
            double m = GetSlope(Startpoint, Endpoint);
            double b = Endpoint.Y - m * Endpoint.X;
            double y_point = m * xTest + b;

            if (Math.Abs(yTest - y_point) < EPSILON)
            {

                return true;
            }
            return false;
        }

        

        public override bool Add(DrawingObject obj)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(DrawingObject obj)
        {
            throw new NotImplementedException();
        }

        public override void Flip()
        {
            int xLast = freehandPoint[0].X;
            int i;
            for(i=1; i<freehandPoint.Count; i++)
            {
                if (xLast < freehandPoint[i].X)
                    xLast = freehandPoint[i].X;
            }

            for(i = 0; i < freehandPoint.Count; i++)
            {
                int newXPoint = freehandPoint[i].X + (2 * (xLast - freehandPoint[i].X));
                freehandPoint[i] = new Point(newXPoint, freehandPoint[i].Y);
            }
            
        }
    }
}