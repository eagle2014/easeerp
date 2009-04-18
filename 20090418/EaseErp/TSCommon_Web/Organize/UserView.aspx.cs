using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TSCommon_Core.Organize.Service;
using TSLibWeb;
using TSCommon_Core.TSWebContext;
using TSLib;
using log4net;
using TSCommon_Core.Organize.Domain;
using TSLibWeb.WEB;

namespace TSCommon_Web.Organize
{
    public partial class UserView : StrutsCorePage
    {
        public static ILog log = LogManager.GetLogger(typeof(UserView)); 
        private IJobTitleService jobTitleService;   // ְ�����õ�Service

        public UserView()
        {
            this.jobTitleService = (IJobTitleService)GetObject("JobTitleService");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindDropDownList();
        }

        protected void BindDropDownList()
        {
            // ��ְ���б�
            IList list = this.jobTitleService.FindAll();

            //��ӿհ�ѡ��
            JobTitle jobTitle = new JobTitle();
            jobTitle.ID = Constants.BLANK_INT_VALUE;
            jobTitle.Unid = "";
            jobTitle.Name = SimpleResourceHelper.GetString("SELECT.EMPTY.LABEL.ANY");
            list.Insert(0, jobTitle);

            this.JobTitleUnid.DataSource = list;
            this.JobTitleUnid.DataTextField = "Name";
            this.JobTitleUnid.DataValueField = "Unid";
            this.JobTitleUnid.DataBind();
        }

        /// <summary>
        /// �жϵ�ǰ�û��Ƿ�Ϊ����Ա
        /// </summary>
        public bool IsManager
        {
            get
            {
                return TSWEBContext.Current.IsHasPrivilege(SimpleResourceHelper.GetString("PRIVILEDGE.CODE.ADMIN_ALL"));
            }
        }
    }
}
