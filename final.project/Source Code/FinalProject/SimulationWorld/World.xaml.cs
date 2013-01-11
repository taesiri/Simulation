using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using FinalProject.SimulationElements;
using FinalProject.SimulationElements.Enums;
using FinalProject.SimulationElements.FutureEventList;

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
            CreateScene();
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
            var entranceStation = new ServiceEntranceStation(new Point3D(-22, 0, 0.5f));

            var platformA = new ServicePlatformElement(new Point3D(-7.5, 0, -4.5));
            var platformB = new ServicePlatformElement(new Point3D(0, 0, -4.5));
            var platformC = new ServicePlatformElement(new Point3D(7.5, 0, -4.5));

            var exitStationA = new ServiceInspectorStation(new Point3D(18, 0, 0.5f));
            //var exitStationB = new ServiceInspectorStation(new Point3D(18, -3, 0.5f));


            var sampleBox = new ServiceBoxElement(new Point3D(0, 0, 2.1), 2.5, 2.5, 2.5, @"Textures\postBox.jpg");

            Mother.Children.Add(platformA);
            Mother.Children.Add(platformB);
            Mother.Children.Add(platformC);
            Mother.Children.Add(entranceStation);
            Mother.Children.Add(exitStationA);
            // Mother.Children.Add(exitStationB);

            Mother.Children.Add(sampleBox);
        }


        public void Simulator()
        {
            bool running = true;
            var fel = new FutureEventList();
            while (running)
            {
                Events imminentEvent = fel.GetImminentEvent();
                switch (imminentEvent)
                {
                    case Events.Arrival:
                        break;
                    case Events.Departure:
                        break;

                    case Events.ServiceEndedOnStationA:
                        break;
                    case Events.ServiceEndedOnStationB:
                        break;
                    case Events.ServiceEndedOnStationC:
                        break;

                    case Events.ServiceStartedOnStationA:
                        break;
                    case Events.ServiceStartedOnStationB:
                        break;
                    case Events.ServiceStartedOnStationC:
                        break;

                    case Events.MovingToStationA:
                        break;
                    case Events.MovingToStationB:
                        break;
                    case Events.MovingToStationC:
                        break;

                    case Events.EndOfSimulation:
                        running = false;
                        break;

                    case Events.BeginingOfSimulation:
                        // Place Holder!
                        break;

                    default:
                        break;
                }
            }


            //END OF Simulation
        }
    }
}