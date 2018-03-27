using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using ZedGraph;
using System.Collections;


namespace ZedGraphExample
{
    public partial class DlgZedTest : Form
    {
        #region variable
     
        #endregion 

        #region Constructor & PrepareFunc
        public DlgZedTest()
        {
            InitializeComponent();
            PrepareProcess();
        }
        private void PrepareProcess()
        {
            getMatlabdata();
            SetSize();

        }
        #endregion


        string matlab_path = @"cd C:\Users\wyz\Desktop\matlabInt";//the folder directory where save m files 

        PointPairList userClickrList = new PointPairList();
        PointPairList userClickrList2 = new PointPairList();
        LineItem userClickCurve = new LineItem("userClickCurve");
        double[] x_plot = null;
        double[] y_plot = null;
        double xVal;//click xValue
        double yVal;//click Yvalue
        double fileNumber;
        int intFileNumber;
        double start_dragXval;
        double start_dragYval;
        double end_dragXval;
        double end_dragYval;
        int xLength;
        
        double[] dintensity = null;
        private void getMatlabdata()
        {


            // Create the MATLAB instance 
            MLApp.MLApp matlab = new MLApp.MLApp();
            matlab.Execute(matlab_path);

            object resultdata = null;
            matlab.Feval("binfileReader2", 2, out resultdata);

            object[] res = resultdata as object[];

       



            double[,] intensity = (double[,])res[0];
            fileNumber = (double)res[1];
            intFileNumber = Convert.ToInt32(fileNumber);
            dintensity = new double[intensity.Length];

            


            for (int i = 0; i < intensity.Length; i++)
            {

                dintensity[i] = intensity[0, i];
            }

            InitialZedGraph();


        }
        private void InitialZedGraph()
        {
            ///plotting 
            GraphPane myPane = zedGraphControl1.GraphPane;
            GraphPane myPane2 = zedGraphControl2.GraphPane;
            // Set the Titles1
            myPane.Title.Text = "Mass Spectrometry";
            myPane.XAxis.Title.Text = "File";
            myPane.YAxis.Title.Text = "Intensity";
           
            // Set the Titles2
            myPane2.Title.Text = "Mass Spectrometry";
            myPane2.XAxis.Title.Text = "time";
            myPane2.YAxis.Title.Text = "Intensity";
            
            //Range of the myPane(first graph)   
            //int intFileNumber = Convert.ToInt32(fileNumber);
            // myPane.XAxis.Max = intFileNumber;


            PointPairList maxInt = new PointPairList();

            for (int i = 0; i < dintensity.Length; i++)
            {

                maxInt.Add(i, dintensity[i]);
            }


            CurveItem graph1 = myPane.AddCurve("Mass A",
                  maxInt, Color.Blue, SymbolType.Circle);



            zedGraphControl1.AxisChange();


        }

        private void SetSize()
        {
            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl2.IsShowPointValues = true;
            // zedGraphControl1.Location = new Point(0, 0);
            // zedGraphControl1.Size = new Size(this.ClientRectangle.Width - 20, this.ClientRectangle.Height - 50);

        }

    
      
        private void masstochargeMatlab(double xVal)
        {
            int selectedfileNumber = Convert.ToInt32(xVal) + 1;
            intFileNumber = Convert.ToInt32(fileNumber);
            if (selectedfileNumber > intFileNumber)
            {
                //not to over max Xvalue
                selectedfileNumber = intFileNumber;
            }

            // Create the MATLAB instance 
            MLApp.MLApp matlab = new MLApp.MLApp();
            matlab.Execute(matlab_path);

            object resultdata = null;
            matlab.Feval("mTcPlot", 2, out resultdata, selectedfileNumber);

            object[] res = resultdata as object[];

            object_To_double(res);
        }

        private void masstochargeMatlab_Range(double startXval,double endXval)
        {
            int selectedfileNumber_start = Convert.ToInt32(startXval) + 1;
            int selectedfileNumber_end = Convert.ToInt32(endXval) + 1;
            int selectedFileNumber = selectedfileNumber_end - selectedfileNumber_start + 1;
            if (selectedfileNumber_start > intFileNumber|| selectedfileNumber_start> intFileNumber)
            {
                //not to over max Xvalue
                selectedfileNumber_start = intFileNumber;
                selectedfileNumber_end = intFileNumber;
            }

            // Create the MATLAB instance 
            MLApp.MLApp matlab = new MLApp.MLApp();
            matlab.Execute(matlab_path);

            object resultdata = null;

            //There is no xLength Data
            if (xLength == 0) {
                xLength = 25000;
            }

            matlab.Feval("mTcPlot_Range", 2, out resultdata, selectedfileNumber_start, selectedfileNumber_end,xLength);

            object[] res = resultdata as object[];

            object_To_double(res);
            
        }

        private void object_To_double(object[] res)
        {
            double[,] temp_x = (double[,])res[0];
            double[,] temp_y = (double[,])res[1];

            x_plot = new double[temp_x.Length];
            y_plot = new double[temp_y.Length];


            for (int i = 0; i < temp_x.Length; i++)
            {
                x_plot[i] = temp_x[0, i];
                y_plot[i] = temp_y[0, i];

            }

        }



        //Red line for selecting one file
        private void zedGraphControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            // Create an instance of Graph Pane
            GraphPane myPane = zedGraphControl1.GraphPane;

            GraphPane myPane2 = zedGraphControl2.GraphPane;

            if (myPane.CurveList.Count > 1)
            {
                myPane.CurveList[myPane.CurveList.Count - 1].Clear();
            }

            if (myPane2.CurveList.Count >= 1)
            {
                myPane2.CurveList[myPane2.CurveList.Count - 1].Clear();
            }


            zedGraphControl1.Refresh();


            // Clear the previous values if any
            userClickrList.Clear();

            myPane.Legend.IsVisible = false;
            myPane2.Legend.IsVisible = false;
            // Use the current mouse locations to get the corresponding 
            // X & Y CO-Ordinates
            myPane.ReverseTransform(e.Location, out xVal, out yVal);

            int intFileNumber = Convert.ToInt32(fileNumber);
            if (xVal < intFileNumber)
            {
                // Create a list using the above x & y values
                userClickrList.Add(xVal, myPane.YAxis.Scale.Max);
                userClickrList.Add(xVal, myPane.YAxis.Scale.Min);

                // Add a curve
                //userClickCurve = myPane.AddCurve(" ", userClickrList, Color.Red, SymbolType.None);

                zedGraphControl1.Refresh();


                masstochargeMatlab(xVal);

                PointPairList msList = new PointPairList();

                for (int i = 0; i < x_plot.Length; i++)
                {

                    msList.Add(x_plot[i], y_plot[i]);

                }

                CurveItem graph2 = myPane2.AddCurve("Mass A",
               msList, Color.Green, SymbolType.None);


                zedGraphControl2.AxisChange();

            }//if


            zedGraphControl2.Refresh();

        }

        private bool zedGraphControl1_MouseDownEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            
            GraphPane myPane = zedGraphControl1.GraphPane;    // X & Y CO-Ordinates
            myPane.ReverseTransform(e.Location, out xVal, out yVal);
            start_dragXval = xVal;
            start_dragYval = yVal;
            
            if (myPane.CurveList.Count > 1)
            {
                myPane.CurveList[myPane.CurveList.Count - 1].Clear();
            }

            zedGraphControl1.Refresh();

            // Clear the previous values if any
            userClickrList.Clear();

            myPane.Legend.IsVisible = false;


            // Create a list using the above x & y values
            userClickrList.Add(start_dragXval, start_dragYval);


            // Add a curve
            userClickCurve = myPane.AddCurve(" ", userClickrList, Color.Red, SymbolType.Circle);

            zedGraphControl1.Refresh();

            return true;
        }
    

        private bool zedGraphControl1_MouseUpEvent(ZedGraphControl sender, MouseEventArgs e)
        {

            GraphPane myPane = zedGraphControl1.GraphPane;
            GraphPane myPane2 = zedGraphControl2.GraphPane;
            myPane.ReverseTransform(e.Location, out xVal, out yVal);
            end_dragXval = xVal;
            end_dragYval = start_dragYval;

            double calculation = end_dragXval - start_dragXval;
            if (calculation > 1) {

                if (myPane.CurveList.Count > 1)
                {
                    myPane.CurveList[myPane.CurveList.Count - 1].Clear();
                }

                if (myPane2.CurveList.Count >= 1)
                {
                    myPane2.CurveList[myPane2.CurveList.Count - 1].Clear();
                }


                zedGraphControl1.Refresh();

                // Clear the previous values if any
                userClickrList.Clear();

                myPane.Legend.IsVisible = false;
                
                if (end_dragXval < intFileNumber)
                {

                    // Create a list using the above x & y values
                    userClickrList.Add(start_dragXval, start_dragYval);
                    userClickrList.Add(end_dragXval, start_dragYval);

                    // Add a curve
                    //  userClickCurve = myPane.AddCurve(" ", userClickrList, Color.Azure, SymbolType.Circle);

                    zedGraphControl1.Refresh();
                    masstochargeMatlab_Range(start_dragXval, end_dragXval);

                    PointPairList msList = new PointPairList();

                    for (int i = 0; i < x_plot.Length; i++)
                    {

                        msList.Add(x_plot[i], y_plot[i]);

                    }

                    CurveItem graph2 = myPane2.AddCurve("Mass A",
                   msList, Color.Black, SymbolType.None);


                    zedGraphControl2.AxisChange();

                }//if
            zedGraphControl2.Refresh();

        }

            return true;
        }

    















        /*
        #region General Function
        private void MakeChart()
        {
            int lineWidth = 2;
            exampleGraphPane = zedGraphControl1.GraphPane;

            exampleGraphPane.Title.Text = "EXAMPLE FOR ZEDGRAPH";
            //exampleGraphPane.Title.IsVisible = false;//그래프 타이틀이 보기싫으면 false. default는 true;
            exampleGraphPane.XAxis.Type = AxisType.Linear;
            exampleLineItem = exampleGraphPane.AddCurve("EXAMPLE", examplePointPairLitst, Color.Yellow, SymbolType.None);

            exampleLineItem.Line.Width = lineWidth;
            exampleLineItem.Symbol.Fill = new Fill(Color.Black);

            exampleGraphPane.XAxis.MajorGrid.IsVisible = true;
            exampleGraphPane.YAxis.MajorGrid.IsVisible = true;
            exampleGraphPane.XAxis.MajorGrid.Color = Color.White;
            exampleGraphPane.YAxis.MajorGrid.Color = Color.White;

            exampleGraphPane.XAxis.Scale.Min = -10;
            exampleGraphPane.XAxis.Scale.Max = 10;
            exampleGraphPane.YAxis.Scale.Min = -10;
            exampleGraphPane.YAxis.Scale.Max = 10;

            exampleGraphPane.Chart.Fill = new Fill(Color.Black);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }
     
        private void SetDataRandom()
        {
            exampleLineItem.Clear();

            Random rand = new Random(1);
            for(int i =0;i<3;i++)
            {
                examplePointPairLitst.Add(i, rand.NextDouble());
            }
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();    
            
        }
        #endregion 
    */

    }
}
