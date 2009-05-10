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
            //����ί�й�ϵ
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

        // ����ί�д������
        public delegate void ATMHandler(object sender, ATMEventArgs e);

    }
}
