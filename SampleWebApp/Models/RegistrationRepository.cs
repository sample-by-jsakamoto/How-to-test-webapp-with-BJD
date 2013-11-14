using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleWebApp.Models
{
    public class RegistrationRepository
    {
        private Dictionary<Guid, AttendeeRegistration> GetStrage()
        {
            const string storeKey = "a97c431e-295d-4055-844d-f24e1e5683a3";
            var appStore = HttpContext.Current.Application;
            lock (appStore)
            {
                if (appStore.AllKeys.Contains(storeKey) == false)
                {
                    appStore.Add(storeKey, new Dictionary<Guid, AttendeeRegistration>());
                }
                return appStore[storeKey] as Dictionary<Guid, AttendeeRegistration>;
            }
        }

        public Guid Regist(AttendeeRegistration registration)
        {
            var store = GetStrage();
            lock (store)
            {
                registration.Id = Guid.NewGuid();
                store.Add(registration.Id, registration);
                return registration.Id;
            }
        }

        public AttendeeRegistration Find(Guid id)
        {
            var store = GetStrage();
            lock (store)
            {
                return store[id];
            }
        }
    }
}