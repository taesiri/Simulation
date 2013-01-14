using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using FinalProject.SimulationElements.Enums;
using FinalProject.SimulationWorld;
using HelixToolkit.Wpf;

namespace FinalProject.SimulationElements
{
    public class ServicePlatformElement : UIElement3D
    {
        public BillboardTextVisual3D PlatformStatusText;
        public TranslateTransform3D Tranformer;
        private StationStatus _status = StationStatus.Empty;

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

        public string Name { get; set; }
        public ServiceBoxElement InnerElement { get; set; }

        public StationStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                PlatformStatusText.Text = value.ToString();
            }
        }


        public int TotalServiceTime { get; set; }

        public bool IsEmpty
        {
            get
            {
                if (ServiceBox != null)
                    return false;
                return true;
                //if (Status == StationStatus.Empty) return true;
                //return false;
            }
        }

        public bool IsBlockedOrAwaiting
        {
            get
            {
                if (Status == StationStatus.WaitingforRobot)
                    return true;
                if (Status == StationStatus.Blocked)
                    return true;
                if (Status == StationStatus.BlockedAndWaitingforRobot)
                    return true;
                return false;
            }
        }

        public ServiceBoxElement ServiceBox { get; set; }


        public void CreatePlatform(Point3D location, string textureUri)
        {
            Initializer();

            var geometryModel = new GeometryModel3D();

            var meshBuilder = new MeshBuilder();
            meshBuilder.AddCylinder(location, new Point3D(location.X, location.Y, location.Z + 5), 2.5, 180);
            meshBuilder.AddBox(new Point3D(location.X, location.Y, location.Z + 5), 5, 5, 0.5);
            //meshBuilder.AddBox(location, xLen, yLen, zLen);

            geometryModel.Geometry = meshBuilder.ToMesh();
            geometryModel.Material = MaterialHelper.CreateImageMaterial(textureUri);

            Visual3DModel = geometryModel;
        }

        public void Initializer()
        {
            PlatformStatusText = new BillboardTextVisual3D();
            PlatformStatusText.DepthOffset = 0.01;
            PlatformStatusText.Position = new Point3D(0, 0, 0);
            PlatformStatusText.Text = "Created!";
            World.Instance.Mother.Children.Add(PlatformStatusText);


            Status = StationStatus.Empty;
            InnerElement = null;
            TotalServiceTime = 0;
            Name = "NewPlatfrom";

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

        public override string ToString()
        {
            return Name;
        }
    }
}