using System;
using ESBasic;
using ESPlus.FileTransceiver;

namespace JustLib.Controls
{
    /// <summary>
    /// 文件传送查看器。用于查看与某个好友的所有文件传送的进度状态信息。
    /// 注意，FileTransferingViewer的所有事件都是在UI线程中触发的。
    /// </summary>
    public interface IFileTransferingViewer
    {
        /// <summary>
        /// 当某个文件开始续传时，触发该事件。参数为FileName - isSending
        /// </summary>
        event CbGeneric<string, bool> FileResumedTransStarted;

        /// <summary>
        /// 当某个文件传送完毕时，触发该事件。参数为FileName - isSending - comment - isFolder
        /// </summary>
        event CbGeneric<string, bool, string, bool> FileTransCompleted;

        /// <summary>
        /// 当某个文件传送中断时，触发该事件。参数为FileName - isSending - FileTransDisrupttedType
        /// </summary>
        event CbGeneric<string, bool, FileTransDisrupttedType> FileTransDisruptted;

        /// <summary>
        /// 当某个文件传送开始时，触发该事件。参数为FileName - isSending
        /// </summary>
        event CbGeneric<string, bool> FileTransStarted;

        /// <summary>
        /// 当所有文件都传送完成时，触发该事件。
        /// </summary>
        event CbSimple AllTaskFinished;      

        /// <summary>
        /// 当前是否有文件正在传送中。
        /// </summary>        
        bool IsFileTransfering { get; }
    }
}
