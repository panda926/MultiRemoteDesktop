using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Managers;
using ESPlus.Rapid;
using ESPlus.Serialization;
using ESBasic;
using System.IO;
using System.Threading;
using JustLib.Caches;
using JustLib;
using GG2014;


namespace MultiRemotePC
{
    public interface IUserNameGetter
    {
        string GetUserName(string userID);
    }

    public class GlobalUserCache : BaseGlobalUserCache<User, Group>, IUserNameGetter
    {
        private IRapidPassiveEngine rapidPassiveEngine;

        public GlobalUserCache(IRapidPassiveEngine engine)
        {
            this.rapidPassiveEngine = engine;
            string persistenceFilePath = SystemSettings.SystemSettingsDir + engine.CurrentUserID + ".dat";
            this.Initialize(this.rapidPassiveEngine.CurrentUserID, persistenceFilePath, GlobalConsts.CompanyGroupID, GlobalResourceManager.Logger);
        }

        public override void Initialize(string curUserID, string persistencePath, string _companyGroupID, ESBasic.Loggers.IAgileLogger _logger)
        {
            base.Initialize(curUserID, persistencePath, _companyGroupID, _logger);
        }

        protected override User DoGetUser(string userID)
        {
            byte[] bUser = this.rapidPassiveEngine.CustomizeOutter.Query(InformationTypes.GetUserInfo, System.Text.Encoding.UTF8.GetBytes(userID));
            if (bUser == null)
            {
                return null;
            }
            return ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<User>(bUser, 0);
        }

        protected override Group DoGetGroup(string groupID)
        {
            byte[] bGroup = this.rapidPassiveEngine.CustomizeOutter.Query(InformationTypes.GetGroup, System.Text.Encoding.UTF8.GetBytes(groupID));
            return CompactPropertySerializer.Default.Deserialize<Group>(bGroup, 0);
        }
        protected override List<Group> DoGetMyGroups()
        {
            byte[] bMyGroups = this.rapidPassiveEngine.CustomizeOutter.Query(InformationTypes.GetMyGroups, null);
            return CompactPropertySerializer.Default.Deserialize<List<Group>>(bMyGroups, 0);
        }
        protected override List<Group> DoGetSomeGroups(List<string> groupIDList)
        {
            byte[] bMyGroups = this.rapidPassiveEngine.CustomizeOutter.Query(InformationTypes.GetSomeGroups, CompactPropertySerializer.Default.Serialize(groupIDList));
            return CompactPropertySerializer.Default.Deserialize<List<Group>>(bMyGroups, 0);
        }
        protected override ContactRTDatas DoGetContactsRTDatas()
        {
            byte[] res = this.rapidPassiveEngine.CustomizeOutter.Query(InformationTypes.GetContactsRTData, null);
            return ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<ContactsRTDataContract>(res, 0);
        }
        protected override List<User> DoGetSomeUsers(List<string> userIDList)
        {
            byte[] bFriends = this.rapidPassiveEngine.CustomizeOutter.Query(InformationTypes.GetSomeUsers, CompactPropertySerializer.Default.Serialize(userIDList));
            return CompactPropertySerializer.Default.Deserialize<List<User>>(bFriends, 0);
        }

        protected override List<User> DoGetAllContacts() //好友，包括组友 
        {
            byte[] bFriends = this.rapidPassiveEngine.CustomizeOutter.Query(InformationTypes.GetAllContacts, null);
            return CompactPropertySerializer.Default.Deserialize<List<User>>(bFriends, 0);
        }

        public IUnit GetUnit(string id, bool isGroup)
        {
            if (isGroup)
            {
                return this.GetGroup(id);
            }

            return this.GetUser(id);
        }
    }
}
