using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using FinalProject.SimulationElements.Enums;
using HelixToolkit.Wpf;

namespace FinalProject.SimulationElements
{
    public class ServiceInspectorStation : UIElement3D
    {
        public Queue<ServiceBoxElement> InspectorQueue;
        public TranslateTransform3D Tranformer;

        public ServiceInspectorStation()
        {
            // Default Box
            CreateStation(new Point3D(0, 0, 0), @"Textures\box1.jpg");
        }

        public ServiceInspectorStation(string textureUri)
        {
            CreateStation(new Point3D(0, 0, 0), textureUri);
        }

        public ServiceInspectorStation(Point3D location)
        {
            CreateStation(location, @"Textures\box1.jpg");
        }

        public ServiceInspectorStation(Point3D location, string textureUri)
        {
            CreateStation(location, textureUri);
        }

        public WorkerStatus Inspector1Status { get; set; }
        public WorkerStatus Inspector2Status { get; set; }

        public string ServiceProviderName { get; set; }

        public void CreateStation(Point3D location, string textureUri)
        {
            var geometryModel = new GeometryModel3D();

            var meshBuilder = new MeshBuilder();

            meshBuilder.AddBox(new Point3D(location.X - 2, location.Y - 3, location.Z), 4, 4, 0.35); //Inspector1
            meshBuilder.AddBox(new Point3D(location.X - 2, location.Y + 3, location.Z), 4, 4, 0.35); //Inspector2
            meshBuilder.AddBox(new Point3D(location.X - 2, location.Y + 20, location.Z), 5, 25, 0.35); // Queue

            geometryModel.Geometry = meshBuilder.ToMesh();
            //geometryModel.Material = MaterialHelper.CreateImageMaterial(textureUri);
            geometryModel.Material = Materials.Brown;

            Visual3DModel = geometryModel;


            Initializer();
        }

        private void Initializer()
        {
            Tranformer = new TranslateTransform3D();
            Transform = Tranformer;

            InspectorQueue = new Queue<ServiceBoxElement>();

            Inspector1Status = WorkerStatus.Idle;
            Inspector2Status = WorkerStatus.Idle;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }
    }
}