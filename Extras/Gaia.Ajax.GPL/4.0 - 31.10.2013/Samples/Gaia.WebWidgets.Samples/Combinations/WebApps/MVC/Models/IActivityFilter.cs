namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Simple model to provide filtering/search capabilities
    /// </summary>
    public interface IActivityFilter
    {
        void BindPersons(IEnumerable<string> persons);
        string SelectedPerson { get;}
        DateTime When { get; set;}
    }
}