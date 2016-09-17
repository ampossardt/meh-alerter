using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MehNotifications.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MehNotifications.Helpers
{
    public class APIHelper
    {
        public APIHelper()
        {
            GetJson();
        }

        private JObject GetJson()
        {
            JObject deals;
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(this.Key);
                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        deals = JObject.Parse(JObject.Parse(json)["deal"].ToString());
                        return deals;
                    }
                    catch (NullReferenceException ne)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public Deal DealsInfo
        {
            get
            {
                var deals = new Deal();
                JObject dealsJson = GetJson();

                if (dealsJson == null)
                {
                    return null;
                }

                deals.Title = dealsJson["title"].ToString();
                deals.Url = dealsJson["url"].ToString();
                deals.Description = dealsJson["features"].ToString();
                deals.Items = new List<Item>();

                // Get all of the different styles and their prices
                JArray items = JArray.Parse(dealsJson["items"].ToString());
                foreach (JToken item in items.Children())
                {
                    JObject itemObject = JObject.Parse(item.ToString());
                    Item attributes = new Item();

                    attributes.Price = itemObject["price"].ToString();
                    attributes.Condition = itemObject["condition"].ToString();
                    attributes.Attributes = new List<string>();

                    JArray attributesArray = JArray.Parse(itemObject["attributes"].ToString());

                    foreach (JToken attr in attributesArray.Children())
                    {
                        attributes.Attributes.Add(string.Format("<strong>{0}</strong>: {1}", attr["key"], attr["value"]));
                    }

                    deals.Items.Add(attributes);
                }

                deals.Launches = new List<DateTime>();

                JArray launchesArray = JArray.Parse(dealsJson["launches"].ToString());

                foreach (JToken launchTime in launchesArray.Children())
                {
                    if (launchTime.HasValues)
                    {
                        deals.InStock = false;
                        if (launchTime["soldOutAt"].Type != JTokenType.Null)
                        {
                            var date = DateTime.Parse(launchTime["soldOutAt"].ToString());
                            deals.Launches.Add(date);
                        }
                        else
                        {
                            deals.Launches.Add(DateTime.MaxValue);
                        }
                    }
                }

                deals.Photo = dealsJson["photos"].HasValues ? dealsJson["photos"][0].ToString() : "";

                return deals;
            }
        }

        public string Key
        {
            get
            {
                return "https://api.meh.com/1/current.json?apikey=voriAHWsHyq8zDM6clejBrJn3cO4RaUd";
            }
        }
    }
}