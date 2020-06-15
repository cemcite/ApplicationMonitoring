using NuevoSoftware.ApplicationMonitoring.Models;
using System.Collections.Generic;
using System.Linq;

namespace NuevoSoftware.ApplicationMonitoring.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceManager
    {
        /// <summary>
        /// 
        /// </summary>
        private static ResourceManager instance;
        /// <summary>
        /// 
        /// </summary>
        public static ResourceManager Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = new ResourceManager();
                return instance;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private List<ResourceModel> resourceList;
        /// <summary>
        /// 
        /// </summary>
        private List<ResourceModel> ResourceList
        {
            get
            {
                if (resourceList != null) return resourceList;

                List<ResourceModel> tempList = new List<ResourceModel>();
                List<NsresourcesT> nsresourcesTList = new List<NsresourcesT>();
                using (NSAppMonDBContext _context = new NSAppMonDBContext())
                {
                    nsresourcesTList = _context.NsresourcesT.Where(entity => entity.Language == "en-US").ToList(); // ToDo for multilanguage support
                }
                foreach (NsresourcesT item in nsresourcesTList)
                {
                    ResourceModel resourceModel = new ResourceModel
                    {
                        Key = item.ResourceKey,
                        Text = item.ResourceText
                    };
                    tempList.Add(resourceModel);
                }
                resourceList = tempList;
                return resourceList;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public string GetResource(string resourceKey)
        {
            List<KeyValuePair<string, string>> resourceList = new List<KeyValuePair<string, string>>();
            ResourceModel resource = ResourceList.SingleOrDefault(resource => resource.Key == resourceKey);
            if (resource == null) return resourceKey;
            else return resource.Text;
        }
    }
}
