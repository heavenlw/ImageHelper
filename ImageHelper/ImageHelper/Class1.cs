using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageHelper
{
    public class Class1
    {
        class ImageHelper
        {
            /// 获取缩略图
            /// 获取缩略图
            public static Image GetThumbnailImage(Image image, int width, int height)
            {
                if (image == null || width < 1 || height < 1)
                    return null;
                // 新建一个bmp图片
                Image bitmap = new System.Drawing.Bitmap(width, height);

                // 新建一个画板
                using (Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                {

                    // 设置高质量插值法
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    // 设置高质量,低速度呈现平滑程度
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    // 高质量、低速度复合
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    // 清空画布并以透明背景色填充
                    g.Clear(Color.Transparent);

                    // 在指定位置并且按指定大小绘制原图片的指定部分
                    g.DrawImage(image, new Rectangle(0, 0, width, height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);
                    return bitmap;
                }
            }
            /// <summary>
            /// 生成缩略图，并保持纵横比
            /// </summary>
            /// <returns>生成缩略图后对象</returns>
            public static Image GetThumbnailImageKeepRatio(Image image, int width, int height)
            {
                Size imageSize = GetImageSize(image, width, height);
                return GetThumbnailImage(image, imageSize.Width, imageSize.Height);
            }

            /// <summary>
            /// 根据百分比获取图片的尺寸
            /// </summary>
            public static Size GetImageSize(Image picture, int percent)
            {
                if (picture == null || percent < 1)
                    return Size.Empty;

                int width = picture.Width * percent / 100;
                int height = picture.Height * percent / 100;

                return GetImageSize(picture, width, height);
            }
            /// <summary>
            /// 根据设定的大小返回图片的大小，考虑图片长宽的比例问题
            /// </summary>
            public static Size GetImageSize(Image picture, int width, int height)
            {
                if (picture == null || width < 1 || height < 1)
                    return Size.Empty;
                Size imageSize;
                imageSize = new Size(width, height);
                double heightRatio = (double)picture.Height / picture.Width;
                double widthRatio = (double)picture.Width / picture.Height;
                int desiredHeight = imageSize.Height;
                int desiredWidth = imageSize.Width;
                imageSize.Height = desiredHeight;
                if (widthRatio > 0)
                    imageSize.Width = Convert.ToInt32(imageSize.Height * widthRatio);
                if (imageSize.Width > desiredWidth)
                {
                    imageSize.Width = desiredWidth;
                    imageSize.Height = Convert.ToInt32(imageSize.Width * heightRatio);
                }
                return imageSize;
            }
            /// <summary>
            /// 获取图像编码解码器的所有相关信息
            /// </summary>
            /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
            /// <returns>返回图像编码解码器的所有相关信息</returns>
            public static ImageCodecInfo GetCodecInfo(string mimeType)
            {
                ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
                foreach (ImageCodecInfo ici in CodecInfo)
                {
                    if (ici.MimeType == mimeType) return ici;
                }
                return null;
            }
            public static ImageCodecInfo GetImageCodecInfo(ImageFormat format)
            {
                ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
                foreach (ImageCodecInfo icf in encoders)
                {
                    if (icf.FormatID == format.Guid)
                    {
                        return icf;
                    }
                }
                return null;
            }
            public static void SaveImage(Image image, string savePath, ImageFormat format)
            {
                SaveImage(image, savePath, GetImageCodecInfo(format));
            }
            /// <summary>
            /// 高质量保存图片
            /// </summary>
            private static void SaveImage(Image image, string savePath, ImageCodecInfo ici)
            {
                // 设置 原图片 对象的 EncoderParameters 对象
                EncoderParameters parms = new EncoderParameters(1);
                EncoderParameter parm = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)95));
                parms.Param[0] = parm;
                image.Save(savePath, ici, parms);
                parms.Dispose();
            }

        }
    }
}
