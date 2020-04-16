using Sample.Server.Core;
using Sample.Server.Core.Database;
using Sample.Server.Core.Database.Models;
using Sample.Server.Core.Logging;
using Sample.Server.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Server.Core.Manager
{
    public class LicenseManager
    {
        public void Register(string key, string hwid)
        {
            License target = null;
            var licenses = new MongoCrud().RetrieveRecords<License>("Licenses");
            foreach(var license in licenses)
            {
                if (license.Key == key)
                    target = license;
            }

            if (target == null)
                return;
               

            target.Hwid = hwid;
            target.Issued = DateTime.Now;
            new MongoCrud().UpdateLicense(target.Key, target);
        }
        
        public void ClearLicense(string key)
        {
            License target = null;
            var licenses = new MongoCrud().RetrieveRecords<License>("Licenses");
            foreach(var license in licenses)
            {
                if (license.Key == key)
                    target = license;
            }

            if (target == null)
                return;

            target.Hwid = "";
            target.Issued = DateTime.Now;
            new MongoCrud().UpdateLicense(target.Key, target);
        }

        public int State(string key)
        {
            var licenses = new MongoCrud().RetrieveRecords<License>("Licenses");

            foreach ( var license in licenses )
            {
                if(license.Key == key)
                {
                    return license.Hwid == "" ? 0 : 1;
                }
            }
            return -1;
        }

        public bool Whitelisted(string hwid)
        {
            var licenses = new MongoCrud().RetrieveRecords<License>("Licenses");

            if (!licenses.Any(license => license.Hwid == hwid))
                return false;

            var target = licenses.First(license => license.Hwid == hwid);
            return target.Issued.AddDays(target.ExpireAfterDays) > DateTime.Now;
        }

        public bool Valid(string key, string hwid)
        {
            var licenses = new MongoCrud().RetrieveRecords<License>("Licenses");

            return licenses.Any(license => license.Hwid == hwid && license.Key == key);
        }

        public bool IsBanned(string ip)
        {
            var bans = new MongoCrud().RetrieveRecords<Ban>("Bans");
            
            foreach (var ban in bans)
            {
                if (ban.IpAddress != ip) continue;
                
                if ((DateTime.Now - ban.Issued).Days > ban.Days)
                {
                    Logger.Log($"[!] Ban revoked -> {ban.IpAddress}");
                    new MongoCrud().RevokeBan(ban);
                } 
                else return true;
            }
            return false;
        }
    }
}
