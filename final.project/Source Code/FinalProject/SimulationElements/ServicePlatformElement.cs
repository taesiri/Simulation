using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using FinalProject.SimulationElements.Enums;
using HelixToolkit.Wpf;

namespace FinalProject.SimulationElements
{
    public class ServicePlatformElement : UIElement3D
    {
        public TranslateTransform3D Tranformer;

        public ServicePlatformElement()
        {
            // Default Box

            CreatePlatform(new Point3D(0, 0, 0), @"Textures\boxDefault.jpg");
        }

        public ServicePlatformElement(string textureUri)
        {
            CreatePlatform(new Point3D(0, 0, 0), textureUri);
        }

        public ServicePlatformElement(Point3D location)
        {
            CreatePlatform(location, @"Textures\boxDefault.jpg");
        }


        public ServicePlatformElement(Point3D location, string textureUri)
        {
            CreatePlatform(location, textureUri);
        }

        public ServiceBoxElement InnerElement { get; set; }
        public StationStatus Status { get; set; }
        public int TotalServiceTime { get; set; }


        public void CreatePlatform(Point3D location, string textureUri)
        {
            var geometryModel = new GeometryModel3D();

            var meshBuilder = new MeshBuilder();
            meshBuilder.AddCylinder(location, new Point3D(location.X, location.Y, location.Z + 5), 2.5, 180);
            meshBuilder.AddBox(new Point3D(location.X, location.Y, location.Z + 5), 5, 5, 0.5);
            //meshBuilder.AddBox(location, xLen, yLen, zLen);

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

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        public void MoveInnerBoxToNextSpot()
        {
            // THIS PLACE is a 'Place Holder'! - Actual transformation will not done here! robot will handle all movements!
        }
    }
}