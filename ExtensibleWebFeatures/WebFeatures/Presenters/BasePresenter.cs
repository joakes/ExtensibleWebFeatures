namespace WebFeatures.Presenters
{
    using System.ComponentModel.Composition;
    using Infrastructure;

    public abstract class BasePresenter : IPartImportsSatisfiedNotification
    {
        [Import]
        private IFilterWebFeature _filterWebFeature;
        protected IWebFeature WebFeature;

        protected BasePresenter()
        {
            WebFeatureManager.ComposeParts(this);
        }
        
        public virtual void OnPageLoad(bool isPostBack) { }

        public void OnImportsSatisfied()
        {
            WebFeature = _filterWebFeature.GetSpecificWebFeature(GetType().Name);
        }
    }
}