namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Tests
{
    using System;

    /// <summary>
    /// This would have been your unit testing framework
    /// </summary>
    public static class DummyAssert
    {
        public static void IsTrue(bool input)
        {
            if (!input) throw new Exception("Dummy test failed ...");
            return;
        }

        public static void IsTrue(bool input, string message)
        {
            if (!input) throw new Exception(message);
            return;
        }
    }
}