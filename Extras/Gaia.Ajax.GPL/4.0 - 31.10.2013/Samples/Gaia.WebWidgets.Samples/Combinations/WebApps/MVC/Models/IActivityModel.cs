namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Models
{
    /// <summary>
    /// This is the model that "glues" together the filter and results and is the
    /// main input to the Controller which then works against this model
    /// </summary>
    public interface IActivityModel
    {
        IActivityFilter Filter { get;}
        IActivityList ViewResults { get;} 
    }
}