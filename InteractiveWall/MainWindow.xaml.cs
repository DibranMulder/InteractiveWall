using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InteractiveWall
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary> Active Kinect sensor </summary>
        private KinectSensor kinectSensor = null;

        /// <summary> Reader for body frames </summary>
        private BodyFrameReader bodyFrameReader = null;

        private Body[] bodies = null;

        private List<GestureDetector> gestureDetectorList = null;

        public MainWindow()
        {
            InitializeComponent();

            GeneralFrame.Content = new Movies();

            this.kinectSensor = KinectSensor.GetDefault();

            this.kinectSensor.Open();

            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            this.bodyFrameReader.FrameArrived += BodyFrameReaderOnFrameArrived;

            this.gestureDetectorList = new List<GestureDetector>();

            int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
            for (int i = 0; i < maxBodies; ++i)
            {
                GestureDetector detector = new GestureDetector(this.kinectSensor);
                gestureDetectorList.Add(detector);
                detector.GesturePerformed += DetectorOnGesturePerformed;
            }
        }

        private void BodyFrameReaderOnFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        // creates an array of 6 bodies, which is the max number of bodies that Kinect can track simultaneously
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                // we may have lost/acquired bodies, so update the corresponding gesture detectors
                if (this.bodies != null)
                {
                    // loop through all bodies to see if any of the gesture detectors need to be updated
                    int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
                    for (int i = 0; i < maxBodies; ++i)
                    {
                        Body body = this.bodies[i];
                        ulong trackingId = body.TrackingId;

                        // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                        if (trackingId != this.gestureDetectorList[i].TrackingId)
                        {
                            this.gestureDetectorList[i].TrackingId = trackingId;

                            // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                            // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                            this.gestureDetectorList[i].IsPaused = trackingId == 0;
                        }
                    }
                }
            }
        }

        private void DetectorOnGesturePerformed(object sender, GestureEventArgs e)
        {
            Console.WriteLine(e.GestureName);
            if (e.GestureName == "Open" && GeneralFrame.Content is Player)
            {
                GeneralFrame.GoBack();
            }
            else if(GeneralFrame.Content is ISwipeablePage)
            {
                ISwipeablePage page = GeneralFrame.Content as ISwipeablePage;
                switch (e.GestureName)
                {
                    case "swipe_Left":
                        page.SwipeLeft();
                        break;
                    case "swipe_Right":
                        page.SwipeRight();
                        break;
                    case "Open":
                        page.Open();
                        break;
                }
            }
        }
    }
}
