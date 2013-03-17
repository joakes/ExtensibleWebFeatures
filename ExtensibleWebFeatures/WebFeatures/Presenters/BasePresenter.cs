namespace WebFeatures.Presenters
{
    using System.ComponentModel.Composition;
    using Infrastructure;

    public abstract class BasePresenter : IPartImportsSatisfiedNotification
    {
        [Import]
        private IFindWebFeature _findWebFeature;
        protected IWebFeature WebFeature;

        protected BasePresenter()
        {
            WebFeatureManager.ComposeParts(this);
        }
        
        public virtual void OnPageLoad(bool isPostBack) { }

        public void OnImportsSatisfied()
        {
            WebFeature = _findWebFeature.GetSpecificWebFeature(GetType().Name);
        }
    }
}