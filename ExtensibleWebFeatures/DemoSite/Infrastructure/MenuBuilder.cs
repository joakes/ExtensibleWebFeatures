namespace DemoSite.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using WebFeatures.Infrastructure;
    using System.Linq;

    public class VerticalMenuBuilder : MenuBuilder
    {
        protected override sealed Func<MenuGroup, string> VisitMenuGroup { get; set; }
        protected override sealed Func<MenuItem, string> VisitMenuItem { get; set; }
        protected override sealed string MenuWrapFormatString { get; set; }

        public VerticalMenuBuilder()
        {
            MenuWrapFormatString = "<div>{0}</div>";
            VisitMenuItem = item => string.Format("<li class=\"{0}\">{1}</li>", item.CssClass, item.Name);
            VisitMenuGroup = @group => string.Format("<p>{0}</p><ul class=\"{1}\">{2}</ul>", @group.Name, @group.CssClass, "{0}");
        }
    }

    public abstract class MenuBuilder
    {
        protected abstract Func<MenuGroup, string> VisitMenuGroup { get; set; }
        protected abstract Func<MenuItem, string> VisitMenuItem { get; set; }
        protected abstract string MenuWrapFormatString { get; set; }

        public string BuildMenu()
        {
            var featureConfig = new FeatureConfiguration().InitialiseFeatureConfigFromXml();
            var strBuilder = new StringBuilder();

            var orderedGroups = featureConfig.MenuGroups.OrderBy(mg => mg.Position);
            
            foreach (var orderedGroup in orderedGroups)
            {
                var featuresInThisGroup = GetMenuItemsThatBelongInThisGroup(featureConfig.Features, orderedGroup.Name);
                var builder = new StringBuilder();
                foreach (var menuItem in featuresInThisGroup)
                {
                    builder.AppendLine(VisitMenuItem(menuItem));
                }
                var group = VisitMenuGroup(orderedGroup);
                strBuilder.AppendFormat(group, builder);
            }
            
            return string.Format(MenuWrapFormatString, strBuilder);
        }

        private static IEnumerable<MenuItem> GetMenuItemsThatBelongInThisGroup(IEnumerable<Feature> features, string groupName)
        {
            return from feature in features
                   where feature.Enabled &&
                         feature.MenuItem != null &&
                         feature.MenuItem.Group == groupName
                 orderby feature.MenuItem.Position
                  select feature.MenuItem;
        }
    }
}