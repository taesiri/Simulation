using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using FinalProject.SimulationWorld;
using HelixToolkit.Wpf;

namespace FinalProject.SimulationElements
{
    public class ServiceEntranceStation : UIElement3D
    {
        public Queue<ServiceBoxElement> EntranceQueue;
        public TranslateTransform3D Tranformer;


        public ServiceEntranceStation()
        {
            // Default Box
            CreateStation(new Point3D(0, 0, 0), @"Textures\box2.jpg");
        }

        public ServiceEntranceStation(string textureUri)
        {
            CreateStation(new Point3D(0, 0, 0), textureUri);
        }

        public ServiceEntranceStation(Point3D location)
        {
            CreateStation(location, @"Textures\box2.jpg");
        }

        public ServiceEntranceStation(Point3D location, string textureUri)
        {
            CreateStation(location, textureUri);
        }

        public int GetQueueLen
        {
            get { return EntranceQueue.Count; }
        }

        public string Name { get; set; }

        public void CreateStation(Point3D location, string textureUri)
        {
            var geometryModel = new GeometryModel3D();

            var meshBuilder = new MeshBuilder();

            meshBuilder.AddBox(new Point3D(location.X, location.Y, location.Z), 18, 4, 0.35);

            geometryModel.Geometry = meshBuilder.ToMesh();
            //geometryModel.Material = MaterialHelper.CreateImageMaterial(textureUri);

            geometryModel.Material = Materials.Blue;
            Visual3DModel = geometryModel;


            LateInitializer();
        }

        private void LateInitializer()
        {
            EntranceQueue = new Queue<ServiceBoxElement>();

            Tranformer = new TranslateTransform3D();
            Transform = Tranformer;

            Name = "NewEntrancePlatfrom";
        }


        public void AddNewBox(ServiceBoxElement box)
        {
            // TODO: Calculate the Position and Render the BOX! 

            int n = EntranceQueue.Count;
            box.Transform = new TranslateTransform3D(-15 - (n*5), 0, 2.1f);
            box.Transformer = new TranslateTransform3D(-15 - (n*5), 0, 2.1f);

            World.Instance.Mother.Children.Add(box);

            EntranceQueue.Enqueue(box);

            //reArrenging Box!
            int counter = 1;
            foreach (ServiceBoxElement boxe in EntranceQueue)
            {
                var tr = new TranslateTransform3D(-15 - (counter * 5), 0, 2.1f);
                boxe.Transform = tr;
                boxe.Transformer = tr;
                counter++;
            }

        }

        public ServiceBoxElement Dequeue()
        {
            ServiceBoxElement item = EntranceQueue.Dequeue();

            //reArrenging Box!
            int counter = 1;
            foreach (ServiceBoxElement box in EntranceQueue)
            {
                var tr = new TranslateTransform3D(-15 - (counter*5), 0, 2.1f);
                box.Transform = tr;
                box.Transformer = tr;
                counter++;
            }

            return item;
        }


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }
    }
}