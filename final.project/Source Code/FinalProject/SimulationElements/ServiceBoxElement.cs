using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace FinalProject.SimulationElements
{
    public class ServiceBoxElement : UIElement3D
    {
        public TranslateTransform3D Tranformer;

        public ServiceBoxElement()
        {
            // Default Box

            CreateBox(new Point3D(0, 0, 0), 1, 1, 1, @"Textures\boxDefault.jpg");
        }

        public ServiceBoxElement(string textureUri)
        {
            CreateBox(new Point3D(0, 0, 0), 1, 1, 1, textureUri);
        }

        public ServiceBoxElement(Point3D location)
        {
            CreateBox(location, 1, 1, 1, @"Textures\boxDefault.jpg");
        }

        public ServiceBoxElement(double xLen, double yLen, double zLen)
        {
            CreateBox(new Point3D(0, 0, 0), xLen, yLen, zLen, @"Textures\boxDefault.jpg");
        }

        public ServiceBoxElement(Point3D location, double xLen, double yLen, double zLen, string textureUri)
        {
            CreateBox(location, xLen, yLen, zLen, textureUri);
        }


        public DateTime ArrivalTime { get; set; } // Entered to the System
        public DateTime DepartureTime { get; set; } // Left the System

        public DateTime MoveToStationAStartTime { get; set; } // Start Time - Moving Box to Station A
        public DateTime StationAServiceStartTime { get; set; } // Service Start Time on Station A
        public TimeSpan StationAServiceDuration { get; set; } // Service Duration on This Station

        public DateTime MoveToStationBStartTime { get; set; } // Start Time - Moving Box to Station B
        public DateTime StationBServiceStartTime { get; set; } // Service Start Time on Station B
        public TimeSpan StationBServiceDuration { get; set; } // Service Duration on This Station

        public DateTime MoveToStationCStartTime { get; set; } // Start Time - Moving Box to Station C
        public DateTime StationCServiceStartTime { get; set; } // Service Start Time on Station C
        public TimeSpan StationCServiceDuration { get; set; } // Service Duration on This Station

        public DateTime MoveToInspector { get; set; } // Moved into Inspector
        public DateTime InspectorServiceStartTime { get; set; } // Inspector Start Time
        public DateTime InspectorQueueTime { get; set; } // Inspector Queue Time
        public TimeSpan InspectorDuration { get; set; } //Inspector Service Duration

        public ServicePlatformElement CurrentServiceStation { get; set; } // Pretty Useless!


        public TimeSpan GetTotalServiceTime // Total Time of 'Service' (on Each Station)
        {
            get { return StationAServiceDuration + StationAServiceDuration + StationCServiceDuration; }
        }


        public void CreateBox(Point3D location, double xLen, double yLen, double zLen, string textureUri)
        {
            var geometryModel = new GeometryModel3D();

            var meshBuilder = new MeshBuilder();
            meshBuilder.AddBox(location, xLen, yLen, zLen);

            geometryModel.Geometry = meshBuilder.ToMesh();
            geometryModel.Material = MaterialHelper.CreateImageMaterial(textureUri);

            Visual3DModel = geometryModel;


            SetTransformer();
        }

        private void SetTransformer()
        {
            Tranformer = new TranslateTransform3D();
            Transform = Tranformer;
        }


        public void ShowDetailedInformation()
        {
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            ShowDetailedInformation();
            base.OnMouseDown(e);
        }
    }
}