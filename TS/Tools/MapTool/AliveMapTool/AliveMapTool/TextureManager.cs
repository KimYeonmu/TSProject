using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.DirectX.Direct3D;

namespace AliveMapTool
{
    class Texture2D
    {
        public Texture TextureImg;
        public ImageInformation ImageInfo;
        public string FileName;
    }

    class TextureManager
    {
        public static Dictionary<string, Texture2D> TextureMap = new Dictionary<string, Texture2D>();

        public static bool AddTexture(string strKey, string strFile)
        {
            for (int i = 0; i < TextureMap.Count; i++)
            {
                if (TextureMap.Values.ToList()[i].FileName.CompareTo(strFile) == 0)
                {
                    ErrorForm form = new ErrorForm();
                    form.ErrorTextLabel.Text = @"이미 추가 된 이미지 입니다.";
                    form.Location = UtilManager.GetFormCenter(form.Size);
                    form.Show();
                    return false;
                }
            }

            Texture2D tex  = new Texture2D();
            tex.ImageInfo = TextureLoader.ImageInformationFromFile(strFile);

            tex.TextureImg = TextureLoader.FromFile(
                Direct3D.D3DDevice,
                strFile,
                tex.ImageInfo.Width,
                tex.ImageInfo.Height,
                1,
                Usage.None,
                Format.A8B8G8R8, 
                Pool.Managed,
                Filter.Point, 
                Filter.Point,
                Color.Transparent.ToArgb());

            tex.FileName = strFile;

            try
            {
                TextureMap.Add(strKey, tex);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
    }
}
