using DevExpress.Images;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Dx.Dashboard.Core
{
    public static class DevExpressHelper
    {
        public static BitmapImage GetGlyphBitmapImage(String glyphName)
        {
            var resources = GetResourceNames();
            var result = resources.FirstOrDefault(x =>
            {
                var parts = x.Split(new[] { @"/", @"\" }, StringSplitOptions.RemoveEmptyEntries);
                var newImagePath = parts != null ? parts.Last() : null;
                return newImagePath.ToLower().Contains(glyphName.ToLower());
            });

            var uri = GetUri(ImageResourceCache.ImagesAssembly.FullName, result);
            return new BitmapImage(uri);
        }

        public static BitmapImage GetGlyphByUri(String path)
        {
            var uri = GetUri(ImageResourceCache.ImagesAssembly.FullName, path);
            return new BitmapImage(uri);
        }

        public static BitmapImage GetGlyph(String glyphName)
        {
            if (String.IsNullOrEmpty(glyphName)) return null;

            var resources = GetResourceNames();
            var result = resources.FirstOrDefault(x =>
            {
                var parts = x.Split(new[] { @"/", @"\" }, StringSplitOptions.RemoveEmptyEntries);
                var newImagePath = parts != null ? parts.Last() : null;
                return newImagePath.ToLower().Contains(glyphName.ToLower());
            });

            var uri = GetUri(ImageResourceCache.ImagesAssembly.FullName, result);
            return new BitmapImage(uri);
        }
        static Uri GetUri(string dllName, string relativeFilePath)
        {
            return new Uri(string.Format("/{0};component/{1}", dllName, relativeFilePath), UriKind.RelativeOrAbsolute);
        }
        public static string[] GetResourceNames()
        {
            var asm = ImageResourceCache.ImagesAssembly;
            var resName = asm.GetName().Name + ".g.resources";
            using (var stream = asm.GetManifestResourceStream(resName))
            using (var reader = new System.Resources.ResourceReader(stream))
            {
                return reader.Cast<DictionaryEntry>().Select(entry => (string)entry.Key).ToArray();
            }
        }
    }

}
