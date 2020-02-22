//------------------------------------------------------------------------------
// <copyright file="ImageExtensions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Buffers;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace K4AdotNet.Sensor
{
    /// <summary>
    /// Extends the <see cref="Image"/> providing a way to get a WinForms <see cref="Bitmap"/>
    /// object from the <see cref="Image"/>.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Creates a WinForms <see cref="Bitmap"/> from the <see cref="Image"/>.
        /// </summary>
        /// <param name="image">The <see cref="Image"/> to convert into a <see cref="Bitmap"/>.</param>
        /// <returns>A <see cref="Bitmap"/> that references the data in <see cref="Image"/>.</returns>
        public static System.Drawing.Image CreateBitmap(this Image image)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            System.Drawing.Imaging.PixelFormat pixelFormat;

            // Take a new reference on the image to ensure that the object
            // cannot be disposed by another thread while we have a copy of its Buffer
            using (Image reference = image.DuplicateReference())
            {
                unsafe
                {
                    switch (reference.Format)
                    {
                        case ImageFormat.ColorBgra32:
                            pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                            break;
                        case ImageFormat.Depth16:
                        case ImageFormat.IR16:
                            pixelFormat = System.Drawing.Imaging.PixelFormat.Format16bppGrayScale;
                            break;
                        case ImageFormat.ColorMjpg:
                            Byte[] buffer = new byte[image.SizeBytes];
                            System.Runtime.InteropServices.Marshal.Copy(image.Buffer, buffer, 0, image.SizeBytes);
                            JpegBitmapDecoder decoder = new JpegBitmapDecoder(new MemoryStream(buffer), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                            BitmapFrame frame = decoder.Frames[0];
                            Bitmap lbmpBitmap = new Bitmap(frame.PixelWidth, frame.PixelHeight);
                            Rectangle lrRect = new Rectangle(0, 0, lbmpBitmap.Width, lbmpBitmap.Height);
                            BitmapData lbdData = lbmpBitmap.LockBits(lrRect, ImageLockMode.WriteOnly, (frame.Format.BitsPerPixel == 24 ? PixelFormat.Format24bppRgb : PixelFormat.Format32bppArgb));
                            frame.CopyPixels(System.Windows.Int32Rect.Empty, lbdData.Scan0, lbdData.Height * lbdData.Stride, lbdData.Stride);
                            lbmpBitmap.UnlockBits(lbdData);
                            return lbmpBitmap;

                        // pixelFormat = System.Drawing.Imaging.PixelFormat.;
                        // MjpegProcessor.MjpegDecoder()
                        //break;
                        default:
                            throw new Exception($"Pixel format {reference.Format} cannot be converted to a BitmapSource");
                    }

                    using (image)
                    {
                        return new Bitmap(
                            image.WidthPixels,
                            image.HeightPixels,
                            image.StrideBytes,
                            pixelFormat,
                            image.Buffer);
                    }
                }
            }
        }
    }
}
