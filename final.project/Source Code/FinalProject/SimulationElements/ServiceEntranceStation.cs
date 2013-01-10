using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace FinalProject.SimulationElements
{
    public class ServiceEntranceStation : UIElement3D
    {
        public TranslateTransform3D Tranformer;


        public ServiceEntranceStation()
        {
            // Default Box
            CreateStation(new Point3D(0, 0, 0), @"Textures\boxDefault.jpg");
        }

        public ServiceEntranceStation(string textureUri)
        {
            CreateStation(new Point3D(0, 0, 0), textureUri);
        }

        public ServiceEntranceStation(Point3D location)
        {
            CreateStation(location, @"Textures\boxDefault.jpg");
        }

        public ServiceEntranceStation(Point3D location, string textureUri)
        {
            CreateStation(location, textureUri);
        }

        public void CreateStation(Point3D location, string textureUri)
        {
            var geometryModel = new GeometryModel3D();

            var meshBuilder = new MeshBuilder();
            //meshBuilder.AddCylinder(location, new Point3D(location.X, location.Y, location.Z + 5), 2.5, 180);
            meshBuilder.AddBox(new Point3D(location.X, location.Y, location.Z), 12, 4,0.75);
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
    }
}