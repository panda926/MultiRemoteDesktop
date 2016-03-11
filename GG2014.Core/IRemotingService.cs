using System;
using System.Collections.Generic;
using System.Text;
using ESBasic;
using JustLib.Records;

namespace GG2014
{
    /// <summary>
    /// 用于提供注册服务的Remoting接口。
    /// </summary>
    public interface IRemotingService :IChatRecordPersister
    {
        RegisterResult Register(User user); 

        /// <summary>
        /// 根据ID或Name搜索用户【完全匹配】。
        /// </summary>   
        List<User> SearchUser(string idOrName);


        RegisterResult Register(List<User> listUser);

        List<User> GetAllUser();

        List<User> GetAllUserPerManager(string strManagerID, string strPassword);

        void AddFriends(string ownerID, List<string> listFriendID, string catalogName);

        int AddFriend(string ownerID, string friendID, string friendPass, string catalogName);
    }

    public enum RegisterResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 0 ,

        /// <summary>
        /// 帐号已经存在
        /// </summary>
        Existed,

        /// <summary>
        /// 过程中出现错误
        /// </summary>
        Error
    }
}
