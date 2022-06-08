#region Header
using Infragistics.Win.UltraWinChart;
using Infragistics.UltraChart.Core.Layers;
using Infragistics.UltraChart.Shared.Styles;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Core;
using Infragistics.UltraChart.Core.Primitives;
using Infragistics.UltraChart.Core.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;

#endregion

namespace SAMMI.QM
{
    public class CPChartManager
    {
        public UltraChart Chart;
        public double Cl = 0;
        public double StdevVal = 0;
        public double AvgVal = 0;
        public double OneSigmaU = 0;
        public double OneSigmaL = 0;
        public double TwoSigmaU = 0;
        public double TwoSigmaL = 0;
        public double ThrSigmaU = 0;
        public double ThrSigmaL = 0;
        public double KSigma = 0;
        public double Sigma = 0;
        public string SpecType = "B";
        public double USL = 0;
        public double LSL = 0;
        public double HCL = 0;


        public CPChartManager(UltraChart chart)
        {
            this.Chart = chart;
            this.Chart.FillSceneGraph += new Infragistics.UltraChart.Shared.Events.FillSceneGraphEventHandler(Chart_FillSceneGraph);
        }
        public CPChartManager(UltraChart chart, string spectype, double ksigma, double sigma, double cl)
        {
            double x = 0;
            this.Chart = chart;
            this.Chart.FillSceneGraph += new Infragistics.UltraChart.Shared.Events.FillSceneGraphEventHandler(Chart_FillSceneGraph);

            this.SpecType = spectype;
            this.KSigma = ksigma;
            this.Sigma = sigma;
            this.Cl = cl;

            x = (this.KSigma * this.Sigma) / 3;
            this.OneSigmaU = this.Cl + x;
            this.TwoSigmaU = this.Cl + 2 * x;
            this.ThrSigmaU = this.Cl + 3 * x;
            this.OneSigmaL = this.Cl - x;
            this.TwoSigmaL = this.Cl - 2 * x;
            this.ThrSigmaL = this.Cl - 3 * x;
        }

        #region Method Define Area
        public void InitChart()
        {
            Cl = 0;
            StdevVal = 0;
            AvgVal = 0;
            OneSigmaU = 0;
            OneSigmaL = 0;
            TwoSigmaU = 0;
            TwoSigmaL = 0;
            ThrSigmaU = 0;
            ThrSigmaL = 0;
            KSigma = 0;
            Sigma = 0;
            USL = 0;
            LSL = 0;
            HCL = 0;
        }

        public void DrawChart(DataTable dtChart, string colname, double axismin, double axismax, double usl, double lsl, double hcl)
        {
            this.USL = usl;
            this.LSL = lsl;

            this.DrawChart(dtChart, axismin, axismax, hcl);
        }

        public void DrawChart(DataTable dtchart, double axismin, double axismax, double hcl)
        {
            this.HCL = hcl;
        }

        public void DrawChart(double axismin, double axismax, double avg, double stddev, double cl, double usl, double lsl)
        {
            this.AvgVal = avg;
            this.StdevVal = stddev;
            this.USL = usl;
            this.LSL = lsl;
            this.Cl = cl;

            if (this.Chart.Axis.X.RangeMin > axismin) this.Chart.Axis.X.RangeMin = axismin;
            if (this.Chart.Axis.X.RangeMin > lsl) this.Chart.Axis.X.RangeMin = lsl;
            if (this.Chart.Axis.X.RangeMin > (this.AvgVal - this.StdevVal * 3)) this.Chart.Axis.X.RangeMin = (this.AvgVal - this.StdevVal * 3);

            if (this.Chart.Axis.X.RangeMax < axismax) this.Chart.Axis.X.RangeMax = axismax;
            if (this.Chart.Axis.X.RangeMax < usl) this.Chart.Axis.X.RangeMax = usl;
            if (this.Chart.Axis.X.RangeMax < (this.AvgVal + this.StdevVal * 3)) this.Chart.Axis.X.RangeMax = (this.AvgVal + this.StdevVal * 3);

            //if (StdevVal == 0) this.Chart.Axis.X.TickmarkInterval = this.Chart.Axis.X.RangeMax - this.Chart.Axis.X.RangeMin;
        }
        #endregion

        #region Event Area
        private void Chart_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            try
            {
                IAdvanceAxis axisX = (IAdvanceAxis)e.Grid["X"];
                IAdvanceAxis axisY = (IAdvanceAxis)e.Grid["Y"];

                double x1 = 0;
                double x2 = 0;
                double y1 = this.Chart.Axis.Y.RangeMin;
                double y2 = 0;

                int xStart = (int)axisY.MapMinimum;
                int xEnd = (int)axisY.MapMaximum;
                int ystart = (int)axisX.MapMaximum;
                int yend = (int)axisX.MapMinimum;

                int uslY = (int)axisX.Map(this.USL);
                int lslY = (int)axisX.Map(this.LSL);
                int clY = (int)axisX.Map(this.Cl);
                int avgY = (int)axisX.Map(this.AvgVal);
                int sigma3ly = (int)axisX.Map(this.AvgVal - this.StdevVal * 3);
                int sigma3uy = (int)axisX.Map(this.AvgVal + this.StdevVal * 3);

                if ((this.AvgVal == 0) && (this.StdevVal == 0)) return;

                Line uslLn = new Line(new Point(uslY, xStart), new Point(uslY, xEnd));
                Line lslLn = new Line(new Point(lslY, xStart), new Point(lslY, xEnd));
                Line clLn = new Line(new Point(clY, xStart), new Point(clY, xEnd));
                Line avgvalLn = new Line(new Point(avgY, xStart), new Point(avgY, xEnd));

                Box sigmabx = new Box(new Point(sigma3ly, xEnd), sigma3uy - sigma3ly, xStart - xEnd);
                sigmabx.lineStyle.DrawStyle = LineDrawStyle.Dash;
                sigmabx.PE.Stroke = Color.Black;
                sigmabx.PE.StrokeOpacity = 120;
                sigmabx.PE.StrokeWidth = 1;
                sigmabx.PE.Fill = Color.Green;
                sigmabx.PE.FillOpacity = 120;
                e.SceneGraph.Insert(e.SceneGraph.Count, sigmabx);


                //Box sigma2bx = new Box(new Point(xStart, sigma2uy), xEnd - xStart, sigma2ly - sigma2uy);
                //sigma2bx.lineStyle.DrawStyle = LineDrawStyle.Dash;
                //sigma2bx.PE.Stroke           = Color.Black;
                //sigma2bx.PE.StrokeOpacity    = 120;
                //sigma2bx.PE.StrokeWidth      = 1;
                //sigma2bx.PE.Fill             = Color.Blue;
                //sigma2bx.PE.FillOpacity      = 50;

                //Box sigma3bx = new Box(new Point(xStart, sigma3uy), xEnd - xStart, sigma3ly - sigma3uy);
                //sigma3bx.lineStyle.DrawStyle = LineDrawStyle.Dash;
                //sigma3bx.PE.Stroke           = Color.Black;
                //sigma3bx.PE.StrokeOpacity    = 120;
                //sigma3bx.PE.StrokeWidth      = 1;
                //sigma3bx.PE.Fill             = Color.Yellow;
                //sigma3bx.PE.FillOpacity      = 50;

                //Box chartBox = new Box(new Point(xStart, ystart), xEnd - xStart, yend - ystart);
                //chartBox.lineStyle.DrawStyle = LineDrawStyle.Solid;
                //chartBox.PE.Stroke           = Color.Black;
                //chartBox.PE.StrokeWidth      = 1;

                //e.SceneGraph.Insert(e.SceneGraph.Count, sigma3bx);
                //e.SceneGraph.Insert(e.SceneGraph.Count, sigma2bx);
                //e.SceneGraph.Insert(e.SceneGraph.Count, sigma1bx);
                //e.SceneGraph.Insert(e.SceneGraph.Count, chartBox);

                uslLn.PE.Stroke = Color.Red; uslLn.PE.StrokeWidth = 2;
                uslLn.lineStyle.DrawStyle = LineDrawStyle.Dash;
                lslLn.PE.Stroke = Color.Red; lslLn.PE.StrokeWidth = 2;
                lslLn.lineStyle.DrawStyle = LineDrawStyle.Dash;
                clLn.PE.Stroke = Color.Blue; clLn.PE.StrokeWidth = 2;
                clLn.lineStyle.DrawStyle = LineDrawStyle.Dash;
                avgvalLn.PE.Stroke = Color.Black; clLn.PE.StrokeWidth = 2;
                avgvalLn.PE.StrokeWidth = 2;
                avgvalLn.lineStyle.DrawStyle = LineDrawStyle.Solid;

                e.SceneGraph.Add(uslLn);

                if (this.LSL > 0)
                    e.SceneGraph.Add(lslLn);

                e.SceneGraph.Add(clLn);
                e.SceneGraph.Add(avgvalLn);

                int k = 0;
                for (int i = (int)axisX.MapMinimum; i < (int)axisX.MapMaximum; i++)
                {
                    x1 = (double)axisX.MapInverse((double)(i));

                    y1 = ((1 / ((System.Math.Sqrt(2 * System.Math.PI)) * this.StdevVal)))
                        * System.Math.Exp(-1 * System.Math.Pow((x1 - this.AvgVal), 2) / (2 * System.Math.Pow(this.StdevVal, 2)));
                    y1 = y1 * (double)axisY.Maximum / ((1 / ((System.Math.Sqrt(2 * System.Math.PI)) * this.StdevVal)))
                        * System.Math.Exp(-1 * System.Math.Pow((this.AvgVal - this.AvgVal), 2) / (2 * System.Math.Pow(this.StdevVal, 2)));

                    if (k == 0)
                    {
                        x2 = x1;
                        y2 = y1;
                    }

                    k++;
                    Line histLn = new Line(new Point((int)axisX.Map(x1), (int)axisY.Map(y1)), new Point((int)axisX.Map(x2), (int)axisY.Map(y2)));

                    histLn.lineStyle.DrawStyle = LineDrawStyle.Solid;
                    histLn.PE.StrokeWidth = 2;
                    histLn.PE.Stroke = Color.Red;

                    e.SceneGraph.Add(histLn);

                    x2 = x1;
                    y2 = y1;

                    // 메모리 정리를 위한 COMMON CleanProcess 함수호출.
                    SAMMI.Common common = new Common();
                    common.CleanProcess("SK_MESDB_V1");
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // 메모리 정리를 위한 COMMON CleanProcess 함수호출.
                SAMMI.Common common = new Common();
                common.CleanProcess("SK_MESDB_V1");
            }
        }
        #endregion
    }
}
