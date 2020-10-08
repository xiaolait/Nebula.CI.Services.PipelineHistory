using System;
using System.Reflection;

namespace Nebula.CI.Services.PipelineHistory
{
    public class InstanceOperator
    {
        private BindingFlags _flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        protected T CreateEntity<T>()
        {
            Assembly assembly = typeof(T).Assembly;
            var name = typeof(T).FullName;
            var entity = (T)assembly.CreateInstance(name, true, _flag, null, null, null, null);
            return entity;
        }

        protected void SetProperty(object instance, string propertyname, object value)
        {
            Type type = instance.GetType();
            var property = type.GetProperty(propertyname, _flag);
            property.SetValue(instance, value, null);
        }

        protected T GetProperty<T>(object instance, string propertyname)
        {
            Type type = instance.GetType();
            var property = type.GetProperty(propertyname, _flag);
            return (T)property.GetValue(instance, null);
        }

        protected void SetField(object instance, string fieldName, object value)
        {
            Type type = instance.GetType();
            var field = type.GetField(fieldName, _flag);
            field.SetValue(instance, value);
        }

        protected T GetField<T>(object instance, string fieldName)
        {
            Type type = instance.GetType();
            var field = type.GetField(fieldName, _flag);
            return (T)field.GetValue(instance);
        }
    }
}
