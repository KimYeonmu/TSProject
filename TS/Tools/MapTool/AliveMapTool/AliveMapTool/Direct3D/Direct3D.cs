using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace AliveMapTool
{
    class Direct3D
    {
        public static Device D3DDevice = null;

        public static bool CreateDevice(Control windowHandle, bool window)
        {
            PresentParameters pp = new PresentParameters();

            pp.Windowed                  = window;
            pp.SwapEffect                = SwapEffect.Discard;
            pp.EnableAutoDepthStencil    = true;
            pp.AutoDepthStencilFormat    = DepthFormat.D24X8;

            try
            {
                D3DDevice = new Device( 0,
                                        DeviceType.Hardware,
                                        windowHandle,
                                        CreateFlags.HardwareVertexProcessing,
                                        pp );
            }
            catch (DirectXException e1)
            {
                Debug.WriteLine(e1);

                try
                {
                    D3DDevice = new Device( 0,
                                            DeviceType.Hardware, 
                                            windowHandle,
                                            CreateFlags.SoftwareVertexProcessing,
                                            pp );
                }
                catch (DirectXException e2)
                {
                    Debug.WriteLine(e2);

                    try
                    {
                        D3DDevice = new Device(0,
                            DeviceType.Reference,
                            windowHandle,
                            CreateFlags.SoftwareVertexProcessing,
                            pp);

                    }
                    catch (DirectXException e3)
                    {
                        MessageBox.Show( e3.ToString(),
                                         "Error",
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Error );

                        return false;
                    }
                }
            }

            return true;
        }

        public static void Release()
        {
            if (D3DDevice != null)
            {
                D3DDevice.Dispose();
            }
        }
    }
}
