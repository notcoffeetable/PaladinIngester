using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPLATFORMLib;

namespace PaladinIngester
{
    public partial class Form1 : Form
    {
        private MLiveClass _cam1;
        private MLiveClass _cam2;
        private MMixerClass _Mixer;
        private MItem pCam1;
        private MItem pCam2;
        //private MRenderer _RendererClass;


        MElement pEditElement;
        RectangleF rcOriginal;
        PointF ptOriginal;
        bool isResizingHor;
        bool isResizingVert;
        bool isTopBound;
        bool isLeftBound;
        bool isMoving;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _cam1 = new MLiveClass();
                _cam2 = new MLiveClass();
                _Mixer = new MMixerClass();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't crate MFile instance: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            //FillCombo("video", videoCombo);
            string cam1Name;
            string cam2Name;
            string camdesc;

            _cam1.DeviceGetByIndex(0, "video", 0, out cam1Name, out camdesc);
            _cam1.DeviceSet("video", cam1Name, "");
            _cam1.ObjectVirtualSourceCreate(1, "Cam1 Virtual", "");
            //_cam1.PreviewWindowSet("", previewPanel.Handle.ToInt32());
            //_cam1.PreviewEnable("", 0, 0);
            cam1.Text = cam1Name;
            _cam1.ObjectStart(null);

            _cam2.DeviceGetByIndex(0, "video", 2, out cam2Name, out camdesc);
            _cam2.DeviceSet("video", cam2Name, "");
            _cam2.ObjectVirtualSourceCreate(1, "cam2 virtual", "");
            //_cam2.PreviewWindowSet("", previewPanel.Handle.ToInt32());
            //_cam2.PreviewEnable("", 0, 1);
            cam2.Text = cam2Name;
            _cam2.ObjectStart(null);

            _Mixer.PreviewWindowSet("", OutputPanel.Handle.ToInt32());
            _Mixer.PreviewEnable("", 0, 1);
            _Mixer.ObjectStart(null);
            _Mixer.FilePlayStart();
            _Mixer.ObjectVirtualSourceCreate(1, "Program Out", "");
        }

        ///// <summary>
        ///// Fill combo boxes (Audio/Video device and Audio/Video input line (if avilable))
        ///// </summary>
        ///// <param name="pDevice"></param>
        ///// <param name="strType"></param>
        ///// <param name="cbxType"></param>
        //private void FillCombo(string strType, ComboBox cbxType)
        //{
        //    cbxType.Items.Clear();
        //    cbxType.Tag = strType;
        //    int nCount = 0;
        //    //Get device count / input line count
        //    _Live.DeviceGetCount(0, strType, out nCount);
        //    cbxType.Enabled = nCount > 0;
        //    if (nCount > 0)
        //    {
        //        for (int i = 0; i < nCount; i++)
        //        {
        //            string strName;
        //            string strDesc;
        //            //Get deveice / input line
        //            _Live.DeviceGetByIndex(0, strType, i, out strName, out strDesc);
        //            cbxType.Items.Add(strName);
        //        }
        //        string strCur = "";
        //        string strParam = "";
        //        int nIndex = 0;
        //        try
        //        {
        //            //Check if there is already selected device / input line
        //            _Live.DeviceGet(strType, out strCur, out strParam, out nIndex);
        //            if (strCur != "")
        //            {
        //                cbxType.SelectedIndex = cbxType.FindStringExact(strCur);
        //            }
        //            else cbxType.SelectedIndex = 0;
        //        }
        //        catch
        //        {
        //            cbxType.SelectedIndex = 0;
        //        }
        //    }
        //}

        private void videoCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbxChanged = (ComboBox)sender;
            MLiveClass newDevice = new MLiveClass();

            string strType = (string)cbxChanged.Tag;
            try
            {
                //Set device
                newDevice.DeviceSet(strType, (string)cbxChanged.SelectedItem, "");
            }
            catch
            {
                MessageBox.Show("Can't set this device, it isn't supported", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            //_Mixer.StreamsRemove(pCurrentDevice, 0);
            //_Mixer.StreamsAdd("", newDevice, "live_src", "", out pCurrentDevice, 0);

        }

        private void buttonInit_Click(object sender, EventArgs e)
        {
            //_Live.ObjectStart(null);
        }

        private void cam1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            if (box.Checked)
            {
                //_cam1.ObjectStart(null);
                _Mixer.StreamsAdd("", _cam1, "live_src", "", out pCam1, 50);
            }
            else
            {
                _Mixer.StreamsRemove(pCam1, 2);
            }
        }

        private void cam2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            if (box.Checked)
            {
                //_cam1.ObjectStart(null);
                _Mixer.StreamsAdd("", _cam2, "live_src", "", out pCam2, 50);
            }
            else
            {
                _Mixer.StreamsRemove(pCam2, 2);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (openMediaFile.ShowDialog() == DialogResult.OK && openMediaFile.FileNames.Length != 0)
            {
                MItem pItem;
                for (int i = 0; i < openMediaFile.FileNames.Length; i++)
                {
                    _Mixer.StreamsAdd("", null, openMediaFile.FileNames[i], "", out pItem, 1.00);
                    _Mixer.FilePlayStart();
                }
            } 
        }

        private PointF PointToRelative(Point ptPos)
        {
            PointF ptRes = new PointF((float)ptPos.X / OutputPanel.Width, (float)ptPos.Y / OutputPanel.Height);
            return ptRes;
        }


        private void OutputPanel_MouseDown(object sender, MouseEventArgs e)
        {
            double XMouse;
            double YMouse;
            XMouse = (PointToRelative(e.Location)).X;
            YMouse = (PointToRelative(e.Location)).Y;

            GetNewXY(ref XMouse, ref YMouse);

            // Select element in element tree
            MElement pElement = null;
            _Mixer.ScenesElementGetByPos(XMouse, YMouse, 0, out pElement);
            if (pElement != null)
            {
                Console.WriteLine("pElement not null");
                pEditElement = pElement;
                try
                {
                    // Could be exception - if scene not rendred yet
                    double x, y, w, h;
                    pEditElement.ElementAbsolutePosGet(out x, out y, out w, out h);
                    rcOriginal.X = (float)x;
                    rcOriginal.Y = (float)y;
                    rcOriginal.Width = (float)w;
                    rcOriginal.Height = (float)h;
                    ptOriginal = new PointF((float)XMouse, (float)YMouse); ;

                    double deltax = 0.04;
                    double deltay = 0.04;
                    if (XMouse > x && XMouse < x + deltax)
                    {
                        _Mixer.PreviewSetCursor("", eMCursorType.eMCT_SIZEWE);
                        isResizingHor = true;
                        isLeftBound = true;
                    }
                    else if (XMouse < x + w && XMouse > x + w - deltax)
                    {
                        _Mixer.PreviewSetCursor("", eMCursorType.eMCT_SIZEWE);
                        isResizingHor = true;
                    }
                    else if (YMouse > y && YMouse < y + deltay)
                    {
                        _Mixer.PreviewSetCursor("", eMCursorType.eMCT_SIZENS);
                        isResizingVert = true;
                        isTopBound = true;
                    }
                    else if (YMouse < y + h && YMouse > y + h - deltay)
                    {
                        _Mixer.PreviewSetCursor("", eMCursorType.eMCT_SIZENS);
                        isResizingVert = true;
                    }
                    else
                    {
                        _Mixer.PreviewSetCursor("", eMCursorType.eMCT_SIZEALL);
                        isMoving = true;
                    }
                    // Make element topmost
                    pEditElement.ElementReorder(10000);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("Exception: {0}", ex);
                }

            }

        }

        private void GetNewXY(ref double x, ref double y)
        {
            IMFormat pFormat = _Mixer;
            M_VID_PROPS vProps;
            int nIndex;
            string strNmae;
            pFormat.FormatVideoGet(eMFormatType.eMFT_Output, out vProps, out nIndex, out strNmae);
            double videoAspectRatio = (double)vProps.nAspectX / (double)vProps.nAspectY;
            if (vProps.nAspectX == 0 && vProps.nAspectY == 0)
            {
                videoAspectRatio = (double)vProps.nWidth / (double)vProps.nHeight;
            }

            double previewAspectRatio = (double)OutputPanel.Width / (double)OutputPanel.Height;
            double kX = 0;
            double kY = 0;

            if (videoAspectRatio < previewAspectRatio)
            {
                kX = (videoAspectRatio * (double)OutputPanel.Height) / (double)OutputPanel.Width;
                x = (x - 0.5) / kX + 0.5;
            }
            else
            {
                kY = (double)OutputPanel.Width / (videoAspectRatio * (double)OutputPanel.Height);
                y = (y - 0.5) / kY + 0.5;
            }
        }

        private void OutputPanel_MouseUp(object sender, MouseEventArgs e)
        {
            // End element move/resize
            _Mixer.PreviewSetCursor("", eMCursorType.eMCT_ARROW);
            pEditElement = null;
            isMoving = false;
            isResizingHor = false;
            isResizingVert = false;
            isLeftBound = false;
            isTopBound = false;
            //mElementsTree.UpdateTree(true);
        }

        private void OutputPanel_MouseMove(object sender, MouseEventArgs e)
        {
            float XMouse = (PointToRelative(e.Location)).X;
            float YMouse = (PointToRelative(e.Location)).Y;
            if (pEditElement != null)
            {
                RectangleF rcNew = rcOriginal;
                if (isResizingHor)
                {
                    _Mixer.PreviewSetCursor("", eMCursorType.eMCT_SIZEWE);

                    if (isLeftBound)
                    {
                        rcNew.X += XMouse - ptOriginal.X;
                        rcNew.Width -= XMouse - ptOriginal.X;
                    }
                    else
                        rcNew.Width += XMouse - ptOriginal.X;

                }
                if (isResizingVert)
                {
                    _Mixer.PreviewSetCursor("", eMCursorType.eMCT_SIZENS);
                    if (isTopBound)
                    {
                        rcNew.Y += YMouse - ptOriginal.Y;
                        rcNew.Height -= YMouse - ptOriginal.Y;
                    }
                    else
                        rcNew.Height += YMouse - ptOriginal.Y;

                }
                else if (isMoving)
                {
                    _Mixer.PreviewSetCursor("", eMCursorType.eMCT_SIZEALL);
                    rcNew.X += XMouse - ptOriginal.X;
                    rcNew.Y += YMouse - ptOriginal.Y;
                }
                pEditElement.ElementAbsolutePosSet(rcNew.X, rcNew.Y, rcNew.Width, rcNew.Height);
            }

        }
    }
}
