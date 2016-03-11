using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESPlus.Application.FileTransfering.Server;
using ESBasic.Helpers;
using ESPlus.FileTransceiver;
using ESPlus.Application.FileTransfering;

namespace JustLib.NetworkDisk.Server
{
    /// <summary>
    /// 网络硬盘，为用户提供在线的网络硬盘服务。可通过INetworkDiskPathManager来将不同用户的目录分散到不同的文件服务器上。
    /// </summary>
    public class NetworkDisk 
    {  
        private INDiskPathManager networkDiskPathManager ;
        private IFileController fileController;
        public NetworkDisk(INDiskPathManager mgr , IFileController controller)
        {
            this.networkDiskPathManager = mgr;
            this.fileController = controller;
            this.fileController.FileRequestReceived += new CbFileRequestReceived(fileController_FileRequestReceived);
        }

        void fileController_FileRequestReceived(string fileID, string senderID, string fileName, ulong fileLength, ResumedProjectItem resumedFileItem, string comment)
        {
            string directoryPath = Comment4NDisk.ParseDirectoryPath(comment);
            if (directoryPath == null)
            {
                return;
            }

            if (resumedFileItem != null)
            {
                this.fileController.BeginReceiveFile(fileID, resumedFileItem.LocalSavePath); //续传
                return;
            }

            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(senderID);
            this.fileController.BeginReceiveFile(fileID, rootPath + directoryPath);
        }           

        #region Methods
        #region GetNetworkDiskRootPath
        public string GetNetworkDiskRootPath(string userID)
        {
            return this.networkDiskPathManager.GetNetworkDiskRootPath(userID);
        }
        #endregion

        #region GetNetworkDisk
        public SharedDirectory GetNetworkDisk(string userID, string dirPath)
        {
            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(userID);
            if (rootPath == null)
            {
                return null;
            }

            if (!Directory.Exists(rootPath + userID))
            {
                Directory.CreateDirectory(rootPath + userID);
            }

            if (dirPath == null)
            {
                SharedDirectory dir = new SharedDirectory();
                DiskDrive disk = new DiskDrive();
                disk.Name = userID;
                disk.TotalSize = this.networkDiskPathManager.GetNetworkDiskTotalSize(userID);
                disk.AvailableFreeSpace = disk.TotalSize - this.networkDiskPathManager.GetNetworkDiskSizeUsed(userID);

                dir.DriveList.Add(disk);
                return dir;
            }

            return SharedDirectory.GetSharedDirectory(rootPath + dirPath);
        }
        #endregion

        #region GetNetworkDiskState
        public NetworkDiskState GetNetworkDiskState(string userID)
        {
            ulong total = this.networkDiskPathManager.GetNetworkDiskTotalSize(userID);
            ulong used = this.networkDiskPathManager.GetNetworkDiskSizeUsed(userID);

            return new NetworkDiskState(total, used);
        }
        #endregion

        #region CreateDirectory
        public void CreateDirectory(string userID, string parentDirectoryPath, string newDirName)
        {
            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(userID);
            Directory.CreateDirectory(rootPath + parentDirectoryPath + newDirName);
        }
        #endregion

        #region DeleteFileOrDirectory
        public void DeleteFileOrDirectory(string userID, string sourceParentDirectoryPath, IList<string> filesBeDeleted, IList<string> directoriesBeDeleted)
        {
            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(userID);
            if (filesBeDeleted != null)
            {
                foreach (string fileName in filesBeDeleted)
                {
                    string filePath = rootPath + sourceParentDirectoryPath + fileName;
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            if (directoriesBeDeleted != null)
            {
                foreach (string dirName in directoriesBeDeleted)
                {
                    string dirPath = rootPath + sourceParentDirectoryPath + dirName + "\\";
                    if (Directory.Exists(dirPath))
                    {
                        FileHelper.DeleteDirectory(dirPath);
                    }
                }
            }
        }
        #endregion

        #region Rename
        public void Rename(string userID, string parentDirectoryPath, bool isFile, string oldName, string newName)
        {
            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(userID);
            if (isFile)
            {
                File.Move(rootPath + parentDirectoryPath + oldName, rootPath + parentDirectoryPath + newName);
            }
            else
            {
                Directory.Move(rootPath + parentDirectoryPath + oldName, rootPath + parentDirectoryPath + newName);
            }
        }
        #endregion

        #region Move
        public void Move(string userID, string oldParentDirectoryPath, IEnumerable<string> filesBeMoved, IEnumerable<string> directoriesBeMoved, string newParentDirectoryPath)
        {
            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(userID);
            FileHelper.Move(rootPath + oldParentDirectoryPath, filesBeMoved, directoriesBeMoved, rootPath + newParentDirectoryPath);
        }
        #endregion

        #region Copy
        public void Copy(string userID, string sourceParentDirectoryPath, IEnumerable<string> filesBeCopyed, IEnumerable<string> directoriesCopyed, string destParentDirectoryPath)
        {
            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(userID);
            FileHelper.Copy(rootPath + sourceParentDirectoryPath, filesBeCopyed, directoriesCopyed, rootPath + destParentDirectoryPath);
        }
        #endregion 
        #endregion       
    }
}
