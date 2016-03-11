using System;
using System.Collections.Generic;
using System.Text;

namespace JustLib.NetworkDisk
{
    /// <summary>
    /// 在网盘的环境中解析BeginSendFile方法的comment参数。
    /// </summary>
    public static class Comment4NDisk
    {
        private const string Prefix = "NDisk:";
        public static string ParseDirectoryPath(string comment)
        {
            if (comment == null || !comment.StartsWith(Comment4NDisk.Prefix))
            {
                return null;
            }

            return comment.Substring(Comment4NDisk.Prefix.Length);
        }

        public static string BuildComment(string directoryPath)
        {
            return Comment4NDisk.Prefix + directoryPath;
        }
    }
}
