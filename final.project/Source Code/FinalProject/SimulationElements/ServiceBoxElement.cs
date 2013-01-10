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

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }
    }
}