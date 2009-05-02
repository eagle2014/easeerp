using System;
using System.Collections.Generic;
using System.Text;
using TSCommon.Core.ATM.Service;

namespace TSCommon.Core.ATM.Service
{
    public class ATMEvent
    {
        public event ATMHandler attachmentHandler;

        public ATMEvent()
        {
            //建立委托关系
            ATMHandlerAction atmHandlerAction = new ATMHandlerAction();
            this.attachmentHandler += new ATMHandler(atmHandlerAction.ActionAttachmentFile);
        }

        public void EventAction(Dictionary<string, string> attachmentData, string oldParentUnid, string newParentUnid, string proceAction)
        {
            ATMEventArgs e = new ATMEventArgs(attachmentData, oldParentUnid, newParentUnid, proceAction);
            OnEventAction(e);
        }

        protected virtual void OnEventAction(ATMEventArgs e)
        {
            if (attachmentHandler != null)
            {
                attachmentHandler(this, e);
            }
        }

        // 定义委托处理程序
        public delegate void ATMHandler(object sender, ATMEventArgs e);

    }
}
