using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Net.RichardLord.AshTests.Core
{
    public class TestingHelpers
    {
        public static void CallMethod(object obj, string name, object[] methodParams = null)
        {
            var dynMethod = obj.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(obj, methodParams);
        }
    }
}
