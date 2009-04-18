using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TSCommon_Core.Security.Domain
{
    public class PrivilegeComparer : IComparer 
    {
        #region IComparer ≥…‘±

        public int Compare(object x, object y)
        {
            Privilege p1 = (Privilege)x;
            Privilege p2 = (Privilege)y;
            return p1.OrderNo.CompareTo(p2.OrderNo);
        }

        #endregion
    }
}
