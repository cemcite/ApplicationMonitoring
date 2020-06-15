using NuevoSoftware.ApplicationMonitoring.Data;
using NuevoSoftware.ApplicationMonitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NuevoSoftware.ApplicationMonitoring.ClientBusiness
{
    /// <summary>
    /// 
    /// </summary>
    public static class ClientBusiness
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public static List<AppMonModel> GetApplicationList(this NsusersT userContext)
        {
            List<AppMonModel> applicationList = new List<AppMonModel>();
            List<NsapplicationsT> nsapplicationsT = userContext.NsapplicationsT.ToList();
            foreach (NsapplicationsT item in nsapplicationsT)
            {
                if (!item.IsActive) continue;
                AppMonModel model = new AppMonModel
                {
                    ID = item.Id,
                    Interval = item.Interval,
                    Name = item.Name,
                    URL = item.Url,
                    CreatedDate = item.CreatedDate,
                };
                bool result = StatusHelper.GetStatus(item.Url);
                model.Status = result ? "OK" : "Fail";
                applicationList.Add(model);
            }
            return applicationList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationList"></param>
        /// <param name="applicationID"></param>
        /// <returns></returns>
        public static AppMonModel GetApplication(this List<AppMonModel> applicationList, int applicationID)
        {
            AppMonModel applicationModel = applicationList.Single(application => application.ID == applicationID);
            return applicationModel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public static UserModel MapUserModel(NsusersT userContext)
        {
            return new UserModel
            {
                ID = userContext.Id,
                Username = userContext.UserName,
                Password = userContext.Password,
                ApplicationList = userContext.GetApplicationList()
            };
        }
    }
}
