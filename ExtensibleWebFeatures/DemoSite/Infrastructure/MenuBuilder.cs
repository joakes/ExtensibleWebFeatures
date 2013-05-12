namespace DemoSite.Infrastructure
{
    using System.Collections.Generic;
    using System.Text;
    using WebFeatures.Infrastructure;
    using System.Linq;

    public class VerticalMenuBuilder : MenuBuilder
    {
        protected override sealed string MenuWrapFormatString { get; set; }

        public VerticalMenuBuilder()
        {
            MenuWrapFormatString = "<div>{0}</div>";
        }

        protected override string VisitMenuGroup(MenuGroup menuGroup)
        {
            return string.Format("<p>{0}</p><ul class=\"{1}\">{2}</ul>", menuGroup.Name, menuGroup.CssClass, "{0}");
        }

        protected override string VisitMenuItem(MenuItem menuItem)
        {
            return string.Format("<li class=\"{0}\">{1}</li>", menuItem.CssClass, menuItem.Name);
        }
    }

    public abstract class MenuBuilder
    {
        protected abstract string VisitMenuGroup(MenuGroup menuGroup);
        protected abstract string VisitMenuItem(MenuItem menuItem);
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