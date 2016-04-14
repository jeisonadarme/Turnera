using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JeisonAdarme.BLL.Common
{
    public class Util
    {
        public static string GetNewNumericGuid()
        {
            var startDate = new DateTime(1970, 1, 1);
            var tmpTicks = DateTime.Now.Ticks - startDate.Ticks;
            var span = new TimeSpan(tmpTicks);
            var tmpId = (long)span.TotalMilliseconds;
            return tmpId.ToString(CultureInfo.InvariantCulture);
        }

    }

    public static class CastObjects
    {
        public static T CastObject<T, TU>(TU obj) where T : new()
        {
            T rawObject = new T();
            Type objType = obj.GetType();
            Type rawType = rawObject.GetType();
            PropertyInfo[] objPropertiesArray = objType.GetProperties();
            PropertyInfo[] rawPropertiesArray = rawType.GetProperties();
            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                foreach (PropertyInfo rawProperty in rawPropertiesArray)
                {
                    if (objProperty.Name == rawProperty.Name)
                    {
                        if (objProperty.PropertyType != rawProperty.PropertyType)
                        {
                            if (objProperty.PropertyType.Name.Equals("String") && rawProperty.PropertyType.Name.Equals("Guid") && !string.IsNullOrEmpty(objProperty.GetValue(obj, null).ToString()))
                            {
                                rawProperty.SetValue(rawObject,
                                    // Convert.ChangeType(
                                        Guid.Parse(objProperty.GetValue(obj, null).ToString()),
                                    // objProperty.PropertyType),
                             null);

                            }
                        }
                        else
                        {
                            rawProperty.SetValue(rawObject,
                                // Convert.ChangeType(
                              objProperty.GetValue(obj, null),
                                // objProperty.PropertyType),
                             null);

                        }
                    }
                }
            }

            return rawObject;
        }
    }
}
