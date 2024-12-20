namespace Subnautica.API.Extensions
{
    using System;

    using Subnautica.API.Features;

    public static class EventExtensions
    {
        /**
         *
         * https://learn.microsoft.com/en-us/dotnet/api/system.delegate.getinvocationlist?view=net-7.0 
         *
         *
         */
        public static void CustomInvoke<T>(this SubnauticaPluginEventHandler<T> ev, T arg)
        {
            if (ev != null)
            {
                foreach (SubnauticaPluginEventHandler<T> handler in ev.GetInvocationList())
                {
                    try
                    {
                        handler.Invoke(arg);
                    }
                    catch (Exception e)
                    {
                        Log.Error(string.Format("Method Name: {0}, Class Name: {1}, Event Name: {2}\n{3}", handler.Method.Name, handler.Method.ReflectedType?.FullName, ev.GetType().FullName, e));
                    }
                }
            }
        }

        /**
         *
         * https://learn.microsoft.com/en-us/dotnet/api/system.delegate.getinvocationlist?view=net-7.0 
         *
         *
         */
        public static void CustomInvoke(this SubnauticaPluginEventHandler ev)
        {
            if (ev != null)
            {
                foreach (SubnauticaPluginEventHandler handler in ev.GetInvocationList())
                {
                    try
                    {
                        handler.Invoke();
                    }
                    catch (Exception e)
                    {
                        Log.Error(string.Format("Method Name: {0}, Class Name: {1}, Event Name: {2}\n{3}", handler.Method.Name, handler.Method.ReflectedType?.FullName, ev.GetType().FullName, e));
                    }
                }
            }
        }

        /**
         *
         * Event Yöneticisi Parametre ile
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public delegate void SubnauticaPluginEventHandler<TEventArgs>(TEventArgs ev);

        /**
         *
         * Event Yöneticisi Parametre olmadan
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public delegate void SubnauticaPluginEventHandler();
    }
}