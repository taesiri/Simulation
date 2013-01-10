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
        private ServiceEntranceStation _platformElem;



        public World()
        {
            InitializeComponent();
        }

        private void BtnDeployOnClick(object sender, RoutedEventArgs e)
        {
            var platformElement = new ServiceEntranceStation();

            Mother.Children.Add(platformElement);

            _platformElem = platformElement;
        }

        private void BtnMoveIt_OnClick(object sender, RoutedEventArgs e)
        {
            var da = new DoubleAnimation {From = -10, To = 10f, Duration = new Duration(TimeSpan.FromSeconds(2))};
            _platformElem.Tranformer.BeginAnimation(TranslateTransform3D.OffsetXProperty, da);
        }


        public void CreateScene()
        {


        }   
    }
}