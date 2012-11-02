using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Problem2.Solvers.Entities;

namespace Problem2.GraphicalOutput
{
    /// <summary>
    /// Interaction logic for DotPoint.xaml
    /// </summary>
    public partial class DotPoint
    {
        private readonly List<FutureEventListRow> _events;
        private readonly string _toolTipText;

        public DotPoint(List<FutureEventListRow> data)
        {
            InitializeComponent();
            _events = data;
            // Generate ToolTip Text

            if (data.Count > 0) { 
                _toolTipText = data[0].Time + Environment.NewLine + data[0].EventType.ToString();
                _toolTipText += Environment.NewLine + Environment.NewLine;
                _toolTipText += data[0].Customer.ToString();
                ttooltip.Content = _toolTipText;
            }
        }

        public DotPoint()
        {
            InitializeComponent();
            _events = new List<FutureEventListRow>();
        }

        private void UserControlMouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void FieldtooltipToolTipOpening(object sender, ToolTipEventArgs e)
        {
            // 
        }
    }
}