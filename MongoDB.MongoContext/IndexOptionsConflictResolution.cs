namespace MongoDB.MongoContext
{
    public enum IndexOptionsConflictResolution
    {
        None = 1,

        /// <summary>
        /// Specifies that the existing index should be dropped and replaced with the new index.
        /// </summary>
        Drop = 2,
    }
}