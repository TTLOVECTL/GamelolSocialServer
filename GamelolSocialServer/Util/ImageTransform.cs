using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
namespace GamelolSocialServer.Util
{
    /// <summary>
    /// 图片转换工具类
    /// </summary>
    public class ImageTransform
    {
        /// <summary>
        /// 将图片转换为字节数组
        /// </summary>
        /// <param name="imagePhoto"></param>
        /// <returns></returns>
        public static byte[] GetPhotoImageByte(Image imagePhoto) {
            MemoryStream memoryStream = new MemoryStream();
            imagePhoto.Save(memoryStream,System.Drawing.Imaging.ImageFormat.Png);
            byte[] byData = new Byte[memoryStream.Length];
            memoryStream.Position= 0;
            memoryStream.Read(byData,0,byData.Length);
            memoryStream.Close();
            return byData;
        }

        /// <summary>
        /// 将字节数组转化为图片
        /// </summary>
        /// <param name="streamByte"></param>
        /// <returns></returns>
        public static Image TransformByteIntoImage(byte[] streamByte) {
            MemoryStream memoryStream = new MemoryStream(streamByte);
            return Image.FromStream(memoryStream);
        }



    }
}
