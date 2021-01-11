namespace litter_tracker.Objects.InternalObjects
{
    /*
    Config class used to hold appsettings at runtime. 
    Injected as IOption<ConnectionStrings> options.
    */
    public class ConnectionStrings
    {
        public string ProjectName { get; set; }
        public string BucketName { get; set; }
    }
}
