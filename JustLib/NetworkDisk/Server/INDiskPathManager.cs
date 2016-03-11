using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESPlus.FileTransceiver;

namespace JustLib.NetworkDisk.Server
{
    /// <summary>
    /// 网络硬盘的目录路径管理。可以按照计划的策略将不同用户的虚拟硬盘分散到不同的文件服务器上；也可以采用分布式文件存储系统，如FastDFS。
    /// </summary>
    public interface INDiskPathManager
    {
        /// <summary>
        /// GetNetworkDiskRootPath 获取名称为userID文件夹所在父目录的路径。返回null表示针对该用户没有网络硬盘服务。
        /// </summary>       
        string GetNetworkDiskRootPath(string userID);

        /// <summary>
        /// GetNetworkDiskTotalSize 获取指定用户的网络硬盘的总大小。
        /// </summary>        
        ulong GetNetworkDiskTotalSize(string userID);

        /// <summary>
        /// GetNetworkDiskSizeUsed 获取指定用户的网络硬盘已经使用的大小。
        /// </summary>        
        ulong GetNetworkDiskSizeUsed(string userID);
    }

    /// <summary>
    /// GG提供的INDiskPathManager接口的基础实现（简化版）。以某个文件目录作为整个网络硬盘的总目录。
    /// </summary>
    public class NetworkDiskPathManager : INDiskPathManager
    {
        public NetworkDiskPathManager() { }
        public NetworkDiskPathManager(ulong _totalSizeOfOneUser)
        {
            this.totalSizeOfOneUser = _totalSizeOfOneUser;            
        }
        public NetworkDiskPathManager(ulong _totalSizeOfOneUser, string _rootPath)
        {
            this.totalSizeOfOneUser = _totalSizeOfOneUser;
            this.rootPath = _rootPath;
        }

        #region RootPath
        private string rootPath = AppDomain.CurrentDomain.BaseDirectory + "\\NetworkDisk\\";
        /// <summary>
        /// RootPath 提供网络硬盘的根目录，可以为网络路径，如 \\192.168.0.11\FTP\
        /// </summary>
        public string RootPath
        {
            get { return rootPath; }
            set
            {
                rootPath = value;
                if (!rootPath.EndsWith("\\"))
                {
                    rootPath += "\\";
                }
            }
        }
        #endregion        

        #region TotalSizeOfOneUser
        private ulong totalSizeOfOneUser = 1024 * 1024 * 1024;
        /// <summary>
        /// 每个用户的空间大小
        /// </summary>
        public ulong TotalSizeOfOneUser
        {
            get { return totalSizeOfOneUser; }
            set { totalSizeOfOneUser = value; }
        } 
        #endregion

        #region INetworkDiskPathManager 成员

        public string GetNetworkDiskRootPath(string userID)
        {
            return this.rootPath;
        }

        public ulong GetNetworkDiskTotalSize(string userID)
        {
            return this.totalSizeOfOneUser;
        }        

        public ulong GetNetworkDiskSizeUsed(string userID)
        {
            ulong size = 0;
            try
            {
                string path = string.Format("{0}{1}\\", this.GetNetworkDiskRootPath(userID) , userID);
                this.GetDirectorySize(path, ref size);
            }
            catch{ }

            return size;
        }

        private void GetDirectorySize(string dirPath ,ref ulong size)
        {
            string[] entries = System.IO.Directory.GetFileSystemEntries(dirPath);
            foreach (string entry in entries)
            {
                if (Directory.Exists(entry))
                {
                    this.GetDirectorySize(entry, ref size);
                }
                else
                {
                    if (!entry.EndsWith(".tmpe$"))
                    {
                        FileInfo fileInfo = new FileInfo(entry);
                        size += (ulong)fileInfo.Length;
                    }
                }
            }
        }
        #endregion
    }
}
