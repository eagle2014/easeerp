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
using TSLibWeb.WEB;
using TSCommon.Core.Organize.Domain;
using TSCommon.Core.Organize.Service;
using log4net;
using TSCommon.Core.TSWebContext;
using TSLib;
using TSLibWeb;
using TSLibStruts.Utils;

namespace TSCommon.Web.Organize
{
    public partial class UserForm :StrutsFormPage<User>
    {
        public static ILog log = LogManager.GetLogger(typeof(UserForm));
        protected bool isActive;
        protected bool isNew;
        private bool isUnidEmpty;

  

        #region ���Service��������

        private IGroupService groupService;         // ��֯���õ�Service
        private IUserService userInfoService;   // ��Ա���õ�Service
        private IJobTitleService jobTitleService;   // ְ�����õ�Service
        private User user;                  // ��Ա����

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        #endregion

        #region ���캯��

        static UserForm()
        {
            if (ConvertUtils.Lookup(typeof(UserStatuses)) == null)
                ConvertUtils.Register(new UserStatusConverter(), typeof(UserStatuses));
        }

        public UserForm()
        {
            this.groupService = (IGroupService)GetObject("GroupService");
            this.userInfoService = (IUserService)GetObject("UserService");
            this.jobTitleService = (IJobTitleService)GetObject("JobTitleService");
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            BindDropDownList();
        }


        protected bool IsUnidEmpty
        {
            get
            {
                isUnidEmpty = false;
                if (string.IsNullOrEmpty(Domain.OUFullName))
                {
                    isUnidEmpty = true;
                }
                return isUnidEmpty;
            }
        }


        /// <summary>
        /// �жϵ�ǰ�û��Ƿ�Ϊ��֯�ṹ����Ա
        /// </summary>
        public bool IsManager
        {
            get
            {
                TSWEBContext tsContext = TSWEBContext.Current;
                User curUser = tsContext.CurUser;
                if (tsContext.IsHasPrivilege(SimpleResourceHelper.GetString("PRIVILEDGE.CODE.ADMIN_ALL"))
                    || tsContext.IsHasPrivilege(SimpleResourceHelper.GetString("PRIVILEDGE.CODE.ADMIN_LOCAL_AND_CHILD"))
                    || tsContext.IsHasPrivilege(SimpleResourceHelper.GetString("PRIVILEDGE.CODE.ADMIN_LOCAL")))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// ��ȡ������ҳ��ı༭״̬
        /// </summary>
        public override bool CanEdit
        {
            get
            {
                return base.CanEdit;
            }
            set
            {
                base.CanEdit = value;
                bool isReadonly = !value;
                this.Name.ReadOnly = isReadonly;
                this.LoginID.ReadOnly = isReadonly;
                this.Email.ReadOnly = isReadonly;
                this.TelephoneNo.ReadOnly = isReadonly;
                this.Mobile.ReadOnly = isReadonly;
                this.OrderNo.ReadOnly = isReadonly;
                this.CardID.ReadOnly = isReadonly;
                this.Office.ReadOnly = isReadonly;
                this.Address.ReadOnly = isReadonly;
                this.JobTitleUnid.Enabled = !isReadonly;
                this.ImgBtnSelectOU.Visible = !isReadonly;
                this.btnLeftAllArrow.Visible = !isReadonly;
                this.btnLeftArrow.Visible = !isReadonly;
                this.btnRightArrow.Visible = !isReadonly;
                this.UserType.Enabled = value;

                if (isReadonly)
                {
                    this.Name.CssClass = this.ZdField;
                    this.LoginID.CssClass = this.ZdField;
                    this.Email.CssClass = this.ZdField;
                    this.TelephoneNo.CssClass = this.ZdField;
                    this.Mobile.CssClass = this.ZdField;
                    this.OrderNo.CssClass = this.ZdField;
                    this.CardID.CssClass = this.ZdField;
                    this.Office.CssClass = this.ZdField;
                    this.Address.CssClass = this.ZdField;
                    this.JobTitleUnid.CssClass = this.ZdField;
                    this.GroupUnids.CssClass = this.ZdField;
                    this.AllGroups.CssClass = this.ZdField;
                }
                else
                {
                    this.Name.CssClass = this.BtField;
                    this.LoginID.CssClass = this.BtField;
                    this.Email.CssClass = "egd-form-field";
                    this.TelephoneNo.CssClass = "egd-form-field";
                    this.Mobile.CssClass = "egd-form-field";
                    this.OrderNo.CssClass = this.BtField;
                    this.CardID.CssClass = "egd-form-field";
                    this.Office.CssClass = "egd-form-field";
                    this.Address.CssClass = "egd-form-field";
                    this.JobTitleUnid.CssClass = this.BtField;
                    this.GroupUnids.CssClass = "egd-form-field";
                    this.AllGroups.CssClass = "egd-form-field";
                }
            }
        }

        #region �������

        private void BindDropDownList()
        {
            // ��ְ���б�
            IList list = this.jobTitleService.FindAll();

            //��ӿհ�ѡ��
            JobTitle jobTitle = new JobTitle();
            jobTitle.ID = TSLibWeb.Constants.BLANK_INT_VALUE;
            jobTitle.Unid = "";
            jobTitle.Name = SimpleResourceHelper.GetString("SELECT.EMPTY.LABEL");
            list.Insert(0, jobTitle);

            // ����ְ��ɾ�����������
            bool isIn = false;
            User user = this.Domain;
            log.Debug("userInfo.JobTitleName=" + user.JobTitleName);
            log.Debug("userInfo.JobTitleUnid=" + (user.JobTitleUnid != null ? user.JobTitleUnid : "null"));
            foreach (JobTitle item in list)
            {
                if (item.Unid == user.JobTitleUnid)
                {
                    isIn = true;
                    break;
                }
            }
            if (!isIn)
            {
                // ����һ���Ѷ�ʧ��ѡ��
                jobTitle = new JobTitle();
                jobTitle.ID = TSLibWeb.Constants.BLANK_INT_VALUE;
                jobTitle.Unid = user.JobTitleUnid;
                jobTitle.Name = user.JobTitleName + "[�ѱ�ɾ����ְ��]";
                list.Add(jobTitle);
            }
            this.JobTitleUnid.DataSource = list;
            this.JobTitleUnid.DataTextField = "Name";
            this.JobTitleUnid.DataValueField = "Unid";
            this.JobTitleUnid.DataBind();

            // ����ӵ�еĸ�λ�б�
            this.GroupUnids.DataSource = user.GroupLists;
            this.GroupUnids.DataTextField = "Name";
            this.GroupUnids.DataValueField = "Unid";
            this.GroupUnids.DataBind();

            // ����δӵ�еĸ�λ�б�
            if (!string.IsNullOrEmpty(user.OUUnid))
            {
                list = this.groupService.FindByOU(user.OUUnid, true);
                if (null != list && list.Count > 0)
                {
                    ArrayList allGroups = new ArrayList();
                    if (null == user.GroupLists || user.GroupLists.Count == 0)
                    {
                        allGroups.AddRange(list);
                    }
                    else
                    {
                        foreach (Group group in list)
                        {
                            bool isHas = false;
                            foreach (Group groupTmp in user.GroupLists)
                            {
                                if (groupTmp.ID == group.ID)
                                {
                                    isHas = true;
                                    break;
                                }
                            }
                            if (!isHas)
                                allGroups.Add(group);
                        }
                    }
                    this.AllGroups.DataSource = allGroups;
                    this.AllGroups.DataTextField = "Name";
                    this.AllGroups.DataValueField = "Unid";
                    this.AllGroups.DataBind();
                }
            }
        }

        #endregion
    }
}
