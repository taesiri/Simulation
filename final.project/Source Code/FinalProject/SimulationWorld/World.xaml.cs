using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using FinalProject.SimulationElements;

namespace FinalProject.SimulationWorld
{
    /// <summary>
    ///     Interaction logic for World.xaml
    /// </summary>
    public partial class World
    {
        private ServiceBoxElement _boxE;

        public World()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //var box = new ServiceBoxElement(new Point3D(1, 2, 3));

            //Mother.Children.Add(box);

            //_boxE = box;
        }

        private void BtnMoveIt_OnClick(object sender, RoutedEventArgs e)
        {
            //var da = new DoubleAnimation();
            //da.From = 0;
            //da.To = 100f;
            //da.Duration = new Duration(TimeSpan.FromSeconds(10));


            //_boxE.Tranformer.BeginAnimation(TranslateTransform3D.OffsetXProperty, da);
        }

    }
}