using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace System.IO
{
    public static class DirectoryExtensions
    {
        /// <summary>
        /// 验证目录是否存在，如果不存在则创建,返回是否已经存在该目录
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns>目录创建是否成功</returns>
        public static bool EnsureDirExists(this string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                // Create the directory it does not exist.
                Directory.CreateDirectory(dirPath);
                return false;
            }
            return true;
        }

        public static string ReplaceInvalidPathChar(this string path, char replace)
        {
            foreach (var c in Path.GetInvalidPathChars())
            {
                path=path.Replace(c, replace);
            }
            return path;
        }

        public static string ReplaceInvalidFileNameChar(this string path, char replace)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                path=path.Replace(c, replace);
            }
            return path;
        }
    }
}
