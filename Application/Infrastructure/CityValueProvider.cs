//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Application.Infrastructure
//{
//    public class CityValueProvider : IValueProvider
//    {
//        HttpSessionStateBase session;
//        public CityValueProvider(HttpSessionStateBase _session)
//        {
//            session = _session;
//        }

//        public bool ContainsPrefix(string prefix)
//        {
//            if(prefix=="cityId")
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public ValueProviderResult GetValue(string key)
//        {
//            int? result = 0;
//            if(key == "cityId")
//            {
//                result = (int?)session["City"];
//                return new ValueProviderResult(result, key, System.Globalization.CultureInfo.CurrentCulture);
//            }
//            return null;
//        }
//    }


//    public class CityValueProviderFactory : ValueProviderFactory
//    {
//        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
//        {
//            return new CityValueProvider(controllerContext.HttpContext.Session);
//        }
//    }
//}