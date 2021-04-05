# MongoDB.MongoContext

> Like DbContext from Entity Framework, but with nowhere near the same features, and it's for MongoDB.

In reality it's a bit of a mix between `DbContext` from Entity Framework and some concepts from event sourcing 
implementations, seeing as each `IMutation<TDocument>` is essentially an event.

## Creating a Context definition

To create a context definition, create a class extending `MongoContext`. Within the constructor of your context class, 
describe the collections in your application using the `Collection<TDocument>` method. This provides a fluent interface 
for specifying the collection name, primary key selector, and any indexes that should be created on the collection.

```c#
public class ArticlesContext : MongoContext
{
    public ArticlesContext(DatabaseContextOptions options)
        : base(options)
    {
        Articles = Collection<Article>("articles", (fb, article) => fb.Where(e => e.Id == article.Id))
            .HasIndex(
                idx => idx.Combine(
                    idx.Text(e => e.Title),
                    idx.Text(e => e.Body)
                ),
                (build, index) =>
                {
                    index.Weights = build.Weights()
                        .Assign(e => e.Title, 5);
                }
            )
            .HasIndex(idx => idx.Ascending(e => e.CreatedAt))
            .ToDbCollection();
    }

    public IMongoSet<Article> Articles { get; }
}
```

## Change Tracking, eh?

`MongoDB.MongoContext` implements a different kind of change tracking. It does not try to implement diff checks between 
documents as of yet, instead taking an explicit dependency on `MongoDB.Driver` by promoting the use of the 
`Update(update => update.Set(...))` method within aggregate commands.

### So how do I actually provide changes?

Through the aggregate itself of course! Each document in your application should extend from either 
`MongoAggregate<TDocument>` or `MongoAggregateRecord<TDocument>` (for record types), and implement changes using this 
pattern:

```c#
public class Article : MongoAggregate<Article>
{
    public Guid Id { get; private set; }

    public string Title { get; private set; } = null!;
    public string Body { get; private set; } = null!;

    public void ChangeTitle(string title)
    {
        // Perform input validation
        Title = title;
        
        Update(update => update
            .Set(e => e.Title, title));
    }
}
```

### Okay, how about saving changes?

Saving changes is done just like in Entity Framework:

```c#
// Find and track an entity
var article = await context.Find(e => e.Id == articleId)
    .FirstAsync();

// Mutate it
article.ChangeTitle("And so it begins...");

// Save all additions, deletions and changes tracked by this context in within a transaction
await context.SaveChangesAsync();
```