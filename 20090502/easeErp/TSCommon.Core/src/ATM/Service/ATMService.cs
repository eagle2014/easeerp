using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using Common.Logging;
using TSCommon.Core.ATM.Dao;
using System.Collections;

namespace TSCommon.Core.ATM.Service
{
    public class ATMService:BaseService<ATM.Domain.ATM>,IATMService
    {
        private static ILog logger = LogManager.GetLogger(typeof(ATMService));

        private ATMEvent atmEvent = new ATMEvent();
        private IATMDao atmDao;
        public IATMDao ATMDao
        {
            get { return atmDao; }
            set { atmDao = value; }
        }

        #region IATMService 成员

        public IList GetATM(string parentUnid)
        {
            return this.atmDao.GetATM(parentUnid);
        }

        public IList GetATM(string parentUnid, string type)
        {
            return this.atmDao.GetATM(parentUnid, type);
        }

        public void DeleteAll(IList<TSCommon.Core.ATM.Domain.ATM> list)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DeleteAll(string parentUnid)
        {
            this.DeleteAll(parentUnid, "");
        }

        public void DeleteAll(string parent, string type)
        {
            IList atmList = this.ATMDao.GetATM(parent, type);
            if (null == atmList || atmList.Count == 0)
                return;
            this.ATMDao.DeleteAll(atmList);
            Dictionary<string, string> atmData = new Dictionary<string, string>();
            for (int i = 0; i < atmList.Count; i++)
            {
                ATM.Domain.ATM atmEntry = (ATM.Domain.ATM)atmList[i];
                atmData.Add(i.ToString(), atmEntry.FileName);
            }
            atmEvent.EventAction(atmData, parent, "", "Delete");
        }

        public void Copy(string sourceParentUnid, string newParentUnid, string type)
        {
            this.CopyAll(sourceParentUnid, "All Of Attachment &!#@$@$@#@", newParentUnid, type);
        }

        public void CopyAll(string sourceParentUnid, string sourceType, string newParentUnid, string destType)
        {
            IList sourceatmList = new ArrayList();
            Dictionary<string, string> atmData = new Dictionary<string, string>();
            if (sourceType == null) sourceType = "";
            if (destType == null) destType = "";
            if (sourceType.Equals("All Of Attachment &!#@$@$@#@"))
                sourceatmList = this.GetATM(sourceParentUnid);
            else
                sourceatmList = this.GetATM(sourceParentUnid, sourceType);
            if (null == sourceatmList || sourceatmList.Count == 0)
                return;
            for (int i = 0; i < sourceatmList.Count; i++)
            {
                ATM.Domain.ATM attachment = (ATM.Domain.ATM)sourceatmList[i];
                atmData.Add(i.ToString(), attachment.FileName);
                ATM.Domain.ATM newAttachment = (ATM.Domain.ATM)attachment.Clone();

                newAttachment.ID = -1;
                newAttachment.Unid = System.Guid.NewGuid().ToString("N");
                newAttachment.ParentUnid = newParentUnid;
                newAttachment.Type = destType;
                this.Save(newAttachment);
            }
            IList newAttachmentList = this.GetATM(newParentUnid);
            atmEvent.EventAction(atmData, sourceParentUnid, newParentUnid, "Copy");

        }

        public void SwapParentUnid(string sourceParentUnid, string newParentUnid)
        {
            IList sourceAttachmentList = this.GetATM(sourceParentUnid);
            Dictionary<string, string> atmData = new Dictionary<string, string>();
            if (null == sourceAttachmentList || sourceAttachmentList.Count == 0)
                return;
            for (int i = 0; i < sourceAttachmentList.Count; i++)
            {
                ATM.Domain.ATM attachment = (ATM.Domain.ATM)sourceAttachmentList[i];
                atmData.Add(i.ToString(), attachment.FileName);
                attachment.ParentUnid = newParentUnid;
                this.Save(attachment);
            }
            atmEvent.EventAction(atmData, sourceParentUnid, newParentUnid, "Move");
        }

        public TSLib.PageInfo GetPage(int pageNo, int pageSize, string sortField, string sortDir, string parent, string type)
        {
            return this.atmDao.GetPage(pageNo, pageSize, sortField, sortDir, parent, type);
        }

        public override void Delete(long id)
        {
            ATM.Domain.ATM atmEntry = this.atmDao.Load(id);
            if (null == atmEntry)
                return;
            this.atmDao.Delete(atmEntry);
            Dictionary<string, string> atmData = new Dictionary<string, string>();
            atmData.Add(atmEntry.ParentUnid, atmEntry.FileName);
            atmEvent.EventAction(atmData, atmEntry.ParentUnid, "", "Delete");
        }
        #endregion
    }
}
