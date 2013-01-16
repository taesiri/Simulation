﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using FinalProject.SimulationElements.Enums;
using FinalProject.SimulationWorld;
using HelixToolkit.Wpf;

namespace FinalProject.SimulationElements
{
    public class ServiceInspectorStation : UIElement3D
    {
        public BillboardTextVisual3D Inspector1StatusText;
        public BillboardTextVisual3D Inspector2StatusText;
        public BillboardTextVisual3D InspectorQStatusText;
        public TranslateTransform3D Tranformer;
        private WorkerStatus _inspector1Status = WorkerStatus.Idle;
        private WorkerStatus _inspector2Status = WorkerStatus.Idle;
        private Queue<ServiceBoxElement> _inspectorQueue;

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


        public WorkerStatus Inspector1Status
        {
            get { return _inspector1Status; }
            set
            {
                _inspector1Status = value;
                Inspector1StatusText.Text = "Worker 1 : " + value.ToString();
            }
        }

        public WorkerStatus Inspector2Status
        {
            get { return _inspector2Status; }
            set
            {
                _inspector2Status = value;
                Inspector2StatusText.Text = "Worker 2 : " + value.ToString();
            }
        }

        public int GetQueueLen
        {
            get { return _inspectorQueue.Count; }
        }

        public string ServiceProviderName { get; set; }

        public ServiceBoxElement Inspector1Box { get; set; }
        public ServiceBoxElement Inspector2Box { get; set; }

        public void PushBoxToQueue(ServiceBoxElement box)
        {
            _inspectorQueue.Enqueue(box);
            InspectorQStatusText.Text = "Queue len :" + _inspectorQueue.Count.ToString();
        }

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
            Inspector1StatusText = new BillboardTextVisual3D();
            Inspector1StatusText.DepthOffset = 0.01;
            Inspector1StatusText.Position = new Point3D(0, 0, 0);
            Inspector1StatusText.Text = "Created!";
            World.Instance.Mother.Children.Add(Inspector1StatusText);

            Inspector2StatusText = new BillboardTextVisual3D();
            Inspector2StatusText.DepthOffset = 0.01;
            Inspector2StatusText.Position = new Point3D(0, 0, 0);
            Inspector2StatusText.Text = "Created!";
            World.Instance.Mother.Children.Add(Inspector2StatusText);

            InspectorQStatusText = new BillboardTextVisual3D();
            InspectorQStatusText.DepthOffset = 0.01;
            InspectorQStatusText.Position = new Point3D(0, 0, 0);
            InspectorQStatusText.Text = "Created!";
            World.Instance.Mother.Children.Add(InspectorQStatusText);


            Tranformer = new TranslateTransform3D();
            Transform = Tranformer;

            _inspectorQueue = new Queue<ServiceBoxElement>();

            Inspector1Status = WorkerStatus.Idle;
            Inspector2Status = WorkerStatus.Idle;
            Inspector1Box = null;
            Inspector2Box = null;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }
    }
}