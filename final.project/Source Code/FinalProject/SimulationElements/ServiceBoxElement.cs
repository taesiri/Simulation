using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using FinalProject.SimulationElements.Dialog;
using HelixToolkit.Wpf;

namespace FinalProject.SimulationElements
{
    public class ServiceBoxElement : UIElement3D
    {
        public TranslateTransform3D Transformer;

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

        public BoxInformation BoxDetails { get; set; }

        public ServicePlatformElement CurrentServiceStation { get; set; } // Pretty Useless!

        public string Identifier { get; set; }


        public TimeSpan GetTotalServiceTime // Total Time of 'Service' (on Each Station)
        {
            get
            {
                return BoxDetails.StationAServiceDuration + BoxDetails.StationAServiceDuration +
                       BoxDetails.StationCServiceDuration;
            }
        }

        public string Name
        {
            get { return BoxDetails.Name; }
            set { BoxDetails.Name = value; }
        }

        public DateTime ArrivalTime
        {
            get { return BoxDetails.ArrivalTime; }
            set { BoxDetails.ArrivalTime = value; }
        }


        public void CreateBox(Point3D location, double xLen, double yLen, double zLen, string textureUri)
        {
            var geometryModel = new GeometryModel3D();

            var meshBuilder = new MeshBuilder();
            meshBuilder.AddBox(location, xLen, yLen, zLen);

            geometryModel.Geometry = meshBuilder.ToMesh();
            geometryModel.Material = MaterialHelper.CreateImageMaterial(textureUri);

            Visual3DModel = geometryModel;


            Initializer();
        }

        private void Initializer()
        {
            Transformer = new TranslateTransform3D();
            Transform = Transformer;
            BoxDetails = new BoxInformation();
        }


        public void ShowDetailedInformation()
        {
            var detailer = new BoxDetails(BoxDetails);
            detailer.Show();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ShowDetailedInformation();
            }
            base.OnMouseDown(e);
        }
    }
}