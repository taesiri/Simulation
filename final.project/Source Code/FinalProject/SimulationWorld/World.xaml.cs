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
        private ServicePlatformElement _platformElem;

        public World()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var platformElement = new ServicePlatformElement();

            Mother.Children.Add(platformElement);

            _platformElem = platformElement;
        }

        private void BtnMoveIt_OnClick(object sender, RoutedEventArgs e)
        {
    
            var da = new DoubleAnimation();
            da.From = 0;
            da.To = 100f;
            da.Duration = new Duration(TimeSpan.FromSeconds(10));


            _platformElem.Tranformer.BeginAnimation(TranslateTransform3D.OffsetXProperty, da);
        }
    }
}