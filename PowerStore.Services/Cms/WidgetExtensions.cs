using PowerStore.Domain.Cms;
using System;

namespace PowerStore.Services.Cms
{
    public static class WidgetExtensions
    {
        public static bool IsWidgetActive(this IWidgetPlugin widget,
            WidgetSettings widgetSettings)
        {
            if (widget == null)
                throw new ArgumentNullException("widget");

            if (widgetSettings == null)
                throw new ArgumentNullException("widgetSettings");

            if (widgetSettings.ActiveWidgetSystemNames == null)
                return false;
            foreach (string activeMethodSystemName in widgetSettings.ActiveWidgetSystemNames)
                if (widget.PluginDescriptor.SystemName.Equals(activeMethodSystemName, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }
    }
}
