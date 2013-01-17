using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace FinalProject.SimulationElements
{
    public class ServiceRobotElement : UIElement3D
    {
        public ServiceRobotElement()
        {   
            Initializer(new Point3D(0, 0, 0));
        }

        private void Initializer(Point3D location)
        {
            var geometryModel = new GeometryModel3D();

            var meshBuilder = new MeshBuilder();

            meshBuilder.AddBox(new Point3D(location.X, location.Y, location.Z + 3.5), 1.5, 1.5, 0.25);

            meshBuilder.AddBox(new Point3D(location.X, location.Y + 1, location.Z + 3.5), 0.25, 1.25, 0.25);
            meshBuilder.AddBox(new Point3D(location.X, location.Y - 1, location.Z + 3.5), 0.25, 1.25, 0.25);
            
            meshBuilder.AddBox(new Point3D(location.X + 1, location.Y , location.Z + 3.5), 1.25, 0.25, 0.25);
            meshBuilder.AddBox(new Point3D(location.X - 1, location.Y, location.Z + 3.5), 1.25, 0.25, 0.25);

            meshBuilder.AddBox(new Point3D(location.X, location.Y + 1.5, location.Z + 3), 0.25, 0.25, 0.75);
            meshBuilder.AddBox(new Point3D(location.X, location.Y - 1.5, location.Z + 3), 0.25, 0.25, 0.75);
            

            meshBuilder.AddBox(new Point3D(location.X + 1.5, location.Y, location.Z + 3), 0.25, 0.25, 0.75);
            meshBuilder.AddBox(new Point3D(location.X - 1.5, location.Y, location.Z + 3), 0.25, 0.25, 0.75);


            meshBuilder.AddBox(new Point3D(location.X, location.Y, location.Z + 4), 0.5, 0.5, 1);


            geometryModel.Geometry = meshBuilder.ToMesh();
            geometryModel.Material = Materials.Gold;

            Visual3DModel = geometryModel;
        }
    }
}