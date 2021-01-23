# MongoDB.MongoContext

> Like DbContext from Entity Framework, but with nowhere near the same features, and it's for MongoDB.

Provides a context for performing operations on a MongoDB database, enabling transactions without manually passing
<see cref="IClientSessionHandle"/> objects around, and provides access to every collection all in one place.

```c#
public class DatabaseContext : MongoContext
{
    public DatabaseContext(MongoContextOptions options) : base(options) {}
    
    public IMongoCollection<Product> Products = Collection<Product>("products");
    public IMongoCollection<Order> Orders = Collection<Order>("orders");
}
```
